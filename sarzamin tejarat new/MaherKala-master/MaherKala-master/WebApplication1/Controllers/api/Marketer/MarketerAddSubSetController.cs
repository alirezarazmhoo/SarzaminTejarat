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

namespace WebApplication1.Controllers.api.Marketer
{
    [Route("api/MarketerAddSubSet")]
    public class MarketerAddSubSetController : ApiController
    {
        DBContext db = new DBContext();

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
                return new { StatusCode = 1, Message = "انجام این اکشن فقط برای بازاریاب مجاز است" };
            }
            var UserPoints = db.MarketerUserPoints.Where(c => c.MarketerUserId == User.Id).FirstOrDefault();

            if (UserPoints == null)
            {
                return new { StatusCode = 1, Message = "شما امتیازی جهت اضافه کردن زیرمجموعه ندارید " };
            }
            else
            {
                double UserTotalPoints = UserPoints.UserPoits;
                if (UserPoints.UserPoits < PricePointForAddSubset.MinimumPrice)
                {
                    return new { StatusCode = 1, Message = "امتیاز شما کمتر از سقف تعیین شده است " };

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

            //            if (پرداخت درگاه بانکی موفقیت آمیز بود)
            //{



            int TicketId = Convert.ToInt32(HttpContext.Current.Request.Form["TicketId"]);
            var ApiToken = HttpContext.Current.Request.Form["Api_Token"];
            var User = db.MarketerUsers.Where(p => p.Api_Token == ApiToken).FirstOrDefault();
            var FindedTicket = db.RateOfAddSubSets.Where(p => p.Id == TicketId).FirstOrDefault();
            int TicketPoint = 0;
            int UserSubsetount = 0;

            if (User == null)
            {
                return new { StatusCode = 1, Message = "کاربر مورد نظر یافت نشد!" };

            }
            if (User.Usertype != 1)
            {
                return new { StatusCode = 1, Message = "انجام این اکشن فقط برای بازاریاب مجاز است" };
            }
            if (FindedTicket == null)
            {
                return new { StatusCode = 1, Message = "تیکت مورد نظر یافت نشد!" };

            }
            TicketPoint = FindedTicket.AddSubsetCounts;
            UserSubsetount = User.SubsetCount;
            User.SubsetCount = UserSubsetount + TicketPoint;

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
        public object ShowTotalSubSets()
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

            var listSubsets = db.MarketerUsers.Where(p => p.Parent_Id == MarketerUSerUser.Id).ToListAsync();


            return new { listSubsets=listSubsets };

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


    }
}
