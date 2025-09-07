using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkConfigurator
{
    public class CommandLineParser
    {
        public static List<string> ParseDnsServers(string[] args)
        {
            var dnsServers = new List<string>();
            
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Equals("--SetDNS", StringComparison.OrdinalIgnoreCase) && i + 1 < args.Length)
                {
                    var dnsParam = args[i + 1];
                    var servers = dnsParam.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var server in servers)
                    {
                        var trimmedServer = server.Trim();
                        if (!string.IsNullOrEmpty(trimmedServer))
                        {
                            dnsServers.Add(trimmedServer);
                        }
                    }
                    i++; // Skip the next argument as it's the DNS parameter
                }
            }
            
            return dnsServers;
        }
    }
}