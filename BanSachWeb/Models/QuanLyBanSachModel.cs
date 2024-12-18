using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace BanSachWeb.Models
{
    public partial class QuanLyBanSachModel : DbContext
    {
        public QuanLyBanSachModel()
            : base("name=QuanLyBanSachModel9")
        {
        }

        public virtual DbSet<ChiNhanh> ChiNhanhs { get; set; }
        public virtual DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }
        public virtual DbSet<ChiTietGioHang> ChiTietGioHangs { get; set; }
        public virtual DbSet<DanhMucChinh> DanhMucChinhs { get; set; }
        public virtual DbSet<DanhMucPhu> DanhMucPhus { get; set; }
        public virtual DbSet<DiaChi> DiaChis { get; set; }
        public virtual DbSet<DonHang> DonHangs { get; set; }
        public virtual DbSet<GioHang> GioHangs { get; set; }
        public virtual DbSet<KhuyenMai> KhuyenMais { get; set; }
        public virtual DbSet<PhanHoi> PhanHois { get; set; }
        public virtual DbSet<Sach> Saches { get; set; }
        public virtual DbSet<TacGia> TacGias { get; set; }
        public virtual DbSet<TaiKhoan> TaiKhoans { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChiNhanh>()
                .HasMany(e => e.Saches)
                .WithMany(e => e.ChiNhanhs)
                .Map(m => m.ToTable("Sach_ChiNhanh").MapLeftKey("MaChiNhanh").MapRightKey("MaSach"));

            modelBuilder.Entity<ChiTietDonHang>()
                .Property(e => e.GiaBan)
                .HasPrecision(10, 2);

            modelBuilder.Entity<ChiTietDonHang>()
                .Property(e => e.ThanhTien)
                .HasPrecision(10, 2);

            modelBuilder.Entity<ChiTietGioHang>()
                .Property(e => e.ThanhTien)
                .HasPrecision(10, 2);

            modelBuilder.Entity<DonHang>()
                .Property(e => e.PhiVanChuyen)
                .HasPrecision(10, 2);

            modelBuilder.Entity<DonHang>()
                .Property(e => e.TongGiaTri)
                .HasPrecision(10, 2);

            modelBuilder.Entity<DonHang>()
                .Property(e => e.LoiNhuan)
                .HasPrecision(10, 2);

            modelBuilder.Entity<DonHang>()
                .HasMany(e => e.KhuyenMais)
                .WithMany(e => e.DonHangs)
                .Map(m => m.ToTable("DonHang_KhuyenMai").MapLeftKey("MaDonHang").MapRightKey("MaKhuyenMai"));

            modelBuilder.Entity<KhuyenMai>()
                .Property(e => e.MucGiam)
                .HasPrecision(5, 2);

            modelBuilder.Entity<KhuyenMai>()
                .HasMany(e => e.Saches)
                .WithMany(e => e.KhuyenMais)
                .Map(m => m.ToTable("KhuyenMai_Sach").MapLeftKey("MaKhuyenMai").MapRightKey("MaSach"));

            modelBuilder.Entity<KhuyenMai>()
                .HasMany(e => e.TaiKhoans)
                .WithMany(e => e.KhuyenMais)
                .Map(m => m.ToTable("KhuyenMai_TaiKhoan").MapLeftKey("MaKhuyenMai").MapRightKey("MaTaiKhoan"));

            modelBuilder.Entity<Sach>()
                .Property(e => e.GiaGoc)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Sach>()
                .Property(e => e.GiaBan)
                .HasPrecision(10, 2);
        }
    }
}
