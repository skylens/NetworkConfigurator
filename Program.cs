using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetworkConfigurator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Resources.AppTitle);
            Console.WriteLine("================");
            
            try
            {
                // 获取命令行参数中的DNS服务器
                var customDnsServers = CommandLineParser.ParseDnsServers(args);
                
                // 获取所有启用且支持IPv4的网卡
                var interfaces = NetworkAdapterManager.GetEnabledIPv4Interfaces();
                
                if (!interfaces.Any())
                {
                    Console.WriteLine(Resources.NoInterfacesFound);
                    return;
                }
                
                // 显示网卡列表
                Console.WriteLine(Resources.AvailableInterfaces);
                for (int i = 0; i < interfaces.Count; i++)
                {
                    Console.WriteLine($"[{i + 1}] {interfaces[i].Name}");
                }
                
                // 让用户选择网卡
                Console.Write(string.Format(Resources.SelectInterfacePrompt, interfaces.Count));
                var input = Console.ReadLine();
                
                if (!int.TryParse(input, out int selectedIndex) || 
                    selectedIndex < 1 || selectedIndex > interfaces.Count)
                {
                    Console.WriteLine(Resources.InvalidSelection);
                    return;
                }
                
                var selectedInterface = interfaces[selectedIndex - 1];
                
                // 显示当前配置
                Console.WriteLine($"\n{Resources.CurrentConfiguration}");
                Console.WriteLine(string.Format(Resources.IPAddressLabel, selectedInterface.IpAddress));
                Console.WriteLine(string.Format(Resources.SubnetMaskLabel, selectedInterface.SubnetMask));
                Console.WriteLine(string.Format(Resources.GatewayLabel, selectedInterface.Gateway));
                Console.WriteLine(string.Format(Resources.DNSLabel, string.Join(", ", selectedInterface.DnsServers)));
                
                // 如果命令行指定了DNS服务器，则使用指定的DNS服务器
                if (customDnsServers.Any())
                {
                    selectedInterface.DnsServers = customDnsServers;
                    Console.WriteLine($"\n{string.Format(Resources.UsingCustomDNS, string.Join(", ", selectedInterface.DnsServers))}");
                }
                
                // 确认操作
                Console.Write($"\n{Resources.ConfirmSetStatic}");
                var confirm = Console.ReadLine();
                
                if (!confirm.Equals("Y", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine(Resources.OperationCancelled);
                    return;
                }
                
                // 设置静态IP
                if (NetworkAdapterManager.SetStaticIP(selectedInterface))
                {
                    Console.WriteLine($"\n{Resources.SetStaticSuccess}");
                    Console.WriteLine(string.Format(Resources.DNSLabel, string.Join(", ", selectedInterface.DnsServers)));
                }
                else
                {
                    Console.WriteLine($"\n{Resources.SetStaticFailure}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format(Resources.ErrorOccurred, ex.Message));
            }
        }
    }
}