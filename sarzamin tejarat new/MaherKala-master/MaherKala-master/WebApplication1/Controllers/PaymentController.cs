using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.Utility;

namespace WebApplication1.Controllers
{
    public class PaymentController : Controller
    {
        DBContext db = new DBContext();
        [Authorize]
        [HttpGet]
        public ActionResult Index(string Address , int? City_Id , long? Price , long? Discount , int? UserId  )
        {
            var cashedProducts = Session["ListProduct"] as List<FactorItem>; 
            Factor factor = new Factor();
           List<FactorItem> factorItem = new List<FactorItem>();
            var usr = db.Users.Where(a => a.Id == UserId).FirstOrDefault();
            var id = usr.Id;        
            int paymentId = 0;
            int transportation = 0;
            var TotalPrice = Price;
            var RedirectPage = "https://sartej.com/Payment/Pay";
            Payment p = new Payment();   
            Setting s = db.Settings.FirstOrDefault();
            factor.Address = Address;
            factor.Buyer = usr.Fullname;
            factor.City_Id = City_Id.Value;
            factor.Date = DateTime.Now;
            factor.Discount_Code = Discount == null ? "0" : Discount.Value.ToString();
            factor.Discount_Amount = Discount == null ? 0 : db.DiscountCode.Where(pd => pd.Code == Discount.Value.ToString()).FirstOrDefault().Price;
            factor.TotalPrice = Price.Value;
            factor.PostalCode = string.IsNullOrEmpty(usr.PostalCode) ? "0" : usr.PostalCode;
            factor.IsAdminShow = false;
            factor.Mobile = usr.Mobile;
            factor.Status = false;
            factor.TransportationFee = City_Id == 1 ? (int)s.TransportationEsfahan : City_Id == 2 ?
                transportation = (int)s.TransportationNajafabad : (int)s.TransportationOther;
            factor.User = usr;
            db.Factors.Add(factor);
            db.Configuration.ValidateOnSaveEnabled = false;
            db.SaveChanges();
            foreach (var item in cashedProducts)
            {
                factorItem.Add(new FactorItem() { Color = item.Color, Factor
                    = factor, Product = item.Product, ProductName = item.ProductName,
                    Qty = item.Qty, UnitPrice = item.UnitPrice });
            }
            db.FactorItems.AddRange(factorItem);
            p.Factor = factor;
            p.User = usr;
            p.Amount =  Price.Value;
            p.StatusPayment = "-100";
            p.PaymentFinished = false;
            p.Date = DateTime.Now;
            p.IsForMarketer = false;
            db.Payments.Add(p);
            db.SaveChanges();
            paymentId = p.Id;
            var client = new BankToken.TokensClient();
            string token = client.MakeToken(p.Amount.ToString(), "HED1", paymentId.ToString(), paymentId.ToString(), "", RedirectPage, "").token;
            var pay = db.Payments.Include("User").Where(q => q.Id == paymentId).FirstOrDefault();
            pay.StatusPayment = token;
            db.SaveChanges();
            if (!string.IsNullOrEmpty(token)&&(token.Length>5))
            {
               
                    pay.ReferenceNumber = token;
                    p.PaymentFinished = false;
                    p.StatusPayment = "-100";
                    db.Entry(p).State = EntityState.Modified;
                    db.SaveChanges();

                    //NameValueCollection collection = new NameValueCollection();
                    //collection.Add("Token", token);
                    //collection.Add("RedirectURL", RedirectPage);
                    //Response.Write(HttpHelper.PreparePOSTForm("https://sep.shaparak.ir/payment.aspx", collection));
                    ViewBag.Url = "https://ikc.shaparak.ir/TPayment/Payment/index";
                    ViewBag.RedirectURL = RedirectPage;
                    ViewBag.Token = token;
                    return View("RedirectToBank");
            }

            p.StatusPayment = token;
            p.ReferenceNumber = null;
            p.PaymentFinished = false;
            db.SaveChanges();
            TempData["BankMessage"] = "درحال حاضر امکان اتصال به درگاه وجود ندارد";
            return Redirect("/Factor/Shipping");
        }
        [HttpPost]
        [Route("/Payment/Pay")]
        public ActionResult Pay()
        {
            int ism = 0;
            if (Request.Form["ResultCode"].ToString().Equals(string.Empty))
            {
                ViewBag.Message = "پاسخی از بانک دریافت نشد";
                return View();
            }
            if (Request.Form["Token"].ToString().Equals(string.Empty))
            {
                ViewBag.Message = "پاسخی از بانک دریافت نشد";
                return View();
            }

            else if (Request.Form["ReferenceId"].ToString().Equals(string.Empty) && Request.Form["state"].ToString().Equals(string.Empty))
            {
                ViewBag.Message = "فرآیند پرداخت با موفقیت صورت گرفت اما امضای دیجیتال به درستی انجام نشد";
                return View();

            }
            else if (Request.Form["InvoiceNumber"].ToString().Equals(string.Empty) && Request.Form["state"].ToString().Equals(string.Empty))
            {
                ViewBag.Message = "خطا در ارتباط با بانک";
                return View();

            }
            else
            {
                string refrenceNumber = string.Empty;
                string token2 = string.Empty;
                string reservationNumber = string.Empty;
                string transactionState = string.Empty;
                token2 = Request.Form["Token"].ToString();
                refrenceNumber = Request.Form["ReferenceId"].ToString();
                reservationNumber = Request.Form["InvoiceNumber"].ToString();
                transactionState = Request.Form["ResultCode"].ToString();
                ViewBag.transactionState = transactionState;
                long paymentId = Convert.ToInt32(reservationNumber);
                var payment = db.Payments.Where(p => p.Id == paymentId).Where(p => p.IsUsed == false).FirstOrDefault();
                if (payment == null)
                {
                    ViewBag.Message = "پرداخت نا معتبر";
                    return View();
                }
                if(payment.IsForMarketer)
                {
                    payment = db.Payments.Include("MarketerUser").Include("MarketerFactor").Where(p => p.Id == paymentId).Where(p => p.IsUsed == false).FirstOrDefault();
                    ism = 1;
                }
                else
                {
                    payment = db.Payments.Include("User").Include("Factor").Where(p => p.Id == paymentId).Where(p => p.IsUsed == false).FirstOrDefault();
                }
                payment.IsUsed = true;
                db.SaveChanges();

                if (transactionState.Equals("100"))
                {
                    ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

                    var srv = new BankVerify.VerifyClient();
                    var result = srv.KicccPaymentsVerification(token2,"HED1",refrenceNumber, "22338240992352910814917221751200141041845518824222260");
                    if (result > 0)
                    {
                        long amount = payment.Amount;
                        if ((long)result == amount)
                        {
                            payment.StatusPayment = transactionState;
                            payment.ReferenceNumber = refrenceNumber;
                            payment.PaymentFinished = true;
                            
                            payment.Date = DateTime.Now;
                            var email = User.Identity.Name;
                            int id = db.Users.Where(p => p.Email == email).FirstOrDefault().Id;

                            if (payment.IsForMarketer)
                            {
                                var order = db.MarketerFactor.Include("MarketerFactorItems.Product.Category").Where(p => p.MarketerUser.Id == id).Where(p => p.Status == 1).FirstOrDefault();
                                order.Status = 0;
                                order.Date = DateTime.Now;
                                payment.MarketerUser.FactorCounter--;
                            }
                            else
                            {
                                var order = db.Factors.Include("FactorItems.Product.Category").Where(p => p.User.Id == id).Where(p => p.Status == false).FirstOrDefault();
                                order.Status = true;
                                order.Date = DateTime.Now;
                            }
                            ViewBag.Message = "پرداخت با موفقیت انجام شد";
                        }         
                    }
                    else
                    {
                        payment.PaymentFinished = false;
                        payment.StatusPayment = result.ToString();
                        payment.ReferenceNumber = refrenceNumber;
                        ViewBag.Message = "خطا در پرداخت";
                    }
                }
                else
                {
                    payment.PaymentFinished = false;
                    payment.StatusPayment = transactionState;
                    payment.ReferenceNumber = refrenceNumber;                 
                        ViewBag.Message = "متاسفانه بانک خرید شما را تایید نکرده است";
                }

            }
            ViewBag.ism = ism;
            db.SaveChanges();
            return View();

        }


    


        }
}


