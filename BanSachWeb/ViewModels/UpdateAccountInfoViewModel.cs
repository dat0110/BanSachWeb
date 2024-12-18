using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BanSachWeb.ViewModels
{
    public class UpdateAccountInfoViewModel
    {
        [Required(ErrorMessage = "Username is required.")]
        public string TenDangNhap { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Full name is required.")]
        public string HoTen { get; set; }

        [StringLength(10, ErrorMessage = "Phone number must be 10 digits.", MinimumLength = 10)]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits.")]
        public string SoDienThoai { get; set; }

        public string ThongTinNhanHang { get; set; }
    }
}
