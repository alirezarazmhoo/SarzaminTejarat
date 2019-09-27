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
    public class UpdateController : ApiController
    {
        DBContext db = new DBContext();

        [Route("api/Update/GetUpdate")]
        [HttpPost]
        public object GetUpdate()
        {
            var versionCode = Convert.ToInt32(HttpContext.Current.Request.Form["VersionCode"]);
            if (versionCode == 0)
            {
                return new { StatusCode = 2, Message = "مقدار VersionCode را وارد کنید" };
            }
            var data = db.Updates.OrderByDescending(p => p.Id).Where(p => p.VersionCode > versionCode).FirstOrDefault();
            if (data == null)
            {
                return new { StatusCode = 0 };
            }
            else
            {
                return new
                {
                    StatusCode = 1,
                    VersionCode = data.VersionCode,
                    VersionName = data.VersionName,
                    Desc = data.Desc,
                    Link = data.Link
                };
            }
        }
    }
}
