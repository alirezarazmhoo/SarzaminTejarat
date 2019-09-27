using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ShowNewsController : Controller
    {
        DBContext db = new DBContext();
        // GET: News
        public ActionResult Index()
        {
            var data =  db.News.OrderByDescending(p=>p.Id);
            var paged = new PagedItem<object>(data, "/ShowNews/Index",10);
            ViewBag.Data = paged;
            return View();
        }
        public ActionResult Detail()
        {
            var id = Convert.ToInt32(Request["Id"]);
            ViewBag.Data = db.News.Where(p => p.Id==id).Select(p => new { p.Text }).FirstOrDefault().Text;
            return View();

        }
    }
}