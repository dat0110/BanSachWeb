using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BanSachWeb.Models.MoMo.Config
{
    public class MomoConfig
    {
        public static string ConfigName => "Momo";
        public static string PartnerCode { get; set; } = string.Empty;
        public static string ReturnUrl { get; set; } = string.Empty;
        public static string IpnUrl { get; set; } = string.Empty;
        public static string AccessKey { get; set; } = string.Empty;
        public static string SecretKey { get; set; } = string.Empty;
        public static string PaymentUrl { get; set; } = string.Empty;
    }
}