using BanSachWeb.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BanSachWeb.ViewModels
{
    public class AddressViewModel
    {
        public int MaDiaChi { get; set; }

        public int MaTaiKhoan { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "Address is required.")]
        public string DiaChiCuThe { get; set; }

        [StringLength(10, ErrorMessage = "Phone number must be 10 digits.", MinimumLength = 10)]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits.")]
        public string SoDienThoaiNhanHang { get; set; }

        [StringLength(255)]
        public string TenNguoiNhan { get; set; }

        public bool MacDinh { get; set; } // Added MacDinh property for default status
       
    }
}