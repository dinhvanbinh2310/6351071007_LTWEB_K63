using System.Collections.Generic;
using System.Web.Mvc;
using WebApplication2;
using System.Linq;
using System;
using System.Data.Entity;


public class CartController : Controller
{
    private QLBanXeGanMayEntities db = new QLBanXeGanMayEntities();

    // Lấy giỏ hàng từ Session
    private List<XEGANMAY> GetCart()
    {
        var cart = Session["Cart"] as List<XEGANMAY>;
        if (cart == null)
        {
            cart = new List<XEGANMAY>();
            Session["Cart"] = cart;
        }
        return cart;
    }

    public ActionResult Index()
    {
        // Kiểm tra nếu người dùng chưa đăng nhập
        var khachHang = Session["User"] as KHACHHANG;

        var donDatHang = db.DONDATHANGs
            .Where(d => d.MaKH == khachHang.MaKH)
            .Include(d => d.CHITIETDONTHANGs.Select(c => c.XEGANMAY)) 
            .ToList();

        return View(donDatHang);
    }




    public ActionResult AddToCart(int MaXe)
    {
        // Kiểm tra xem người dùng đã đăng nhập hay chưa
        var khachHang = Session["User"] as KHACHHANG;
        // Tìm xe trong cơ sở dữ liệu
        XEGANMAY xe = db.XEGANMAYs.FirstOrDefault(x => x.MaXe == MaXe);
        if (xe != null)
        {
            // Tạo mới đơn đặt hàng
            DONDATHANG donDatHang = new DONDATHANG
            {
                Dathanhtoan = false,
                Tinhtranggiaohang = false,
                Ngaydat = DateTime.Now,
                Ngaygiao = DateTime.Now.AddDays(5), // Giả sử giao hàng sau 5 ngày
                MaKH = khachHang.MaKH
            };

            // Lưu đơn đặt hàng vào database
            db.DONDATHANGs.Add(donDatHang);
            db.SaveChanges();
        }

        return RedirectToAction("OrderSuccess");
    }



    [HttpPost]
    public ActionResult RemoveFromCart(int MaXe)
    {
        List<XEGANMAY> cart = Session["Cart"] as List<XEGANMAY>;
        if (cart != null)
        {
            var itemToRemove = cart.FirstOrDefault(x => x.MaXe == MaXe);
            if (itemToRemove != null)
            {
                cart.Remove(itemToRemove);
            }
            Session["Cart"] = cart;
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult Checkout()
    {
        var cart = GetCart();
        if (cart.Any())
        {
            var donDatHang = new DONDATHANG
            {
                Ngaydat = DateTime.Now,
                Dathanhtoan = false,
                Tinhtranggiaohang = false,
                MaKH = 1 // Bạn cần sửa lại để lấy ID khách hàng động
            };

            db.DONDATHANGs.Add(donDatHang);
            db.SaveChanges();

            foreach (var xe in cart)
            {
                var chiTiet = new CHITIETDONTHANG
                {
                    MaDonHang = donDatHang.MaDonHang,
                    MaXe = xe.MaXe,
                    Soluong = 1, // Số lượng có thể tùy chỉnh
                    Dongia = xe.Giaban
                };
                db.CHITIETDONTHANGs.Add(chiTiet);
            }
            db.SaveChanges();

            Session["Cart"] = null; // Xóa giỏ hàng sau khi hoàn tất
            return RedirectToAction("OrderSuccess");
        }

        return RedirectToAction("Index");
    }

    public ActionResult OrderSuccess()
    {
        return View();
    }
}
