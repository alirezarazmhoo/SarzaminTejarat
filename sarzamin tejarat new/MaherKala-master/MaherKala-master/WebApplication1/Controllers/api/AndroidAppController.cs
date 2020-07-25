using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.Models;

namespace WebApplication1.Controllers.api
{
    public class AndroidAppController : ApiController
    {
        DBContext db = new DBContext();
        [HttpPost]
		//[Route("AndroidApp/GetAndroidApp")]

		public object GetAndroidApp()
        {
            var obj = new { Data= db.AndroidApps.FirstOrDefault() };
            return obj;
        }
    }
}
