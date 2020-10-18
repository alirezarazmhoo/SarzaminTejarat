using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebApplication1.Filter;
using WebApplication1.Models;
using WebApplication1.SmsService;
using WebApplication1.Utility;

namespace WebApplication1.Controllers.api
{
    public class UserController : ApiController
    {
        DBContext db = new DBContext();
        [HttpPost]
        [Route("api/User/Register")]
        public object Register()
        {
            SendSms sendSms = new SendSms();
            Random random = new Random();
            string Address = HttpContext.Current.Request.Form["Address"];
            string Mobile = HttpContext.Current.Request.Form["Mobile"];
            string PostalCode = HttpContext.Current.Request.Form["PostalCode"];
            string Fullname = HttpContext.Current.Request.Form["Fullname"];

            if (string.IsNullOrEmpty(Fullname))
            {
                return new
                {
                    Message = 1,
                    Text = "نام و نام خانوادگی را وارد کنید"
                };
            }

            if (string.IsNullOrEmpty(Mobile))
            {
                return new
                {
                    Message = 1,
                    Text = "شماره همراه را وارد کنید"
                };
            }
            if (string.IsNullOrEmpty(Address))
            {
                return new
                {
                    Message = 1,
                    Text = "آدرس را وارد کنید"
                };
            }

            try
            {
                var setting = db.Settings.FirstOrDefault();
                if (db.Users.Any(p => p.Mobile == Mobile))
                {
                    return new
                    {
                        Message = 1,
                        Text = "این شماره همراه تکراری است"
                    };
                }
                Role r = db.Roles.Where(p => p.RoleNameEn == "Member").FirstOrDefault();
                var user = new User();
                user.Role = r;
                user.Status = true;
                user.LinkStatus = true;
                string key = random.Next(1000, 9999).ToString();
                user.Api_Token = Guid.NewGuid().ToString().Replace('-', '0');
                user.Password = "No Pass";
                user.Email = "No Email";
                user.Fullname = String.IsNullOrEmpty(Fullname) ? "No FullName" : Fullname;
                user.Address = Address;
                user.PhoneNumber = "0";
                user.Mobile = Mobile;
                user.PostalCode = String.IsNullOrEmpty(PostalCode) ? "0" : PostalCode;
                ConfirmEmail c = new ConfirmEmail();
                c.Key = key;
                c.User = user;
                db.ConfirmEmails.Add(c);
                db.Users.Add(user);
                 sendSms.CallSmSMethod(Convert.ToInt64(Mobile), 34956, "VerificationCode", key.ToString());
                //sms.Close();  
                    db.SaveChanges();
                    return new
                    {
                        Message = 0,
                        Api_Token = user.Api_Token
                    };
            }
            catch (Exception ex)
            {
                return new
                {
                    Message = 1,
                    Text = "عملیات ثبت نام موفقیت آمیز نبود",
                    Error = ex.Message
                };
            }

        }
        [HttpPost]
        [Route("api/User/VerifyToken")]
        public object VerifyToken()
        {
            string token = HttpContext.Current.Request.Form["Token"];
            var c = db.ConfirmEmails.Include("User").Where(p => p.IsUsed == false && p.Key == token).OrderByDescending(p => p.Id).FirstOrDefault();
            if (c == null)
            {
                return new { StatusCode = 1 };
            }
            else
            {
                c.User.LinkStatus = true;
                c.IsUsed = true;
                db.SaveChanges();
            }
            return new { StatusCode = 0 };

        }
        [HttpPost]
        [Route("api/User/Login")]
        [AcceptVerbs("POST")]
        public object Login()
        {
            string Mobile = HttpContext.Current.Request.Form["Mobile"];
            string Password = HttpContext.Current.Request.Form["Password"];
            SendSms sendSms = new SendSms();
            Random random = new Random();
            try
            {
                var data = db.Users.Where(p => p.Role.Id != 1).Where(p => p.Mobile == Mobile).FirstOrDefault();
                if (data == null)
                {
                    return new { Message = 1 };
                }
                if (data.Status == false)
                {
                    return new { Message = 3 };
                }
                if (!string.IsNullOrEmpty(Mobile) && string.IsNullOrEmpty(Password))
                {
                    ConfirmEmail c = new ConfirmEmail();
                    string key = random.Next(1000, 9999).ToString();
                    var Code2 = db.ConfirmEmails.Where(s => s.User.Id == data.Id).FirstOrDefault();
                    if (Code2 != null)
                    {
                        db.ConfirmEmails.Remove(Code2);
                        db.SaveChanges();
                    }
                    c.Key = key;
                    c.User = data;
                    db.ConfirmEmails.Add(c);
                     sendSms.CallSmSMethod(Convert.ToInt64(Mobile), 34956, "VerificationCode", key.ToString());
                 
                        db.SaveChanges();

                        return new
                        {
                            Message = 0,
                            Text = "SmsSent!"
                        };

                }
                var Code = db.ConfirmEmails.Where(s => s.User.Id == data.Id).FirstOrDefault();
                if (Code == null)
                {
                    return new { Message = 5 };

                }
                if (Code.Key != Password)
                {
                    return new { Message = -4, text = "کد ورود صحیح نیست" };

                }
                else
                {
                    db.ConfirmEmails.Remove(Code);
                    db.SaveChanges();
                    return new
                    {
                        Message = 0,
                        Api_Token = data.Api_Token
                    };
                }
            }
            catch (Exception ex)
            {
                return new
                {
                    Message = 1,
                    Text = "عملیات ورود  موفقیت آمیز نبود",
                    Error = ex.Message
                };
            }
        }

