using BanSachWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BanSachWeb.ViewModels
{
    public class BookReviewDetailsViewModel
    {
        public Sach Book { get; set; }
        public IEnumerable<ReviewViewModel> Reviews { get; set; }
        public double AverageRating { get; set; }
        public IEnumerable<Sach> RelevantBooks { get; set; }
    }
}