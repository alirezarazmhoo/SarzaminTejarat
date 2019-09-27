using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using WebApplication1.Models;

namespace WebApplication1.Areas.Admin.Controllers
{
    public class SettingController : Controller
    {
        DBContext db = new DBContext();
        // GET: Admin/Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AboutUs()
        {
            var s = db.Settings.First();
            return View(s);
        }
        public ActionResult CallUs()
        {
            var s = db.Settings.First();
            return View(s);
        }
         

        public ActionResult HowToWork()
        {
            var s = db.Settings.First();
            return View(s);
        }
        public ActionResult Email()
        {
            var s = db.Settings.First();
            return View(s);
        }
        public ActionResult EmailStore()
        {
            var Email= Request["Email"];
            var HostName = Request["HostName"];
            var EmailPassword = Request["EmailPassword"];
            var s = db.Settings.First();
            s.Email = Email;
            s.EmailPassword= EmailPassword;
            s.HostName= HostName;
            db.SaveChanges();
            return RedirectToAction("Email");
        }
        [HttpPost]
        [Route("/Admin/Home/StoreSetting")]
        public ActionResult StoreSetting(Setting s)
        {
            string url = "/Admin/Setting/";
            var data = db.Settings.First();
            if (Request["AboutUsFlag"] == "1")
            {
                data.AboutUs = s.AboutUs;
                url += "AboutUs";
            }
            
           if (Request["CallUsFlag"] == "1")
            {
                data.CallUs = s.CallUs;

                url += "CallUs";
            }

            if (Request["HowtoWorkFlag"] == "1")
            {
                data.HowToWork = s.HowToWork;

                url += "HowtoWork";
            }


            db.SaveChanges();
            return Redirect(url);
        }

        [HttpPost]
        public ActionResult UploadEditor()
        {
            var img = Request.Files[0];
            var name = Guid.NewGuid().ToString().Replace('-', '0') + "." + img.FileName.Split('.')[1];
            var imageUrl = "/Upload/Editor/" + name;
            string path = Server.MapPath(imageUrl);
            img.SaveAs(path);
            
            string s = "<script type='text/javascript'>window.parent.CKEDITOR.tools.callFunction(1, '{" + imageUrl + "}', '')</script>";
           
            return JavaScript("<script>alert('sss')</script>");
            

        }
    }
}