using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using WebApplication1.Models;

namespace WebApplication1.Controllers.api
{
    public class SettingController : ApiController
    {
        DBContext db = new DBContext();
        [HttpGet]
        [Route("api/Setting/AboutUs")]
        public HttpResponseMessage AboutUs()
        {
            var s = db.Settings.First();
            var res = Request.CreateResponse(HttpStatusCode.OK);
            var data = "<html>";
            data += "<head>";
            data += "<style type='text/css'>";

            data += "@font-face {";
            data += "font-family: MyFont;";
            data += "src: url(\"file:///android_asset/fonts/iransans_m.ttf\");";
            data += "}";

            data += "body{";
            data += "font-family: MyFont;";
            data += "font-size:13px;";
            data += "text-align:justify;";
            data += "direction:rtl;";
            data += "}";

            data += "</style>";
            data += "</head>";
            data += "<body>";
            data += s.AboutUs;

            data += "</body>";
            data += "</html>";
            
            res.Content = new StringContent(data, Encoding.UTF8, "text/html");
            return res;
        }
        [HttpGet]
        [Route("api/Setting/CallUs")]
        public object CallUs()
        {
            var s = db.Settings.First();
            var res = Request.CreateResponse(HttpStatusCode.OK);
            res.Content = new StringContent(s.CallUs, Encoding.UTF8, "text/html");
            return res;
        }
        [HttpPost]
        [Route("api/Setting/Email")]
        public object StoreEmail()
        {
            var Email = new Email();
            Email.Address =  HttpContext.Current.Request.Form["Email"];
            db.Emails.Add(Email);
            db.SaveChanges();
            return new { Message = 0};
        }
    }
}
