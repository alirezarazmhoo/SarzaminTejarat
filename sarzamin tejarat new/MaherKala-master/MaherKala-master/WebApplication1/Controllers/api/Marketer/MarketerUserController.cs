using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using WebApplication1.Controllers.api.Repository;
using WebApplication1.Models;

namespace WebApplication1.Controllers.api.Marketer
{
    public class MarketerUserController : ApiController
    {
        DBContext db = new DBContext();
        CheckUserDateLogin checkUserDateLogin = new CheckUserDateLogin();
        [HttpPost]
        public object Register()
        {
            string Name = HttpContext.Current.Request.Form["Name"];
            string LastName = HttpContext.Current.Request.Form["LastName"];
            string Mobile = HttpContext.Current.Request.Form["Mobile"];
            string Phone = HttpContext.Current.Request.Form["Phone"];
            string Password = HttpContext.Current.Request.Form["Password"];
            string Address = HttpContext.Current.Request.Form["Address"];

            double Lat = double.Parse(HttpContext.Current.Request.Form["Lat"], CultureInfo.InvariantCulture);

            double Lng = double.Parse(HttpContext.Current.Request.Form["Lng"], CultureInfo.InvariantCulture);

            string IDCardNumber = HttpContext.Current.Request.Form["IDCardNumber"];
            string CertificateNumber = HttpContext.Current.Request.Form["CertificateNumber"];
            bool IsMarrid = Convert.ToBoolean(HttpContext.Current.Request.Form["IsMarrid"]);
            string AccountNumber = HttpContext.Current.Request.Form["AccountNumber"];
            string CardAccountNumber = (HttpContext.Current.Request.Form["CardAccountNumber"]);
            string IBNA = HttpContext.Current.Request.Form["IBNA"];
            string Description = HttpContext.Current.Request.Form["Description"];
            //string Parent_Id = HttpContext.Current.Request.Form["Parent_Id"];

            byte UserType = Convert.ToByte(HttpContext.Current.Request.Form["Usertype"]);
            string PersonalReagentCode = HttpContext.Current.Request.Form["PersonalReagentCode"];


            if (Password.Length < 8)
            {
                return new { StatusCode = 100, Message = "طول رمز عبور حداقل باید هشت رقم باشد" };
            }


            if (PersonalReagentCode == null)
            {
                return new
                {
                    StatusCode = 101,
                    Message = "کد معرف خود را وارد کنید"

                };
            }
            //if (UserType == 0)
            //{
            //    return new
            //    {
            //        StatusCode = 10,
            //        Message = "نوع کاربر را تعیین کنید"

            //    };
            //}


            MarketerUser m = new MarketerUser();
            m.Name = Name;
            m.LastName = LastName;
            m.Mobile = Mobile;
            m.Phone = Phone;
            //m.Password = DevOne.Security.Cryptography.BCrypt.BCryptHelper.HashPassword(Password, DevOne.Security.Cryptography.BCrypt.BCryptHelper.GenerateSalt());
            m.Password = Password;
            m.Address = Address;
            m.Lat = Lat;
            m.Lng = Lng;
            m.IDCardNumber = IDCardNumber;
            m.CertificateNumber = CertificateNumber;
            m.IsMarrid = IsMarrid;
            m.AccountNumber = AccountNumber;
            m.CardAccountNumber = CardAccountNumber;
            m.IBNA = IBNA;
            m.Description = Description;
            m.IsAvailable = false;
            m.Parent_Id = 0;
            m.Usertype = UserType;
            m.PersonalReagentCode = IDCardNumber;
            m.AcceptedByParent = false;
            m.SubsetCount = 0;
            m.CreatedDate = DateTime.Now;
            if (!HttpContext.Current.Request.Form.AllKeys.Contains("IDCardPicture"))
            {
                return new { StatusCode = 102, Message = "تصویر کارت ملی را ارسال کنید" };
            }
            var data = HttpContext.Current.Request["IDCardPicture"];
            data = data.Replace("data:image/png;base64,", "");
            data = data.Replace("data:image/jpeg;base64,", "");
            data = data.Replace(" ", "+");
            byte[] imgBytes = Convert.FromBase64String(data);



