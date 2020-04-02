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
      
            if(UserType == 1)
            {
             return new { StatusCode = 301, Message = "انجام این عملیات برای بازاریابان مجاز نمی باشد" };
            }
            List<PaySetting> paySettings =await db.PaySettings.ToListAsync();
            return new { StatusCode = 0, paySettings = paySettings };
        }
    }
}
