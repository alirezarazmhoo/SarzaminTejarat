using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebApplication1.Filter;
using WebApplication1.Models;

namespace WebApplication1.Controllers.api.Marketer
{
    public class MarketerPVChatController : ApiController
    {
        DBContext db = new DBContext();

        #region MarketerPVTicketChat
        [HttpPost]
        [Route("api/MarketerPVChat/LoadMarketerPVChatData")]
        //[ApiAuthorize]
        public PagedItem<MarketerPVTicketChat> LoadMarketerPVChatData(string Api_Token)
        {
            //var Token = HttpContext.Current.Request.Form["Api_Token"];
            var user = db.MarketerUsers.Where(p => p.Api_Token == Api_Token).FirstOrDefault();


            var query = db.MarketerPVTicketChats.Where(p => p.UserId == user.Id).AsQueryable();
            if (HttpContext.Current.Request.Form.AllKeys.Contains("Id"))
            {
                int ID = Convert.ToInt32(HttpContext.Current.Request.Form["Id"]);
                query = query.Where(p => p.Id == ID);

            }
            foreach (var item in query)
            {
                item.IsRead = true;
            }
            db.SaveChanges();
           
            var result = query.OrderByDescending(p => p.Id);
            return new PagedItem<MarketerPVTicketChat>(result, "");
        }
        //[HttpPost]
        //[Route("api/MarketerPVChat/LoadMarketerPVTicketChatMessageData")]
        
        //public PagedItem<MarketerPVTicketChatMessage> LoadMarketerPVTicketChatMessageData(int id)
        //{
        //    var Token = HttpContext.Current.Request.Form["Api_Token"];
        //    var user = db.MarketerUsers.Where(p => p.Api_Token == Token).FirstOrDefault();
        //    var MarketerPVTicketChat = db.MarketerPVTicketChats.FirstOrDefault(p => p.Id == id);
        //    if (MarketerPVTicketChat.State == TicketStateEnum.ResponceByAdmin || MarketerPVTicketChat.State == TicketStateEnum.CreateByAdmin)
        //        MarketerPVTicketChat.IsRead = true;
        //    db.SaveChanges();
        //    var query = db.MarketerPVTicketChatMessages.Where(p => p.MarketerPVTicketChatId == id).AsQueryable();
        //    var result = query.OrderByDescending(p => p.Id);
        //    return new PagedItem<MarketerPVTicketChatMessage>(result, "");
        //}
        [HttpPost]
        [Route("api/MarketerPVChat/AddMarketerPVTicketChat")]
       
        public object AddMarketerPVTicketChat()
        {
            var Token = HttpContext.Current.Request.Form["Api_Token"];
            var user = db.MarketerUsers.Where(p => p.Api_Token == Token).FirstOrDefault();
            string LastMessage = HttpContext.Current.Request.Form["LastMessage"];
            
            var MarketerPVTicketChat = new MarketerPVTicketChat();
            MarketerPVTicketChat.CreateDate = DateTime.Now;
            MarketerPVTicketChat.LastMessage = LastMessage;
            MarketerPVTicketChat.UserId = user.Id;
            //MarketerPVTicketChat.State = TicketStateEnum.CreateByUser;
            MarketerPVTicketChat.State = TicketStateEnum.User;

            db.MarketerPVTicketChats.Add(MarketerPVTicketChat);
            try
            {
                db.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
            return new
            {
                Message = 0
            };
        }


        //[HttpPost]
        //[Route("api/MarketerPVChat/AddMarketerPVTicketChatMessage")]
        
        //public object AddMarketerPVTicketChatMessage()
        //{
        //    var Token = HttpContext.Current.Request.Form["Api_Token"];
        //    var user = db.MarketerUsers.Where(p => p.Api_Token == Token).FirstOrDefault();
        //    string Message = HttpContext.Current.Request.Form["LastMessage"];
        //    int MarketerPVTicketChatId = Convert.ToInt32(HttpContext.Current.Request.Form["MarketerPVTicketChatId"]);
           
        //    var MarketerPVTicketChatMessage = new MarketerPVTicketChatMessage();
        //    MarketerPVTicketChatMessage.MarketerPVTicketChatId = MarketerPVTicketChatId;
        //    MarketerPVTicketChatMessage.Message = Message;
        //    MarketerPVTicketChatMessage.CreateDate = DateTime.Now;
        //    MarketerPVTicketChatMessage.State = SendStateEnum.User;
        //    var MarketerPVTicketChat = db.MarketerPVTicketChats.FirstOrDefault(x => x.Id == MarketerPVTicketChatId);
        //    MarketerPVTicketChat.State = TicketStateEnum.ResponceByUser;
        //    MarketerPVTicketChat.IsRead = false;
        //    MarketerPVTicketChat.LastMessage = Message;
        //    db.MarketerPVTicketChatMessages.Add(MarketerPVTicketChatMessage);
        //    db.SaveChanges();
        //    return new
        //    {
        //        Message = 0
        //    };
        //}

        #endregion
    }
}
