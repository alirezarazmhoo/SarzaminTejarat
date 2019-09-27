using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Areas.Admin.Controllers
{
    public class BannerController : Controller
    {
        DBContext db = new DBContext();
        // GET: Admin/Banner
        public ActionResult Index()
        {
            ViewBag.DataH = db.Banners.Where(p => p.Type == 0).OrderByDescending(p => p.Id).ToList();
            ViewBag.DataV = db.Banners.Where(p=>p.Type==1).OrderByDescending(p => p.Id).ToList();
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
            Banner s = new Banner();
            var img = Request.Files["Image"];
            var name = Guid.NewGuid().ToString().Replace('-', '0') + "." + img.FileName.Split('.')[1];

            var imageUrl = "/Upload/Banner/" + name;
            string path = Server.MapPath(imageUrl);
            img.SaveAs(path);
            s.Image= imageUrl;
            s.Link = Request["Link"];
            s.Type= Convert.ToInt32( Request["Type"]);
            db.Banners.Add(s);
            db.SaveChanges();

            return Redirect("/Admin/Banner/Index");


        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var banner= db.Banners.Find(id);
            try
            {
                System.IO.File.Delete(Server.MapPath(banner.Image));
            }
            catch
            {

            }
            db.Banners.Remove(banner);
            db.SaveChanges();
            return Redirect("/Admin/Banner/Index");
        }
    }
}