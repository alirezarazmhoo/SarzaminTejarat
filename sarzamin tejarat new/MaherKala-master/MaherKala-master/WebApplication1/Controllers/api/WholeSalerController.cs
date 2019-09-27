using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebApplication1.Models;

namespace WebApplication1.Controllers.api
{
    public class WholeSalerController : ApiController
    {
        [Route("api/WholeSaler/Store")]
        [HttpPost]
        public object Store()
        {
            string Type = HttpContext.Current.Request.Form["Type"];
            string Fullname = HttpContext.Current.Request.Form["Fullname"];
            string Email = HttpContext.Current.Request.Form["Email"];
            string Address = HttpContext.Current.Request.Form["Address"];
            string PhoneNumber = HttpContext.Current.Request.Form["PhoneNumber"];
            string Mobile = HttpContext.Current.Request.Form["Mobile"];
            string PostalCode = HttpContext.Current.Request.Form["PostalCode"];
            string Desc = HttpContext.Current.Request.Form["Desc"];

            int type = Convert.ToInt32(Type);
            if (type < 1 || type > 4)
            {
                
                return new { Message=1};
            }
            WholeSaler w = new Models.WholeSaler();
            w.Address = Address;
            w.Desc = Desc;
            w.Fullname = Fullname;
            w.Mobile = Mobile;
            w.Phone = PhoneNumber;
            w.PostalCode = PostalCode;
            w.Type = type;
            DBContext db = new DBContext();
            db.WholeSalers.Add(w);
            db.SaveChanges();
            return new { Message=0};
        }
    }
}
