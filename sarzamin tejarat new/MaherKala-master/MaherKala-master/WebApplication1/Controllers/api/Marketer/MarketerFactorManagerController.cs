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
    public class MarketerFactorManagerController : ApiController
    {
        DBContext db = new DBContext();

        
        [Route("api/MarketerFactorManager/GetUserTotalFactor")]

        [HttpPost]
       public async Task<object> GetUserTotalFactor()
        {
            var ApiToken = HttpContext.Current.Request.Form["Api_Token"];

            var date1 = HttpContext.Current.Request.Form["date1"];
            var date2 = HttpContext.Current.Request.Form["date2"];
            if((date1 !="" && date2 != "") && (date1 !=null && date2 !=null))
            {

                DateTime dt;
                if (!DateTime.TryParse(date1, out dt))
                {
                    return new { StatusCode = 1, Message = "فرمت تاریخ صحیح نیست" };

                }
                if (!DateTime.TryParse(date2, out dt))
                {
                    return new { StatusCode = 1, Message = "فرمت تاریخ صحیح نیست" };

                }

                var convertdate1 = WebApplication1.Areas.Admin.Utility.DateChanger.ToGeorgianDateTime(date1);
                var convertdate2 = WebApplication1.Areas.Admin.Utility.DateChanger.ToGeorgianDateTime(date2);

                var _totalFactorItem = await db.MarketerFactorItem.Where(s => s.MarketerFactor.MarketerUser.Api_Token == ApiToken && s.MarketerFactor.Date >= convertdate1 && s.MarketerFactor.Date <= convertdate2).ToListAsync();
                if (_totalFactorItem == null)
                {

                    return new { StatusCode = 1, Message = "کاربر مورد نظر یافت نشد!" };


                }
                long _TotalPrice = 0;
                foreach (var item in _totalFactorItem)
                {
                    _TotalPrice += item.UnitPrice;


                }
                return _TotalPrice;



            }
    
            var totalFactorItem =await db.MarketerFactorItem.Where(s => s.MarketerFactor.MarketerUser.Api_Token == ApiToken).ToListAsync();
            if (totalFactorItem == null)
            {
             
                    return new { StatusCode = 1, Message = "کاربر مورد نظر یافت نشد!" };

               
            }
            long TotalPrice = 0;
            foreach (var item in totalFactorItem)
            {
                TotalPrice += item.UnitPrice;


            }
            return  TotalPrice;

        }
        [Route("api/MarketerFactorManager/GetAllUsersTotalFactor")]
        [HttpPost]
        public async Task<object> GetAllUsersTotalFactor()
        {
            var ApiToken = HttpContext.Current.Request.Form["Api_Token"];


            var date1 = HttpContext.Current.Request.Form["date1"];
            var date2 = HttpContext.Current.Request.Form["date2"];
            if ((date1 != "" && date2 != "") && (date1 != null && date2 != null))
            {

                DateTime dt;
                if (!DateTime.TryParse(date1, out dt))
                {
                    return new { StatusCode = 1, Message = "فرمت تاریخ صحیح نیست" };

                }
                if (!DateTime.TryParse(date2, out dt))
                {
                    return new { StatusCode = 1, Message = "فرمت تاریخ صحیح نیست" };

                }
                var convertdate1 = WebApplication1.Areas.Admin.Utility.DateChanger.ToGeorgianDateTime(date1);
                var convertdate2 = WebApplication1.Areas.Admin.Utility.DateChanger.ToGeorgianDateTime(date2);
                var _userItem = await db.MarketerUsers.Where(s => s.Api_Token == s.Api_Token).FirstOrDefaultAsync();

                if (_userItem == null)
                {

                    return new { StatusCode = 1, Message = "کاربر مورد نظر یافت نشد!" };


                }

                var _totalFactors = await db.MarketerFactorItem.Where(p => p.MarketerFactor.MarketerUser.Parent_Id 
                == _userItem.Id && p.MarketerFactor.Date >=convertdate1 && p.MarketerFactor.Date <= convertdate2 ).ToListAsync();

                long _totalPrice = 0;
                foreach (var item in _totalFactors)
                {
                    _totalPrice += item.UnitPrice;


                }
                return _totalPrice;



            }

            var userItem =await db.MarketerUsers.Where(s => s.Api_Token == s.Api_Token).FirstOrDefaultAsync();
            if (userItem == null)
            {

                return new { StatusCode = 1, Message = "کاربر مورد نظر یافت نشد!" };


            }
            var totalFactors =await db.MarketerFactorItem.Where(p => p.MarketerFactor.MarketerUser.Parent_Id == userItem.Id).ToListAsync();
            long totalPrice = 0;
            foreach (var item in totalFactors)
            {
                totalPrice += item.UnitPrice;


            }
            return totalPrice;
        }

    
        }
}
