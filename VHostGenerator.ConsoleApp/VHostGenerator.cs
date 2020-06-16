using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace VHostGenerator.ConsoleApp
{
    internal abstract class VHostGenerator
    {
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

        internal abstract void PrepareVHost(string domaineName);

        internal abstract void CreateVHostFile(string domaineName);

        internal abstract void EnableVHost(string domaineName);

        internal abstract void RestartServerCommande();
    }
}