            if (imgBytes.Length < 10000)
            {
                return new { StatusCode = 103, Message = "تصویر کارت ملی با مشکل مواجه است" };
            }
            if (imgBytes.Length >= 5242880)
            {
                return new { StatusCode = 104, Message = "تصویر کارت ملی با حجم بیش از پنج مگابایت مجاز نیست" };
            }







            if (!HttpContext.Current.Request.Form.AllKeys.Contains("PersonalPicture"))
            {
                return new { StatusCode = 105, Message = "تصویر خود را ارسال کنید" };
            }
            var data2 = HttpContext.Current.Request["PersonalPicture"];
            data2 = data2.Replace("data:image/png;base64,", "");
            data2 = data2.Replace("data:image/jpeg;base64,", "");
            data2 = data2.Replace(" ", "+");
            byte[] imgBytes2 = Convert.FromBase64String(data2);



            if (imgBytes2.Length < 10000)
            {
                return new { StatusCode = 106, Message = "تصویر شما با مشکل مواجه است" };
            }
            if (imgBytes2.Length >= 5242880)
            {
                return new { StatusCode = 107, Message = "تصویر شما با حجم بیش از پنج مگابایت مجاز نیست" };
            }

            var unique2 = Guid.NewGuid().ToString().Replace('-', '0') + "." + "jpg";
            var imageUrl2 = "/Upload/MarketerUpload/" + unique2;
            string path2 = HttpContext.Current.Server.MapPath(imageUrl2);
            m.PersonalPicture = imageUrl2;


            if (db.MarketerUsers.Any(p => p.IDCardNumber == IDCardNumber))
            {
                return new { StatusCode = 108, Message = "شماره ملی تکراری است" };
            }

            if (db.MarketerUsers.Any(p => p.Mobile == Mobile))
            {
                return new { StatusCode = 109, Message = "شماره موبایل تکراری است" };
            }
            //var img = HttpContext.Current.Request.Files[0];


            var unique = Guid.NewGuid().ToString().Replace('-', '0') + "." + "jpg";
            var imageUrl = "/Upload/MarketerUpload/" + unique;
            string path = HttpContext.Current.Server.MapPath(imageUrl);
            m.IDCardPhotoAddress = imageUrl;


            #region Findparent
            var Parent = db.MarketerUsers.Where(p => p.PersonalReagentCode == PersonalReagentCode).FirstOrDefault();
            if (Parent == null)
            {
                return new { StatusCode = 110, Message = "کاربری با این کد معرف یافت نشد" };
            }
            else
            {
                //Parent_Id = Parent.Id;
                m.Parent_Id = Parent.Id;
            }

            #endregion

            m.Api_Token = Guid.NewGuid().ToString().Replace('-', '0');
            //if(Parent_Id!=null)
            //{
            //    if(db.MarketerUsers.Any(p=>p.Id==Convert.ToInt32(Parent_Id)))
            //    {
            //        m.Parent_Id = Convert.ToInt32(Parent_Id);
            //    }
            //}
            var plan = db.Plannns.Where(p => p.Level == 1 && p.PlanType.Id == 1).FirstOrDefault();

            if (plan != null)
            {

                m.PlannnID = plan.Id;
            }

            m.IsFirstTime = true;
            db.MarketerUsers.Add(m);
            try
            {

                db.SaveChanges();
                using (var imageFile = new FileStream(path, FileMode.Create))
                {
                    imageFile.Write(imgBytes, 0, imgBytes.Length);
                    imageFile.Flush();
                }
                using (var imageFile = new FileStream(path2, FileMode.Create))
                {
                    imageFile.Write(imgBytes2, 0, imgBytes2.Length);
                    imageFile.Flush();
                }
                return new { StatusCode = 0, user = m };
            }
            catch (DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors
                  .SelectMany(x => x.ValidationErrors)
                  .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join(" - ", errorMessages);
                return new { StatusCode = 1, Message = fullErrorMessage };
            }



        }

