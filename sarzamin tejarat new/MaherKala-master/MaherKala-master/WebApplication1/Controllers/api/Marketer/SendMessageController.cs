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
using WebApplication1.Models.ChatModel;
namespace WebApplication1.Controllers.api.Marketer
{
    public class SendMessageController : ApiController
    {
        DBContext db = new DBContext();
		[Route("api/SendMessage/ShowMessages/{page}")]
		[HttpPost]
		public async Task<object> ShowMessages(int page)
		{
			var ApiToken = HttpContext.Current.Request.Form["Api_Token"];
			var useritem = await db.MarketerUsers.Where(s => s.Api_Token == ApiToken).FirstOrDefaultAsync();
			if (useritem == null)
			{
				return new { StatusCode = 200, Message = "کاربر مورد نظر یافت نشد!" };
			}
			//var messages =  db.SendMessages.Where(s => s.UserType == useritem.Usertype).AsQueryable();
			var messages = db.Converstions.Where(s => s.MarketerUserID == useritem.Id).AsQueryable();
			var res = messages.OrderByDescending(p => p.Id);
			var Result = new PagedItem<object>(res, "");
			return new
			{
				Result.TotalPage,
				PageNumber = page,
				Result.TotalItemCount,
				PrivateMessage = messages.OrderByDescending(x => x.Id).Skip(10 * (page - 1)).Take(10).ToList()
			};
        }
		[Route("api/SendMessage/publicMessage/{page}")]
		[HttpPost]
		public async Task<object> publicMessage(int page)
		{
			var ApiToken = HttpContext.Current.Request.Form["Api_Token"];
			var useritem = await db.MarketerUsers.Where(s => s.Api_Token == ApiToken).FirstOrDefaultAsync();
			if (useritem == null)
			{
				return new { StatusCode = 200, Message = "کاربر مورد نظر یافت نشد!" };
			}
			var publicMessage = db.SendMessages.Where(s => s.UserType == useritem.Usertype).AsQueryable();
			//var respublicMessage = publicMessage.OrderBy(s => s.id);
			//var ResultrespublicMessage = new PagedItem<SendMessage>(respublicMessage, "");

			//return new { publicMessage = ResultrespublicMessage };
			var res = publicMessage.OrderByDescending(p => p.id);
			var Result = new PagedItem<object>(res, "");
			return new
			{
				Result.TotalPage,
				PageNumber = page,
				Result.TotalItemCount,
				publicMessage = publicMessage.OrderByDescending(x => x.id).Skip(10 * (page - 1)).Take(10).ToList()
			};
		}
		[Route("api/SendMessage/UserAnswer")]
        [HttpPost]
        public async Task<object> UserAnswer()
        {
            Converstions converstions = new Converstions();
            var ApiToken = HttpContext.Current.Request.Form["Api_Token"];
            var UserMessage = HttpContext.Current.Request.Form["UserMessage"];

            var UserItem =await db.MarketerUsers.Where(s => s.Api_Token == ApiToken).FirstOrDefaultAsync();
			if (string.IsNullOrEmpty(UserMessage))
			{
				return new { StatusCode = 408, Message = "متن پیام خالی است " };
			}
			converstions.CreateDate = DateTime.Now;
            converstions.LastMessage = UserMessage;
            converstions.State = 2;
            converstions.MarketerUserID = UserItem.Id;
            db.Converstions.Add(converstions);
            db.SaveChanges();
                return new { StatusCode = 200, Message = "پیام ثبت گردید" };
        }
        [Route("api/SendMessage/UserConversions")]
        [HttpPost]
        public async Task<object> UserConversions()
        {
            UserConversions converstions = new UserConversions();
            var ApiToken = HttpContext.Current.Request.Form["Api_Token"];
            var UserMessage = HttpContext.Current.Request.Form["UserMessage"];
            var TargetUserApi_Token = HttpContext.Current.Request.Form["TargetUserApi_Token"];
            var UserItem = await db.MarketerUsers.Where(s => s.Api_Token == ApiToken).FirstOrDefaultAsync();
            var TargetUserItem = await db.MarketerUsers.Where(s => s.Api_Token == TargetUserApi_Token).FirstOrDefaultAsync();
			if(string.IsNullOrEmpty(UserMessage))
			{
				return new { StatusCode = 408, Message = "متن پیام خالی است " };
			}
			if (UserItem !=null && TargetUserItem != null) {

                converstions.CreateDate = DateTime.Now;
                converstions.LastMessage = UserMessage;
				if(UserItem.Parent_Id == TargetUserItem.Id)
				{
                converstions.State = 2;
				}
				else
				{
					converstions.State = 1;
				}
				converstions.MarketerUserID = UserItem.Id;
                converstions.TargetUserId = TargetUserItem.Id;

                db.UserConversions.Add(converstions);
                db.SaveChanges();
                return new { StatusCode = 200, Message = "پیام ثبت گردید" };
            }
            else
            {
                return new { StatusCode = 400, Message = "خطایی رخ داده" };
            }
        }
        [Route("api/SendMessage/UserShowMessages/{page}")]
        [HttpPost]
        public async Task<object> UserShowMessages(int page)
        {
            var ApiToken = HttpContext.Current.Request.Form["Api_Token"];
            var TargetUserApi_Token = HttpContext.Current.Request.Form["TargetUserApi_Token"];
            var useritem = await db.MarketerUsers.Where(s => s.Api_Token == ApiToken).FirstOrDefaultAsync();
            var TargetUserItem = await db.MarketerUsers.Where(s => s.Api_Token == TargetUserApi_Token).FirstOrDefaultAsync();
            if (useritem == null || TargetUserItem== null)
            {
                return new { StatusCode = 1, Message = "کاربر مورد نظر یافت نشد!" };
            }
				var messages = db.UserConversions.Where(s => (s.MarketerUserID == TargetUserItem.Id && s.TargetUserId == useritem.Id) || (s.MarketerUserID == useritem.Id && s.TargetUserId == TargetUserItem.Id)).AsQueryable();
				var res = messages.OrderByDescending(s => s.Id);
				var Result = new PagedItem<UserConversions>(res, "");
				return new
				{
					Result.TotalPage,
					PageNumber = page,
					Result.TotalItemCount,
					Message = messages.OrderByDescending(x => x.Id).Skip(10 * (page - 1)).Take(10).ToList()
				};
        }
    }
}
