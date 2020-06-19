using System;
using System.Reflection;
using System.Text.RegularExpressions;

namespace VHostGenerator.ConsoleApp
{
    /// <summary>
    /// 
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// 
        /// </summary>
        private static string _domaineName = string.Empty;
        
        /// <summary>
        /// 
        /// </summary>
        private static bool _isDomaineValid = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
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

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].StartsWith("os="))
                {
                    if (args[i].Contains("debian"))
                    {
                        
                    }
                    if (args[i].Contains("centos"))
                    {
                        
                    }
                }
                
                if (args[i].StartsWith("webserver="))
                {
                    if (args[i].Contains("apache"))
                    {

                    }
                    if (args[i].Contains("ngnix"))
                    {

                    }
                }
            }

            ApacheDebian(_domaineName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domaineName"></param>
        private static void CheckDomaineNameValidity(string domaineName)
        {
            // Regular expression from : https://stackoverflow.com/questions/106179/regular-expression-to-match-dns-hostname-or-ip-address
            if (string.IsNullOrEmpty(domaineName) || !Regex.IsMatch(domaineName, @"^(([a-zA-Z0-9]|[a-zA-Z0-9][a-zA-Z0-9\-]*[a-zA-Z0-9])\.)*([A-Za-z0-9]|[A-Za-z0-9][A-Za-z0-9\-]*[A-Za-z0-9])$"))
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
                VHostGenerator vHostGenerator = new DebianApacheVHost();
                vHostGenerator.CreateWebSiteDirectoryWithHtmlFile(webSitePath, domaineName);
                vHostGenerator.PrepareVHost(domaineName);
                vHostGenerator.CreateVHostFile(domaineName);
                vHostGenerator.EnableVHost(domaineName);
                vHostGenerator.RestartServerCommande();
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
    }
}