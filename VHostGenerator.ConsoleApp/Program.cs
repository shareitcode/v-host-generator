using System;
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
            Console.WriteLine("\n----- Generate Apache v-host");
            Console.WriteLine($" 1 > Create web site directory (/var/www/{domaineName})");
            Console.WriteLine(" 2 > Prepare Apache v-host");
            _vHost = _vHost.Replace("$domain_name", domaineName);
            Console.WriteLine($" 3 > Create v-host file in Apache directory (/etc/apache2/sites-available/{domaineName}.conf)");
            Console.WriteLine($" 4 > Enable v-host (a2ensite {domaineName}.conf)");
            Console.WriteLine(" 5 > Restart Apache server");
        }
    }
}