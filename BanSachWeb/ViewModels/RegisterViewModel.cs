using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BanSachWeb.ViewModels
{
    public class RegisterViewModel
    {
        //validate tendangnhap
        [Required]
        [StringLength(255)]
        public string TenDangNhap { get; set; }

        //validate pass
        [Required]
        [StringLength(255)]
        [DataType(DataType.Password)]// ma hoa hien thi mat khau
        public string MatKhau { get; set; }

        [Required]
        [StringLength(255)]
        [EmailAddress]// regex for email
        public string Email { get; set; }

        [Required]
        [StringLength(255)]
        public string HoTen { get; set; }

        [Required]
        [StringLength(15)]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Số điện thoại phải chứa đúng 10 chữ số.")]
        public string SoDienThoai { get; set; }
    }
}