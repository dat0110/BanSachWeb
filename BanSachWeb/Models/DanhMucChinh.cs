namespace BanSachWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DanhMucChinh")]
    public partial class DanhMucChinh
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DanhMucChinh()
        {
            DanhMucPhus = new HashSet<DanhMucPhu>();
        }

        [Key]
        public int MaDanhMucChinh { get; set; }

        [StringLength(255)]
        public string TenDanhMuc { get; set; }

        public string MoTa { get; set; }

        public bool? Visible { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DanhMucPhu> DanhMucPhus { get; set; }
    }
}
