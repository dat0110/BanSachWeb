using BanSachWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BanSachWeb.ViewModels
{
    public class SearchViewModel
    {
        public string SearchKey {  get; set; }
        public List<Sach> BooksResult { get; set; }
    }
}