using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebApplication1.Models;
using WebApplication1.SmsService;
using WebApplication1.Utility;

namespace WebApplication1.Controllers
{
    public class UserController : Controller
    {
        DBContext db = new DBContext();
        // GET: User
        [HttpGet]

        public ActionResult Register()
        {
            ViewBag.User = new User();
            return View();
        }
        [HttpGet]

        public ActionResult Login()
        {
            return View();
        }
        [NonAction]
        public bool PasswordStrong(string s, out string message)
        {
            bool isdigit = false;
            bool ischar = false;
            bool isSpecial = false;
            if (s.Length < 8)
            {
                message = "طول رمز عبور حداقل باید هشت رقم باشد";
                return false;
            }
            foreach (char c in s)
            {
                if (char.IsDigit(c))
                {
                    isdigit = true;
                    break;
                }
            }
            foreach (char c in s)
            {
                if (char.IsLetter(c))
                {
                    ischar = true;
                    break;
                }

            }
            foreach (char c in s)
            {
                if (char.IsSymbol(c))
                {
                    isSpecial = true;
                    break;
                }

            }

            if (!ischar)
            {
                message = "باید حداقل از یک حرف استفاده کنید";
                return false;
            }
            if (!isdigit)
            {
                message = "باید حداقل از یک عدد استفاده کنید";
                return false;
            }
            if (!isSpecial)
            {
                message = "باید حداقل از یک کاراکتر خاص استفاده کنید";
                return false;
            }
            message = "";
            return true;
        }

