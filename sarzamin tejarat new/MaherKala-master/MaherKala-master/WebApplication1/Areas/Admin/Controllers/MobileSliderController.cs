using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Areas.Admin.Controllers
{
    public class MobileSliderController : Controller
    {
        // GET: Admin/MobileSlider
        DBContext db = new DBContext();
        // GET: Admin/Slider
        public ActionResult Index()
        {
            ViewBag.Data = db.MobileSliders.OrderByDescending(p => p.Id).ToList();
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Store()
        {
            if (Request.Files["Image"] == null)
            {
                return null;

            }
            MobileSlider s = new MobileSlider();
            var img = Request.Files["Image"];
            var name = Guid.NewGuid().ToString().Replace('-', '0') + "." + img.FileName.Split('.')[1];

            var imageUrl = "/Upload/Sliders/" + name;
            string path = Server.MapPath(imageUrl);
            img.SaveAs(path);
            s.Image = imageUrl;
            db.MobileSliders.Add(s);
            db.SaveChanges();

            return Redirect("/Admin/MobileSlider/Index");


        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var slider = db.MobileSliders.Find(id);
            try
            {
                System.IO.File.Delete(Server.MapPath(slider.Image));
            }
            catch
            {

            }
            db.MobileSliders.Remove(slider);
            db.SaveChanges();
            return Redirect("/Admin/MobileSlider/Index");
        }
    }
}