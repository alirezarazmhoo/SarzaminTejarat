using SmsIrRestful;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.Models.Errors;
using WebApplication1.Models.ViewModel;
using WebApplication1.Utility;

namespace WebApplication1.Areas.Admin.Controllers.Marketer
{
    public class CreditPaymentRequestAttempsController : Controller
    {
        private DBContext db = new DBContext();
        SendSms _sendSms = new SendSms();
        // GET: Admin/CreditPaymentRequestAttemps
        public ActionResult Index()
        {
            var url = "/Admin/CreditPaymentRequestAttemps/Index";

            var creditPaymentRequestAttemps = db.CreditPaymentRequestAttemps.Include("CreditPayConditations").Include("MarketerUser").AsQueryable();
            var res = creditPaymentRequestAttemps.OrderByDescending(p => p.Id);
            ViewBag.Data = new PagedItem<CreditPaymentRequestAttemp>(res, url);
            return View();
        }

        public async Task<ActionResult> Details(int Id)
        {
            CreditPaymentRequestAttempViewModel CreditPaymentRequestAttempViewModel = new CreditPaymentRequestAttempViewModel();
            CreditPaymentRequestAttemp creditPaymentRequestAttemp = await db.CreditPaymentRequestAttemps.Where(s => s.Id == Id).FirstOrDefaultAsync();
            List<CreditPaymentRequestAttempPictures> creditPaymentRequestAttempPictures = await db.CreditPaymentRequestAttempPictures.Where(s => s.creditPaymentRequestAttempId == Id).ToListAsync();
            CreditPaymentRequestAttempViewModel.creditPaymentRequestAttempPictures = creditPaymentRequestAttempPictures;
            CreditPaymentRequestAttempViewModel.AdminComment = creditPaymentRequestAttemp.AdminComment;
            CreditPaymentRequestAttempViewModel.CreditPayConditations = await db.creditPayConditations.Where(s => s.Id == creditPaymentRequestAttemp.CreditPayConditationsId).FirstOrDefaultAsync();
            CreditPaymentRequestAttempViewModel.CreditPayConditationsId = creditPaymentRequestAttemp.CreditPayConditationsId;
            CreditPaymentRequestAttempViewModel.CreditPaymentRequestAttempStatus = creditPaymentRequestAttemp.CreditPaymentRequestAttempStatus;
            CreditPaymentRequestAttempViewModel.CreatedDate = creditPaymentRequestAttemp.CreatedDate;
            CreditPaymentRequestAttempViewModel.Id = creditPaymentRequestAttemp.Id;
            CreditPaymentRequestAttempViewModel.InitializePricePaymentConditaionType = creditPaymentRequestAttemp.InitializePricePaymentConditaionType;
            CreditPaymentRequestAttempViewModel.MarketerUser = await db.MarketerUsers.Where(s => s.Id == creditPaymentRequestAttemp.MarketerUserId).FirstOrDefaultAsync();
            CreditPaymentRequestAttempViewModel.MarketerUserId = creditPaymentRequestAttemp.MarketerUserId;
            return View(CreditPaymentRequestAttempViewModel);
        }
        [HttpPost]
        public async Task<ActionResult> UpdateRequestAttemp(int status, int IdRequestAttemp, string AdminComment, int MarketerUserId, long Price)
        {
            CreditPaymentRequestAttemp creditPaymentRequestAttemp = db.CreditPaymentRequestAttemps.Find(IdRequestAttemp);
            MarketerUser marketerUser = db.MarketerUsers.Find(MarketerUserId);
            PaymentCodes paymentCodes = new PaymentCodes();
            string _Token = string.Empty;
            Random random = new Random();
            int randomNumber = random.Next(100000, 999999);
            if (creditPaymentRequestAttemp != null && marketerUser != null)
            {
                creditPaymentRequestAttemp.CreditPaymentRequestAttempStatus = status == 1 ? CreditPaymentRequestAttempStatus.Confirm : CreditPaymentRequestAttempStatus.Rejected;
                if (!String.IsNullOrEmpty(AdminComment))
                {
                   creditPaymentRequestAttemp.AdminComment = AdminComment.Trim();
                }
                if (status == 1)
                {
                    _Token = _sendSms.GetToken(_sendSms.userApiKey, _sendSms.secretKey);

                    var ultraFastSend = new UltraFastSend()
                    {
                        Mobile = Convert.ToInt64(marketerUser.Mobile),
                        TemplateId = 288172,
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
                        paymentCodes.PaymentCodeType = PaymentCodeType.CreditDiscountCode;
                        paymentCodes.Price = Price;
                        paymentCodes.CodeNumber = randomNumber;
                        paymentCodes.CreateDate = DateTime.Now;
                        db.PaymentCodes.Add(paymentCodes);
                        TempData["Error"] = ErrorsText.SmsNotSendtButSaved;
                        await db.SaveChangesAsync();
                        string url = string.Format("../CreditPaymentRequestAttemps/Details?Id={0}", IdRequestAttemp);
                        return Redirect(url);
                    }
                }
                else
                {
                    _Token = _sendSms.GetToken(_sendSms.userApiKey, _sendSms.secretKey);
                    var ultraFastSend = new UltraFastSend()
                    {
                        Mobile = Convert.ToInt64(marketerUser.Mobile),
                        TemplateId = 288182,
                        ParameterArray = new List<UltraFastParameters>()
                {
               new UltraFastParameters()
                 {
                   Parameter = "MarketerUser" , ParameterValue =marketerUser.Name + " "+ marketerUser.LastName
                 }
                 }.ToArray()
                    };
                    UltraFastSendRespone ultraFastSendRespone = new UltraFast().Send(_Token, ultraFastSend);
                    await db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}