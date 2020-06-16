﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace VHostGenerator.ConsoleApp
{
    internal abstract class ApacheVHost : VHostGenerator
    {
        internal string VHost = @"<VirtualHost *:80>
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

        /// <summary>
        /// Replace domaine name variable in v-host string
        /// </summary>
        /// <param name="domaineName"></param>
        internal override void PrepareVHost(string domaineName)
        {
            Console.WriteLine("\n 2 > Prepare Apache v-host");
            VHost = VHost.Replace("$domain_name", domaineName);
        }

        internal abstract void ApacheCommande(string domaineName);
    }
}