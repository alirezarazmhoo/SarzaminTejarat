using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Areas.Admin.Controllers
{
    public class AndroidAppController : Controller
    {
        DBContext db = new DBContext();

        // GET: Admin/AndroidApp
        public ActionResult Index()
        {
            return View();
        }
          public ActionResult AndroidApp(string Version)
        {
            var a = db.AndroidApps.FirstOrDefault();
            ViewBag.Version = Version;
            return View(a);
        }
        [HttpPost]
        
        

        public ActionResult StoreAndroidApp(AndroidApp page)
        {
            var AndroidApp = new AndroidApp();
            if (page.ID == 0 || page.ID == 0)
            {
                String FileExt = Path.GetExtension(page.files.FileName).ToUpper();
                if (FileExt == ".APK")
                {
                    Stream str = page.files.InputStream;
                    BinaryReader Br = new BinaryReader(str);
                    Byte[] FileDet = Br.ReadBytes((Int32)str.Length);
                    AndroidApp.Version = page.Version;
                    var name = page.files.FileName;
                    var Url = "/Upload/AndroidApps/" + name;
                    string path = Server.MapPath(Url);
                    page.files.SaveAs(path);
                    AndroidApp.AppAndroidUrl = Url;
                    db.AndroidApps.Add(AndroidApp);
                }
                else
                {
                    return Json( new { Data = "Error"}, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                AndroidApp = db.AndroidApps.Find(page.ID);
                AndroidApp.Version = page.Version;
                try
                {
                    System.IO.File.Delete(Server.MapPath(AndroidApp.AppAndroidUrl));
                }
                catch (Exception)
                {
                   return Json( new { Data = "Error",}, JsonRequestBehavior.AllowGet);

                }
                String FileExt = Path.GetExtension(page.files.FileName).ToUpper();
                if (FileExt == ".APK")
                {
                    Stream str = page.files.InputStream;
                    BinaryReader Br = new BinaryReader(str);
                    Byte[] FileDet = Br.ReadBytes((Int32)str.Length);
                    var name = page.files.FileName;
                    var Url = "/Upload/AndroidApps/" + name;
                    string path = Server.MapPath(Url);
                    page.files.SaveAs(path);
                    AndroidApp.AppAndroidUrl = Url;

                }
                else
                {
                    return Json( new { Data = "Error"}, JsonRequestBehavior.AllowGet);

                }
            }
            db.SaveChanges();
            return RedirectToAction("AndroidApp", new {@Version=page.Version});

        }
    }
}