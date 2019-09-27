using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Areas.Admin.Controllers
{
    public class WholeSalersController : Controller
    {
        DBContext db = new DBContext();
        // GET: Admin/WholeSaler
        public ActionResult Index()
        {
            var data= db.WholeSalers.OrderByDescending(p => p.Id).OrderBy(p => p.IsShow);
            var paginate = new PagedItem<WholeSaler>(data,"/Admin/WholeSaler/Index");
            ViewBag.Data = paginate;
            return View();
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            db.WholeSalers.Remove(db.WholeSalers.Find(id));
            db.SaveChanges();
            return RedirectToAction("Index");

        }
        [HttpGet]
        public ActionResult Detail(int id)
        {
            var data = db.WholeSalers.Find(id);
            data.IsShow = true;
            db.SaveChanges();
            ViewBag.Data = data;
            return View();
        }
    }
}