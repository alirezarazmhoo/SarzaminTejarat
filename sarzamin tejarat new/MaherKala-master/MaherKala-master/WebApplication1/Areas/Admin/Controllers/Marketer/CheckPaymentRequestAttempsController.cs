using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.Models.ViewModel;
using WebApplication1.Utility;
using SmsIrRestful;
using StructureMap.Diagnostics;
using WebApplication1.Models.Errors;

namespace WebApplication1.Areas.Admin.Controllers.Marketer
{
    public class CheckPaymentRequestAttempsController : Controller
    {
        private DBContext db = new DBContext();
        SendSms _sendSms = new SendSms();

        // GET: Admin/CheckPaymentRequestAttemps
        public async Task<ActionResult> Index()
        {
            var url = "/Admin/CheckPaymentRequestAttemps/Index";
          
            var checkPaymentRequestAttemps = db.CheckPaymentRequestAttemps.Include(c => c.CheckPaymentConditaion).Include(c => c.MarketerUser).AsQueryable();
            var res = checkPaymentRequestAttemps.OrderByDescending(p => p.Id);

            ViewBag.Data = new PagedItem<CheckPaymentRequestAttemp>(res, url);
            return View();


        }

        public async Task<ActionResult> Details(int Id)
		{
            CheckPaymentRequestAttempViewModel checkPaymentRequestAttempViewModel = new CheckPaymentRequestAttempViewModel();
            CheckPaymentRequestAttemp checkPaymentRequestAttemp =await db.CheckPaymentRequestAttemps.Where(s => s.Id == Id).FirstOrDefaultAsync();
            List<CheckPaymentRequestAttempPictures> checkPaymentRequestAttempPictures =await db.CheckPaymentRequestAttempPictures.Where(s => s.checkPaymentRequestAttempId == Id).ToListAsync();
            checkPaymentRequestAttempViewModel.CheckPaymentRequestAttempPictures = checkPaymentRequestAttempPictures;
            checkPaymentRequestAttempViewModel.AdminComment = checkPaymentRequestAttemp.AdminComment;
            checkPaymentRequestAttempViewModel.CheckPaymentConditaion =await db.checkPaymentConditaions.Where(s=>s.Id == checkPaymentRequestAttemp.CheckPaymentConditaionId).FirstOrDefaultAsync();
            checkPaymentRequestAttempViewModel.CheckPaymentConditaionId = checkPaymentRequestAttemp.CheckPaymentConditaionId;
            checkPaymentRequestAttempViewModel.CheckPaymentRequestAttempStatus = checkPaymentRequestAttemp.CheckPaymentRequestAttempStatus;
            checkPaymentRequestAttempViewModel.CreatedDate = checkPaymentRequestAttemp.CreatedDate;
            checkPaymentRequestAttempViewModel.Id = checkPaymentRequestAttemp.Id;
            checkPaymentRequestAttempViewModel.InitializePricePaymentConditaionType = checkPaymentRequestAttemp.InitializePricePaymentConditaionType;
            checkPaymentRequestAttempViewModel.MarketerUser =await db.MarketerUsers.Where(s=>s.Id == checkPaymentRequestAttemp.MarketerUserId).FirstOrDefaultAsync();
            checkPaymentRequestAttempViewModel.MarketerUserId = checkPaymentRequestAttemp.MarketerUserId;
            return View(checkPaymentRequestAttempViewModel);
		}
        [HttpPost]
        public async Task<ActionResult> UpdateRequestAttemp(int status , int IdRequestAttemp, string AdminComment , int MarketerUserId,long Price)
        {
            CheckPaymentRequestAttemp checkPaymentRequestAttemp = db.CheckPaymentRequestAttemps.Find(IdRequestAttemp);
            MarketerUser marketerUser = db.MarketerUsers.Find(MarketerUserId);
            PaymentCodes paymentCodes = new PaymentCodes();
            string _Token = string.Empty;
            Random random = new Random();
            int randomNumber = random.Next(100000, 999999);
            if (checkPaymentRequestAttemp != null && marketerUser !=null)
			{
                 checkPaymentRequestAttemp.CheckPaymentRequestAttempStatus = status == 1 ? CheckPaymentRequestAttempStatus.Confirm : CheckPaymentRequestAttempStatus.Rejected;
				if (!String.IsNullOrEmpty(AdminComment))
				{
                    
                checkPaymentRequestAttemp.AdminComment = AdminComment.Trim();

                }
                if(status == 1)
				{
                 _Token = _sendSms.GetToken(_sendSms.userApiKey, _sendSms.secretKey);

                 var ultraFastSend = new UltraFastSend()
                  {
                        Mobile =Convert.ToInt64( marketerUser.Mobile),
                        TemplateId = 28817,
                     ParameterArray = new List<UltraFastParameters>()
                {
               new UltraFastParameters()
                 {
                   Parameter = "PaymentCode" , ParameterValue = randomNumber.ToString()
                 }
                 }.ToArray()

                    };
                    UltraFastSendRespone ultraFastSendRespone = new UltraFast().Send(_Token, ultraFastSend);

                    if (ultraFastSendRespone.IsSuccessful)
                    {
                        //Sucess
                        paymentCodes.IsUsed = false;
                        paymentCodes.MarketerUserId = marketerUser.Id;
                        paymentCodes.PaymentCodeType = PaymentCodeType.PaymentDiscountCode;
                        paymentCodes.Price = Price;
                        paymentCodes.CodeNumber = randomNumber;
                        paymentCodes.CreateDate = DateTime.Now;
                        db.PaymentCodes.Add(paymentCodes);            
                        await db.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        //Fail
                        paymentCodes.IsUsed = false;
                        paymentCodes.MarketerUserId = marketerUser.Id;
                        paymentCodes.PaymentCodeType = PaymentCodeType.PaymentDiscountCode;
                        paymentCodes.Price = Price;
                        paymentCodes.CodeNumber = randomNumber;
                        paymentCodes.CreateDate = DateTime.Now;
                        db.PaymentCodes.Add(paymentCodes);
                        TempData["Error"] = ErrorsText.SmsNotSendtButSaved;
                        await db.SaveChangesAsync();
                        string url = string.Format("../CheckPaymentRequestAttemps/Details?Id={0}", IdRequestAttemp);
                        return Redirect(url);
                    }
                }
				else
				{
                  _Token = _sendSms.GetToken(_sendSms.userApiKey, _sendSms.secretKey);
                    var ultraFastSend = new UltraFastSend()
                    {
                        Mobile = Convert.ToInt64(marketerUser.Mobile),
                        TemplateId = 28818,
                        ParameterArray = new List<UltraFastParameters>()
                {
               new UltraFastParameters()
                 {
                   Parameter = "MarketerUser" , ParameterValue =marketerUser.Name + " "+ marketerUser.LastName
                 }
                 }.ToArray() };
                    UltraFastSendRespone ultraFastSendRespone = new UltraFast().Send(_Token, ultraFastSend);
                    await db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            return RedirectToAction(nameof(Index));
        
        }



    }
}
