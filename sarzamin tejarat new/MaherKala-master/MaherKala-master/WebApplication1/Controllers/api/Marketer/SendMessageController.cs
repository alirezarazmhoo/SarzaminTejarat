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
    public class SendMessageController : ApiController
    {
        DBContext db = new DBContext();

        [Route("api/SendMessage/ShowMessages")]
        [HttpPost]
        public async Task<object> ShowMessages()
        {
            var ApiToken = HttpContext.Current.Request.Form["Api_Token"];
            var useritem =await db.MarketerUsers.Where(s => s.Api_Token == ApiToken).FirstOrDefaultAsync();

            if(useritem == null)
            {
                return new { StatusCode = 1, Message = "کاربر مورد نظر یافت نشد!" };

            }
         

            var messages = await db.SendMessages.Where(s => s.UserType == useritem.Usertype).ToListAsync();
            return new { Message = messages };

        }
    }
}
