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
        public object Store()
        {

            string Fullname = Request["Fullname"].Trim();
            string Password = Request["Password"].Trim();
            string Email = Request["Email"].Trim();
            string Address = Request["Address"].Trim();
            string PhoneNumber = Request["Phone"].Trim();
            string Mobile = Request["Mobile"].Trim();
            string PostalCode = Request["PostalCode"].Trim();

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

            if (Email == null || Email == "")
            {
                ModelState.AddModelError("", "ایمیل را وارد کنید");
                return View("Register");
            }
            if (Password == null || Password == "")
            {
                ModelState.AddModelError("", "کلمه عبور را وارد کنید");
                return View("Register");
            }
            if (Fullname == null || Fullname == "")
            {
                ModelState.AddModelError("", "نام را وارد کنید");
                return View("Register");
            }
            if (Address == null || Address == "")
            {
                ModelState.AddModelError("", "آدرس را وارد کنید");
                return View("Register");
            }
            if (PostalCode == null || PostalCode == "")
            {
                ModelState.AddModelError("", "کدپستی را وارد کنید");
                return View("Register");
            }
            if (PostalCode.Trim().Length != 10)
            {
                ModelState.AddModelError("", "کدپستی باید ده رقم باشد");
                return View("Register");
            }
            long nnn2;
            if (long.TryParse(PostalCode, out nnn2) == false)
            {
                ModelState.AddModelError("", "کدپستی باید عدد باشد");
                return View("Register");
            }
            if (!long.TryParse(PhoneNumber, out nnn2))
            {
                ModelState.AddModelError("", "تلفن باید عدد باشد");
                return View("Register");
            }
            if (!long.TryParse(Mobile, out nnn2))
            {
                ModelState.AddModelError("", "موبایل باید عدد باشد");
                return View("Register");
            }
            if (PhoneNumber == null || PhoneNumber == "")
            {
                ModelState.AddModelError("", "شماره تلفن را وارد کنید");
                return View("Register");
            }
            if (Mobile == null || Mobile == "")
            {
                ModelState.AddModelError("", "موبایل را وارد کنید");
                return View("Register");
            }

            if (db.Users.Any(p => p.Email == Email))
            {
                ModelState.AddModelError("", "ایمیل تکراری است");
                return View("Register");
            }

            if (db.Users.Any(p => p.Mobile == Mobile))
            {
                ModelState.AddModelError("", "تلفن همراه تکراری است");
                return View("Register");
            }

            try
            {
                MailAddress m = new MailAddress(Email);
            }
            catch
            {
                ModelState.AddModelError("", "فرمت ایمیل صحیح نیست");
                return View("Register");
            }



            db.Users.Add(user);
            //var setting = db.Settings.First();

            //if (setting.Email == null || setting.Email.Trim() == "")
            //{
            //    user.LinkStatus = true;
            //    ViewBag.Message = "registered";
            //    db.SaveChanges();


            //}
            //else
            //{
            //SendEmail s = new Utility.SendEmail(setting);
            string key = Guid.NewGuid().ToString().Replace('-', '0').Substring(0, 4);
            ConfirmEmail c = new ConfirmEmail();
            c.Key = key;
            c.User = user;
            db.ConfirmEmails.Add(c);
            //var list = new List<string>();
            //list.Add(user.Email);
            //    var body = "<div>لینک فعالسازی <br> برروی <a target='_blank' href='" + setting.Domain + "/User/ActiveLink/" + key + "'>این لینک</a><span> جهت فعالسازی حساب کاربری خود کلیک کنید</span></div>";
            //    s.Send(body, "لینک فعالسازی", list);


            //SendServiceClient sms = new SmsService.SendServiceClient();
            //long[] recId = null;
            //byte[] status = null;

            //var s = sms.SendSMS("m.atrincom.com", "61758", "10009611", new string[] { user.Mobile.ToString() }, c.Key, false, ref recId, ref status);


            // }
            //sms.Close();
    
                db.SaveChanges();

            return View(nameof(Login));

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
        public ActionResult SignIn(User user)
        {
            if (user.Password == "" || user.Password == null || user.Email == "" || user.Email == null)
            {
                ModelState.Clear();
                ModelState.AddModelError("", "نام کاربری یا رمز عبور صحیح نیست");
                return View("Login");
            }
            var u = db.Users.Where(p => p.Email == user.Email).Where(p => p.RoleId == 2).FirstOrDefault();
            if (u == null)
            {
                ModelState.Clear();
                ModelState.AddModelError("", "نام کاربری یا رمز عبور صحیح نیست");
                return View("Login");
            }
            if (!DevOne.Security.Cryptography.BCrypt.BCryptHelper.CheckPassword(user.Password, u.Password))
            {
                ModelState.Clear();
                ModelState.AddModelError("", "نام کاربری یا رمز عبور صحیح نیست");
                return View("Login");
            }
            if (u.Status == false)
            {
                ModelState.Clear();
                ModelState.AddModelError("", "ورود غیر مجاز");
                return View("Login");
            }
            if (u.LinkStatus == false)
            {
                SendServiceClient sms = new SmsService.SendServiceClient();
                long[] recId = null;
                byte[] status = null;

                string key = Guid.NewGuid().ToString().Replace('-', '0').Substring(0, 4);
                ConfirmEmail c = new ConfirmEmail();
                c.Key = key;
                c.User = user;
                db.ConfirmEmails.Add(c);
                var res = sms.SendSMS("m.atrincom.com", "61758", "10009611", new string[] { user.Mobile.ToString() }, key, false, ref recId, ref status);
                sms.Close();
                if (res == 0)
                {
                    db.SaveChanges();
                }
                else
                {
                    ViewBag.Message = "متاسفانه امکان ثبت نام وجود ندارد";
                }
                return Redirect("/User/Verify");
            }
            FormsAuthentication.SetAuthCookie(u.Email, false);
            var url = Request["Url"];
            if (url != null && url.Trim() != "")
            {
                return Redirect(url);
            }

            return Redirect("/Home/Index");
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