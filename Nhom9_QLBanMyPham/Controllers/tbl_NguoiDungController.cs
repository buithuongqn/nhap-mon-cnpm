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
    public class tbl_NguoiDungController : Controller
    {
        private QLSanPham db = new QLSanPham();

        // 1. Hàm kiểm tra quyền Admin nhanh (để dùng lại nhiều lần)
        private bool IsAdmin()
        {
            // 1. Nếu Session trống hoặc chưa đăng nhập -> Không phải Admin
            if (Session["Quyen"] == null)
            {
                return false;
            }

            string quyen = Session["Quyen"].ToString().Trim();

            // 2. So sánh không phân biệt chữ hoa chữ thường (Admin, admin, ADMIN đều được)
            return string.Equals(quyen, "Admin", StringComparison.OrdinalIgnoreCase);
        }
        // GET: tbl_NguoiDung
        public ActionResult Index()
        {
            if (!IsAdmin())
            {
                TempData["Error"] = "Bạn không có quyền truy cập vào danh sách người dùng!";
                return RedirectToAction("Index", "tbl_NguoiDung");
            }
            return View(db.tbl_NguoiDung.ToList());
        }

        // GET: tbl_NguoiDung/Details/5
        public ActionResult Details(string id)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "tbl_DonHang");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_NguoiDung tbl_NguoiDung = db.tbl_NguoiDung.Find(id);
            if (tbl_NguoiDung == null)
            {
                return HttpNotFound();
            }
            return View(tbl_NguoiDung);
        }

        // GET: tbl_NguoiDung/Create
        public ActionResult Create()
        {
            if (!IsAdmin()) return RedirectToAction("Index", "tbl_DonHang");
            return View();
        }

        // POST: tbl_NguoiDung/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PK_sMaNV,sTenNV,sSDT,sVaiTro,sMatKhau,sPhanQuyen,fLuong")] tbl_NguoiDung tbl_NguoiDung)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "tbl_DonHang");

            if (ModelState.IsValid)
            {
                try
                {
                    db.tbl_NguoiDung.Add(tbl_NguoiDung);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Mã nhân viên này đã tồn tại. Vui lòng kiểm tra lại!");
                }
            }
            return View(tbl_NguoiDung);
        }

        // GET: tbl_NguoiDung/Edit/5
        public ActionResult Edit(string id)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "tbl_DonHang");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_NguoiDung tbl_NguoiDung = db.tbl_NguoiDung.Find(id);
            if (tbl_NguoiDung == null)
            {
                return HttpNotFound();
            }
            return View(tbl_NguoiDung);
        }

        // POST: tbl_NguoiDung/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PK_sMaNV,sTenNV,sSDT,sVaiTro,sMatKhau,sPhanQuyen,fLuong")] tbl_NguoiDung tbl_NguoiDung)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "tbl_DonHang");

            if (ModelState.IsValid)
            {
                db.Entry(tbl_NguoiDung).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_NguoiDung);
        }

        // GET: tbl_NguoiDung/Delete/5
        public ActionResult Delete(string id)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "tbl_DonHang");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_NguoiDung tbl_NguoiDung = db.tbl_NguoiDung.Find(id);
            if (tbl_NguoiDung == null)
            {
                return HttpNotFound();
            }
            return View(tbl_NguoiDung);
        }

        // POST: tbl_NguoiDung/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "tbl_DonHang");

            tbl_NguoiDung tbl_NguoiDung = db.tbl_NguoiDung.Find(id);
            db.tbl_NguoiDung.Remove(tbl_NguoiDung);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}