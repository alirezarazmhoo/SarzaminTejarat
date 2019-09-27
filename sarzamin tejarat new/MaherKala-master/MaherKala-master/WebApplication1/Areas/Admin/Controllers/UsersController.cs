using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Areas.Admin.Controllers
{
    public class UsersController : Controller
    {
        DBContext db = new DBContext();

        // GET: Admin/User
        public ActionResult Index()
        {
            var url = "/Admin/Users/Index?";
            var role = db.Roles.Where(n => n.RoleNameEn == "Member").FirstOrDefault();
            string name = Request["Name"];
            string mobile = Request["Mobile"];
            var query = db.Users.Where(p => p.Role.Id == role.Id);
            if (Request["Name"]!=null && Request["Name"] != "")
            {
                query = query.Where(p => p.Fullname.Contains(name));
                if (url.Contains("?"))
                    url = url + "&Name=" + name;
                else
                    url = url + "?Name=" + name;
            }
            if (Request["Mobile"] != null && Request["Mobile"] !="")
            {
                query = query.Where(p => p.Mobile.Contains(mobile));
                if (url.Contains("?"))
                    url = url + "&Mobile=" + mobile;
                else
                    url = url + "?Mobile=" + mobile;
            }
            var ordered = query.OrderByDescending(p => p.Id);
            var paginated = new PagedItem<User>(ordered,url);
            ViewBag.Data = paginated;
            return View();
        }
        [HttpGet]
        public ActionResult Deactive(int id)
        {
            var user = db.Users.Find(id);
            user.Status = false;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Active(int id)
        {
            var user = db.Users.Find(id);
            user.Status = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Details(int id)
        {
            var user = db.Users.Find(id);
            ViewBag.Data = user;
            return View();
        }
    }
}