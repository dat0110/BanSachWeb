namespace BanSachWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ChiTietGioHang")]
    public partial class ChiTietGioHang
    {
        [Key]
        public int MaChiTietGioHang { get; set; }

        public int? MaGioHang { get; set; }

        public int? MaSach { get; set; }

        public int? SoLuong { get; set; }

        public decimal? ThanhTien { get; set; }
        public bool IsSelected { get; set; } // New property

        public virtual GioHang GioHang { get; set; }

        public virtual Sach Sach { get; set; }
    }
}
