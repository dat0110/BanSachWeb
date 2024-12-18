using BanSachWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BanSachWeb.ViewModels
{
    public class BookDetailsViewModel
    {
        public Sach Book { get; set; }
        public List<Sach> RelevantBooks { get; set; }
    }
}