using System;
using System.Reflection;

namespace VHostGenerator.ConsoleApp
{
    internal static class ProgramTest
    {
        private static string _domaineName = string.Empty;
        private static bool _isDomaineValid = false;

        private static void MainTest(string[] args)
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
            // TODO: Add domain regex
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