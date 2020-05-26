using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace VHostGenerator.ConsoleApp
{
    internal static class Program
    {
        private static string _vHost = @"<VirtualHost *:80>
    ServerName $domain_name
    ServerAlias www.$domain_name

    DocumentRoot /var/www/$domain_name/

    <Directory /var/www/$domain_name/>
            Options -Indexes +FollowSymLinks -MultiViews
            AllowOverride All
            Require all granted
    </Directory>

    ErrorLog ${APACHE_LOG_DIR}/$domain_name.log
    CustomLog ${APACHE_LOG_DIR}/$domain_name.log combined

    ServerSignature Off
</VirtualHost>";
        private static string _html = @"<!DOCTYPE html>
<html lang=""fr"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Welcome to $domain_name!</title>
</head>
<body>
    <h1>$domain_name</h1>
</body>
</html>";
        private static string _domaineName = string.Empty;
        private static bool _isDomaineValid = false;

        private static void Main(string[] args)
        {
            Console.WriteLine("+----------------------------------------+");
            Console.WriteLine("+            V-HOST GENERATOR            +");
            Console.WriteLine($"+                 v{Assembly.GetEntryAssembly().GetName().Version.ToString(3)}                 +");
            Console.WriteLine("+            by Share IT Code            +");
            Console.WriteLine("+               MIT Licence              +");
            Console.WriteLine("+----------------------------------------+");

            while (!_isDomaineValid)
            {
                Console.Write("\nEnter domaine name (ex: github.com): ");
                _domaineName = Console.ReadLine();
                CheckDomaineNameValidity(_domaineName);
            }
            ApacheDebian(_domaineName);
        }

        private static void CheckDomaineNameValidity(string domaineName)
        {
            if (string.IsNullOrEmpty(domaineName))
            {
                Console.WriteLine("Invalide domaine!");
                return;
            }
            _isDomaineValid = true;
        }

        /// <summary>
        /// Generate Apache v-host for Debian
        /// </summary>
        /// <param name="domaineName"></param>
        private static void ApacheDebian(string domaineName)
        {
            try
            {
                string webSitePath = $"/var/www/{domaineName}";
                Console.WriteLine("\n----- Generate Apache v-host");
                PrepareWelcomeHtml(domaineName);
                CreateWebSiteDirectory(webSitePath);

                PrepareApacheVHost(domaineName);

                CreateVHostFileIntoApacheDirectory(domaineName);

                EnableApacheVHost(domaineName);

                RestartApacheServer();
            }
            catch (Exception exception)
            {
                #if DEBUG
                Console.WriteLine("---------- EXCEPTION ----------");
                Console.WriteLine($"---------- Message: {exception.Message}");
                Console.WriteLine($"---------- StackTrace: {exception.StackTrace}");
                #else
                Console.WriteLine("---------- An error is occured! :(");
                #endif
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private static void RestartApacheServer()
        {
            Console.WriteLine("\n 5 > Restart Apache server");
            RestartServerApacheDebianCommande();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domaineName"></param>
        private static void EnableApacheVHost(string domaineName)
        {
            Console.WriteLine($"\n 4 > Enable v-host (a2ensite {domaineName}.conf)");
            ApacheDebianCommande(domaineName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domaineName"></param>
        private static void CreateVHostFileIntoApacheDirectory(string domaineName)
        {
            string vHostFile = $"/etc/apache2/sites-available/{domaineName}.conf";
            Console.WriteLine($"\n 3 > Create v-host file in Apache directory ({vHostFile})");
            if (!File.Exists(vHostFile))
            {
                File.WriteAllText(vHostFile, _vHost);
            }
            else
            {
                Console.WriteLine($" --- V-host file in Apache directory ({vHostFile}) already existe!");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domaineName"></param>
        private static void PrepareApacheVHost(string domaineName)
        {
            Console.WriteLine("\n 2 > Prepare Apache v-host");
            _vHost = _vHost.Replace("$domain_name", domaineName);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="domaineName"></param>
        private static void PrepareWelcomeHtml(string domaineName)
        {
            _html = _html.Replace("$domain_name", domaineName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="webSitePath"></param>
        private static void CreateWebSiteDirectory(string webSitePath)
        {
            Console.WriteLine($" 1 > Create web site directory ({webSitePath})");
            if (!Directory.Exists(webSitePath))
            {
                Directory.CreateDirectory(webSitePath);
                File.WriteAllText($"{webSitePath}/index.html", _html);
            }
            else
            {
                Console.WriteLine($" --- Web site directory ({webSitePath}) already existe!");
            }
        }

        /// <summary>
        /// Execute a2ensite Apache commande
        /// </summary>
        /// <param name="domaineName"></param>
        private static void ApacheDebianCommande(string domaineName)
        {
            Process process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"a2ensite {domaineName}.conf\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            Console.WriteLine($" --- a2ensite command result:\n{result}");
        }

        /// <summary>
        /// Execute commande for restart Apache server
        /// </summary>
        private static void RestartServerApacheDebianCommande()
        {
            Process process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = "-c \"/etc/init.d/apache2 restart\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            Console.WriteLine($" --- apache2 restart command result:\n{result}");
        }
    }
}