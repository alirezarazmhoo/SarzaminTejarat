using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Areas.Admin.Controllers.Marketer
{
    public class MarketerPrizesController : Controller
    {
        DBContext db = new DBContext();
        [HttpGet]
        [Route("Admin/MarketerPrizes")]
        public ActionResult Index()
        {
            var p = db.MarketerPrizes.OrderByDescending(x => x.Id);
            ViewBag.Data = new PagedItem<MarketerPrize>(p, "/Admin/MarketerPrizes/Index");
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("Admin/MarketerPrizes/Store")]
        [ValidateAntiForgeryToken]
        public ActionResult Store(MarketerPrize  prize)
        {
            if (prize.Title == "" || prize.Title == null)
            {
                TempData["Error"] = "عنوان را وارد کنید";
                return Redirect("/Admin/MarketerPrizes/Create");
            }

            if (prize.Text == "" || prize.Text == null)
            {
                TempData["Error"] = "متن را وارد کنید";
                return Redirect("/Admin/MarketerPrizes/Create");
            }
            var img = Request.Files["Main_Image"];
            if (img == null || img.FileName == "")
            {
                TempData["Error"] = "تصویر را انتخاب کنید";
                return Redirect("/Admin/MarketerPrizes/Create");

            }
            if (!(img.ContentType == "image/jpeg" || img.ContentType == "image/png" || img.ContentType == "image/bmp"))
            {
                TempData["Error"] = "نوع تصویر غیر قابل قبول است";
                return Redirect("/Admin/MarketerPrizes/Create");
            }
            var name = Guid.NewGuid().ToString().Replace('-', '0') + "." + img.FileName.Split('.')[1];

            var imageUrl = "/Upload/MarketerPrizes/" + name;
            string path = Server.MapPath(imageUrl);
            img.SaveAs(path);
            prize.PicAddress = imageUrl;

            db.MarketerPrizes.Add(prize);
            db.SaveChanges();
            return Redirect("/Admin/MarketerPrizes/Index");
        }

        [HttpGet]
        [Route("Admin/MarketerPrizes/Edit")]
        public ActionResult Edit(int id)
        {
            var model = db.MarketerPrizes.Find(id);
            return View(model);
        }

        [HttpGet]
        [Route("Admin/MarketerPrizes/Delete")]
        public ActionResult Delete(int id)
        {
            db.MarketerPrizes.Remove(db.MarketerPrizes.Find(id));
            db.SaveChanges();
            return Redirect("/Admin/MarketerPrizes/Index");
        }

        [HttpPost]
        [Route("Admin/MarketerPrizes/Update")]
        public ActionResult Update(MarketerPrize prize)
        {

            var img = Request.Files["Main_Image"];
            var id = Convert.ToInt32(Request["Id"]);
            var update = db.MarketerPrizes.Find(id);

            if (img != null && img.ContentLength > 0)
            {

                System.IO.File.Delete(Server.MapPath(update.PicAddress));

                if (!(img.ContentType == "image/jpeg" || img.ContentType == "image/png" || img.ContentType == "image/bmp"))
                    throw new DbEntityValidationException("نوع فایل غیر قابل قبول است");
                var name = Guid.NewGuid().ToString().Replace('-', '0') + "." + img.FileName.Split('.')[1];

                var imageUrl = "/Upload/MarketerPrizes/" + name;
                string path = Server.MapPath(imageUrl);
                img.SaveAs(path);
                update.PicAddress = imageUrl;

            }
            update.Text = prize.Text;
            update.Title = prize.Title;

            db.SaveChanges();
            return Redirect("/Admin/MarketerPrizes/Index");
        }
    }
}
