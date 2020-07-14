using SmsIrRestful;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
using System.Web.Http.Description;
using System.Web.Http.ModelBinding;
using WebApplication1.Controllers.api.Repository;
using WebApplication1.Models;
using WebApplication1.Models.Errors;
using WebApplication1.Models.Mobile;
using WebApplication1.Utility;

namespace WebApplication1.Controllers.api.Marketer
{
    public class MarketerUserController : ApiController
    {
        DBContext db = new DBContext();
        CheckUserDateLogin checkUserDateLogin = new CheckUserDateLogin();
        SendSms sendSms = new SendSms();
        List<SmsParameters> _SmsParameters = new List<SmsParameters>();

        string UserTypeText = string.Empty;
        [HttpPost]
        public object Register()
        {
            string Name = HttpContext.Current.Request.Form["Name"];
            string LastName = HttpContext.Current.Request.Form["LastName"];
            string Mobile = HttpContext.Current.Request.Form["Mobile"];
            string Phone = HttpContext.Current.Request.Form["Phone"];
            string Password = HttpContext.Current.Request.Form["Password"];
            string Address = HttpContext.Current.Request.Form["Address"];
            string TextAddress = HttpContext.Current.Request.Form["TextAddress"];
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
			if(IDCardNumber.Length > 10)
			{
				return new
				{
					StatusCode = 777,
					Message = "کد ملی باید 10 رقم باشد"

				};
			}
			if (CardAccountNumber.Length != 16)
			{
				return new
				{
					StatusCode = 888,
					Message = "شماره کارت باید 16 رقم باشد"

				};
			}
			if (IBNA.Length != 24)
			{
				return new
				{
					StatusCode = 999,
					Message = "شماره شبا باید 24 رقم باشد"
				};
			}
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
            m.TextAddress = TextAddress;
            m.Description = Description;
            m.IsAvailable = false;
            m.Parent_Id = 0;			
            m.Usertype = UserType;
			if (UserType != 0)
			{
				m.PersonalReagentCode = "0";
			}
			if(UserType == 0)
			{
			m.PersonalReagentCode = IDCardNumber;
			}
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
            if (Parent == null || Parent.PersonalReagentCode == "0")
            {
                return new { StatusCode = 110, Message = "کاربری با این کد معرف یافت نشد" };
            }
            else
            {
                if(Parent.Usertype != 0)
                {
                    return new { StatusCode = 110, Message = "کد معرف باید مخصوص بازاریابان باشد" };
                }
                //Parent_Id = Parent.Id;
                m.Parent_Id = Parent.Id;
            }
            #endregion
            m.Api_Token = Guid.NewGuid().ToString().Replace('-', '0');
       
            var plan = db.Plannns.Where(p => p.Level == 0 && p.PlanType.Id == 1).FirstOrDefault();

            if (plan != null)
            {
                m.PlannnID = plan.Id;
            }
            if(plan == null)
            {
                Plannn plannn = new Plannn();
                plannn.Level = 0;
                plannn.PlanTypeID = 1;
                plannn.Price = 0;
                db.Plannns.Add(plannn);
                m.PlannnID = plannn.Id;
            }
            m.IsFirstTime = true;
            db.MarketerUsers.Add(m);
			//RegisterPlanDate
			PlanDateregister planDateregister = new PlanDateregister();
			planDateregister.RegisterDate = DateTime.Now;
			planDateregister.IDCardNumber = m.Api_Token;
			db.PlanDateregister.Add(new PlanDateregister
			{
				RegisterDate = DateTime.Now,
				IDCardNumber = m.IDCardNumber
			}); 
            if(UserType == 0)
            {
                UserTypeText = "بازاریاب";
            }
            else if(UserType == 1)
            {
                UserTypeText = "خرده فروش";

            }
            else if (UserType == 2)
            {
                UserTypeText = "خریدار عمده";
            }

