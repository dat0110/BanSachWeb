namespace BanSachWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TaiKhoan")]
    public partial class TaiKhoan
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TaiKhoan()
        {
            DiaChis = new HashSet<DiaChi>();
            DonHangs = new HashSet<DonHang>();
            GioHangs = new HashSet<GioHang>();
            PhanHois = new HashSet<PhanHoi>();
            KhuyenMais = new HashSet<KhuyenMai>();
        }

        [Key]
        public int MaTaiKhoan { get; set; }

        [StringLength(255)]
        public string TenDangNhap { get; set; }

        [StringLength(255)]
        public string MatKhau { get; set; }

        [StringLength(255)]
        public string Email { get; set; }

        [StringLength(255)]
        public string HoTen { get; set; }

        [StringLength(15)]
        public string SoDienThoai { get; set; }

        [StringLength(50)]
        public string VaiTro { get; set; }

        public int? DiemThuong { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DiaChi> DiaChis { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DonHang> DonHangs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GioHang> GioHangs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PhanHoi> PhanHois { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<KhuyenMai> KhuyenMais { get; set; }
    }
}
