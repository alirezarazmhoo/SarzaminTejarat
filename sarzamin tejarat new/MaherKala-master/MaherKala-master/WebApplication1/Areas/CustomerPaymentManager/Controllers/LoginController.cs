using SmsIrRestful;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Results;
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
		#region PublicMethods
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
			SendSms sendSms = new SendSms();
			long _Mobile = 0;
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
                    Random random = new Random();
                    int randomNumber = random.Next(100000, 999999);
                    paymentCodes.IsUsed = false;
                    paymentCodes.MarketerUserId = marketerUser.Id;
				if(_Mode == 0)
                    paymentCodes.PaymentCodeType = PaymentCodeType.CheckPaymenyLogin;
				else
					paymentCodes.PaymentCodeType = PaymentCodeType.CreditPaymentLogin;

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
				//#region SendSmS
				_Mobile = Convert.ToInt64(marketerUser.Mobile);
				var Token = sendSms.GetToken(sendSms.userApiKey, sendSms.secretKey);
				var ultraFastSend = new UltraFastSend()
				{
					Mobile = _Mobile,
					TemplateId = _Mode == 0 ? 26504 : 26505,
					ParameterArray = new List<UltraFastParameters>()
				{
				  new UltraFastParameters()
				  {
				Parameter = "password" , ParameterValue = randomNumber.ToString()
				 }
				 }.ToArray()
				};
				UltraFastSendRespone ultraFastSendRespone = new UltraFast().Send(Token, ultraFastSend);
				if (ultraFastSendRespone.IsSuccessful)
				{
					return Json(new { success = true, responseText = SucccessText.SmsSent }, JsonRequestBehavior.AllowGet);
				}
				else
				{
					return Json(new { success = false, responseText = ErrorsText.CantSendSms }, JsonRequestBehavior.AllowGet);
				}
				//#endregion
				//return Json(new { success = true, responseText = SucccessText.SmsSent }, JsonRequestBehavior.AllowGet);
			}
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
					Session["UserInfo"] = marketerUser.Id;
					//return RedirectToAction("Index", "/CustomerPaymentManager/Home/", new { Id = marketerUser.Id });
					//return RedirectToAction(nameof(Home));
					return Json(new { success = true, responseText = Mode}, JsonRequestBehavior.AllowGet);

				
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
		[PaymentAuthorize]
		public ActionResult Home(string Mode)
		{
			int _mode = Int32.Parse(Mode);
			//CheckPayment
			if(_mode == 0)
			{
				return RedirectToAction(nameof(Check));
			}
			if(_mode == 1)
			{
				return RedirectToAction(nameof(Credit));
			}
			return View();
		}
		#endregion
		#region CheckConditations
		[PaymentAuthorize]
		public ActionResult Check() => View();
		[PaymentAuthorize]
		public ActionResult ShowCheckConditations()
		{
			using (DBContext db = new DBContext())
			{
				var url = "/CustomerPaymentManager/Login/ShowCheckConditations";
				var data = db.checkPaymentConditaions.AsQueryable();
				var res = data.OrderByDescending(p => p.Id);
				ViewBag.Data = new PagedItem<CheckPaymentConditaion>(res, url);
				return View();
			}
		}
		[PaymentAuthorize]
		public ActionResult ShowCheckPaymentInformationsAndCreateRequest(int? Id)
		{
			using (DBContext db = new DBContext())
			{
				CheckPaymentConditaion checkPaymentConditaion = db.checkPaymentConditaions.Where(s => s.Id == Id).FirstOrDefault();
				var UserId = Session["UserInfo"];
				Session["ChechPaymentConditationId"] = Id;
				if (checkPaymentConditaion == null )
				{
					return HttpNotFound();
				}
				else
				{
					ViewBag.Description = checkPaymentConditaion.Description;
					ViewBag.checkprice = checkPaymentConditaion.CheckPrice;
					ViewBag.Interestrate = checkPaymentConditaion.Interestrate;
					ViewBag.InitialPayment = checkPaymentConditaion.InitialPayment;
					ViewBag.Type = checkPaymentConditaion.conditaionType;
				}
				int _UserId = Convert.ToInt32(UserId);


				var marketerUser = db.MarketerUsers.Where(s => s.Id == _UserId).Select(s => new { s.Name, s.LastName, s.Mobile, s.Phone, s.TextAddress, s.IDCardNumber }).FirstOrDefault();
				if(marketerUser == null)
				{
					return HttpNotFound();
				}
				else
				{
					ViewBag.Name = marketerUser.Name;
					ViewBag.LastName = marketerUser.LastName;
					ViewBag.Mobile = marketerUser.Mobile;
					ViewBag.Phone = marketerUser.Phone;
					ViewBag.TextAddress = marketerUser.TextAddress;
					ViewBag.IDCardNumber = marketerUser.IDCardNumber;
				}

			}
			return View();
		}
		public async Task<ActionResult> SaveRequestCheckPayment(HttpPostedFileBase[] Images)
		{
			var UserId = Session["UserInfo"];
			MarketerUser UserItem = new MarketerUser();
			var ChechPaymentConditationId = Session["ChechPaymentConditationId"];
			string url = string.Format("/CustomerPaymentManager/Login/ShowCheckPaymentInformationsAndCreateRequest/{0}",(Int32)ChechPaymentConditationId);
			string SavedRequestUrl = "/CustomerPaymentManager/MyCheckPaymentRequests/";
			if (Images[0] == null)
			{
				TempData["Error"] = "عکسی انتخاب نشده است";
				return Redirect(url);
			}	
			using (DBContext db = new DBContext())
			{
				CheckPaymentRequestAttemp checkPaymentRequestAttemp = new CheckPaymentRequestAttemp();
				checkPaymentRequestAttemp.AdminComment = string.Empty;
				checkPaymentRequestAttemp.CheckPaymentRequestAttempStatus = CheckPaymentRequestAttempStatus.Waiting;
				checkPaymentRequestAttemp.CreatedDate = DateTime.Now;
				checkPaymentRequestAttemp.MarketerUserId =(Int32) UserId;
				checkPaymentRequestAttemp.CheckPaymentConditaionId =(Int32)ChechPaymentConditationId;
				checkPaymentRequestAttemp.InitializePricePaymentConditaionType = InitializePricePaymentConditaion.notPayed;
				db.CheckPaymentRequestAttemps.Add(checkPaymentRequestAttemp);
				if (Images != null)
			     {
				foreach (HttpPostedFileBase file in Images)
				{
					if (file != null)
					{
						var InputFileName = Path.GetFileName(file.FileName);
						var ServerSavePath = Path.Combine(Server.MapPath("/Upload/CheckPaymentDocument/") + InputFileName);  
						file.SaveAs(ServerSavePath);
						if (!(file.ContentType == "image/jpeg" || file.ContentType == "image/png" || file.ContentType == "image/bmp"))
						{
							TempData["Error"] = "نوع تصویر غیر قابل قبول است";
							return Redirect(url);
						}
						db.CheckPaymentRequestAttempPictures.Add(new CheckPaymentRequestAttempPictures
						{
							 checkPaymentRequestAttempId = checkPaymentRequestAttemp.Id,
							ImageUrl = "Upload/CheckPaymentDocument/" + InputFileName
						});
					}

				}
			}
				await db.SaveChangesAsync();
				UserItem = db.MarketerUsers.Find(UserId);
				if(UserItem != null)
				{
				_sendSms.CallSmSMethod(_sendSms.AdminMobile, 29345, "MarketerUser", UserItem.Name + " " + UserItem.LastName);
				}
				return Redirect(SavedRequestUrl);
         }
		}
		#endregion
		#region Credit
		[PaymentAuthorize]
		public ActionResult Credit() => View();
		[PaymentAuthorize]
		public ActionResult ShowCreditConditations()
		{
			using (DBContext db = new DBContext())
			{
				var url = "/CustomerPaymentManager/Login/ShowCreditConditations";
				var data = db.creditPayConditations.AsQueryable();
				var res = data.OrderByDescending(p => p.Id);
				ViewBag.Data = new PagedItem<CreditPayConditations>(res, url);
				return View();
			}

		}
		[PaymentAuthorize]
		public ActionResult ShowCreditPaymentInformationsAndCreateRequest(int? Id)
		{
			using (DBContext db = new DBContext())
			{
				CreditPayConditations creditPayConditations = db.creditPayConditations.Where(s => s.Id == Id).FirstOrDefault();
				var UserId = Session["UserInfo"];
				Session["CreditPaymentConditationId"] = Id;
				if (creditPayConditations == null)
				{
					return HttpNotFound();
				}
				else
				{
					ViewBag.Description = creditPayConditations.Description;
					ViewBag.checkprice = creditPayConditations.CheckPrice;
					ViewBag.Interestrate = creditPayConditations.Interestrate;
					ViewBag.InitialPayment = creditPayConditations.InitialPayment;
					ViewBag.Type = creditPayConditations.conditaionType;
				}
				int _UserId = Convert.ToInt32(UserId);
				var marketerUser = db.MarketerUsers.Where(s => s.Id == _UserId).Select(s => new { s.Name, s.LastName, s.Mobile, s.Phone, s.TextAddress, s.IDCardNumber }).FirstOrDefault();
				if (marketerUser == null)
				{
					return HttpNotFound();
				}
				else
				{
					ViewBag.Name = marketerUser.Name;
					ViewBag.LastName = marketerUser.LastName;
					ViewBag.Mobile = marketerUser.Mobile;
					ViewBag.Phone = marketerUser.Phone;
					ViewBag.TextAddress = marketerUser.TextAddress;
					ViewBag.IDCardNumber = marketerUser.IDCardNumber;
				}
			}
			return View();
		}
		[PaymentAuthorize]
		public async Task<ActionResult> SaveRequestCreditPayment(HttpPostedFileBase[] Images)
		{
			MarketerUser UserItem = new MarketerUser();

			var UserId = Session["UserInfo"];
			var CreditPaymentConditationId = Session["CreditPaymentConditationId"];
			string url = string.Format("/CustomerPaymentManager/Login/ShowCreditPaymentInformationsAndCreateRequest/{0}", (Int32)CreditPaymentConditationId);
			string SavedRequestUrl = "/CustomerPaymentManager/MyCreditPaymentRequests/";
			if (Images[0] == null)
			{
				TempData["Error"] = "عکسی انتخاب نشده است";
				return Redirect(url);
			}
			using (DBContext db = new DBContext())
			{
				CreditPaymentRequestAttemp creditPaymentRequestAttemp = new CreditPaymentRequestAttemp();
				creditPaymentRequestAttemp.AdminComment = string.Empty;
				creditPaymentRequestAttemp.CreditPaymentRequestAttempStatus = CreditPaymentRequestAttempStatus.Waiting;
				creditPaymentRequestAttemp.CreatedDate = DateTime.Now;
				creditPaymentRequestAttemp.MarketerUserId = (Int32)UserId;
				creditPaymentRequestAttemp.CreditPayConditationsId = (Int32)CreditPaymentConditationId;
				creditPaymentRequestAttemp.InitializePricePaymentConditaionType = InitializePricePaymentConditaion.notPayed;
				db.CreditPaymentRequestAttemps.Add(creditPaymentRequestAttemp);
				if (Images != null)
				{
					foreach (HttpPostedFileBase file in Images)
					{
						if (file != null)
						{
							var InputFileName = Path.GetFileName(file.FileName);
							var ServerSavePath = Path.Combine(Server.MapPath("~/Upload/CreditPaymentDocument/") + InputFileName);
							file.SaveAs(ServerSavePath);
							if (!(file.ContentType == "image/jpeg" || file.ContentType == "image/png" || file.ContentType == "image/bmp"))
							{
								TempData["Error"] = "نوع تصویر غیر قابل قبول است";
								return Redirect(url);
							}
							db.CreditPaymentRequestAttempPictures.Add(new CreditPaymentRequestAttempPictures
							{
								creditPaymentRequestAttempId = creditPaymentRequestAttemp.Id,
								ImageUrl = "Upload/CreditPaymentDocument/" + InputFileName
							});
						}
					}
				}
				await db.SaveChangesAsync();
				UserItem = db.MarketerUsers.Find(UserId);
				if (UserItem != null)
				{
				_sendSms.CallSmSMethod(_sendSms.AdminMobile, 29345, "MarketerUser", UserItem.Name + " " + UserItem.LastName);
				}
				return Redirect(SavedRequestUrl);
			}
		}
		#endregion
	}
}