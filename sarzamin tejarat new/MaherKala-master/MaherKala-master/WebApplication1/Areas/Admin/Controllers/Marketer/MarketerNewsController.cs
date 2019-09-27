using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Areas.Admin.Controllers.Marketer
{
    public class MarketerNewsController : Controller
    {
        DBContext db = new DBContext();
        [HttpGet]
        // GET: Admin/News
        [Route("Admin/MarketerNews")]
        public ActionResult Index()
        {
            var News = db.MarketerNews.OrderByDescending(p => p.Id);
            ViewBag.Data = new PagedItem<MarketerNews>(News, "/Admin/MarketerNews/Index");
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("Admin/MarketerNews/Store")]
        [ValidateAntiForgeryToken]
        public ActionResult Store(MarketerNews news)
        {
            if (news.Title == "" || news.Title == null)
            {
                TempData["Error"] = "عنوان را وارد کنید";
                return Redirect("/Admin/News/Create");
            }
            
            if (news.Text == "" || news.Text == null)
            {
                TempData["Error"] = "متن را وارد کنید";
                return Redirect("/Admin/News/Create");
            }
            var img = Request.Files["Main_Image"];
            if (img == null || img.FileName == "")
            {
                TempData["Error"] = "تصویر را انتخاب کنید";
                return Redirect("/Admin/News/Create");

            }
            if (!(img.ContentType == "image/jpeg" || img.ContentType == "image/png" || img.ContentType == "image/bmp"))
            {
                TempData["Error"] = "نوع تصویر غیر قابل قبول است";
                return Redirect("/Admin/News/Create");
            }
            var name = Guid.NewGuid().ToString().Replace('-', '0') + "." + img.FileName.Split('.')[1];

            var imageUrl = "/Upload/MarketerNews/" + name;
            string path = Server.MapPath(imageUrl);
            img.SaveAs(path);
            news.PicAddress= imageUrl;

            db.MarketerNews.Add(news);
            db.SaveChanges();
            return Redirect("/Admin/MarketerNews/Index");
        }

        [HttpGet]
        [Route("Admin/MarketerNews/Edit")]
        public ActionResult Edit(int id)
        {
            var model = db.MarketerNews.Find(id);
            return View(model);
        }

        [HttpGet]
        [Route("Admin/MarketerNews/Delete")]
        public ActionResult Delete(int id)
        {
            db.MarketerNews.Remove(db.MarketerNews.Find(id));
            db.SaveChanges();
            return Redirect("/Admin/MarketerNews/Index");
        }

        [HttpPost]
        [Route("Admin/MarketerNews/Update")]
        public ActionResult Update(MarketerNews news)
        {
            
            var img = Request.Files["Main_Image"];
            var id = Convert.ToInt32(Request["Id"]);
            var update = db.MarketerNews.Find(id);

            if (img!=null && img.ContentLength > 0)
            {

                System.IO.File.Delete(Server.MapPath(update.PicAddress));

                if (!(img.ContentType == "image/jpeg" || img.ContentType == "image/png" || img.ContentType == "image/bmp"))
                    throw new DbEntityValidationException("نوع فایل غیر قابل قبول است");
                var name = Guid.NewGuid().ToString().Replace('-', '0') + "." + img.FileName.Split('.')[1];

                var imageUrl = "/Upload/MarketerNews/" + name;
                string path = Server.MapPath(imageUrl);
                img.SaveAs(path);
                update.PicAddress = imageUrl;

            }
            update.Text = news.Text;
            update.Title = news.Title;
                
            db.SaveChanges();
            return Redirect("/Admin/MarketerNews/Index");
        }
    }
}