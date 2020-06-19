using System;
using System.IO;

namespace VHostGenerator.ConsoleApp
{
    /// <summary>
    /// 
    /// </summary>
    internal abstract class VHostGenerator
    {
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="webSitePath"></param>
        public void CreateWebSiteDirectory(string webSitePath)
        {
            Console.WriteLine($" 1 > Create web site directory ({webSitePath})");
            if (!Directory.Exists(webSitePath))
            {
                Directory.CreateDirectory(webSitePath);
            }
            else
            {
                Console.WriteLine($" --- Web site directory ({webSitePath}) already existe!");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="webSitePath"></param>
        /// <param name="domaineName"></param>
        public void CreateWebSiteDirectoryWithHtmlFile(string webSitePath, string domaineName)
        {
            Console.WriteLine($" 1 > Create web site directory ({webSitePath})");
            if (!Directory.Exists(webSitePath))
            {
                Directory.CreateDirectory(webSitePath);
                _html = _html.Replace("$domain_name", domaineName);
                File.WriteAllText($"{webSitePath}/index.html", _html);
            }
            else
            {
                Console.WriteLine($" --- Web site directory ({webSitePath}) already existe!");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domaineName"></param>
        internal abstract void PrepareVHost(string domaineName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domaineName"></param>
        internal abstract void CreateVHostFile(string domaineName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domaineName"></param>
        internal abstract void EnableVHost(string domaineName);

        /// <summary>
        /// 
        /// </summary>
        internal abstract void RestartServerCommande();
    }
}