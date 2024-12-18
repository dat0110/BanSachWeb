namespace BanSachWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TacGia")]
    public partial class TacGia
    {
        [Key]
        public int MaTacGia { get; set; }

        [StringLength(255)]
        public string TenTacGia { get; set; }

        [StringLength(255)]
       

        public string MoTa { get; set; }

        public bool? Visible { get; set; }
    }
}