        [HttpPost]
        [Route("api/MarketerUser/Login")]
        public object Login()
        {
            string Mobile = HttpContext.Current.Request.Form["Mobile"];
            string Password = HttpContext.Current.Request.Form["Password"];
            var user = db.MarketerUsers.Where(p => p.Mobile == Mobile).Select(s=>new { s.Id, Password,s.AccountNumber, s.Address, userToken = s.Api_Token, s.CardAccountNumber, s.CertificateNumber, s.Description, s.FactorCounter, s.IBNA, s.IDCardNumber, s.IDCardPhotoAddress, s.IsFirstTime, s.IsMarrid, s.LastName, s.Lat, s.Lng, s.Mobile, s.Name, s.PersonalPicture, s.PersonalReagentCode, s.Phone, s.Usertype, s.AcceptedByParent, s.IsAvailable,s.Parent_Id,s.SubsetCount,s.PlannnID }).FirstOrDefault();

            var MarketerLimitSale = db.MarketerLimitSale.FirstOrDefault();


            var userPlan = db.Plannns.Where(s => s.Id == user.PlannnID).Select(p=>new { p.Level, p.PlanTypeID }).FirstOrDefault();
            var userplanType = db.PlanTypes.Where(s => s.Id == userPlan.PlanTypeID).Select(p=>new { p.Name}).FirstOrDefault();



            if (user == null)
            {
                return new { StatusCode = 200, Message = "کاربر مورد نظر یافت نشد" };
            }
            if (user.IsAvailable == false)
            {
                return new { StatusCode = 201, Message = "نام کاربری شما هنوز فعال نشده است" };
            }
            //if (!DevOne.Security.Cryptography.BCrypt.BCryptHelper.CheckPassword(Password,user.Password))
            //{
            //    return new { StatusCode = 1, Message = "شماره موبایل یا رمز عبور صحیح نیست" };
            //}
            if (user.Password != Password)
            {
                return new { StatusCode = 202, Message = "شماره موبایل یا رمز عبور صحیح نیست" };
            }
            #region CheckUserDate

            var result = checkUserDateLogin.ActiveForaYear(user.Id);
            if (result == false)
            {
                return new { StatusCode = 203, Message = "حساب کاربری شما از یکسال گذشته است ، آن را فعال کنید" };

            }

            #endregion

            if (MarketerLimitSale.Enable && MarketerLimitSale !=null)
            {
                var CheckResult = checkUserDateLogin.CheckLazyMarketerUser(user.Id);
                if(CheckResult == false)
                {
                    return new { StatusCode = 204, Message = "شما به مدت "+MarketerLimitSale.Days+" روز فروش نداشته اید لطفا حساب خود را فعال کنید" };
                }
                

            }

            checkthelazysubsets(user.userToken);

            #region CheckLazyMarketerUser


            #endregion
            return new { StatusCode = 0, user = user ,userPlan=userPlan,userplanType=userplanType};
        }

        private void checkthelazysubsets(string  apitoken)
        {
            var useritem = db.MarketerUsers.Where(s => s.Api_Token == apitoken).FirstOrDefault();

            var listsubsets = db.MarketerUsers.Where(s => s.Parent_Id == useritem.Id).ToList();
            var today = DateTime.Now;

            foreach (var item in listsubsets)
            {
                var max = 0;
                var factors = db.MarketerFactor.Where(s => s.MarketerUser.Id == item.Id).ToList();
                foreach (var item2 in factors)
                {
                    if(item2.Id >= max)
                    {
                        max = item2.Id;
                    }

                }
                if(max != 0)
                {
                    var factoritem = db.MarketerFactor.Where(s => s.Id == max).FirstOrDefault();
                    if(factoritem != null)
                    {

                    var days = (today - factoritem.Date).TotalDays;
                        if (days > 30)
                        {
                            item.Islazy = true;
                        }
                        if (days < 30)
                        {
                            item.Islazy = false;
                        }
                    }
                  

                }
                else
                {
                    var _days = (today - item.CreatedDate).TotalDays;

                    if(_days > 30)
                    {
                        item.Islazy = true;
                    }


                }


            }



            db.SaveChanges();
     
        }

