using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebApplication1.Filter;
using WebApplication1.Models;

namespace WebApplication1.Controllers.api.Marketer
{
    public class MarketerChatController : ApiController
    {
        DBContext db = new DBContext();
        [MarketerAuthorize]
        [HttpPost]
        [Route("api/MarketerChat/Send")]
        public object Send()
        {
            string Text = HttpContext.Current.Request.Form["Text"];
            var msg = new MarketerChat();
            msg.Text = Text;
            var token = System.Web.HttpContext.Current.Request.Form["Api_Token"];
            var user = db.MarketerUsers.Where(p => p.Api_Token == token).FirstOrDefault();
            msg.User = user;

            var dateTime = DateTime.Now;
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var unixDateTime = (long)(dateTime.ToUniversalTime() - epoch).TotalMilliseconds;

            msg.Timestamp = unixDateTime;
            db.MarketerChats.Add(msg);
            db.SaveChanges();
            return new { Message=0};
        }
        [MarketerAuthorize]
        [HttpPost]
        [Route("api/MarketerChat/Receive")]
        public object Receive()
        {
            long Timestamp = Convert.ToInt64(HttpContext.Current.Request.Form["Timestamp"]);
            var token = System.Web.HttpContext.Current.Request.Form["Api_Token"];
            var data = db.MarketerChats.Where(p => p.Timestamp > Timestamp).Select(p => new { p.Id, p.Text, User = p.User.Name + " " + p.User.LastName, isOwnMessage = (p.User.Api_Token == token) ? true : false }).OrderBy(x => x.Id);
            var paged = new PagedItem<object>(data, "/api/MarketerChat/Receive");
            return new { Data = paged, Message = 0 };
        }
    }
}
