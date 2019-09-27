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
    public class MarketerCommisionController : ApiController
    {
        DBContext db = new DBContext();
        [MarketerAuthorize]
        [HttpPost]
        [Route("api/MarketerCommision/GetCommisions")]
        public object index()
        {
            var msg = new MarketerChat();
            var token = System.Web.HttpContext.Current.Request.Form["Api_Token"];
            var user = db.MarketerUsers.Where(p => p.Api_Token == token).FirstOrDefault();

            var data = db.Commission.Include("MarketerFactor").Where(p => p.MarketerUser.Id == user.Id).Select(p=>new { p.Id,p.Date,p.Detail,p.Formula,p.Money}).OrderByDescending(p=>p.Id);

            var paged = new PagedItem<object>(data, "");
            return paged;
        }
    }
}