        public ActionResult ActiveLink(string id)
        {
            var data = db.ConfirmEmails.Include("User").Where(p => p.Key == id).FirstOrDefault();
            if (data == null)
            {
                throw new Exception();
            }
            data.User.LinkStatus = true;
            db.ConfirmEmails.Remove(data);
            db.SaveChanges();
            return View();
        }
        [HttpPost]
        public JsonResult Store(string Fullname ,string Address ,string Mobile ,string PostalCode)
        {
            try
            {
                Random random = new Random();
                SendSms sendSms = new SendSms();
                string _Fullname = Fullname.Trim();
                string Password = "No Pass";
                string Email = "No Mail";
                string _Address = Address.Trim();
                string PhoneNumber = "0";
                string _Mobile = Mobile.Trim();
                string _PostalCode = PostalCode.Trim();
                Role r = db.Roles.Where(p => p.RoleNameEn == "Member").FirstOrDefault();
                var user = new User();
                //user.Role = r;
                user.Status = true;
                user.Api_Token = Guid.NewGuid().ToString().Replace('-', '0');
                user.Password = DevOne.Security.Cryptography.BCrypt.BCryptHelper.HashPassword(Password, DevOne.Security.Cryptography.BCrypt.BCryptHelper.GenerateSalt());
                user.Email = Email;
                user.Mobile = Mobile;
                user.PostalCode = PostalCode;
                user.Fullname = Fullname;
                user.Address = Address;
                user.PhoneNumber = PhoneNumber;
                user.LinkStatus = true;
                user.RoleId = r.Id;
                ViewBag.User = user;
                if (Fullname == null || Fullname == "")
                {
    
                    return Json(new { success = false, responseText = "نام را وارد کنید" }, JsonRequestBehavior.AllowGet);
                }
                if (Address == null || Address == "")
                {
                    return Json(new { success = false, responseText = "آدرس را وارد کنید" }, JsonRequestBehavior.AllowGet);

                }
                if (PostalCode == null || PostalCode == "")
                {
                    return Json(new { success = false, responseText = "کدپستی را وارد کنید" }, JsonRequestBehavior.AllowGet);
                }
                if (PostalCode.Trim().Length != 10)
                {
                    return Json(new { success = false, responseText = "کدپستی باید ده رقم باشد" }, JsonRequestBehavior.AllowGet);
                }
                long nnn2;
                if (long.TryParse(PostalCode, out nnn2) == false)
                {
                    return Json(new { success = false, responseText = "کدپستی باید عدد  باشد" }, JsonRequestBehavior.AllowGet);
                }

                if (!long.TryParse(Mobile, out nnn2))
                {
                    return Json(new { success = false, responseText = "موبایل باید عدد  باشد" }, JsonRequestBehavior.AllowGet);
                }

                if (Mobile == null || Mobile == "")
                {
                    return Json(new { success = false, responseText = "موبایل را وارد کنید" }, JsonRequestBehavior.AllowGet);              
                }
                if (db.Users.Any(p => p.Mobile == Mobile))
                {
                    return Json(new { success = false, responseText = "تلفن همراه تکراری است" }, JsonRequestBehavior.AllowGet);
                }
                db.Users.Add(user);
                string key = random.Next(1000, 9999).ToString();
                ConfirmEmail c = new ConfirmEmail();
                c.Key = key;
                c.User = user;
                db.ConfirmEmails.Add(c);
                db.SaveChanges();
                sendSms.CallSmSMethod(Convert.ToInt64(Mobile), 34331, "password", key.ToString());
                return Json(new { success = true, responseText = "ثبت نام انجام شد ، لطفا کد پیامک شده را وارد کنید" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {       
                return Json(new { success = false, responseText = "در حال حاضر امکان ثبت نام وجود ندارد" }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult Verify()
        {
            return View();

        }

        [HttpPost]
        public ActionResult VerifyToken()
        {
            var token = Request["Token"];
            var t = db.ConfirmEmails.Include("User").Where(p => p.Key == token).Where(p => p.IsUsed == false).OrderByDescending(p => p.Id).FirstOrDefault();
            if (t == null)
            {
                ViewBag.Message = "چنین کدی در سیستم وجود ندارد";
                return View("Verify");
            }
            t.IsUsed = true;
            t.User.LinkStatus = true;
            db.SaveChanges();
            FormsAuthentication.SetAuthCookie(t.User.Email, false);
            return Redirect("/User/Profile");

        }
        [HttpPost]
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();

            return Redirect("/Home/Index");
        }
        [HttpPost]
        public JsonResult SignIn(User user)
        {
            Random random = new Random();
            SendSms sendSms = new SendSms();
            if (string.IsNullOrEmpty(user.Mobile))
            { 
             return Json(new { success = false, responseText = "شماره همراه خالی است" }, JsonRequestBehavior.AllowGet);
            }
            var u = db.Users.Where(p => p.Mobile == user.Mobile).Where(p => p.RoleId == 2).FirstOrDefault();
            if (u == null)
            {
            return Json(new { success = false, responseText = 
                "چنین شماره همراهی در سیستم وجود ندارد" }, JsonRequestBehavior.AllowGet);
            }
            if (!string.IsNullOrEmpty(user.Mobile) && string.IsNullOrEmpty(user.Password))
            {
                ConfirmEmail c = new ConfirmEmail();
                string key = random.Next(1000, 9999).ToString();
                var Code2 = db.ConfirmEmails.Where(s => s.User.Id == u.Id).FirstOrDefault();
                if (Code2 != null)
                {
                    db.ConfirmEmails.Remove(Code2);
                    db.SaveChanges();
                }
                c.Key = key;
                c.User = u;
                db.ConfirmEmails.Add(c);
                sendSms.CallSmSMethod(Convert.ToInt64(user.Mobile), 34331, "password", key.ToString());
                db.SaveChanges();
                //TempData["SucessSms"] = "پیامک ارسال شد";
                TempData["MobileNumber"] = user.Mobile;

             return Json(new { success = true, responseText = "پیامک ارسال شد" }, JsonRequestBehavior.AllowGet);

            }
            var Code = db.ConfirmEmails.Where(s => s.User.Id == u.Id).FirstOrDefault();
            if (Code == null)
            {

             return Json(new { success = false, responseText = "کد وارد شده معتبر نمی باشد" },
             JsonRequestBehavior.AllowGet);
            }
            if (Code.Key != user.Password)
            {
              return Json(new { success = false, responseText = "کد وارد شده صحیح نمی باشد" },JsonRequestBehavior.AllowGet);
            }
            db.ConfirmEmails.Remove(Code);
            db.SaveChanges();
            FormsAuthentication.SetAuthCookie(u.Mobile, false);
            var url = string.Empty;
            if (Session["ReturnUrl"] != null)
            {
                //var url = Request["Url"];
                 url = Session["ReturnUrl"].ToString();
            }

            if (url != null && url.Trim() != "")
            {
                return Json(new { success = true, responseText = url }, JsonRequestBehavior.AllowGet);

            }
            return Json(new { success = true, responseText = "" }, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]

        public ActionResult Admin()
        {
            return View();
        }

        //[HttpPost]
        //public ActionResult SignInAdmin(User user)
        //{
        //    if (user.Email == null || user.Email == "")
        //    {
        //        ModelState.Clear();
        //        ModelState.AddModelError("", "نام کاربری را وارد کنید");
        //        return View("Admin");
        //    }
        //    if (user.Password == null || user.Password == "")
        //    {
        //        ModelState.Clear();
        //        ModelState.AddModelError("", "رمز عبور را وارد کنید");
        //        return View("Admin");
        //    }
        //    var u = db.Users.Include("Role").Where(p => p.Email == user.Email).Where(p => p.Role.RoleNameEn == "Admin").FirstOrDefault();
        //    //if (u == null)
        //    //{
        //    //    ModelState.Clear();
        //    //    ModelState.AddModelError("", "نام کاربری یا رمز عبور صحیح نیست");
        //    //    return View("Admin");
        //    //}
        //    //if (!DevOne.Security.Cryptography.BCrypt.BCryptHelper.CheckPassword(user.Password, u.Password))
        //    //{
        //    //    ModelState.Clear();
        //    //    ModelState.AddModelError("", "نام کاربری یا رمز عبور صحیح نیست");
        //    //    return View("Admin");
        //    //}

        //    if (u.Status == false)
        //    {
        //        ModelState.Clear();
        //        ModelState.AddModelError("", "ورود غیر مجاز");
        //        return View("Admin");
        //    }
        //    if (u.LinkStatus == false)
        //    {
        //        ModelState.Clear();
        //        ModelState.AddModelError("", "حساب کاربری غیر فعال است. بر روی ایمیل فعالسازی کلیک کنید");
        //        return View("Admin");
        //    }
        //    FormsAuthentication.SetAuthCookie(u.Email, false);

        //    return Redirect("/Admin/Product");
        //}
        [HttpPost]
        public ActionResult SignInAdmin(AdminUsers adminUsers)
        {
            if (adminUsers.UserName == null || adminUsers.Password == "")
            {
                ModelState.Clear();
                TempData["ErrorLogin"] = "نام کاربری را وارد کنید";
             
                return View("Admin");
            }
            var adminuserItem = db.AdminUsers.Where(s => s.Password == adminUsers.Password && s.UserName == adminUsers.UserName).FirstOrDefault();

            if(adminuserItem == null)
            {
                TempData["ErrorLogin"] = "نام کاربری یا رمز عبور صحیح نیست";

                return View("Admin");
            }
            
            FormsAuthentication.SetAuthCookie(adminuserItem.UserName, false);

            return Redirect("/Admin/Product");



        }








        [Authorize(Roles = "Member")]
        public ActionResult Profile()
        {
            var email = User.Identity.Name;
            ViewBag.User = db.Users.Where(p => p.Email == email).FirstOrDefault();
            return View();
        }
        [HttpPost]
        public ActionResult Update()
        {
            var email = User.Identity.Name;
            var user = db.Users.Where(p => p.Email == email).FirstOrDefault();

            string Fullname = Request["Fullname"];
            string Password = Request["Password"];
            string Email = Request["Email"];
            string Address = Request["Address"];
            string PhoneNumber = Request["PhoneNumber"];
            string Mobile = Request["Mobile"];
            string PostalCode = Request["PostalCode"];

            user.Address = Address;
            user.Fullname = Fullname;
            user.Mobile = Mobile;
            if (Password != null)
            {
                user.Password = DevOne.Security.Cryptography.BCrypt.BCryptHelper.HashPassword(Password, DevOne.Security.Cryptography.BCrypt.BCryptHelper.GenerateSalt());
            }
            user.PhoneNumber = PhoneNumber;
            user.PostalCode = PostalCode;
            db.SaveChanges();
            ViewBag.User = user;
            ViewBag.Message = "user";
            return View("Profile");
        }
        [HttpGet]
        public ActionResult RecoverUser(string id)
        {
            var data = db.UserRecover.Include("User").Where(p => p.Key == id).Where(p => p.Status == false).FirstOrDefault();
            //string Message = "";
            if (data == null)
            {
                ViewBag.Message = "درخواست شما معتبر نیست";
                return View();
            }
            User usr = data.User;

            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            string pass = new string(Enumerable.Repeat(chars, 6)
              .Select(s => s[random.Next(s.Length)]).ToArray());

            const string chars2 = "!@#$%&";
            string pass2 = new string(Enumerable.Repeat(chars2, 3)
              .Select(s => s[random.Next(s.Length)]).ToArray());

            int i = random.Next(4);
            data.Status = true;

            string password = pass.Substring(0, i) + pass2 + pass.Substring(i + 1, pass.Length);
            usr.Password = DevOne.Security.Cryptography.BCrypt.BCryptHelper.HashPassword(password, DevOne.Security.Cryptography.BCrypt.BCryptHelper.GenerateSalt());

            db.SaveChanges();

            ViewBag.Message = "پسورد شما با موفقیت تغییر یافت. پسورد جدید شما : <br> " + password;
            return View();

        }
    }
}