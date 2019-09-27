using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Areas.Admin.Controllers
{
    public class CommentController : Controller
    {
        DBContext db = new Models.DBContext();

        // GET: Admin/Comment
        public ActionResult Index()
        {
            ViewBag.Data = db.Comments.Include("Product").Where(p => p.Status == false).ToList();
            return View();
        }
        public ActionResult Accept(int id)
        {
            db.Comments.Find(id).Status = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            var data = db.Comments.Find(id);
            db.Comments.Remove(data);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}