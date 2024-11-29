using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        private QLBanXeGanMayEntities db = new QLBanXeGanMayEntities(); // DbContext được tạo tự động từ Entity Framework

        public ActionResult Index()
        {
            // Lấy toàn bộ danh sách xe
            var danhSachXe = db.XEGANMAYs.ToList();
            if (danhSachXe == null || !danhSachXe.Any())
            {
                return Content("Không có xe nào!");
            }

            // Lấy danh sách xe đặc biệt (ví dụ, lấy 8 xe đầu tiên)
            var featuredBikes = db.XEGANMAYs.Take(8).ToList();

            // Truyền dữ liệu vào View
            ViewBag.DanhSachXe = danhSachXe;
            ViewBag.FeaturedBikes = featuredBikes;

            return View();
        }
    }
}
