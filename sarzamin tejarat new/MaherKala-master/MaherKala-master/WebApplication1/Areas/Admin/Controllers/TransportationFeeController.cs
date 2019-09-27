using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Areas.Admin.Controllers
{
    public class TransportationFeeController : Controller
    {
        DBContext db = new DBContext();
        // GET: Admin/TransportationFee
        public ActionResult Index()
        {
            Setting s = db.Settings.FirstOrDefault();
            if (s == null)
            {
                var ss = new Setting();
                ss.AboutUs = "";
                ss.CallUs = "";
                ss.HowToWork = "";
                db.Settings.Add(ss);
                db.SaveChanges();
                s = ss;
            }
            ViewBag.Setting = s;
            return View();
        }

        [HttpPost]
        public ActionResult Store()
        {
            int esfahan = Convert.ToInt32(Request["Esfahan"]);
            int najafabad = Convert.ToInt32(Request["Najafabad"]);
            int other = Convert.ToInt32(Request["Other"]);
            Setting s = db.Settings.FirstOrDefault();
            bool flag = false;
            if (s == null)
            {
                s = new Setting();
                flag = true;
            }
            s.TransportationEsfahan = esfahan;
            s.TransportationNajafabad = najafabad;
            s.TransportationOther = other;
            if (flag)
            {
                db.Settings.Add(s);
            }
            db.SaveChanges();

            return Redirect("/Admin/TransportationFee");

        }
    }
}