            //End
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
                sendSms.CallSmSMethod(Convert.ToInt64(Mobile), 29126, "MarketerUser", Name + " " + LastName);
                _SmsParameters.Add(new SmsParameters() { Parameter = "MarketerUserName", ParameterValue = Name + " " + LastName });
                _SmsParameters.Add(new SmsParameters() { Parameter = "MarketerMobile", ParameterValue = Mobile.ToString() });
                _SmsParameters.Add(new SmsParameters() { Parameter = "MarketrtUserType", ParameterValue = UserTypeText });
                sendSms.CallSmSMethodAdvanced(Convert.ToInt64(Parent.Mobile), 29129, _SmsParameters);
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
            var user = db.MarketerUsers.Where(p => p.Mobile == Mobile && p.Password == Password).Select(s=>new { s.Id, s.Password,s.AccountNumber, s.Address, userToken = s.Api_Token, s.CardAccountNumber, s.CertificateNumber, s.Description, s.FactorCounter, s.IBNA, s.IDCardNumber, s.IDCardPhotoAddress, s.IsFirstTime, s.IsMarrid, s.LastName, s.Lat, s.Lng, s.Mobile, s.Name, s.PersonalPicture, s.PersonalReagentCode, s.Phone, s.Usertype, s.AcceptedByParent, s.IsAvailable,s.Parent_Id,s.SubsetCount,s.PlannnID,s.CanCheckPayment,s.CanPromissoryPayment }).FirstOrDefault();
			bool CanCheckPayment = false;
			bool CanPromissoryPayment = false;
			var MarketerLimitSale = db.MarketerLimitSale.FirstOrDefault();


   
            if (user == null)
            {
                return new { StatusCode = 200, Message = "شماره موبایل یا رمز عبور صحیح نیست" };
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
			//var userPlan = db.Plannns.Where(s => s.Id == user.PlannnID).Select(p => new { p.Level, p.PlanTypeID }).FirstOrDefault();
			var userPlan = db.Plannns.Where(s => s.Id == user.PlannnID).Select(p => new { p.Level, p.PlanTypeID, p.Price }).FirstOrDefault();
			var userplanType = db.PlanTypes.Where(s => s.Id == userPlan.PlanTypeID).Select(p => new { p.Name }).FirstOrDefault();
			#region CheckUserDate
			if (user.Usertype == 0)
			{
			var result = checkUserDateLogin.ActiveForaYear(user.Id);
            if (result == false)
            {
                return new { StatusCode = 203, Message = "حساب کاربری شما از یکسال گذشته است ، آن را فعال کنید" };
            }
			}
            #endregion


			if(user.Usertype == 0)
			{
           

            if (MarketerLimitSale != null && (MarketerLimitSale.Enable && MarketerLimitSale !=null))
            {
				if (!user.IsFirstTime)
				{
                var CheckResult = checkUserDateLogin.CheckLazyMarketerUser(user.Id);
                if(CheckResult == false)
                {
                    return new { StatusCode = 204, Message = "شما به مدت "+MarketerLimitSale.Days+" روز فروش نداشته اید لطفا حساب خود را فعال کنید" };
                }
				}
            }
                
			}
            checkthelazysubsets(user.userToken);
			//CheckPlanRegisterDate
			var ParentToken = db.MarketerUsers.Where(s => s.Id == user.Parent_Id).Select(s=>new { s.Api_Token}).FirstOrDefault();


			var PlanRegisterDateItem = db.PlanDateregister.Where(s => s.IDCardNumber == user.IDCardNumber).FirstOrDefault();
			if(PlanRegisterDateItem != null)
			{
			if(PlanRegisterDateItem.RegisterDate.Year != DateTime.Now.Year || PlanRegisterDateItem.RegisterDate.Month != DateTime.Now.Month)
			{
					var FirstPlanItem = db.Plannns.Where(s => s.Level == 0 && s.PlanTypeID == 1).FirstOrDefault();
					if(FirstPlanItem != null)
					{
						var _userItem = db.MarketerUsers.Where(s => s.Id == user.Id).FirstOrDefault();
						_userItem.PlannnID = FirstPlanItem.Id;
						PlanRegisterDateItem.RegisterDate = DateTime.Now;
						db.SaveChanges();
					}
			}
			}

			if (user.CanCheckPayment)
			{
				CanCheckPayment = true;
			}
			else
			{
				CanCheckPayment = false;
			}
			if (user.CanPromissoryPayment)
			{
				CanPromissoryPayment = true;
			}
			else
			{
				CanPromissoryPayment = false;
			}


			//End
			#region CheckLazyMarketerUser
			#endregion
			return new { StatusCode = 0, user = user ,userPlan=userPlan,userplanType=userplanType, ParentToken = ParentToken == null? "WithOutParent":ParentToken.Api_Token,unreadMessageFromAdmin= UnreadAdminMessageCount(user.Id), UnreadUsersMessageCount = UnreadUsersMessageCount(user.Id), UserCanCheckPayment = CanCheckPayment, UserCanPromissoryPayment = CanPromissoryPayment };
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

                StatusCode = 0
            };

        }
		[HttpGet]
		[Route("api/MarketerUser/ShowPriceForActiveAccountAfterOneYear")]
		public async Task<object> ShowPriceForActiveAccountAfterOneYear()
		{
            var Price =await db.MarketerActiveAccountTickets.FirstOrDefaultAsync();
			if(Price == null)
			{
				return new
				{
					StatusCode = 1,
					Message = "بدون مبلغ"
				};
			}
			return new
			{
				StatusCode = 0,
				Message = Price
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
			bool CanCheckPayment = false;
			bool CanPromissoryPayment = false;

			var user = db.MarketerUsers.Where(p => p.Api_Token == ApiToken).Select(s => new { s.Id, s.AccountNumber, s.Address, userToken = s.Api_Token, s.CardAccountNumber, s.CertificateNumber, s.Description, s.FactorCounter, s.IBNA, s.IDCardNumber, s.IDCardPhotoAddress, s.IsFirstTime, s.IsMarrid, s.LastName, s.Lat, s.Lng, s.Mobile, s.Name, s.PersonalPicture, s.PersonalReagentCode, s.Phone, s.Usertype, s.AcceptedByParent, s.IsAvailable, s.Parent_Id, s.SubsetCount, s.PlannnID,s.CanCheckPayment,s.CanPromissoryPayment }).FirstOrDefault();
            var MarketerLimitSale = db.MarketerLimitSale.FirstOrDefault();


            var userPlan = db.Plannns.Where(s => s.Id == user.PlannnID).Select(p => new { p.Level, p.PlanTypeID,p.Price }).FirstOrDefault();
            var userplanType = db.PlanTypes.Where(s => s.Id == userPlan.PlanTypeID).Select(p => new { p.Name }).FirstOrDefault();

            var result = checkUserDateLogin.ActiveForaYear(user.Id);
            if (result == false)
            {
                return new { StatusCode = 203, Message = "حساب کاربری شما از یکسال گذشته است ، آن را فعال کنید" };
            }
            if (user.Usertype == 0)
            {
                if (MarketerLimitSale != null && (MarketerLimitSale.Enable && MarketerLimitSale != null))
                {
                    if (!user.IsFirstTime)
                    {
                        var CheckResult = checkUserDateLogin.CheckLazyMarketerUser(user.Id);
                        if (CheckResult == false)
                        {
                            return new { StatusCode = 204, Message = "شما به مدت " + MarketerLimitSale.Days + " روز فروش نداشته اید لطفا حساب خود را فعال کنید" };
                        }
                    }
                }
            }
            //CheckPlanRegisterDate
            var PlanRegisterDateItem = db.PlanDateregister.Where(s => s.IDCardNumber == user.IDCardNumber).FirstOrDefault();
			var ParentToken = db.MarketerUsers.Where(s => s.Id == user.Parent_Id).Select(s => new { s.Api_Token }).FirstOrDefault();

			if (PlanRegisterDateItem != null)
			{
				if (PlanRegisterDateItem.RegisterDate.Year != DateTime.Now.Year || PlanRegisterDateItem.RegisterDate.Month != DateTime.Now.Month)
				{
					var FirstPlanItem = db.Plannns.Where(s => s.Level == 0 && s.PlanTypeID == 1).FirstOrDefault();
					if (FirstPlanItem != null)
					{
						var _userItem = db.MarketerUsers.Where(s => s.Id == user.Id).FirstOrDefault();
						_userItem.PlannnID = FirstPlanItem.Id;
						PlanRegisterDateItem.RegisterDate = DateTime.Now;
						db.SaveChanges();
					}
				}
			}
			if (user.CanCheckPayment)
			{
				CanCheckPayment = true;
			}
			else
			{
				CanCheckPayment = false;
			}
			if (user.CanPromissoryPayment)
			{
				CanPromissoryPayment = true;
			}
			else
			{
				CanPromissoryPayment = false;
			}
			//End
			return new { StatusCode = 0, user = user, userPlan = userPlan, userplanType = userplanType , ParentToken = ParentToken == null ? "WithOutParent" : ParentToken.Api_Token, unreadMessageFromAdmin = UnreadAdminMessageCount(user.Id), UnreadUsersMessageCount = UnreadUsersMessageCount(user.Id), UserCanCheckPayment= CanCheckPayment, UserCanPromissoryPayment=CanPromissoryPayment };


        }


		#region ChangePassword
		[HttpPost]
		[Route("api/MarketerUser/ChangePassword")]
		public async Task<object> ChangePassword()
		{
			var ApiToken = HttpContext.Current.Request.Form["Api_Token"];
			var LastPassword = HttpContext.Current.Request.Form["LastPassword"];
			var NewPassword = HttpContext.Current.Request.Form["NewPassword"];
			if(String.IsNullOrEmpty(NewPassword) || String.IsNullOrEmpty(LastPassword))
			{
				return new { status = 1000, message = "فیلدهای مورد نیاز خالی است" };

			}

			var UserItem =await db.MarketerUsers.Where(s => s.Api_Token == ApiToken).FirstOrDefaultAsync();


			if(UserItem == null)
			{
				return new { status = 200, message = "کاربر مورد نظر یافت نشد" };
			}
			bool CurrectPass = db.MarketerUsers.Where(s => s.Id == UserItem.Id).Any(s => s.Password == LastPassword);

			if (!CurrectPass)
			{
				return new { status = 900, message = "رمز قبلی اشتباه است" };
			}
			if(NewPassword.Length < 8)
			{
				return new { status = 901, message = "طول رمز عبور جدید باید حداقل 8 کاراکتر باشد" };
			}

			try
			{
				UserItem.Password = NewPassword;
			await	db.SaveChangesAsync();
				return new { status = 400, message = "تغییر رمز انجام شد" };

			}
			catch (Exception ex)
			{
				throw ex;
			}
		
		}

		#endregion

		public int UnreadAdminMessageCount(int UserId)
		{

			return db.Converstions.Where(s => s.MarketerUserID == UserId && s.State == 1 && s.IsRead == false).Count();
		}

		public int UnreadUsersMessageCount(int UserId)
		{
			int Count = 0;
			foreach (var item in db.UserSavedConversionInfo.Where(s=>s.UserId == UserId).ToList())
			{
				var MessageItem = db.UserConversions.Where(s => s.MessageToken == item.TokenMessage && s.IsRead == false).FirstOrDefault();
				if(MessageItem != null)
				{
					Count++;
				}
			}
			return Count;
		}

        [ResponseType(typeof(Boolean))]
        [HttpPost]
        [Route("api/MarketerUser/UserCheckPeyment")]
        public async Task<IHttpActionResult> UserCheckPeyment(UserCheckPeymentModel userCheckPeymentModel)
        {
            if(String.IsNullOrEmpty(userCheckPeymentModel.Api_Token))
            {
                return new System.Web.Http.Results.ResponseMessageResult(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, new HttpError(ErrorsText.EmptyToken)));
            }
            MarketerUser marketerUser =await db.MarketerUsers.Where(s => s.Api_Token == userCheckPeymentModel.Api_Token).FirstOrDefaultAsync();
            if(marketerUser == null)
            {
                return new System.Web.Http.Results.ResponseMessageResult(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, new HttpError(ErrorsText.UserNotFound)));
            }
            if (marketerUser.CanCheckPayment)
            {
                return Ok(true);
            }
            else
            {
                return Ok(false);

            }
        }
        [ResponseType(typeof(Boolean))]
        [HttpPost]
        [Route("api/MarketerUser/CheckUserPromissoryPayment")]
        public async Task<IHttpActionResult> CheckUserPromissoryPayment(UserCheckPeymentModel userCheckPeymentModel)
        {
            if (String.IsNullOrEmpty(userCheckPeymentModel.Api_Token))
            {
                return new System.Web.Http.Results.ResponseMessageResult(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, new HttpError(ErrorsText.EmptyToken)));
            }
            MarketerUser marketerUser = await db.MarketerUsers.Where(s => s.Api_Token == userCheckPeymentModel.Api_Token).FirstOrDefaultAsync();
            if (marketerUser == null)
            {
                return new System.Web.Http.Results.ResponseMessageResult(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, new HttpError(ErrorsText.UserNotFound)));
            }
            if (marketerUser.CanPromissoryPayment)
            {
                return Ok(true);
            }
            else
            {
                return Ok(false);

            }
        }

        public class UserCheckPeymentModel
        {
            public string Api_Token { get; set; }
        }
        [HttpPost]
        [Route("api/MarketerUser/ForgetPassword")]
        public async Task<IHttpActionResult> ForgetPassword(SearchModel MobileNum)
        {
            SendSms sendSms = new SendSms();
            long n;
            if (!Int64.TryParse(MobileNum.MobileNumber, out n))
            {
                return new System.Web.Http.Results.ResponseMessageResult(
               Request.CreateErrorResponse(HttpStatusCode.InternalServerError, new HttpError(ErrorsText.MobileIncorrectTypeError)));
            }
            long _MobileNum = Int64.Parse(MobileNum.MobileNumber);
            MarketerUser marketerUser =await db.MarketerUsers.Where(s => s.Mobile == MobileNum.MobileNumber).FirstOrDefaultAsync();
            if(marketerUser == null)
            {
                return new System.Web.Http.Results.ResponseMessageResult(
                 Request.CreateErrorResponse(HttpStatusCode.InternalServerError, new HttpError(ErrorsText.UserNotFound)));
            }
            var Token = sendSms.GetToken(sendSms.userApiKey , sendSms.secretKey) ;
            var ultraFastSend = new UltraFastSend()
            {
                Mobile = _MobileNum,
                TemplateId = 26031,
                ParameterArray = new List<UltraFastParameters>()
                {
               new UltraFastParameters()
                  {
                Parameter = "password" , ParameterValue = marketerUser.Password
                 }
              }.ToArray()

            };
            UltraFastSendRespone ultraFastSendRespone = new UltraFast().Send(Token, ultraFastSend);

            if (ultraFastSendRespone.IsSuccessful)
            {
                return new System.Web.Http.Results.ResponseMessageResult(
                                       Request.CreateResponse(HttpStatusCode.OK, SucccessText.SmsSent));
            }
            else
            {         
                return new System.Web.Http.Results.ResponseMessageResult(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, new HttpError(ErrorsText.SmsNotSendt)));
            }
        }
    }
}
