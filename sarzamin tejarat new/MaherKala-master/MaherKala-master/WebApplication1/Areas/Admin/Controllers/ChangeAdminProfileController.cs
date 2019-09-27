using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebApplication1.Models;

namespace WebApplication1.Areas.Admin.Controllers
{
    public class ChangeAdminProfileController : Controller
    {
            DBContext db = new Models.DBContext();
        // GET: Admin/ChangeAdminProfile
        public ActionResult Index()
        {
            var role_id = db.Roles.Where(p => p.RoleNameEn == "Admin").FirstOrDefault().Id;
            var user = db.Users.Where(p => p.Role.Id == role_id).FirstOrDefault();
            ViewBag.Data = user;

            return View();
        }
        [HttpPost]
        public ActionResult Store()
        {
            var role_id = db.Roles.Where(p => p.RoleNameEn == "Admin").FirstOrDefault().Id;
            var user = db.Users.Where(p => p.Role.Id == role_id).FirstOrDefault();
            user.Email = Request["Username"];
            if(Request["Password"]!=null&& Request["Password"] != "")
                user.Password = DevOne.Security.Cryptography.BCrypt.BCryptHelper.HashPassword(Request["Password"], DevOne.Security.Cryptography.BCrypt.BCryptHelper.GenerateSalt());
            db.SaveChanges();
            FormsAuthentication.SignOut();
            return Redirect("/User/Admin");
            

        }
    }
}