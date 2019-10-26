using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Areas.Admin.Controllers
{
    public class AdminUserManagerController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Admin/AdminUserManager
        public ActionResult Index()
        {
            var items = db.AdminUsers.ToList();

            return View(items);
        }

       public ActionResult CreateAdmin()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateAdmin(AdminUsers adminUsers)
        {
            if(adminUsers.FirstName == null || adminUsers.LastName == null|| adminUsers.Mobile == 0 || adminUsers.Password == null)
            {
                TempData["ErrorCreateAdmin"] = "همه فیلد ها را پر کنید";
                return RedirectToAction(nameof(CreateAdmin));

            }
            if(db.AdminUsers.Any(s=>s.UserName == adminUsers.UserName))
            {
                TempData["ErrorCreateAdmin"] = "نام کاربری تکراری است";
                return RedirectToAction(nameof(CreateAdmin));
            }
            db.AdminUsers.Add(adminUsers);
         await   db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }


        public ActionResult RemoveAdmin()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> RemoveAdmin(int? id)
        {
            int usercounts = db.AdminUsers.Count();
            if(usercounts == 1)
            {
                TempData["ErrorRemoveAdmin"] = "نمی توان همه مدیران را حذف کرد!";
                return RedirectToAction(nameof(RemoveAdmin));
            }


            if (id == 0)
            {
                return RedirectToAction(nameof(Index));
            }


            var item = db.AdminUsers.Find(id);
            try
            {


                if (item != null)
                {
                    db.AdminUsers.Remove(item);
                }
              await  db.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }

            return RedirectToAction(nameof(Index));

        }



        public async Task<ActionResult> CreateRoleForAdmin(int AdminUser,int[] RoleId)
        {
            if (RoleId == null)
            {
                TempData["Faild"] = "نقشی انتخاب نشد است";
                return RedirectToAction(nameof(Index));

            }


            var listRoels = db.AdminsInRoles.Where(s => s.AdminUsersID == AdminUser).ToList();

            if(listRoels.Count != 0)
            {
                foreach (var RoleItem in listRoels)
                {
                    db.AdminsInRoles.Remove(db.AdminsInRoles.Find(RoleItem.Id));

                }
            }
            AdminsInRoles AdminsInRoles = new AdminsInRoles();
            var adminUserItem = db.AdminUsers.Where(s => s.Id == AdminUser).FirstOrDefault();
            //adminUserItem.AdminsInRoles.Clear();
    
            foreach (var item in RoleId)
            {
             
                db.AdminsInRoles.Add(new AdminsInRoles
                {
                    AdminUsersID = AdminUser,
                    AdminsRolesID = item
                });
            }
           await db.SaveChangesAsync();
            TempData["Sucess"] = "تخصیص نقش با موفقیت انجام شد";
            return RedirectToAction(nameof(Index));

            
        }

        public ActionResult AdminRoles(int? id)
        {
            return View(db.AdminUsers.Where(s => s.Id == id).FirstOrDefault());
        }
    }
}