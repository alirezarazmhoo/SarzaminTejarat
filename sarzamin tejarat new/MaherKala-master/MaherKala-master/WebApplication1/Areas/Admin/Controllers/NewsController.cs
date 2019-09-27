using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Areas.Admin.Controllers
{
    public class NewsController : Controller
    {
        DBContext db = new DBContext();
        [HttpGet]
        // GET: Admin/News
        [Route("Admin/News")]
        public ActionResult Index()
        {
            var News = db.News.OrderByDescending(p => p.Id);
            ViewBag.Data= new PagedItem<News>(News, "/Admin/News/Index");
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("Admin/News/Store")]
        [ValidateAntiForgeryToken]
        public ActionResult Store(News news)
        {
            if (news.Title == "" || news.Title == null)
            {
                TempData["Error"] = "عنوان را وارد کنید";
                return Redirect("/Admin/News/Create");
            }
            if (news.Desc==""|| news.Desc == null)
            {
                TempData["Error"] = "خلاصه را وارد کنید";
                return Redirect("/Admin/News/Create");
            }
            if (news.Text == "" || news.Text == null)
            {
                TempData["Error"] = "متن را وارد کنید";
                return Redirect("/Admin/News/Create");
            }
          
            db.News.Add(news);
            db.SaveChanges();
            return Redirect("/Admin/News/Index");
        }

        [HttpGet]
        [Route("Admin/News/Edit")]
        public ActionResult Edit(int id)
        {
            var model = db.News.Find(id);

            return View(model);
        }

        [HttpGet]
        [Route("Admin/News/Delete")]
        public ActionResult Delete(int id)
        {
            db.News.Remove(db.News.Find(id));
            db.SaveChanges();
            return Redirect("/Admin/News/Index");
        }

        [HttpPost]
        [Route("Admin/News/Update")]
        public ActionResult Update(News news)
        {
            db.Entry<News>(news).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return Redirect("/Admin/News/Index");
        }
    }
}