        [HttpPost]
        [Route("api/MarketerUser/ShowProfile")]
        public object ShowProfile()
        {
            var token = HttpContext.Current.Request.Form["Api_Token"];
            var user = db.MarketerUsers.Where(p => p.Api_Token == token).Select(p => new { p.Id, p.Address, p.AccountNumber, p.CardAccountNumber, p.CertificateNumber, p.Description, p.IBNA, p.IDCardNumber, p.IDCardPhotoAddress, p.IsMarrid, p.LastName, p.Lat, p.Lng, p.Mobile, p.Name, p.Parent_Id, p.Phone, p.PersonalPicture, p.Password }).FirstOrDefault();
            return user;
        }

        [HttpPost]
        [Route("api/MarketerUser/EditUser")]
        public object EditUser()
        {

            string Api_Token = HttpContext.Current.Request.Form["Api_Token"];

            //string PersonalPicture = Convert.ToString(HttpContext.Current.Request.Files["PersonalPicture"]);
            var data = HttpContext.Current.Request["PersonalPicture"];


            var FindUser = db.MarketerUsers.FirstOrDefault(s => s.Api_Token == Api_Token);
            if (FindUser != null)
            {
                try
                {



                    if (HttpContext.Current.Request.Form.AllKeys.Contains("Name"))
                    {
                        string Name = HttpContext.Current.Request.Form["Name"];
                        FindUser.Name = Name;
                    }

                    if (HttpContext.Current.Request.Form.AllKeys.Contains("LastName"))
                    {
                        string LastName = HttpContext.Current.Request.Form["LastName"];
                        FindUser.LastName = LastName;
                    }
                    if (HttpContext.Current.Request.Form.AllKeys.Contains("Mobile"))
                    {
                        string Mobile = HttpContext.Current.Request.Form["Mobile"];
                        FindUser.Mobile = Mobile;
                    }
                    if (HttpContext.Current.Request.Form.AllKeys.Contains("Phone"))
                    {
                        string Phone = HttpContext.Current.Request.Form["Phone"];
                        FindUser.Phone = Phone;
                    }
                    if (HttpContext.Current.Request.Form.AllKeys.Contains("Password"))
                    {
                        string Password = HttpContext.Current.Request.Form["Password"];
                        if (Password.Length < 8)
                        {
                            return new { StatusCode = 1, Message = "طول رمز عبور حداقل باید هشت رقم باشد" };
                        }
                        FindUser.Password = Password;
                    }
                    if (HttpContext.Current.Request.Form.AllKeys.Contains("Address"))
                    {
                        string Address = HttpContext.Current.Request.Form["Address"];

                        FindUser.Address = Address;
                    }
                    if (HttpContext.Current.Request.Form.AllKeys.Contains("Lat"))
                    {
                        double Lat = double.Parse(HttpContext.Current.Request.Form["Lat"], CultureInfo.InvariantCulture);

                        FindUser.Lat = Lat;
                    }
                    if (HttpContext.Current.Request.Form.AllKeys.Contains("Lng"))
                    {
                        double Lng = double.Parse(HttpContext.Current.Request.Form["Lng"], CultureInfo.InvariantCulture);

                        FindUser.Lng = Lng;
                    }
                    if (HttpContext.Current.Request.Form.AllKeys.Contains("IsMarrid"))
                    {
                        bool IsMarrid = Convert.ToBoolean(HttpContext.Current.Request.Form["IsMarrid"]);

                        FindUser.IsMarrid = IsMarrid;
                    }
                    if (HttpContext.Current.Request.Form.AllKeys.Contains("Description"))
                    {
                        string Description = HttpContext.Current.Request.Form["Description"];


                        FindUser.Description = Description;
                    }
                    if (data != null)
                    {

                        if (FindUser.PersonalPicture != null)
                        {


                            try
                            {
                                System.IO.File.Delete(HostingEnvironment.MapPath(FindUser.PersonalPicture));
                            }
                            catch
                            {
                                return new
                                {
                                    message = "Error while Removeing pcture"
                                };


                            }
                        }

                    }
                    if (data != null)
                    {

                        data = data.Replace("data:image/png;base64,", "");
                        data = data.Replace("data:image/jpeg;base64,", "");
                        data = data.Replace(" ", "+");
                        byte[] imgBytes2 = Convert.FromBase64String(data);


                        if (imgBytes2.Length < 10000)
                        {
                            return new { StatusCode = 1, Message = "تصویر شما با مشکل مواجه است" };
                        }
                        if (imgBytes2.Length >= 5242880)
                        {
                            return new { StatusCode = 1, Message = "تصویر شما با حجم بیش از پنج مگابایت مجاز نیست" };
                        }


                        var unique2 = Guid.NewGuid().ToString().Replace('-', '0') + "." + "jpg";
                        var imageUrl2 = "/Upload/MarketerUpload/" + unique2;
                        string path2 = HttpContext.Current.Server.MapPath(imageUrl2);
                        FindUser.PersonalPicture = imageUrl2;
                        using (var imageFile = new FileStream(path2, FileMode.Create))
                        {
                            imageFile.Write(imgBytes2, 0, imgBytes2.Length);
                            imageFile.Flush();
                        }




                    }



                }
                catch (DbEntityValidationException ex)
                {
                    var errorMessages = ex.EntityValidationErrors
                      .SelectMany(x => x.ValidationErrors)
                      .Select(x => x.ErrorMessage);

                    // Join the list to a single string.
                    var fullErrorMessage = string.Join(" - ", errorMessages);
                    return new { StatusCode = 1, Message = fullErrorMessage };
                }
            }


