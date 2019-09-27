using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Areas.Admin.Controllers
{
    public class UpdateController : Controller
    {
        DBContext db = new DBContext();
        // GET: Admin/Update
        public ActionResult Index()
        {
            var list = db.Updates.ToList();
            ViewBag.Data = list;
            return View();
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();

        }
        [HttpPost]
        public ActionResult Store()
        {
            var versionName = Request["VersionName"];
            var versionCode = Convert.ToInt32(Request["VersionCode"]);
            var desc = Request["Desc"];
            var link = Request["Link"];
            Update u = new Update();
            u.Desc = desc;
            u.Link = link;
            u.VersionCode = versionCode;
            u.VersionName = versionName;
            db.Updates.Add(u);
            db.SaveChanges();
            return Redirect("/Admin/Update/Index");
        }
        [HttpGet]
        public ActionResult Delete()
        {
            var id = Convert.ToInt32(Request["Id"]);
            db.Updates.Remove(db.Updates.Find(id));
            db.SaveChanges();
            return Redirect("/Admin/Update");
        }
    }
}