using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebApplication1.Filter;
using WebApplication1.Models;

namespace WebApplication1.Controllers.api
{
    public class PaymentController : ApiController
    {
        DBContext db = new DBContext();

        [HttpPost]
        [ApiAuthorize]
        [Route("api/Payment/Discount/Code")]

        public object DiscountCode()
        {
            var dc = HttpContext.Current.Request.Form["DiscountCode"];
            DiscountCode d = db.DiscountCode.Where(x => x.Code == dc && x.Status == true && x.ExpireDate >= DateTime.Now).FirstOrDefault();
            if (d == null)
            {
                return new
                {
                    StatusCode = 1,
                    Message = "کد تخفیف اشتباه است",
                    Amount = 0
                };

            }
            else
            {
                var utoken = HttpContext.Current.Request.Form["Api_Token"];
                var user = db.Users.Where(x => x.Api_Token == utoken).FirstOrDefault();
                var id = user.Id;

                var order = db.Factors.Include("FactorItems.Product.Category").Where(w => w.User.Id == id).Where(w => w.Status == false).FirstOrDefault();
                if (order.ComputeTotalPrice() + order.Discount_Amount - d.Price < 1000)
                {
                    return new
                    {
                        StatusCode = 2,
                        Message = "امکان انجام این عملیات وجود ندارد"
                    };
                }
                order.Discount_Code = d.Code;
                order.Discount_Amount = d.Price;
                db.SaveChanges();
                return new
                {
                    StatusCode = 0,
                    Amount = d.Price
                };
            }
        }
        [Route("api/Payment/Discount/Remove")]
        [ApiAuthorize]
        [HttpPost]
        public object RemoveDiscountCode()
        {
            var utoken = HttpContext.Current.Request.Form["Api_Token"];
            var user = db.Users.Where(x => x.Api_Token == utoken).FirstOrDefault();
            var id = user.Id;
            var order = db.Factors.Include("FactorItems.Product.Category").Where(w => w.User.Id == id).Where(w => w.Status == false).FirstOrDefault();
            int tmp = order.Discount_Amount;
            order.Discount_Amount = 0;
            order.Discount_Code = null;

            return new
            {
                DecreaseAmount = tmp,
                Message = 0
            };
        }
        [HttpPost]
        [ApiAuthorize]
        [Route("api/Payment/Token")]

        public object GetToken()
        {


            string PostalCode = HttpContext.Current.Request.Form["PostalCode"];
            if (PostalCode.Length != 10)
            {
                return new
                {
                    StatusCode = 1,
                    Message = "طول کدپستی باید ده رقم باشد"
                };
            }

            string PhoneNumber = HttpContext.Current.Request.Form["PhoneNumber"];
            if (PhoneNumber.Length > 15 || PhoneNumber.Length < 7)
            {
                return new
                {
                    StatusCode = 1,
                    Message = "طول ارقام تلفن مجاز نیست"
                };
            }

            string Mobile = HttpContext.Current.Request.Form["Mobile"];
            if (PhoneNumber.Length != 11)
            {
                return new
                {
                    StatusCode = 1,
                    Message = "طول ارقام موبایل مجاز نیست"
                };
            }

            string Address = HttpContext.Current.Request.Form["Address"];
            if (Address.Length > 1000 || Address.Length < 5)
            {
                return new
                {
                    StatusCode = 1,
                    Message = "طول آدرس مجاز نیست"
                };
            }
            int City_Id = Convert.ToInt32(HttpContext.Current.Request.Form["City_Id"]);
            if (City_Id != 1 && City_Id != 2 && City_Id != 3)
            {
                return new
                {
                    StatusCode = 1,
                    Message = "شهر را انتخاب کنید"
                };

            }


            Setting setting = db.Settings.FirstOrDefault();
            int transportation = 0;
            if (City_Id == 1)
            {
                transportation = (int)setting.TransportationEsfahan;
            }
            else if (City_Id == 2)
            {
                transportation = (int)setting.TransportationNajafabad;
            }
            else if (City_Id == 3)
            {
                transportation = (int)setting.TransportationOther;
            }

            var utoken = HttpContext.Current.Request.Form["Api_Token"];
            var user = db.Users.Where(x => x.Api_Token == utoken).FirstOrDefault();
            var id = user.Id;

            var order = db.Factors.Include("FactorItems.Product.Category").Where(w => w.User.Id == id).Where(w => w.Status == false).FirstOrDefault();
            if (order == null)
            {
                return new
                {
                    StatusCode = 2,
                    Message = "فاکتوری وجود ندارد"
                };
            }
            if (order.FactorItems == null || order.FactorItems.Count == 0)
            {
                return new
                {
                    StatusCode = 2,
                    Message = "آیتمی انتخاب نشده"
                };
            }
            foreach (var f in order.FactorItems)
            {
                f.UnitPrice = f.Product.Price - f.Product.Discount;
            }



            order.TransportationFee = transportation;
            order.Date = DateTime.Now;
            order.Address = Address;
            order.Buyer = user.Fullname;
            order.Mobile = Mobile;
            order.PostalCode = PostalCode;
            order.TotalPrice = order.ComputeTotalPrice();
            try
            {

                db.SaveChanges();
            }

