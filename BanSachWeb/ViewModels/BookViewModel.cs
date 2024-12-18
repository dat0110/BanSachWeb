using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BanSachWeb.Models
{
    public class BookViewModel
    {
        [Key]
        public int MaSach { get; set; }

        [StringLength(255)]
        public string TenSach { get; set; }

        [StringLength(255)]
        public string AnhSach { get; set; }

        public decimal? GiaGoc { get; set; }

        public decimal? GiaBan { get; set; }

        public int? SoLuongDaBan { get; set; }
        public string TenTacGia { get; set; }
        public double DiscountPercentage
        {
            get
            {
                if (GiaGoc > 0)
                {
                    double discount = (double)(1 - (GiaBan / GiaGoc)) * 100;
                    return Math.Round(discount, 0); // Round to two decimal places
                }
                return 0;
            }
        }
        public int? MaDanhMuc { get; set; } // Update to include MaDanhMuc


    }
}