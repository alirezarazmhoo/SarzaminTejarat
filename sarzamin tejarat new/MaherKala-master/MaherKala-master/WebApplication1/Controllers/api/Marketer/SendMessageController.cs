using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
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
         

            var messages =  db.SendMessages.Where(s => s.UserType == useritem.Usertype).AsQueryable();
            var res = messages.OrderByDescending(s => s.id);
            var Result = new PagedItem<SendMessage>(res, "");

            return new { Message = Result };

        }
        [Route("api/SendMessage/UserAnswer")]
        [HttpPost]
        public async Task<object> UserAnswer()
        {
            UserAnswer userAnswer = new UserAnswer();
            var ApiToken = HttpContext.Current.Request.Form["Api_Token"];
            var UserMessage = HttpContext.Current.Request.Form["UserMessage"];

            var UserItem =await db.MarketerUsers.Where(s => s.Api_Token == ApiToken).FirstOrDefaultAsync();

            userAnswer.CreateDate = DateTime.Now;
            userAnswer.MarketerUserID = UserItem.Id;
            userAnswer.UserMessage = UserMessage;
            db.UserAnswers.Add(userAnswer);
            db.SaveChanges();
                return new { StatusCode = 200, Message = "پیام ثبت گردید" };
        


        }

       
    }
}
