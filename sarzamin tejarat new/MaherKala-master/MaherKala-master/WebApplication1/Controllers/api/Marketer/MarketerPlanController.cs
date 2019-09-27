using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.Models;

namespace WebApplication1.Controllers.api.Marketer
{
    [Route("api/MarketerPlan")]
    public class MarketerPlanController : ApiController
    {
        DBContext db = new DBContext();
        #region ShowTickets
        [Route("api/MarketerPlan/GetPlans/{page}")]
        [HttpGet]
        public object GetPlans(int page)
        {
            var Items = db.MarketerPlans.Select(p => new { p.Id, p.Price, p.Description,p.ImageUrl,p.Level,PlanTypeName=p.PlanType.Name }).AsQueryable();
            return new { Data = Items.OrderByDescending(x => x.Id).Skip(10 * (page - 1)).Take(10).ToList(), totalCount = Items.Count() };
        }
        #endregion
    }
}
