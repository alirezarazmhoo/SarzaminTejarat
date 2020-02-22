using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WebApplication1.Models;
using WebApplication1.Utility;

namespace WebApplication1.Controllers.api.Marketer
{
    [Route("api/MarketerAddSubSet")]
    public class MarketerAddSubSetController : ApiController
    {
        DBContext db = new DBContext();
		SendSms _sendSms = new SendSms();

		#region AddSubSetByPoints
		[Route("api/MarketerAddSubSet/AddSubSetByPoints")]
        [HttpPost]
        public async Task<object> AddSubSetByPoints()
        {
            var ApiToken = HttpContext.Current.Request.Form["Api_Token"];
            var User = db.MarketerUsers.Where(p => p.Api_Token == ApiToken).FirstOrDefault();
            var PricePointForAddSubset = db.pricePointForAddSubsets.FirstOrDefault();
            double Minimum = PricePointForAddSubset.MinimumPrice;
            double TotalResult = 0;
            if (User == null)
            {
                return Json("UserNotFound");
            }
            if (User.Usertype != 0)
            {
                return new { StatusCode = 401, Message = "انجام این اکشن فقط برای بازاریاب مجاز است" };
            }
            var UserPoints = db.MarketerUserPoints.Where(c => c.MarketerUserId == User.Id).FirstOrDefault();

            if (UserPoints == null)
            {
                return new { StatusCode =402 , Message = "شما امتیازی جهت اضافه کردن زیرمجموعه ندارید " };
            }
            else
            {
                double UserTotalPoints = UserPoints.UserPoits;
                if (UserPoints.UserPoits < PricePointForAddSubset.MinimumPrice)
                {
                    return new { StatusCode = 403, Message = "امتیاز شما کمتر از سقف تعیین شده است " };

                }
                if (UserPoints.UserPoits >= PricePointForAddSubset.MinimumPrice)
                {
                    User.SubsetCount += 1;
                    TotalResult = UserTotalPoints - Minimum;
                    UserPoints.UserPoits = TotalResult;
                }

            }

            try
            {
                await db.SaveChangesAsync();
                return new { StatusCode = 0, Message = "عملیات افزایش عضو انجام شد" };

            }
            catch (DbUpdateConcurrencyException)
            {
                throw;

            }





        }

        #endregion
        #region AddSubSetByPursheTicket
        /*این اکشن نیاز به درگاه بانکی دارد*/
        [Route("api/MarketerAddSubSet/AddSubSetByPursheTicket")]
        [HttpPost]
        public async Task<object> AddSubSetByPursheTicket()
        {

            //            if (اجرای کد های زیر در صورتی که پرداخت بانکی با موفقیت انجام شد)
            //{



            int TicketId = Convert.ToInt32(HttpContext.Current.Request.Form["TicketId"]);
            var ApiToken = HttpContext.Current.Request.Form["Api_Token"];
            var User = db.MarketerUsers.Where(p => p.Api_Token == ApiToken).FirstOrDefault();
            var FindedTicket = db.RateOfAddSubSets.Where(p => p.Id == TicketId).FirstOrDefault();
            int TicketPoint = 0;
            int UserSubsetount = 0;
            if (User == null)
            {
                return new { StatusCode = 200, Message = "کاربر مورد نظر یافت نشد!" };

            }
            if (User.Usertype != 0)
            {
                return new { StatusCode = 401, Message = "انجام این اکشن فقط برای بازاریاب مجاز است" };
            }
            if (FindedTicket == null)
            {
                return new { StatusCode = 403, Message = "تیکت مورد نظر یافت نشد!" };

            }
            TicketPoint = FindedTicket.AddSubsetCounts;
            UserSubsetount = User.SubsetCount;
            User.SubsetCount = UserSubsetount + TicketPoint;
			string _Token = _sendSms.GetToken(_sendSms.userApiKey, _sendSms.secretKey);
			if (!string.IsNullOrEmpty(_Token))
			{
				_sendSms.Send_Sms(_sendSms.CreateUserPursheForAddSubsetMessage(User.Name, User.LastName,FindedTicket.AddSubsetCounts.ToString()), User.Mobile, _Token, _sendSms.LineNumber);	
			}
			try
            {
                await db.SaveChangesAsync();
                return new { StatusCode = 0, Message = "عملیات افزایش عضو انجام شد" };

            }
            catch (DbUpdateConcurrencyException)
            {
                throw;

            }
		
            //}

        }

