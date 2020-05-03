using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WebApplication1.Models;

namespace WebApplication1.Controllers.api.Marketer
{
    public class PaySettingController : ApiController
    {
        DBContext db = new DBContext();

        [Route("api/PaySetting/GetPaySetting")]
        [HttpPost]
        public async Task<object> GetPaySetting()
        {
            var ApiToken = HttpContext.Current.Request.Form["Api_Token"];

            if (String.IsNullOrEmpty(ApiToken))
            {    
             return new { StatusCode = 300, Message = "توکن خالی است" };
            }
            if (db.MarketerUsers.Where(s => s.Api_Token == ApiToken).FirstOrDefault() == null)
            {
                return new { StatusCode = 302, Message = "کاربر مورد نظر یافت نشد" };

            }
            int UserType = db.MarketerUsers.Where(s => s.Api_Token == ApiToken).FirstOrDefault().Usertype;
            List<PaySetting> paySettings = await db.PaySettings.ToListAsync();

            if (UserType == 0)
            {
                var PriceFortransfare0 = await db.PriceForTranslates.Select(s => new { Darsad = s.MarketerPriceTranslate, Rayegan = s.Marketergratis }).FirstOrDefaultAsync();
                paySettings = null;
                return new
                {
                    StatusCode = 0,
                    PriceFortransfare = PriceFortransfare0,
                    paySettings= paySettings
                };
            }
            if (UserType == 1)
            {
                var PriceFortransfare1 = await db.PriceForTranslates.Select(s => new { Darsad = s.BigBuyerPriceTranslate, Rayegan = s.Buyergratis }).FirstOrDefaultAsync();
                return new
                {
                    StatusCode = 0,
                    PriceFortransfare = PriceFortransfare1,
                    paySettings= paySettings
                };
            }
            if (UserType == 2)
            {
                var PriceFortransfare2 = await db.PriceForTranslates.Select(s => new { Darsad = s.RetailerPriceTranslate, Rayegan = s.Retailergratis }).FirstOrDefaultAsync();
                return new
                {
                    StatusCode = 0,
                    PriceFortransfare = PriceFortransfare2,
                    paySettings = paySettings
                };
            }
            return new { StatusCode = 1, Message = "Error" };
        }

		[Route("api/PaySetting/GetCheckPaymentConditaion")]
		[HttpPost]
		public async Task<IHttpActionResult> GetCheckPaymentConditaion()
		{
			var checkPaymentConditaion = await db.checkPaymentConditaions.ToListAsync();
			return Ok(checkPaymentConditaion);
		}
		[Route("api/PaySetting/GetCreditPayConditations")]
		[HttpPost]
		public async Task<IHttpActionResult> GetCreditPayConditations()
		{
			var creditPayConditations = await db.creditPayConditations.ToListAsync();
			return Ok(creditPayConditations);
		}


	}
}
