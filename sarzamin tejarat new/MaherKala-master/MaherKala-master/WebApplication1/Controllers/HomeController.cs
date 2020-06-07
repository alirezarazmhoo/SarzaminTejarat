using DevOne.Security.Cryptography.BCrypt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Filter;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        DBContext db = new DBContext();
        // GET: Home
        public ActionResult Index()
        {
            var latest = db.Products.Where(p => p.Status == true).OrderByDescending(o => o.Id).Take(12).ToList();
            ViewBag.Latest = latest;

            var setting = db.Settings.FirstOrDefault();

            var first = db.Products.Where(p => p.Category.Id == setting.FirstCategory).Where(p => p.Status == true).Take(12).ToList();
            var secound= db.Products.Where(p => p.Category.Id == setting.SecoundCategory).Where(p => p.Status == true).Take(12).ToList();
            ViewBag.First = first;
            ViewBag.Secound= secound;

            if (setting.FirstCategory > 0) { 
            var data= db.Categories.FirstOrDefault(p=>p.Id==setting.FirstCategory);
                ViewBag.FirstTitle = data != null ? data.Name : "";
            }
            if (setting.SecoundCategory > 0) { 
                var data = db.Categories.FirstOrDefault(p =>p.Id == setting.SecoundCategory);
                ViewBag.SecoundTitle = data != null ? data.Name : "";
            }
            var now = DateTime.Now.Date;
            var special = db.SpecialProducts.Include("Product").Where(p => p.Product.Status == true && !p.Product.IsOnlyForMarketer).Where(p=>p.ExpireDate>=now).ToList();
            ViewBag.Special = special;

            var notification = setting.Notificaion;
            ViewBag.Notification = notification;
            ViewBag.NotificationUrl = setting.NotificaionUrl;
            return View();
            
        }
        public ActionResult AboutUs()
        {
            var s = db.Settings.FirstOrDefault().AboutUs;
            ViewBag.Data = s;
            return View();
        }
        public ActionResult CallUs()
        {
            var s = db.Settings.FirstOrDefault().CallUs;
            ViewBag.Data = s;
            return View();
        }
        public ActionResult HowToWork()
        {
            var s = db.Settings.FirstOrDefault().HowToWork;
            ViewBag.Data = s;
            return View();
        }
        [HttpPost]
        public ActionResult SendEmail()
        {
            var email = Request["Email"];
            var res = Regex.IsMatch(email, @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$");
            if(res)
            {
                if(db.Emails.Any(p=>p.Address==email))
                {
                    TempData["Message"] = -2;
                    return RedirectToAction("Index");
                }
                Email e = new Email();
                e.Address = email;
                db.Emails.Add(e);
                db.SaveChanges();
                TempData["Message"] = 1;
                return RedirectToAction("Index");
            }
            TempData["Message"] = -1;
            return RedirectToAction("Index");

        }

    }
}