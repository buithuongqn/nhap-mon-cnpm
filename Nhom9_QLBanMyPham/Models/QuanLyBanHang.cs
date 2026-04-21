using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nhom9_QLBanMyPham.Models
{
    public class QuanLyBanHang
    {
        public string MaKH { get; set; }
        public decimal TongTienDonHang { get; set; }
        public List<ChiTietHang> DanhSachSanPham { get; set; }
    }

    public class ChiTietHang
    {
        public string MaSP { get; set; }
        public int SoLuong { get; set; }
        public double DonGia { get; set; }
    }
}