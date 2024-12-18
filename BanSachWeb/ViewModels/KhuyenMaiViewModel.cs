using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BanSachWeb.ViewModels
{
    public class KhuyenMaiViewModel
    {
        [Display(Name = "Mô Tả")]
        public string MoTa { get; set; }

        [Display(Name = "Mức Giảm")]
        public decimal? MucGiam { get; set; }

        [Display(Name = "Điều Kiện Áp Dụng")]
        public string DieuKienApDung { get; set; }

        [Display(Name = "Thời Gian Bắt Đầu")]
        [DataType(DataType.DateTime)]
        public DateTime? ThoiGianBatDau { get; set; }

        [Display(Name = "Thời Gian Kết Thúc")]
        [DataType(DataType.DateTime)]
        public DateTime? ThoiGianKetThuc { get; set; }
        
    }
}