            else
            {
                return new
                {
                    message = "User Not Found"
                };
            }


            db.SaveChanges();

            return new
            {

                Message = 0
            };




        }




        [HttpPost]
        [Route("api/MarketerUser/ActiveAccountForAYear")]
        public async Task<object> ActiveAccountForAYear()
        {
            string Api_Token = HttpContext.Current.Request.Form["Api_Token"];
            var UserItem = db.MarketerUsers.Where(p => p.Api_Token == Api_Token).FirstOrDefault();
            if (UserItem == null)
            {
                return new
                {
                    StatusCode = 1,
                    Message = "User not found"
                };

            }
            var today = DateTime.Now;
            UserItem.CreatedDate = today;
            await db.SaveChangesAsync();
            return new
            {

                StatusCode = 0,
                Message = "اکانت شما فعال شد"
            };
              

           

        }



        [HttpPost]
        [Route("api/MarketerUser/AutoLogin")]
        public object AutoLogin()
        {
            var ApiToken = HttpContext.Current.Request.Form["Api_Token"];
            var user = db.MarketerUsers.Where(p => p.Api_Token == ApiToken).Select(s => new { s.Id, s.AccountNumber, s.Address, userToken = s.Api_Token, s.CardAccountNumber, s.CertificateNumber, s.Description, s.FactorCounter, s.IBNA, s.IDCardNumber, s.IDCardPhotoAddress, s.IsFirstTime, s.IsMarrid, s.LastName, s.Lat, s.Lng, s.Mobile, s.Name, s.PersonalPicture, s.PersonalReagentCode, s.Phone, s.Usertype, s.AcceptedByParent, s.IsAvailable, s.Parent_Id, s.SubsetCount, s.PlannnID }).FirstOrDefault();
            var MarketerLimitSale = db.MarketerLimitSale.FirstOrDefault();


            var userPlan = db.Plannns.Where(s => s.Id == user.PlannnID).Select(p => new { p.Level, p.PlanTypeID }).FirstOrDefault();
            var userplanType = db.PlanTypes.Where(s => s.Id == userPlan.PlanTypeID).Select(p => new { p.Name }).FirstOrDefault();

            var result = checkUserDateLogin.ActiveForaYear(user.Id);
            if (result == false)
            {
                return new { StatusCode = 203, Message = "حساب کاربری شما از یکسال گذشته است ، آن را فعال کنید" };

            }
            if (MarketerLimitSale.Enable && MarketerLimitSale != null)
            {
                var CheckResult = checkUserDateLogin.CheckLazyMarketerUser(user.Id);
                if (CheckResult == false)
                {
                    return new { StatusCode = 204, Message = "شما به مدت " + MarketerLimitSale.Days + " روز فروش نداشته اید لطفا حساب خود را فعال کنید" };
                }


            }
            return new { StatusCode = 0, user = user, userPlan = userPlan, userplanType = userplanType };


        }


    }
}
