namespace BanSachWeb.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DanhMucPhu")]
    public partial class DanhMucPhu
    {
        [Key]
        public int MaDanhMucPhu { get; set; }

        [StringLength(255)]
        public string TenDanhMuc { get; set; }

        public string MoTa { get; set; }

        public bool? Visible { get; set; }

        public int? MaDanhMucChinh { get; set; }

        public virtual DanhMucChinh DanhMucChinh { get; set; }
    }
}
