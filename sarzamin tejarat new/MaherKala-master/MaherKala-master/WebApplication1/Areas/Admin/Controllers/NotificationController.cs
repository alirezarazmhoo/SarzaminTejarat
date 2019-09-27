using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Areas.Admin.Controllers
{
    public class NotificationController : Controller
    {
        DBContext db = new DBContext();

        // GET: Admin/Notofication
        public ActionResult Create()
        {
            var data = db.Settings.FirstOrDefault().Notificaion;


            ViewBag.Notificaion = data;
            return View();
        }
        [HttpPost]
        public ActionResult Store()
        {
            var file = Request.Files[0];
            var link = Request["Link"];
            var name = Guid.NewGuid().ToString().Replace('-', '0') + "." + file.FileName.Split('.')[1];
            var Url = "/Upload/Notification/" + name;
            string path = Server.MapPath(Url);
            file.SaveAs(path);
            var data = db.Settings.FirstOrDefault();
            data.Notificaion = Url;
            data.NotificaionUrl = link;
            db.SaveChanges();
            return RedirectToAction("Create");
        }
        public ActionResult DeleteNotification()
        {
            var data = db.Settings.FirstOrDefault();
            if(data.Notificaion!="")
            {
                try
                {
                    System.IO.File.Delete(Server.MapPath(data.Notificaion));
                }
                catch { }
            }
            data.Notificaion = "";
            data.NotificaionUrl = "";
            db.SaveChanges();
            return RedirectToAction("Create");

        }
    }
}