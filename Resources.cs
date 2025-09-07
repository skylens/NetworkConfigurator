using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace NetworkConfigurator
{
    public class Resources
    {
        // 定义英文资源
        private static Dictionary<string, string> EnglishResources = new Dictionary<string, string>
        {
            {"AppTitle", "Network Interface IP Configurator"},
            {"NoInterfacesFound", "No enabled IPv4 network interfaces found."},
            {"AvailableInterfaces", "Available network interfaces:"},
            {"SelectInterfacePrompt", "Please select interface number [1-{0}]: "},
            {"InvalidSelection", "Invalid selection."},
            {"CurrentConfiguration", "Current configuration:"},
            {"IPAddressLabel", "IP: {0}"},
            {"SubnetMaskLabel", "Subnet Mask: {0}"},
            {"GatewayLabel", "Gateway: {0}"},
            {"DNSLabel", "DNS: {0}"},
            {"UsingCustomDNS", "Will use specified DNS servers: {0}"},
            {"ConfirmSetStatic", "Set this configuration as static? (Y/N): "},
            {"OperationCancelled", "Operation cancelled."},
            {"SetStaticSuccess", "Configuration successfully set to static IP."},
            {"SetStaticFailure", "Failed to set static IP. Please ensure the program is run with administrator privileges."},
            {"ErrorOccurred", "An error occurred during program execution: {0}"}
        };

        // 定义中文资源
        private static Dictionary<string, string> ChineseResources = new Dictionary<string, string>
        {
            {"AppTitle", "网卡IP配置管理器"},
            {"NoInterfacesFound", "未找到启用且支持IPv4的网卡。"},
            {"AvailableInterfaces", "检测到以下可用网卡："},
            {"SelectInterfacePrompt", "请选择网卡编号 [1-{0}]: "},
            {"InvalidSelection", "无效的选择。"},
            {"CurrentConfiguration", "当前配置："},
            {"IPAddressLabel", "IP: {0}"},
            {"SubnetMaskLabel", "子网掩码: {0}"},
            {"GatewayLabel", "网关: {0}"},
            {"DNSLabel", "DNS: {0}"},
            {"UsingCustomDNS", "将使用指定的DNS服务器: {0}"},
            {"ConfirmSetStatic", "是否将此配置设为静态？(Y/N): "},
            {"OperationCancelled", "操作已取消。"},
            {"SetStaticSuccess", "设置成功！当前网卡已配置为静态IP。"},
            {"SetStaticFailure", "设置失败，请确保以管理员权限运行程序。"},
            {"ErrorOccurred", "程序运行时发生错误: {0}"}
        };

        private static Dictionary<string, string> CurrentResources;
        
        static Resources()
        {
            // 根据系统语言初始化资源
            var systemLanguage = CultureInfo.InstalledUICulture.Name;
            if (systemLanguage.StartsWith("zh", StringComparison.OrdinalIgnoreCase))
            {
                CurrentResources = ChineseResources;
            }
            else
            {
                CurrentResources = EnglishResources;
            }
        }

        public static string GetString(string key)
        {
            if (CurrentResources.ContainsKey(key))
            {
                return CurrentResources[key];
            }
            return $"[[{key}]]";
        }

        // 程序标题
        public static string AppTitle => GetString("AppTitle");

        // 网卡列表相关
        public static string NoInterfacesFound => GetString("NoInterfacesFound");
        public static string AvailableInterfaces => GetString("AvailableInterfaces");
        public static string SelectInterfacePrompt => GetString("SelectInterfacePrompt");
        public static string InvalidSelection => GetString("InvalidSelection");

        // 配置显示相关
        public static string CurrentConfiguration => GetString("CurrentConfiguration");
        public static string IPAddressLabel => GetString("IPAddressLabel");
        public static string SubnetMaskLabel => GetString("SubnetMaskLabel");
        public static string GatewayLabel => GetString("GatewayLabel");
        public static string DNSLabel => GetString("DNSLabel");
        public static string UsingCustomDNS => GetString("UsingCustomDNS");

        // 操作确认相关
        public static string ConfirmSetStatic => GetString("ConfirmSetStatic");
        public static string OperationCancelled => GetString("OperationCancelled");
        public static string SetStaticSuccess => GetString("SetStaticSuccess");
        public static string SetStaticFailure => GetString("SetStaticFailure");
        public static string ErrorOccurred => GetString("ErrorOccurred");
    }
}