using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkConfigurator
{
    public class NetworkInterfaceInfo
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string IpAddress { get; set; }
        public string SubnetMask { get; set; }
        public string Gateway { get; set; }
        public List<string> DnsServers { get; set; }
    }
}