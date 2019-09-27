using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Areas.Admin.Controllers
{
    public class FirstSecoundCategoryController : Controller
    {
        DBContext db = new DBContext();

        // GET: Admin/FirstSecoundCategory
        public ActionResult Index()
        {
            var data = db.Database.SqlQuery<Category>("select * from Categories c where Id Not In (select Parent_Id from Categories where Parent_Id Is Not Null) and  c.Parent_Id Is Not null");
            ViewBag.Categories = data;
            ViewBag.Setting = db.Settings.First();
            return View();
        }

        [HttpPost]
        public ActionResult Store()
        {
            int f = Convert.ToInt32(Request["FirstCategory"]);
            int s = Convert.ToInt32(Request["SecoundCategory"]);
            var setting = db.Settings.First();
            setting.FirstCategory = f;
            setting.SecoundCategory = s;
            db.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}