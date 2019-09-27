using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Areas.Admin.Controllers
{
    public class DiscountCodeController : Controller
    {
        DBContext db = new DBContext();
        // GET: Admin/Discount
        public ActionResult Index()
        {
            var data = db.DiscountCode.OrderByDescending(p => p.Id);
            ViewBag.Data = new PagedItem<DiscountCode>(data, "/Admin/DiscountCode/Index");
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Store()
        {
            if(Request["Code"]=="")
            {
                TempData["Error"] = "کد را وارد کنید";
                return Redirect("/Admin/DiscountCode/Create");
            }
            if (Request["Price"] == "")
            {
                TempData["Error"] = "مبلغ را وارد کنید";
                return Redirect("/Admin/DiscountCode/Create");
            }
            if (Request["ExpireDate"] == "")
            {
                TempData["Error"] = "تاریخ را وارد کنید";
                return Redirect("/Admin/DiscountCode/Create");
            }
            
           DiscountCode d = new Models.DiscountCode();
            d.Status = true;
            d.Code = Request["Code"];
            d.Price = Convert.ToInt32(Request["Price"]);
            string date = Request["ExpireDate"];

            string[] array = date.Split('/');
            PersianCalendar p = new PersianCalendar();
            d.ExpireDate = new DateTime(Convert.ToInt32(array[0]), Convert.ToInt32(array[1]), Convert.ToInt32(array[2]), p);
            db.DiscountCode.Add(d);
            db.SaveChanges();
            return Redirect("/Admin/DiscountCode/Index");
        }
        public ActionResult Status(int id)
        {
            var data = db.DiscountCode.Find(id);
            data.Status = !data.Status;
            db.SaveChanges();
            return Redirect("/Admin/DiscountCode/Index");
            
        }
    }
}

