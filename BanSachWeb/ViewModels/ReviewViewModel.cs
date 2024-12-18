using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BanSachWeb.ViewModels
{
    public class ReviewViewModel
    {
        public string Username { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
    }
}