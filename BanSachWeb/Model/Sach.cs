namespace BanSachWeb.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Sach")]
    public partial class Sach
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Sach()
        {
            ChiTietDonHangs = new HashSet<ChiTietDonHang>();
            ChiTietGioHangs = new HashSet<ChiTietGioHang>();
            PhanHois = new HashSet<PhanHoi>();
            KhuyenMais = new HashSet<KhuyenMai>();
            ChiNhanhs = new HashSet<ChiNhanh>();
            TacGias = new HashSet<TacGia>();
        }

        [Key]
        public int MaSach { get; set; }

        [StringLength(255)]
        public string TenSach { get; set; }

        [StringLength(255)]
        public string AnhSach { get; set; }

        public decimal? GiaGoc { get; set; }

        public decimal? GiaBan { get; set; }

        public int? SoLuongDaBan { get; set; }

        public int? SoLuongConDu { get; set; }

        public string TomTat { get; set; }

        [StringLength(255)]
        public string NhaXuatBan { get; set; }

        public int? NamXuatBan { get; set; }

        [StringLength(255)]
        public string HinhThuc { get; set; }

        public int? SoTrang { get; set; }

        [StringLength(50)]
        public string KichThuoc { get; set; }

        public double? TrongLuong { get; set; }

        public int? MaTacGia { get; set; }

        public int? MaDanhMuc { get; set; }

        public int? MaGioHang { get; set; }

        public int? MaChiTietDonHang { get; set; }

        public bool? Visible { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietGioHang> ChiTietGioHangs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PhanHoi> PhanHois { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<KhuyenMai> KhuyenMais { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiNhanh> ChiNhanhs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TacGia> TacGias { get; set; }
    }
}
