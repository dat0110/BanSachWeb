namespace BanSachWeb.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PhanHoi")]
    public partial class PhanHoi
    {
        [Key]
        public int MaPhanHoi { get; set; }

        public string NoiDung { get; set; }

        public int? DiemDanhGia { get; set; }

        public int? MaSach { get; set; }

        public int? MaTaiKhoan { get; set; }

        public virtual Sach Sach { get; set; }

        public virtual TaiKhoan TaiKhoan { get; set; }
    }
}
