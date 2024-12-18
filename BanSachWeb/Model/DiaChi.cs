namespace BanSachWeb.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DiaChi")]
    public partial class DiaChi
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DiaChi()
        {
            DonHangs = new HashSet<DonHang>();
        }

        [Key]
        public int MaDiaChi { get; set; }

        public string DiaChiCuThe { get; set; }

        public bool? MacDinh { get; set; }

        [StringLength(15)]
        public string SoDienThoaiNhanHang { get; set; }

        [StringLength(255)]
        public string TenNguoiNhan { get; set; }

        public int? MaTaiKhoan { get; set; }

        public virtual TaiKhoan TaiKhoan { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DonHang> DonHangs { get; set; }
    }
}
