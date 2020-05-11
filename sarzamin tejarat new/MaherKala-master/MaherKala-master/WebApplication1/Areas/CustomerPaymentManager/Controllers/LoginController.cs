using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebApplication1.Areas.CustomerPaymentManager.Authorize;
using WebApplication1.Models;
using WebApplication1.Models.Errors;
using WebApplication1.Utility;

namespace WebApplication1.Areas.CustomerPaymentManager.Controllers
{
    public class LoginController : Controller
    {
        // GET: CustomerPaymentManager/Login
		SendSms _sendSms = new SendSms();


		[HttpGet]
        public ActionResult Index(string mode)
        {
			int _mode ;
			if (string.IsNullOrEmpty(mode))
			{
				_mode = 0;
			}
			else
			{
			 _mode = Int32.Parse(mode);

			}
			ViewBag.Mode = _mode;
			return View();
        }
        public JsonResult SendMessage(string Mobile,string Mode)
        {
			int _Mode = Int32.Parse(Mode);
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
				 if(_Mode == 0)
				{
				    if (marketerUser.Usertype == 0 || marketerUser.CanCheckPayment == false)
					{
						return Json(new { success = false, responseText = ErrorsText.Forbiden }, JsonRequestBehavior.AllowGet);
					}
			
				}
				 if(_Mode == 1)
				{
					if (marketerUser.Usertype == 0 || marketerUser.CanPromissoryPayment == false)
					{
						return Json(new { success = false, responseText = ErrorsText.Forbiden }, JsonRequestBehavior.AllowGet);
					}
	
				}
                else
                {
                    Random random = new Random();
                    int randomNumber = random.Next(100000, 999999);
                    paymentCodes.IsUsed = false;
                    paymentCodes.MarketerUserId = marketerUser.Id;
                    paymentCodes.PaymentCodeType = PaymentCodeType.CheckPaymenyLogin;
                    paymentCodes.CreateDate = DateTime.Now;
					paymentCodes.CodeNumber = randomNumber;
					if (_Mode == 0)
					{
						PaymentCodes paymentCodesItem = db.PaymentCodes.Where(s => s.MarketerUserId == marketerUser.Id && s.PaymentCodeType == PaymentCodeType.CheckPaymenyLogin).FirstOrDefault();
						if(paymentCodesItem != null)
						{ 
							db.PaymentCodes.Remove(paymentCodesItem);
						}
					}
					if (_Mode == 1)
					{
						PaymentCodes paymentCodesItem = db.PaymentCodes.Where(s => s.MarketerUserId == marketerUser.Id && s.PaymentCodeType == PaymentCodeType.CreditPaymentLogin).FirstOrDefault();
						if (paymentCodesItem != null)
						{
							db.PaymentCodes.Remove(paymentCodesItem);
						}
					}
					db.PaymentCodes.Add(paymentCodes);
                    db.SaveChanges();			
					//string _Token = _sendSms.GetToken(_sendSms.userApiKey, _sendSms.secretKey);
					//if (!string.IsNullOrEmpty(_Token))
					//{
					//   _sendSms.Send_Sms(_sendSms.MessageForCheckPaymentLoginCode(marketerUser.Name, marketerUser.LastName, randomNumber), marketerUser.Mobile, _Token, _sendSms.LineNumber);
					//}
					//else
					//{
					//    return Json(new { success = false, responseText = ErrorsText.CantSendSms }, JsonRequestBehavior.AllowGet);
					//}
					return Json(new { success = true, responseText = SucccessText.Created }, JsonRequestBehavior.AllowGet);
				}
            }
			return null;
            }

