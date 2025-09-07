using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace NetworkConfigurator
{
    public class NetworkAdapterManager
    {
        public static List<NetworkInterfaceInfo> GetEnabledIPv4Interfaces()
        {
            var interfaces = new List<NetworkInterfaceInfo>();
            
            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                // 只处理启用且支持IPv4的网卡
                if (ni.OperationalStatus == OperationalStatus.Up && 
                    ni.Supports(NetworkInterfaceComponent.IPv4))
                {
                    var ipProps = ni.GetIPProperties();
                    var ipv4Addresses = ipProps.UnicastAddresses
                        .Where(addr => addr.Address.AddressFamily == AddressFamily.InterNetwork)
                        .ToList();
                    
                    // 只处理有IPv4地址的网卡
                    if (ipv4Addresses.Any())
                    {
                        var interfaceInfo = new NetworkInterfaceInfo
                        {
                            Name = ni.Name,
                            Id = ni.Id,
                            DnsServers = new List<string>()
                        };
                        
                        // 获取IP地址和子网掩码
                        var ipv4Addr = ipv4Addresses.First();
                        interfaceInfo.IpAddress = ipv4Addr.Address.ToString();
                        interfaceInfo.SubnetMask = ipv4Addr.IPv4Mask.ToString();
                        
                        // 获取默认网关
                        var gateway = ipProps.GatewayAddresses
                            .Where(g => g.Address.AddressFamily == AddressFamily.InterNetwork)
                            .FirstOrDefault();
                        interfaceInfo.Gateway = gateway?.Address?.ToString() ?? "";
                        
                        // 获取DNS服务器
                        var dnsAddresses = ipProps.DnsAddresses
                            .Where(dns => dns.AddressFamily == AddressFamily.InterNetwork)
                            .Select(dns => dns.ToString());
                        interfaceInfo.DnsServers.AddRange(dnsAddresses);
                        
                        interfaces.Add(interfaceInfo);
                    }
                }
            }
            
            return interfaces;
        }
        
        public static bool SetStaticIP(NetworkInterfaceInfo interfaceInfo)
        {
            try
            {
                // 使用WMI设置静态IP配置
                using (var managementClass = new ManagementClass("Win32_NetworkAdapterConfiguration"))
                {
                    var managementObjects = managementClass.GetInstances();
                    
                    foreach (ManagementObject mo in managementObjects)
                    {
                        if ((bool)mo["IPEnabled"] && mo["SettingID"].ToString() == interfaceInfo.Id)
                        {
                            // 设置静态IP和子网掩码
                            var ipAddresses = new string[] { interfaceInfo.IpAddress };
                            var subnetMasks = new string[] { interfaceInfo.SubnetMask };
                            
                            var setIpParams = mo.GetMethodParameters("EnableStatic");
                            setIpParams["IPAddress"] = ipAddresses;
                            setIpParams["SubnetMask"] = subnetMasks;
                            mo.InvokeMethod("EnableStatic", setIpParams, null);
                            
                            // 设置默认网关
                            if (!string.IsNullOrEmpty(interfaceInfo.Gateway))
                            {
                                var gatewayParams = mo.GetMethodParameters("SetGateways");
                                gatewayParams["DefaultIPGateway"] = new string[] { interfaceInfo.Gateway };
                                mo.InvokeMethod("SetGateways", gatewayParams, null);
                            }
                            
                            // 设置DNS服务器
                            if (interfaceInfo.DnsServers.Any())
                            {
                                var dnsParams = mo.GetMethodParameters("SetDNSServerSearchOrder");
                                dnsParams["DNSServerSearchOrder"] = interfaceInfo.DnsServers.ToArray();
                                mo.InvokeMethod("SetDNSServerSearchOrder", dnsParams, null);
                            }
                            
                            return true;
                        }
                    }
                }
                
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format(Resources.ErrorOccurred, ex.Message));
                return false;
            }
        }
    }
}