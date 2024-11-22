using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication2.Controllers
{
    public class textController : Controller
    {
        private QLBanXeGanMayEntities db = new QLBanXeGanMayEntities(); // DbContext được tạo tự động từ Entity Framework

        public ActionResult Index()
        {
            // Truy vấn danh sách tên xe
            var tenXeList = db.XEGANMAYs.Select(x => x.TenXe).ToList();

            // Truyền danh sách tên xe sang View
            ViewBag.TenXeList = tenXeList;

            return View();
        }
    }
}