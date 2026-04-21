using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Nhom9_QLBanMyPham.Models;
using System.Data.Entity;

namespace Nhom9_QLBanMyPham.Controllers
{
    public class BaoCaoController : Controller
    {
        private QLSanPham db = new QLSanPham();

        public ActionResult Index(DateTime? tuNgay, DateTime? denNgay)
        {
            // 1. Thiết lập khoảng thời gian
            DateTime start = tuNgay ?? DateTime.Now.Date.AddDays(-29);
            DateTime end = denNgay ?? DateTime.Now.Date;
            DateTime endPoint = end.AddDays(1).AddTicks(-1);

            ViewBag.TuNgay = start.ToString("yyyy-MM-dd");
            ViewBag.DenNgay = end.ToString("yyyy-MM-dd");

            // Khởi tạo model DUY NHẤT một lần ở đây
            var model = new BaoCaoViewModel();

            try
            {
                // 2. Lấy đơn hàng và chi tiết đơn hàng
                var danhSachDonHang = db.tbl_DonHang
                    .Include(x => x.tbl_CTDonHang.Select(ct => ct.tbl_SanPham))
                    .Where(x => x.dNgayLap >= start && x.dNgayLap <= endPoint)
                    .ToList();

                // 3. Tính các thẻ thống kê
                model.TongDoanhThu = (decimal)(danhSachDonHang.Sum(x => x.fTongTien) ?? 0);
                model.TongDonHang = danhSachDonHang.Count;
                model.SoKhachHang = danhSachDonHang.Select(x => x.FK_sMaKH_DonHang).Distinct().Count();
                model.GiaTriTrungBinhDon = model.TongDonHang > 0 ? model.TongDoanhThu / model.TongDonHang : 0;

                // 4. Xử lý biểu đồ (SỬA LỖI TẠI ĐÂY)
                for (var date = start.Date; date <= end.Date; date = date.AddDays(1))
                {
                    var doanhThuNgay = danhSachDonHang
                        .Where(x => x.dNgayLap.HasValue && x.dNgayLap.Value.Date == date)
                        .Sum(x => (double?)x.fTongTien) ?? 0;

                    // Thêm dữ liệu vào danh sách đã có, KHÔNG khai báo lại model
                    model.BieuDoDoanhThu.Add(new DoanhThuTheoThang
                    {
                        Thang = date.ToString("dd/MM"),
                        DoanhThu = (decimal)doanhThuNgay
                    });
                }

                // 5. Tính Top 5 sản phẩm bán chạy
                model.SanPhamBanChay = danhSachDonHang
                    .SelectMany(x => x.tbl_CTDonHang)
                    .GroupBy(x => new { x.tbl_SanPham.sTenSP })
                    .Select(g => new TopSanPham
                    {
                        TenSP = g.Key.sTenSP,
                        SoLuongDaBan = g.Sum(x => (int?)x.iSoLuongBan) ?? 0,
                        DoanhThu = (decimal)(g.Sum(x => (double?)(x.fGiaBanLucDo * x.iSoLuongBan)) ?? 0)
                    })
                    .OrderByDescending(x => x.DoanhThu)
                    .Take(5)
                    .ToList();
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi hệ thống: " + ex.Message;
            }

            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}