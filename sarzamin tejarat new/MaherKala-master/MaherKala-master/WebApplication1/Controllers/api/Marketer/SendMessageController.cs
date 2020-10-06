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
using WebApplication1.Models.Errors;

namespace WebApplication1.Controllers.api.Marketer
{
    public class SendMessageController : ApiController
    {
        DBContext db = new DBContext();
		PrivateChatModel privateChatModel = new PrivateChatModel();
		PublicChatModel publicChatModel = new PublicChatModel();
		UserChatModel userChatModel = new UserChatModel();
		[Route("api/SendMessage/ShowMessages/{page}")]
		[HttpPost]
		public async Task<IHttpActionResult> ShowMessages(int page)
		{
			var ApiToken = HttpContext.Current.Request.Form["Api_Token"];
			var useritem = await db.MarketerUsers.Where(s => s.Api_Token == ApiToken).FirstOrDefaultAsync();
			if (useritem == null)
			{
				return new System.Web.Http.Results.ResponseMessageResult(
			  Request.CreateErrorResponse(HttpStatusCode.NotFound, new HttpError(ErrorsText.MarketerNotFound)));
				//return new { StatusCode = 404, Message = "کاربر مورد نظر یافت نشد!" };
			}
			//var messages =  db.SendMessages.Where(s => s.UserType == useritem.Usertype).AsQueryable();
			foreach (var item in await db.Converstions.Where(s => s.MarketerUserID == useritem.Id && s.IsRead==false && s.State==1).ToListAsync())
			{
			item.IsRead = true;
			}
			await  db.SaveChangesAsync();
			var messages = db.Converstions.Where(s => s.MarketerUserID == useritem.Id).AsQueryable();
			var res = messages.OrderByDescending(p => p.Id);
			var Result = new PagedItem<object>(res, "");
			privateChatModel.TotalPage = Result.TotalPage;
			privateChatModel.PageNumber = page;
			privateChatModel.TotalItemCount = Result.TotalItemCount;
			privateChatModel.PrivateMessage = messages.OrderByDescending(x => x.Id).Skip(10 * (page - 1)).Take(10).ToList();

			return Ok(privateChatModel);
			//return new
			//{
			//	Result.TotalPage,
			//	PageNumber = page,
			//	Result.TotalItemCount,
			//	PrivateMessage = messages.OrderByDescending(x => x.Id).Skip(10 * (page - 1)).Take(10).ToList()
			//};
        }
		[Route("api/SendMessage/publicMessage/{page}")]
		[HttpPost]
		public async Task<IHttpActionResult> publicMessage(int page)
		{
			var ApiToken = HttpContext.Current.Request.Form["Api_Token"];
			var useritem = await db.MarketerUsers.Where(s => s.Api_Token == ApiToken).FirstOrDefaultAsync();
			if (useritem == null)
			{
				return new System.Web.Http.Results.ResponseMessageResult(
		  Request.CreateErrorResponse(HttpStatusCode.NotFound, new HttpError(ErrorsText.MarketerNotFound)));
			}
			var publicMessage = db.SendMessages.Where(s => s.UserType == useritem.Usertype).AsQueryable();
			var res = publicMessage.OrderByDescending(p => p.id);
			var Result = new PagedItem<object>(res, "");
			publicChatModel.TotalPage = Result.TotalPage;
			publicChatModel.PageNumber = page;
			publicChatModel.TotalItemCount = Result.TotalItemCount;
			publicChatModel.publicMessage = publicMessage.OrderByDescending(x => x.id).Skip(10 * (page - 1)).Take(10).ToList();
			return Ok(publicChatModel);
		}
		[Route("api/SendMessage/UserAnswer")]
        [HttpPost]
        public async Task<IHttpActionResult> UserAnswer()
        {
            Converstions converstions = new Converstions();
            var ApiToken = HttpContext.Current.Request.Form["Api_Token"];
            var UserMessage = HttpContext.Current.Request.Form["UserMessage"];

            var UserItem =await db.MarketerUsers.Where(s => s.Api_Token == ApiToken).FirstOrDefaultAsync();
			if (UserItem == null)
			{
				return new System.Web.Http.Results.ResponseMessageResult(
		  Request.CreateErrorResponse(HttpStatusCode.NotFound, new HttpError(ErrorsText.MarketerNotFound)));
			}
			if (string.IsNullOrEmpty(UserMessage))
			{
			return new System.Web.Http.Results.ResponseMessageResult(
              Request.CreateErrorResponse(HttpStatusCode.BadRequest, new HttpError(ErrorsText.EmptyMessageText)));			
			}
			converstions.CreateDate = DateTime.Now;
            converstions.LastMessage = UserMessage;
            converstions.State = 2;
			converstions.IsRead = false;
            converstions.MarketerUserID = UserItem.Id;
            db.Converstions.Add(converstions);
            db.SaveChanges();
			//return new { StatusCode = 200, Message = "پیام ثبت گردید" };
			return Ok();
        }
        [Route("api/SendMessage/UserConversions")]
        [HttpPost]
        public async Task<IHttpActionResult> UserConversions()
        {
            UserConversions converstions = new UserConversions();
			UserSavedConversionInfo userSavedConversionInfo = new UserSavedConversionInfo();
			var ApiToken = HttpContext.Current.Request.Form["Api_Token"];
            var UserMessage = HttpContext.Current.Request.Form["UserMessage"];
            var TargetUserApi_Token = HttpContext.Current.Request.Form["TargetUserApi_Token"];
            var UserItem = await db.MarketerUsers.Where(s => s.Api_Token == ApiToken).FirstOrDefaultAsync();
            var TargetUserItem = await db.MarketerUsers.Where(s => s.Api_Token == TargetUserApi_Token).FirstOrDefaultAsync();
			if(string.IsNullOrEmpty(UserMessage))
			{
				return new System.Web.Http.Results.ResponseMessageResult(
	  Request.CreateErrorResponse(HttpStatusCode.BadRequest, new HttpError(ErrorsText.EmptyMessageText)));
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
				converstions.IsRead = false;
				Random rnd = new Random();
				var RandomNumber = rnd.Next(20, 990000000).ToString();
				converstions.MessageToken = RandomNumber;
				db.UserConversions.Add(converstions);
				userSavedConversionInfo.UserId = TargetUserItem.Id;
				userSavedConversionInfo.TokenMessage = RandomNumber;
				db.UserSavedConversionInfo.Add(userSavedConversionInfo);
				db.SaveChanges();
				return Ok();
            }
            else
            {
				return new System.Web.Http.Results.ResponseMessageResult(
		  Request.CreateErrorResponse(HttpStatusCode.InternalServerError, new HttpError(ErrorsText.SystemError)));
				//return new { StatusCode = 400, Message = "خطایی رخ داده" };
			}
        }
        [Route("api/SendMessage/UserShowMessages/{page}")]
        [HttpPost]
        public async Task<IHttpActionResult> UserShowMessages(int page)
        {
            var ApiToken = HttpContext.Current.Request.Form["Api_Token"];
            var TargetUserApi_Token = HttpContext.Current.Request.Form["TargetUserApi_Token"];
            var useritem = await db.MarketerUsers.Where(s => s.Api_Token == ApiToken).FirstOrDefaultAsync();
            var TargetUserItem = await db.MarketerUsers.Where(s => s.Api_Token == TargetUserApi_Token).FirstOrDefaultAsync();
            if (useritem == null || TargetUserItem== null)
            {
				return new System.Web.Http.Results.ResponseMessageResult(
				Request.CreateErrorResponse(HttpStatusCode.NotFound, new HttpError(ErrorsText.MarketerNotFound)));
			}
			foreach (var item in await db.UserConversions.Where(s=>s.TargetUserId == useritem.Id && s.IsRead == false).ToListAsync())
			{
				item.IsRead = true;

			}
			await db.SaveChangesAsync();
				var messages = db.UserConversions.Where(s => (s.MarketerUserID == TargetUserItem.Id && s.TargetUserId == useritem.Id) || (s.MarketerUserID == useritem.Id && s.TargetUserId == TargetUserItem.Id)).AsQueryable();
				var res = messages.OrderByDescending(s => s.Id);
				var Result = new PagedItem<UserConversions>(res, "");
			userChatModel.TotalPage = Result.TotalPage;
			userChatModel.PageNumber = page;
			userChatModel.TotalItemCount = Result.TotalItemCount;
			userChatModel.Message = messages.OrderByDescending(x => x.Id).Skip(10 * (page - 1)).Take(10).ToList();
			return Ok(userChatModel);
			//return new
			//	{
			//		Result.TotalPage,
			//		PageNumber = page,
			//		Result.TotalItemCount,
			//		Message = messages.OrderByDescending(x => x.Id).Skip(10 * (page - 1)).Take(10).ToList()
			//	};
        }
    }
}
