using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace VHostGenerator.ConsoleApp
{
    internal sealed class DebianApacheVHost : ApacheVHost
    {
        internal override void CreateVHostFile(string domaineName)
        {
            string vHostFile = $"/etc/apache2/sites-available/{domaineName}.conf";
            Console.WriteLine($"\n 3 > Create v-host file in Apache directory ({vHostFile})");
            if (!File.Exists(vHostFile))
            {
                File.WriteAllText(vHostFile, VHost);
            }
            else
            {
                Console.WriteLine($" --- V-host file in Apache directory ({vHostFile}) already existe!");
            }
        }
        
        internal override void EnableVHost(string domaineName)
        {
            Console.WriteLine($"\n 4 > Enable v-host (a2ensite {domaineName}.conf)");
            ApacheCommande(domaineName);
        }
        
        internal override void ApacheCommande(string domaineName)
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

        internal override void RestartServerCommande()
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