            catch (DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors
                   .SelectMany(x => x.ValidationErrors)
                   .Select(x => x.ErrorMessage);
                return new
                {
                    StatusCode = 2,
                    Message = errorMessages
                };
            }

            //payment

            int paymentId = 0;

            
            Models.Payment p = new Models.Payment();
            p.User = user;
            p.Amount = order.ComputeTotalPrice() * 10;
            p.StatusPayment = "-100";
            p.PaymentFinished = false;
            p.Date = DateTime.Now;
            p.Factor = order;
            p.IsForMarketer = false;
            db.Payments.Add(p);
            db.SaveChanges();
            paymentId = p.Id;

            var RedirectPage = "https://sarzamintejarat.com/Payment/Pay";

            var url = "https://ikc.shaparak.ir/TPayment/Payment/index";

            var client = new BankToken.TokensClient();
            string token = client.MakeToken(p.Amount.ToString(), "HED1", paymentId.ToString(), paymentId.ToString(), "",RedirectPage , "").token;
            var pay = db.Payments.Include("User").Where(q => q.Id == paymentId).FirstOrDefault();
            pay.StatusPayment = token;
            db.SaveChanges();

            if (!string.IsNullOrEmpty(token) && (token.Length > 5))
            {

                pay.ReferenceNumber = token;
                p.PaymentFinished = false;
                p.StatusPayment = "-100";
                db.Entry(p).State = EntityState.Modified;
                db.SaveChanges();

                return new {StatusCode=0,Url= url,Token = token, MerchantId="HED1" };
               
            }

            p.StatusPayment = token;
            p.ReferenceNumber = null;
            p.PaymentFinished = false;
            db.SaveChanges();
            
            return new
            {
                StatusCode = 2,
                Message = "درحال حاضر امکان اتصال به درگاه وجود ندارد"
            };
        }

        [HttpPost]
        [Route("api/Transportation/Fee")]
        public object GetTransportation()
        {
            Setting s = db.Settings.FirstOrDefault();
            var obj = new
            {
                s.TransportationEsfahan,
                s.TransportationNajafabad,
                s.TransportationOther
            };

            return new { StatusCode = 0, Data = obj };
        }

        [HttpPost]
        [MarketerAuthorize]
        [Route("api/Marketer/Payment/Token")]

        public object GetTokenForMarketer()
        {
            var tr = db.Database.BeginTransaction();
            var mtoken = HttpContext.Current.Request.Form["Api_Token"];
            var fid = Convert.ToInt32(HttpContext.Current.Request.Form["Factor_Id"]);

            var usr = db.MarketerUsers.Where(w => w.Api_Token == mtoken).FirstOrDefault();
            int id = usr.Id;
            var factor = db.MarketerFactor.Include("MarketerFactorItems.Product.Category").Where(x => x.Id == fid).Where(x => x.Status == 1).Where(x => x.MarketerUser.Id == id).FirstOrDefault();
            if (factor == null)
            {
                return new { Message = 1 };
            }
            List<object> Empty = new List<object>();
            foreach (var item in factor.MarketerFactorItems)
            {
                item.UnitPrice = item.Product.Price - item.Product.Discount;
                item.ProductName = item.Product.Name;
                if (item.Product.Qty < item.Qty)
                    Empty.Add(new { Detail = "محصول " + item.Product.Name + " به تعداد انتخابی شما وجود ندارد" });
            }
            if (Empty.Count > 0)
                return new { Message = 2, Empty };
            factor.TotalPrice = factor.ComputeTotalPrice();
            
            db.SaveChanges();
            tr.Commit();

            int paymentId = 0;


            Models.Payment p = new Models.Payment();
            p.MarketerUser = usr;
            p.Amount = factor.ComputeTotalPrice() * 10;
            p.StatusPayment = "-100";
            p.PaymentFinished = false;
            p.Date = DateTime.Now;
            p.MarketerFactor= factor;
            p.IsForMarketer = true;
            db.Payments.Add(p);
            db.SaveChanges();
            paymentId = p.Id;

            var RedirectPage = "https://sarzamintejarat.com/Payment/Pay";

            var url = "https://ikc.shaparak.ir/TPayment/Payment/index";

            var client = new BankToken.TokensClient();
            string token = client.MakeToken(p.Amount.ToString(), "HED1", paymentId.ToString(), paymentId.ToString(), "", RedirectPage, "").token;
            var pay = db.Payments.Include("MarketerUser").Where(q => q.Id == paymentId).FirstOrDefault();
            pay.StatusPayment = token;
            db.SaveChanges();

            if (!string.IsNullOrEmpty(token) && (token.Length > 5))
            {

                pay.ReferenceNumber = token;
                p.PaymentFinished = false;
                p.StatusPayment = "-100";
                db.Entry(p).State = EntityState.Modified;
                db.SaveChanges();

                return new { StatusCode = 0, Url = url, Token = token, MerchantId = "HED1" };

            }

            p.StatusPayment = token;
            p.ReferenceNumber = null;
            p.PaymentFinished = false;
            db.SaveChanges();

            return new
            {
                StatusCode = 2,
                Message = "درحال حاضر امکان اتصال به درگاه وجود ندارد"
            };
        }
    }
}