		[HttpPost]
		public ActionResult ConfirmLogin(string Mobile, string Mode,string Pass)
		{
			int _Mode = Int32.Parse(Mode);
			DateTime time = DateTime.Now;
			TimeSpan timeSpan;
			if (String.IsNullOrEmpty(Mobile))
			{
				return Json(new { success = false, responseText = ErrorsText.MobileEmptyError }, JsonRequestBehavior.AllowGet);
			}
			if (CheckNumberValidity(Mobile) == false)
			{
				return Json(new { success = false, responseText = ErrorsText.MobileIncorrectTypeError }, JsonRequestBehavior.AllowGet);
			}
			if (String.IsNullOrEmpty(Pass))
			{
				return Json(new { success = false, responseText = ErrorsText.EmptyPass }, JsonRequestBehavior.AllowGet);
			}
			if (CheckNumberValidity(Pass) == false)
			{
				return Json(new { success = false, responseText = ErrorsText.PassIncorrectTypeError }, JsonRequestBehavior.AllowGet);
			}
			int _Pass = Int32.Parse(Pass);
			using (DBContext db = new DBContext())
			{
				MarketerUser marketerUser = db.MarketerUsers.Where(s => s.Mobile == Mobile).FirstOrDefault();
				if (marketerUser == null)
				{
					return Json(new { success = false, responseText = ErrorsText.MobileNotUserFoundByThisNumber }, JsonRequestBehavior.AllowGet);
				}
				if (_Mode == 0)
				{
					if (marketerUser.Usertype == 0 || marketerUser.CanCheckPayment == false)
					{
						return Json(new { success = false, responseText = ErrorsText.Forbiden }, JsonRequestBehavior.AllowGet);
					}
					if(!db.PaymentCodes.Any(s=>s.MarketerUserId==marketerUser.Id && s.PaymentCodeType == PaymentCodeType.CheckPaymenyLogin  && s.CodeNumber == _Pass))
					{
						return Json(new { success = false, responseText = ErrorsText.InCorrrectInformations }, JsonRequestBehavior.AllowGet);
					}
				}
				if (_Mode == 1)
				{
					if (marketerUser.Usertype == 0 || marketerUser.CanPromissoryPayment == false)
					{
						return Json(new { success = false, responseText = ErrorsText.Forbiden }, JsonRequestBehavior.AllowGet);
					}
					if (!db.PaymentCodes.Any(s => s.MarketerUserId == marketerUser.Id && s.PaymentCodeType == PaymentCodeType.CreditPaymentLogin  && s.CodeNumber == _Pass))
					{
						return Json(new { success = false, responseText = ErrorsText.InCorrrectInformations }, JsonRequestBehavior.AllowGet);
					}
				}
				else
				{
					if (_Mode == 0)
					{
						PaymentCodes paymentCodesItem = db.PaymentCodes.Where(s => s.MarketerUserId == marketerUser.Id && s.PaymentCodeType == PaymentCodeType.CheckPaymenyLogin && s.CodeNumber == _Pass).FirstOrDefault();
						if (paymentCodesItem.IsUsed)
						{
							timeSpan = time - paymentCodesItem.CreateDate;
							if (timeSpan.TotalMinutes > 2)
							{
								db.PaymentCodes.Remove(paymentCodesItem);
								return Json(new { success = false, responseText = ErrorsText.expiredLoginCode }, JsonRequestBehavior.AllowGet);
							}				
						}
					}
					if (_Mode == 1)
					{
						PaymentCodes paymentCodesItem = db.PaymentCodes.Where(s => s.MarketerUserId == marketerUser.Id && s.PaymentCodeType == PaymentCodeType.CreditPaymentLogin && s.CodeNumber == _Pass).FirstOrDefault();
						if (paymentCodesItem.IsUsed)
						{
							timeSpan = time - paymentCodesItem.CreateDate;
							if (timeSpan.TotalMinutes > 2)
							{
								db.PaymentCodes.Remove(paymentCodesItem);
								return Json(new { success = false, responseText = ErrorsText.expiredLoginCode }, JsonRequestBehavior.AllowGet);
							}
						}
					}
					PaymentCodes _paymentCodesItem = db.PaymentCodes.Where(s => s.CodeNumber == _Pass).FirstOrDefault();
					if(_paymentCodesItem != null)
					{
						_paymentCodesItem.IsUsed = true;
						db.SaveChanges();
					}
					FormsAuthentication.SetAuthCookie(marketerUser.Id.ToString(),false);
					//return RedirectToAction("Index", "/CustomerPaymentManager/Home/", new { Id = marketerUser.Id });
					//return RedirectToAction(nameof(Home));
					return Json(new { success = true, responseText = marketerUser.Id }, JsonRequestBehavior.AllowGet);

				}
			}
			return null;
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
		[PaymentAuthorize]
		public ActionResult Home(int?Id)
		{
			//ViewBag.UserId = Id;
			return View();
		}

    }
}