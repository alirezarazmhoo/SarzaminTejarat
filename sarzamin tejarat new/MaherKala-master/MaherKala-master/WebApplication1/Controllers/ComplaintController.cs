using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ComplaintController : Controller
    {
        // GET: Complaint
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Store(Complaint c)
        {
            DBContext db = new Models.DBContext();
            if(ModelState.IsValid)
            {
                c.IsAdminShow = false;
                db.Complaints.Add(c);
                db.SaveChanges();
                ViewBag.Message = "پیام شما با موفقیت دریافت شد و بزودی توسط مدیران سایت بررسی خواهد شد";
            }
            return View("Create");

        }
    }
}