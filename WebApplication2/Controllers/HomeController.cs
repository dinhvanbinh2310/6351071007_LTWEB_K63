using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        private QLBanXeGanMayEntities db = new QLBanXeGanMayEntities(); // DbContext được tạo tự động từ Entity Framework

        public ActionResult Index()
        {
            var danhSachXe = db.XEGANMAYs.ToList(); // Corrected property name
            if (danhSachXe == null || !danhSachXe.Any())
            {
                return Content("Không có xe nào!");
            }
            return View(danhSachXe);
        }
    }
}
