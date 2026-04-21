using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Nhom9_QLBanMyPham.Models
{
    public partial class QLSanPham : DbContext
    {
        public QLSanPham()
            : base("name=QLSanPham")
        {
        }

        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<tbl_CTDonHang> tbl_CTDonHang { get; set; }
        public virtual DbSet<tbl_DanhMuc> tbl_DanhMuc { get; set; }
        public virtual DbSet<tbl_DonHang> tbl_DonHang { get; set; }
        public virtual DbSet<tbl_HoaDon> tbl_HoaDon { get; set; }
        public virtual DbSet<tbl_KhachHang> tbl_KhachHang { get; set; }
        public virtual DbSet<tbl_NguoiDung> tbl_NguoiDung { get; set; }
        public virtual DbSet<tbl_NhaCungCap> tbl_NhaCungCap { get; set; }
        public virtual DbSet<tbl_SanPham> tbl_SanPham { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tbl_CTDonHang>()
                .Property(e => e.PK_sMaSP_CTDonHang)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_DanhMuc>()
                .Property(e => e.PK_sMaDM)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_DanhMuc>()
                .HasMany(e => e.tbl_SanPham)
                .WithOptional(e => e.tbl_DanhMuc)
                .HasForeignKey(e => e.FK_sMaDM_SanPham);

            modelBuilder.Entity<tbl_DonHang>()
                .Property(e => e.FK_sMaKH_DonHang)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_DonHang>()
                .Property(e => e.FK_sMaNV_DonHang)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_DonHang>()
                .HasMany(e => e.tbl_CTDonHang)
                .WithRequired(e => e.tbl_DonHang)
                .HasForeignKey(e => e.PK_iMaDH_CTDonHang)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_DonHang>()
                .HasMany(e => e.tbl_HoaDon)
                .WithOptional(e => e.tbl_DonHang)
                .HasForeignKey(e => e.FK_iMaDH);

            modelBuilder.Entity<tbl_KhachHang>()
                .Property(e => e.PK_sMaKH)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_KhachHang>()
                .Property(e => e.sSDT)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_KhachHang>()
                .Property(e => e.sEmail)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_KhachHang>()
                .HasMany(e => e.tbl_DonHang)
                .WithOptional(e => e.tbl_KhachHang)
                .HasForeignKey(e => e.FK_sMaKH_DonHang);

            modelBuilder.Entity<tbl_NguoiDung>()
                .Property(e => e.PK_sMaNV)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_NguoiDung>()
                .Property(e => e.sSDT)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_NguoiDung>()
                .Property(e => e.sMatKhau)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_NguoiDung>()
                .HasMany(e => e.tbl_DonHang)
                .WithOptional(e => e.tbl_NguoiDung)
                .HasForeignKey(e => e.FK_sMaNV_DonHang);

            modelBuilder.Entity<tbl_NhaCungCap>()
                .Property(e => e.PK_sMaNCC)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_NhaCungCap>()
                .Property(e => e.sSDT)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_NhaCungCap>()
                .HasMany(e => e.tbl_SanPham)
                .WithOptional(e => e.tbl_NhaCungCap)
                .HasForeignKey(e => e.FK_sMaNCC_SanPham);

            modelBuilder.Entity<tbl_SanPham>()
                .Property(e => e.PK_sMaSP)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_SanPham>()
                .Property(e => e.FK_sMaNCC_SanPham)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_SanPham>()
                .Property(e => e.FK_sMaDM_SanPham)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_SanPham>()
                .HasMany(e => e.tbl_CTDonHang)
                .WithRequired(e => e.tbl_SanPham)
                .HasForeignKey(e => e.PK_sMaSP_CTDonHang)
                .WillCascadeOnDelete(false);
        }
    }
}
