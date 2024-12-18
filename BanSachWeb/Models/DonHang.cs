namespace BanSachWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DonHang")]
    public partial class DonHang
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DonHang()
        {
            ChiTietDonHangs = new HashSet<ChiTietDonHang>();
            KhuyenMais = new HashSet<KhuyenMai>();
        }

        [Key]
        public int MaDonHang { get; set; }

        public DateTime? ThoiGianDatHang { get; set; }

        [StringLength(100)]
        public string TrangThai { get; set; }

        [StringLength(255)]
        public string DonViVanChuyen { get; set; }

        public DateTime? ThoiGianGiaoHangDuKien { get; set; }

        [StringLength(100)]
        public string PhuongThucThanhToan { get; set; }

        [StringLength(255)]
        public string MaQR { get; set; }

        public decimal? PhiVanChuyen { get; set; }

        public decimal? TongGiaTri { get; set; }

        public decimal? LoiNhuan { get; set; }

        public bool? DaThanhToan { get; set; }

        public int? MaTaiKhoan { get; set; }

        public int? MaDiaChi { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; }

        public virtual DiaChi DiaChi { get; set; }

        public virtual TaiKhoan TaiKhoan { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<KhuyenMai> KhuyenMais { get; set; }
    }
}
