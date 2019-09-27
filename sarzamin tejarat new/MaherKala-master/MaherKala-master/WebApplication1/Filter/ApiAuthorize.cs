using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using WebApplication1.Models;

namespace WebApplication1.Filter
{
    public class ApiAuthorize : ActionFilterAttribute
    {
        DBContext db = new DBContext();
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var token = System.Web.HttpContext.Current.Request.Form["Api_Token"];
            var data = db.Users.Where(p => p.Api_Token == token).FirstOrDefault();
            if(data==null)
            {
                actionContext.Response = new System.Net.Http.HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.Unauthorized, // use whatever http status code is appropriate
                    RequestMessage = actionContext.ControllerContext.Request
                };


            }
        }

        
    }
}