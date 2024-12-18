namespace BanSachWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ChiTietDonHang")]
    public partial class ChiTietDonHang
    {
        [Key]
        public int MaChiTietDonHang { get; set; }

        public int? SoLuong { get; set; }

        public decimal? GiaBan { get; set; }

        public decimal? ThanhTien { get; set; }

        public int? MaDonHang { get; set; }

        public int? MaSach { get; set; }

        public virtual DonHang DonHang { get; set; }

        public virtual Sach Sach { get; set; }
    }
}
