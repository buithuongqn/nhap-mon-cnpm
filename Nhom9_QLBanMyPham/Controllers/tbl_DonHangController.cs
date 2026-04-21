using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Nhom9_QLBanMyPham.Models;

namespace Nhom9_QLBanMyPham.Controllers
{
    public class tbl_DonHangController : Controller
    {
        private QLSanPham db = new QLSanPham();

        // GET: tbl_DonHang
        public ActionResult Index()
        {
            var tbl_DonHang = db.tbl_DonHang
                .Include(t => t.tbl_KhachHang)
                .Include(t => t.tbl_NguoiDung)
                .OrderByDescending(x => x.dNgayLap); // Đơn hàng mới nhất lên đầu
            return View(tbl_DonHang.ToList());
        }

        // GET: tbl_DonHang/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            tbl_DonHang tbl_DonHang = db.tbl_DonHang
                .Include(t => t.tbl_CTDonHang.Select(ct => ct.tbl_SanPham)) // Load chi tiết và thông tin sản phẩm
                .FirstOrDefault(m => m.PK_iMaDH == id);

            if (tbl_DonHang == null) return HttpNotFound();

            return View(tbl_DonHang);
        }

        // GET: tbl_DonHang/Create
        public ActionResult Create()
        {
            ViewBag.sMaKH = new SelectList(db.tbl_KhachHang, "PK_sMaKH", "sTenKH");
            ViewBag.sMaNV = new SelectList(db.tbl_NguoiDung, "PK_sMaNV", "sTenNV");
            return View();
        }

        [HttpPost]
        public JsonResult CreateNew(QuanLyBanHang model)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    if (model == null || model.DanhSachSanPham == null || !model.DanhSachSanPham.Any())
                    {
                        return Json(new { ok = false, msg = "Vui lòng thêm sản phẩm!" });
                    }

                    var dh = new tbl_DonHang();
                    dh.dNgayLap = DateTime.Now;
                    dh.FK_sMaKH_DonHang = model.MaKH;
                    dh.FK_sMaNV_DonHang = "NV01"; // Gợi ý: Thay bằng Session["MaNV"]
                    dh.fTongTien = (double)model.DanhSachSanPham.Sum(x => x.SoLuong * x.DonGia);

                    db.tbl_DonHang.Add(dh);
                    db.SaveChanges();

                    foreach (var item in model.DanhSachSanPham)
                    {
                        var sanPham = db.tbl_SanPham.Find(item.MaSP);
                        if (sanPham == null)
                        {
                            transaction.Rollback();
                            return Json(new { ok = false, msg = "Sản phẩm " + item.MaSP + " không tồn tại!" });
                        }

                        if (sanPham.iSoLuongTon < item.SoLuong)
                        {
                            transaction.Rollback();
                            return Json(new { ok = false, msg = $"Sản phẩm [{sanPham.sTenSP}] không đủ tồn kho!" });
                        }

                        sanPham.iSoLuongTon -= item.SoLuong; // Trừ kho

                        var ct = new tbl_CTDonHang();
                        ct.PK_iMaDH_CTDonHang = dh.PK_iMaDH;
                        ct.PK_sMaSP_CTDonHang = item.MaSP;
                        ct.iSoLuongBan = item.SoLuong;
                        ct.fGiaBanLucDo = (float)item.DonGia;

                        db.tbl_CTDonHang.Add(ct);
                    }

                    db.SaveChanges();
                    transaction.Commit();
                    return Json(new { ok = true });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return Json(new { ok = false, msg = "Lỗi hệ thống: " + ex.Message });
                }
            }
        }

        // GET: tbl_DonHang/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            tbl_DonHang tbl_DonHang = db.tbl_DonHang.Find(id);
            if (tbl_DonHang == null) return HttpNotFound();

            ViewBag.FK_sMaKH_DonHang = new SelectList(db.tbl_KhachHang, "PK_sMaKH", "sTenKH", tbl_DonHang.FK_sMaKH_DonHang);
            ViewBag.FK_sMaNV_DonHang = new SelectList(db.tbl_NguoiDung, "PK_sMaNV", "sTenNV", tbl_DonHang.FK_sMaNV_DonHang);
            return View(tbl_DonHang);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PK_iMaDH,dNgayLap,FK_sMaKH_DonHang,FK_sMaNV_DonHang,fTongTien")] tbl_DonHang tbl_DonHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_DonHang).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Success"] = "Cập nhật đơn hàng thành công!";
                return RedirectToAction("Index");
            }
            ViewBag.FK_sMaKH_DonHang = new SelectList(db.tbl_KhachHang, "PK_sMaKH", "sTenKH", tbl_DonHang.FK_sMaKH_DonHang);
            ViewBag.FK_sMaNV_DonHang = new SelectList(db.tbl_NguoiDung, "PK_sMaNV", "sTenNV", tbl_DonHang.FK_sMaNV_DonHang);
            return View(tbl_DonHang);
        }

        // GET: tbl_DonHang/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            // Tìm đơn hàng và bao gồm khách hàng + chi tiết để hiển thị trang xác nhận
            tbl_DonHang tbl_DonHang = db.tbl_DonHang
                .Include(t => t.tbl_KhachHang)
                .Include(t => t.tbl_CTDonHang)
                .FirstOrDefault(m => m.PK_iMaDH == id);

            if (tbl_DonHang == null) return HttpNotFound();

            return View(tbl_DonHang);
        }

        // POST: tbl_DonHang/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    // Tải đơn hàng kèm chi tiết sản phẩm
                    tbl_DonHang tbl_DonHang = db.tbl_DonHang
                        .Include(d => d.tbl_CTDonHang)
                        .FirstOrDefault(d => d.PK_iMaDH == id);

                    if (tbl_DonHang == null)
                    {
                        TempData["Error"] = "Không tìm thấy đơn hàng cần xóa.";
                        return RedirectToAction("Index");
                    }

                    // 1. Hoàn tồn kho và Xóa chi tiết đơn hàng
                    if (tbl_DonHang.tbl_CTDonHang != null)
                    {
                        var chiTiets = tbl_DonHang.tbl_CTDonHang.ToList();
                        foreach (var item in chiTiets)
                        {
                            var sanPham = db.tbl_SanPham.Find(item.PK_sMaSP_CTDonHang);
                            if (sanPham != null)
                            {
                                sanPham.iSoLuongTon += item.iSoLuongBan; // Hoàn lại kho
                            }
                            db.tbl_CTDonHang.Remove(item); // Xóa dòng chi tiết
                        }
                    }

                    // 2. Xóa đơn hàng chính
                    db.tbl_DonHang.Remove(tbl_DonHang);

                    db.SaveChanges();
                    transaction.Commit();

                    TempData["Success"] = "Đã xóa đơn hàng và hoàn lại kho thành công!";
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    TempData["Error"] = "Lỗi hệ thống khi xóa: " + ex.Message;
                }
                return RedirectToAction("Index");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}