using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Areas.Admin.Controllers
{
    public class MarketerSliderController : Controller
    {
        DBContext db = new DBContext();
        // GET: Admin/Slider
        public ActionResult Index()
        {
            ViewBag.Data = db.MarketerSliders.OrderByDescending(p => p.Id).ToList();
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Store()
        {
            if (Request.Files["Image"].ContentLength == 0)
            {
                ViewData["EmptyFile"] = "فایلی انتخاب نشده";

                return Redirect("/Admin/MarketerSlider/Create");


            }
            MarketerSlider s = new MarketerSlider();
            var img = Request.Files["Image"];

            if (string.Equals(img.ContentType, "image/jpg", StringComparison.OrdinalIgnoreCase) ||
         string.Equals(img.ContentType, "image/jpeg", StringComparison.OrdinalIgnoreCase) ||
         string.Equals(img.ContentType, "image/pjpeg", StringComparison.OrdinalIgnoreCase) ||
         string.Equals(img.ContentType, "image/gif", StringComparison.OrdinalIgnoreCase) ||
         string.Equals(img.ContentType, "image/x-png", StringComparison.OrdinalIgnoreCase) ||
         string.Equals(img.ContentType, "image/png", StringComparison.OrdinalIgnoreCase))
            {




                var name = Guid.NewGuid().ToString().Replace('-', '0') + "." + img.FileName.Split('.')[1];

                var imageUrl = "/Upload/MarketerSliders/" + name;

                string path = Server.MapPath(imageUrl);



                img.SaveAs(path);
                s.Image = imageUrl;
                db.MarketerSliders.Add(s);
                s.IsVideo = false;
                db.SaveChanges();
            }


            else
            {
                int MaxContentLength = 1024 * 1024 * 10;

                if(img.ContentLength > MaxContentLength)
                {
                    ViewData["BigLenght"] = "حجم بیش از حد مجاز است";
                    return Redirect("/Admin/MarketerSlider/Create");

                }


                var name = Guid.NewGuid().ToString().Replace('-', '0') + "." + img.FileName.Split('.')[1];

                var imageUrl = "/Upload/MarketerSliders/" + name;

                string path = Server.MapPath(imageUrl);



                img.SaveAs(path);
                s.Image = imageUrl;
                s.IsVideo = true;
                db.MarketerSliders.Add(s);
                db.SaveChanges();

            }

            return Redirect("/Admin/MarketerSlider/Index");


        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var slider = db.MarketerSliders.Find(id);
            try
            {
                System.IO.File.Delete(Server.MapPath(slider.Image));
            }
            catch
            {

            }
            db.MarketerSliders.Remove(slider);
            db.SaveChanges();
            return Redirect("/Admin/MarketerSlider/Index");
        }
    }
}