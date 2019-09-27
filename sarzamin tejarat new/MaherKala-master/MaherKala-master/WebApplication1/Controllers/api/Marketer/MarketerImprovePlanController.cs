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
                return new { StatusCode = 1, Message = "کاربرمورد نظر یافت نشد " };
            }
            var MarketerCurrentPlan = db.Plannns.Where(p => p.Id == User.PlannnID).FirstOrDefault();
            if(MarketerCurrentPlan.PlanTypeID == 1)
            {

                var _AvalibalePlans = db.MarketerImprovePlans.Where(s =>
                (s.CurrentPlanTypeId == MarketerCurrentPlan.PlanTypeID 
                 && s.CurrentLevel == MarketerCurrentPlan.Level)
                || (s.CurrentLevel == MarketerCurrentPlan.Level &&
                s.CurrentPlanTypeId == MarketerCurrentPlan.PlanTypeID && s.CanGoPlanTypeId == 2))
                .Select(p => new {  p.Price,   p.CanGoPlanType.Name,  p.LevelCanGo }).ToList();
                return _AvalibalePlans;
            }
            if(MarketerCurrentPlan.PlanTypeID == 2)
            {
                var AvalibalePlans = db.MarketerImprovePlans.Where(s =>  s.CurrentPlanTypeId == MarketerCurrentPlan.PlanTypeID &&
                s.CurrentLevel == MarketerCurrentPlan.Level).Select(o=>new { o.Price,o.CanGoPlanType.Name,o.LevelCanGo}).ToList();
                return AvalibalePlans;
            }
            else
            {
                return null ;
            }
        }
    }
}
