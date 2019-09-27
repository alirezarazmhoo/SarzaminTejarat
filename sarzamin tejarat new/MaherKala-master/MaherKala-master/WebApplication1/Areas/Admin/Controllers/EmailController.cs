using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.Utility;

namespace WebApplication1.Areas.Admin.Controllers
{
    public class EmailController : Controller
    {
        DBContext db = new DBContext();
        // GET: Admin/Email
        [HttpGet]
        public ActionResult Create()
        {
            var data = db.Emails.OrderByDescending(p => p.Id);
            var paginate = new PagedItem<Email>(data, "/Admin/Email/Create");
            ViewBag.Data = paginate;
            return View();
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            db.Emails.Remove(db.Emails.Find(id));
            db.SaveChanges();
            return RedirectToAction("Create");

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Store(News n)
        {
            var title = n.Title;
            var text = n.Text;
            if(title==null || title.Trim()=="")
            {
                TempData["Error"] = "عنوان ایمیل راوارد کنید";
                return Redirect("/Admin/Email/Create");

            }
            if (text== null || text.Trim() == "")
            {
                TempData["Error"] = "متن ایمیل راوارد کنید";
                return Redirect("/Admin/Email/Create");


            }
            var ss = db.Settings.First();
            SendEmail s = new SendEmail(ss);
            var list = db.Emails.ToList();
            var temp = new List<string>();
            foreach (var item in list)
            {
                temp.Add(item.Address);
            }
            try
            {

                s.Send(text.Trim(), title.Trim(), temp);
            }
            catch
            {

            }
            TempData["Message"] = "با موفقیت ارسال شد"; 
            return Redirect("/Admin/Email/Create");
        }
    }
}