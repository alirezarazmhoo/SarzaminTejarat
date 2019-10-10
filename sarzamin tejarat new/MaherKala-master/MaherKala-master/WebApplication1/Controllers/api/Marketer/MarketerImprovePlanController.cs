using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WebApplication1.Models;

namespace WebApplication1.Controllers.api.Marketer
{
    public class MarketerImprovePlanController : ApiController
    {
        DBContext db = new DBContext();


        [Route("api/MarketerImprovePlan/ShowAvalibalePlans")]
        [HttpPost]
        public object ShowAvalibalePlans()
        {
            var ApiToken = HttpContext.Current.Request.Form["Api_Token"];
            var User = db.MarketerUsers.Where(p => p.Api_Token == ApiToken).FirstOrDefault();
            if (User == null)
            {

                return new { StatusCode = 200, Message = "کاربرمورد نظر یافت نشد " };
            }
            var userplan = db.Plannns.Where(s => s.Id == User.PlannnID).FirstOrDefault();
            var planType = db.PlanTypes.Where(p => p.Id == userplan.PlanTypeID).FirstOrDefault();
            if (planType.Id == 1)
            {
                var avalibaleplan = db.MarketerImprovePlans.Select(s=>new { s.Id,s.Price}).FirstOrDefault();
                return new { avalibaleplan = avalibaleplan };
                    }
            return null;
        }

    }
}