using System.Web.Mvc;
using WebApplication2; // Đảm bảo import đúng namespace của model KHACHHANG
using System.Linq;
using System;

namespace WebApplication2.Controllers
{
    public class AccountController : Controller
    {
        private QLBanXeGanMayEntities db = new QLBanXeGanMayEntities(); // Đảm bảo ApplicationDbContext được cấu hình đúng

        // GET: Account/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(KHACHHANG khachHang)
        {
            var user = db.KHACHHANGs.FirstOrDefault(u => u.Taikhoan == khachHang.Taikhoan && u.Matkhau == khachHang.Matkhau);
            if (user != null)
            {
                // Lưu đối tượng khách hàng vào Session
                Session["User"] = user;
                return RedirectToAction("Index", "Cart");
            }
            else
            {
                ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không đúng!");
                return View(khachHang);
            }
        }


        // Đăng xuất
        public ActionResult Logout()
        {
            Session.Clear(); // Xóa tất cả thông tin trong session
            return RedirectToAction("Login", "Account"); // Chuyển hướng đến trang đăng nhập
        }

        // GET: Account/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(KHACHHANG khachHang)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem tài khoản đã tồn tại chưa
                var existingUser = db.KHACHHANGs.FirstOrDefault(u => u.Taikhoan == khachHang.Taikhoan);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Taikhoan", "Tài khoản đã tồn tại!");
                    return View(khachHang);
                }

                // Thêm khách hàng vào cơ sở dữ liệu
                khachHang.Ngaysinh = DateTime.Now; // Bạn có thể để lại hoặc bỏ qua tùy theo yêu cầu

                // Mã hóa mật khẩu (nếu cần thiết)
                // khachHang.Matkhau = HashPassword(khachHang.Matkhau);  // Sử dụng phương thức mã hóa mật khẩu nếu cần

                db.KHACHHANGs.Add(khachHang);
                db.SaveChanges(); // Lưu vào cơ sở dữ liệu

                // Chuyển hướng đến trang đăng nhập hoặc trang thành công
                return RedirectToAction("Login", "Account");
            }

            // Nếu không hợp lệ, trả về view để người dùng sửa lỗi
            return View(khachHang);
        }
    }
}