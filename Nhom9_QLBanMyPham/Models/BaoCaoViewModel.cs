using System;
using System.Collections.Generic; // Thêm dòng này
using System.Linq;
namespace Nhom9_QLBanMyPham.Models
{
    public class BaoCaoViewModel
    {
        public decimal TongDoanhThu { get; set; }
        public int TongDonHang { get; set; }
        public int SoKhachHang { get; set; }
        public decimal GiaTriTrungBinhDon { get; set; }
        public List<DoanhThuTheoThang> BieuDoDoanhThu { get; set; }
        public List<TopSanPham> SanPhamBanChay { get; set; }

        public BaoCaoViewModel()
        {
            BieuDoDoanhThu = new List<DoanhThuTheoThang>();
            SanPhamBanChay = new List<TopSanPham>();
        }
    }


    public class DoanhThuTheoThang
    {
        public string Thang { get; set; }
        public decimal DoanhThu { get; set; }
    }

    public class TopSanPham
    {
        public string TenSP { get; set; }
        public int SoLuongDaBan { get; set; }
        public decimal DoanhThu { get; set; }
        public string HinhAnh { get; set; }
    }
}