        [HttpPost]
        [Route("api/User/UpdateProfile")]
        [ApiAuthorize]

        public object Update()
        {
            var token = HttpContext.Current.Request.Form["Api_Token"];
            var user = db.Users.Where(p => p.Api_Token == token).FirstOrDefault();

            string Fullname = HttpContext.Current.Request.Form["Fullname"];
            string Password = HttpContext.Current.Request.Form["Password"];

            string Address = HttpContext.Current.Request.Form["Address"];
            string PhoneNumber = HttpContext.Current.Request.Form["PhoneNumber"];
            string Mobile = HttpContext.Current.Request.Form["Mobile"];
            string PostalCode = HttpContext.Current.Request.Form["PostalCode"];

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
            return new { Message = 0 };
        }
        [HttpPost]
        [Route("api/User/ShowProfile")]
        public object ShowProfile()
        {
            var token = HttpContext.Current.Request.Form["Api_Token"];
            var user = db.Users.Where(p => p.Api_Token == token).Select(p => new {p.Id, p.Address, p.Fullname, p.Email, p.Mobile, p.PhoneNumber, p.PostalCode }).FirstOrDefault();
            return user;
        }
        [HttpPost]
        [Route("api/User/RecoverPassword")]
        [ApiAuthorize]
        public object Recover()
        {
            string key = Guid.NewGuid().ToString().Replace('-', '0');
            var token = HttpContext.Current.Request.Form["Api_Token"];
            var user = db.Users.Where(p => p.Api_Token == token).FirstOrDefault();
            var recover = db.UserRecover.Where(p => p.Status == false && p.User.Id == user.Id).FirstOrDefault();
            if(recover!=null)
            {
                var diff = (DateTime.Now - recover.Time);
                if(diff.Days<=1)
                {
                    return new { Message = 1 };

                }
            }
            UserRecover r = new UserRecover();
            r.Time = DateTime.Now;
            r.User = user;
            r.Key = key;
            r.Status = false;
            db.UserRecover.Add(r);

            Setting setting = db.Settings.FirstOrDefault();
            SendEmail s = new Utility.SendEmail(setting);
            db.SaveChanges();
            var list = new List<string>();
            list.Add(user.Email);
            db.SaveChanges();

            try
            {
                s.Send("<h3>"+setting.SiteName+"<h3>"+"<br/>"+"تغییر رمز عبور<br> برروی <a target='_blank' href='" + setting.Domain + "/User/RecoverUser/" + key + "'>این لینک</a> جهت تغییر رمز عبور کلیک کنید <br/><b> چنانچه شما درخواستی برای تغییر رمز عبور خود صادر نکرده اید، به این ایمیل بی توجه باشید</b>", "لینک فعالسازی", list);
            }
            catch
            {

            }
            return new {Message=0 };
        }
    }
}
