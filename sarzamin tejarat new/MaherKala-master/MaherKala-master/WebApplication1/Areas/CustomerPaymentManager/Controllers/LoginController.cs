using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.Models.Errors;
using WebApplication1.Utility;

namespace WebApplication1.Areas.CustomerPaymentManager.Controllers
{
    public class LoginController : Controller
    {
        // GET: CustomerPaymentManager/Login
		SendSms _sendSms = new SendSms();

        public ActionResult Index(string MobileNumber)
        {

            ViewBag.Mobile = MobileNumber;
            return View();
        }
        public ActionResult SendMessage(string Mobile)
        {
            PaymentCodes paymentCodes = new PaymentCodes();
            if (String.IsNullOrEmpty(Mobile))
            {
                return Json(new { success = false, responseText = ErrorsText.MobileEmptyError }, JsonRequestBehavior.AllowGet);
            }
            if (CheckNumberValidity(Mobile) == false)
            {
                return Json(new { success = false, responseText = ErrorsText.MobileIncorrectTypeError }, JsonRequestBehavior.AllowGet);
            }
            using (DBContext db = new DBContext())
            {
                MarketerUser marketerUser = db.MarketerUsers.Where(s => s.Mobile == Mobile).FirstOrDefault();
                if (marketerUser==null)
                {
                    return Json(new { success = false, responseText = ErrorsText.MobileNotUserFoundByThisNumber }, JsonRequestBehavior.AllowGet);
                }
                else if (marketerUser.Usertype == 0 || marketerUser.CanCheckPayment == false)
                {
                    return Json(new { success = false, responseText = ErrorsText.Forbiden }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Random random = new Random();
                    int randomNumber = random.Next(0, 100000);
                    paymentCodes.IsUsed = false;
                    paymentCodes.MarketerUserId = marketerUser.Id;
                    paymentCodes.PaymentCodeType = PaymentCodeType.CheckPaymenyLogin;
                    paymentCodes.CreateDate = DateTime.Now;
                    db.PaymentCodes.Add(paymentCodes);
                    db.SaveChanges();
                    string _Token = _sendSms.GetToken(_sendSms.userApiKey, _sendSms.secretKey);
                    if (!string.IsNullOrEmpty(_Token))
                    {
                       _sendSms.Send_Sms(_sendSms.MessageForCheckPaymentLoginCode(marketerUser.Name, marketerUser.LastName, randomNumber), marketerUser.Mobile, _Token, _sendSms.LineNumber);
                    }
                    else
                    {
                        return Json(new { success = false, responseText = ErrorsText.CantSendSms }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { success = true, responseText = SucccessText.Created }, JsonRequestBehavior.AllowGet);
                }
            }
            }
            private bool CheckNumberValidity(string Number)
            {
            long n;
            if (!long.TryParse(Number, out n))
            {
                return false;
            }
            else
            {
                return true;
            }
        }


    }
}