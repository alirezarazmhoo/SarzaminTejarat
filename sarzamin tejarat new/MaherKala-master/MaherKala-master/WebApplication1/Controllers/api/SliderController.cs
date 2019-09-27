using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.Models;

namespace WebApplication1.Controllers.api
{
    public class SliderController : ApiController
    {
        DBContext db = new DBContext();
        [HttpGet]
        public object GetSliders()
        {
            var list = new { Data= db.MobileSliders.ToList() };
            return list;
        }
    }
}
