using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WebApplication1.Models;

namespace WebApplication1.Controllers.api.Marketer
{
    public class RequsetForTransferController : ApiController
    {
        DBContext db = new DBContext();

    
        [Route("api/RequsetForTransfer/GetRequestTransfare")]
        [HttpPost]
        public async Task<object> AddSubSetByPoints()
        {
            var ApiToken = HttpContext.Current.Request.Form["Api_Token"];
            RequestForTransfer requestForTransfer = new RequestForTransfer();
            var item = db.MarketerUsers.Where(s => s.Api_Token == ApiToken).FirstOrDefault();
            if(item == null)
            {
                return new { StatusCode = 200, Message = "کاربر مورد نظر یافت نشد!" };

            }
            if(item.Usertype == 0)
            {
                return new { StatusCode = 401, Message = "انجام این عمل فقط برای خرده فروشان و خریداران عمده است" };

            }
            requestForTransfer.MarketerUserID = item.Id;
            db.RequestForTransfers.Add(requestForTransfer);
       await     db.SaveChangesAsync();

            return new { StatusCode = 0, Message = "در خواست شما ثبت شد!" };
        }


    }
}
