using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class WholeSalerController : Controller
    {
        // GET: WholeSaler
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Store()
        {
            string Type = Request["Type"];
            string Fullname = Request["Fullname"];
            string Password = Request["Password"];
            string Email = Request["Email"];
            string Address = Request["Address"];
            string PhoneNumber = Request["PhoneNumber"];
            string Mobile = Request["Mobile"];
            string PostalCode = Request["PostalCode"];
            string Desc = Request["Desc"];

            int type = Convert.ToInt32(Type);
            if(type<1 || type>4)
            {
                ModelState.AddModelError("", "نوع کاربر را انتخاب کنید");
                return View("Index");
            }
            WholeSaler w = new Models.WholeSaler();
            w.Address = Address;
            w.Desc = Desc;
            w.Fullname = Fullname;
            w.Mobile = Mobile;
            w.Phone = PhoneNumber;
            w.PostalCode = PostalCode;
            w.Type = type;
            w.IsShow = false;
            DBContext db = new DBContext();
            db.WholeSalers.Add(w);
            db.SaveChanges();
            ViewBag.Message = "user";
            return View("Index");
        }
    }
}