using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.Utility;
using WebApplication1.Utility.Payment;

namespace WebApplication1.Controllers.api.Remitment
{
    public class DisbursementController : Controller
    {
		DBContext db = new DBContext();

		public object DisbursementResult(int? Id)
		{
            MarketerFactor factorItem = new MarketerFactor();
            factorItem = db.MarketerFactor.FirstOrDefault(s => s.Id == Id);
            if(factorItem != null)
			{
            var ZarinPalItem = Purshes.GetResult(factorItem.TotalPrice, Id.Value); 
            if (ZarinPalItem.status == 100)
			{
				Response.Redirect(Purshes.ZarinPalTestUrl + ZarinPalItem.authority);
                    return null;
                }
                return null;
			}
			else
			{
                return HttpNotFound();
			}
		}
        public ActionResult Verify(int? id)
        {
            MarketerFactor factorItem = new MarketerFactor();
            factorItem = db.MarketerFactor.FirstOrDefault(s => s.Id == id);
            if (!string.IsNullOrEmpty(Request.QueryString["Status"]) && !string.IsNullOrEmpty(Request.QueryString["Authority"]))
            {
                if (Request.QueryString["Status"].Equals("OK"))
                {
                    long refId;
                    //ServicePointManager.Expect100Continue = false;
                    //ZarinPalTest.PaymentGatewayImplementationServicePortTypeClient client = new ZarinPalTest.PaymentGatewayImplementationServicePortTypeClient();
                    //int status = client.PaymentVerification(Purshes.MerchantID, Request.QueryString["Authority"], amount, out refId);
                    var ZarinPalItem = Purshes.Verification(factorItem.TotalPrice,factorItem.Id , Request.QueryString["Authority"] ,out refId);
                    if(factorItem != null)
					{
                        if (ZarinPalItem.status == 100 || ZarinPalItem.status == 101)
                        {
							switch (factorItem.FactorType)
							{
                                case FactorTypes.MarketerFactor:
                                    factorItem.Status = 2;
                                    db.SaveChanges();
                                    break; 
                            }
                            ViewBag.RefId = "کد پیگیری: " + refId + " - کد سفارش: " + id;
                        }
                    }        
                    else
                    {
                        ViewBag.Message = PaymentResult.GetMessage(ZarinPalItem.status);
                    }
                }
                else
                {
                    ViewBag.Message = "کد مرجع: " + Request.QueryString["Authority"] + " - وضعیت:" + Request.QueryString["Status"];
                }
            }
            else
            {
                ViewBag.Message = "ورودی نامعتبر است.";
            }
            return View();
        }
    }
}
