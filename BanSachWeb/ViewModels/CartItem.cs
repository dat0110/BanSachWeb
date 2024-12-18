using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BanSachWeb.Models
{
    public class CartItem
    {
        public Sach product{ get; set; }
        public int quantity {  get; set; }
    }
}