        #endregion

        #region Show Total SubSets 
        [Route("api/MarketerAddSubSet/ShowTotalSubSets")]
        [HttpPost]
        public object ShowTotalSubSetsAsync()
        {

            var ApiToken = HttpContext.Current.Request.Form ["Api_Token"];

            var MarketerUSerUser =  db.MarketerUsers.Where(p => p.Api_Token == ApiToken).FirstOrDefault();
            if (MarketerUSerUser == null)
            {
                return new { StatusCode = 200, Message = "کاربر مورد نظر یافت نشد!" };

            }
            if(MarketerUSerUser.Usertype != 0)
            {
                return new { StatusCode = 401, Message = "انجام این اکشن فقط برای بازاریاب مجاز است!" };

            }

            var listSubsets = db.MarketerUsers.Where(p => p.Parent_Id == MarketerUSerUser.Id && p.IsAvailable && p.AcceptedByParent ).Select(s=>new { s.Id,s.IBNA,s.IDCardNumber,s.IDCardPhotoAddress,s.IsAvailable,s.IsFirstTime,s.Islazy,s.IsMarrid,s.LastName,s.Lat,s.Lng,s.Mobile,s.Name,s.Parent_Id,s.Password,s.PersonalPicture,s.PersonalReagentCode,s.Phone,s.PlannnID,s.SubsetCount,s.Usertype,s.AcceptedByParent,s.AccountNumber,s.Address,userToken=s.Api_Token,s.CardAccountNumber,s.CertificateNumber,s.CreatedDate,s.Description}).ToList();




            var thismonth = DateTime.Now.Month;
            var thisYear = DateTime.Now.Year;
            if (!(db.MarketerUsers.Any(s => s.Api_Token == ApiToken)))
            {
                return new { StatusCode = 200, Message = "کاربر مورد نظر یافت نشد!" };

            }

            var ListSell = from s in db.MarketerFactorItem where s.MarketerFactor.MarketerUser.Api_Token==ApiToken && s.MarketerFactor.Date.Year==thisYear && s.MarketerFactor.Date.Year==thisYear select new { s.UnitPrice, s.Qty };

             ListSell.ToList();
            double TotalSell = 0;
            foreach (var item in ListSell)
            {
                TotalSell += item.UnitPrice * item.Qty;

            }

            var ListSellInAllyears = from s in db.MarketerFactorItem where s.MarketerFactor.MarketerUser.Api_Token==ApiToken select new { s.UnitPrice, s.Qty };

             ListSell.ToList();
            double TotalSellInAllYears = 0;
            foreach (var item in ListSell)
            {
                TotalSellInAllYears += item.UnitPrice * item.Qty;

            }



            return new { listSubsets=listSubsets, TotalSellInMonth = TotalSell, TotalSellInAllYears = TotalSellInAllYears };

        }

        #endregion
        #region Show Count Of SubSets

        [Route("api/MarketerAddSubSet/ShowCountOfSubSets")]
        [HttpPost]
        public async Task<object> ShowCountOfSubSets()
        {
            var ApiToken = HttpContext.Current.Request.Form["Api_Token"];

            var user = db.MarketerUsers.Where(s => s.Api_Token == ApiToken).FirstOrDefault();
            if(user == null)
            {
            
                    return new { StatusCode = 200, Message = "کاربر مورد نظر یافت نشد!" };

                
            }
            var subsetItems = from s in db.MarketerUsers where s.Parent_Id == user.Id select s;
           await subsetItems.ToListAsync();
            int count = 0;
            foreach (var item in subsetItems)
            {
                count += 1;

            }
            return new { count = count };

        }
        #endregion
        [Route("api/MarketerAddSubSet/CanAddSubset")]
        [HttpPost]
        public async Task<object> CanAddSubset()
        {
            var ApiToken = HttpContext.Current.Request.Form["Api_Token"];

            var useritem =await db.MarketerUsers.Where(p => p.Api_Token == ApiToken).FirstOrDefaultAsync();
            if(useritem == null)
            {
                return new { StatusCode = 200, Message = "کاربر مورد نظر یافت نشد!" };

            }
            return new
            {
                count = useritem.SubsetCount
            };

        }

        }
}
