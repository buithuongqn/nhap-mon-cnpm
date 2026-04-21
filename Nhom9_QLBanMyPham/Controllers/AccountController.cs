using System.Linq;
using System.Web.Mvc;
using Nhom9_QLBanMyPham.Models;

namespace Nhom9_QLBanMyPham.Controllers
{
    public class AccountController : Controller
    {
        private QLSanPham db = new QLSanPham();

        // 1. Hiển thị trang đăng nhập
        public ActionResult Login()
        {
            return View();
        }

        // 2. Xử lý khi bấm nút Đăng nhập
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Tìm trong DB xem có user/pass này không
                var user = db.tbl_NguoiDung.FirstOrDefault(u => u.sTenNV == model.Username && u.sMatKhau == model.Password);

                if (user != null)
                {
                    // LƯU VÀO SESSION (Bộ nhớ tạm của server)
                    Session["MaNV"] = user.PK_sMaNV;
                    Session["TenNV"] = user.sTenNV;
                    Session["Quyen"] = user.sPhanQuyen;

                    // Đăng nhập đúng thì chuyển về trang danh sách đơn hàng
                    return RedirectToAction("Index", "tbl_DonHang");
                }
                else
                {
                    ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu sai!");
                }
            }
            return View(model);
        }

        // 3. Đăng xuất
        public ActionResult Logout()
        {
            Session.Clear(); // Xóa sạch phiên làm việc
            return RedirectToAction("Login");
        }
    }
}