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
    public class tbl_SanPhamController : Controller
    {
        private QLSanPham db = new QLSanPham();

        // GET: tbl_SanPham
        public ActionResult Index(string searchString, string maDM)
        {
            ViewBag.maDM = new SelectList(db.tbl_DanhMuc, "PK_sMaDM", "sTenDM");
            var sanPhams = db.tbl_SanPham.Include(s => s.tbl_DanhMuc).Include(s => s.tbl_NhaCungCap);

            if (!String.IsNullOrEmpty(searchString))
            {
                sanPhams = sanPhams.Where(s => s.sTenSP.Contains(searchString) || s.PK_sMaSP.Contains(searchString));
            }

            if (!String.IsNullOrEmpty(maDM))
            {
                sanPhams = sanPhams.Where(s => s.FK_sMaDM_SanPham == maDM);
            }

            var listForStats = sanPhams.ToList();
            ViewBag.TongSP = listForStats.Count();
            ViewBag.GiaTriKho = listForStats.Sum(s => (double?)(s.fGiaBan * s.iSoLuongTon)) ?? 0;
            ViewBag.SapHetHang = listForStats.Count(s => s.iSoLuongTon > 0 && s.iSoLuongTon <= 10);
            ViewBag.HetHang = listForStats.Count(s => s.iSoLuongTon == 0);

            return View(listForStats);
        }

        [HttpGet]
        public JsonResult SearchProduct(string name)
        {
            var data = db.tbl_SanPham
                .Where(s => s.sTenSP.Contains(name) && s.iSoLuongTon > 0)
                .Select(s => new {
                    MaSP = s.PK_sMaSP,
                    TenSP = s.sTenSP,
                    Gia = s.fGiaBan
                })
                .Take(10)
                .ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Details(string id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            tbl_SanPham tbl_SanPham = db.tbl_SanPham.Find(id);
            if (tbl_SanPham == null) return HttpNotFound();
            return View(tbl_SanPham);
        }

        // GET: tbl_SanPham/Create
        public ActionResult Create()
        {
            ViewBag.FK_sMaDM_SanPham = new SelectList(db.tbl_DanhMuc, "PK_sMaDM", "sTenDM");
            ViewBag.FK_sMaNCC_SanPham = new SelectList(db.tbl_NhaCungCap, "PK_sMaNCC", "sTenNCC");
            return View();
        }

        // POST: tbl_SanPham/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        // 1. Thêm tham số HttpPostedFileBase uploadAnh
        public ActionResult Create([Bind(Include = "PK_sMaSP,sTenSP,sMoTa,sDungTich,dNgayHetHan,fGiaBan,iSoLuongTon,FK_sMaNCC_SanPham,sMauSac,sHinhAnh,FK_sMaDM_SanPham")] tbl_SanPham tbl_SanPham, HttpPostedFileBase uploadAnh)
        {
            if (ModelState.IsValid)
            {
                // --- PHẦN XỬ LÝ FILE ẢNH MỚI THÊM ---
                if (uploadAnh != null && uploadAnh.ContentLength > 0)
                {
                    string fileName = System.IO.Path.GetFileName(uploadAnh.FileName);
                    string path = System.IO.Path.Combine(Server.MapPath("~/Content/Images/"), fileName);
                    uploadAnh.SaveAs(path);
                    tbl_SanPham.sHinhAnh = fileName; // Lưu tên file vào model
                }
                // ------------------------------------

                var checkMa = db.tbl_SanPham.Find(tbl_SanPham.PK_sMaSP);
                if (checkMa != null)
                {
                    ModelState.AddModelError("PK_sMaSP", "Bạn không được quyền thêm trùng mã sản phẩm!");
                }
                else
                {
                    db.tbl_SanPham.Add(tbl_SanPham);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            ViewBag.FK_sMaDM_SanPham = new SelectList(db.tbl_DanhMuc, "PK_sMaDM", "sTenDM", tbl_SanPham.FK_sMaDM_SanPham);
            ViewBag.FK_sMaNCC_SanPham = new SelectList(db.tbl_NhaCungCap, "PK_sMaNCC", "sTenNCC", tbl_SanPham.FK_sMaNCC_SanPham);
            return View(tbl_SanPham);
        }

        // GET: tbl_SanPham/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            tbl_SanPham tbl_SanPham = db.tbl_SanPham.Find(id);
            if (tbl_SanPham == null) return HttpNotFound();
            ViewBag.FK_sMaDM_SanPham = new SelectList(db.tbl_DanhMuc, "PK_sMaDM", "sTenDM", tbl_SanPham.FK_sMaDM_SanPham);
            ViewBag.FK_sMaNCC_SanPham = new SelectList(db.tbl_NhaCungCap, "PK_sMaNCC", "sTenNCC", tbl_SanPham.FK_sMaNCC_SanPham);
            return View(tbl_SanPham);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PK_sMaSP,sTenSP,sMoTa,sDungTich,dNgayHetHan,fGiaBan,iSoLuongTon,FK_sMaNCC_SanPham,sMauSac,sHinhAnh,FK_sMaDM_SanPham")] tbl_SanPham tbl_SanPham, HttpPostedFileBase uploadAnh)
        {
            if (ModelState.IsValid)
            {
                if (uploadAnh != null && uploadAnh.ContentLength > 0)
                {
                    string fileName = System.IO.Path.GetFileName(uploadAnh.FileName);
                    string path = System.IO.Path.Combine(Server.MapPath("~/Content/Images/"), fileName);
                    uploadAnh.SaveAs(path);
                    tbl_SanPham.sHinhAnh = fileName;
                }

                db.Entry(tbl_SanPham).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FK_sMaDM_SanPham = new SelectList(db.tbl_DanhMuc, "PK_sMaDM", "sTenDM", tbl_SanPham.FK_sMaDM_SanPham);
            ViewBag.FK_sMaNCC_SanPham = new SelectList(db.tbl_NhaCungCap, "PK_sMaNCC", "sTenNCC", tbl_SanPham.FK_sMaNCC_SanPham);
            return View(tbl_SanPham);
        }

        public ActionResult Delete(string id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            tbl_SanPham tbl_SanPham = db.tbl_SanPham.Find(id);
            if (tbl_SanPham == null) return HttpNotFound();
            return View(tbl_SanPham);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            tbl_SanPham tbl_SanPham = db.tbl_SanPham.Find(id);
            db.tbl_SanPham.Remove(tbl_SanPham);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}