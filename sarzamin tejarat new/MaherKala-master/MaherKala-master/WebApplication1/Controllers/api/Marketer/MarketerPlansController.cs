using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WebApplication1.Models;

namespace WebApplication1.Controllers.api.Marketer
{
    public class MarketerPlansController : ApiController
    {
        DBContext db = new DBContext();

        [Route("api/MarketerPlans/ShowPlans")]
        [HttpPost]

        public async Task<object> ShowPlans()
        {

            var PlanId =Convert.ToInt64( HttpContext.Current.Request.Form["PlanId"]);


            var _item =await db.Plannns.Where(p => p.Id == PlanId).FirstOrDefaultAsync();

            var listPlans =await db.Plannns.Where(s => s.PlanTypeID == _item.PlanTypeID).ToListAsync();



            return new { plans = listPlans };

        }
        [Route("api/MarketerPlans/ShowAllPlans")]
        [HttpGet]
        public async Task<object> ShowAllPlans()
        {
            var PlanItems =await db.Plannns.Select(s => new { s.Level, s.Price, s.Id,s.Description,s.PlanTypeID,s.ImageUrl}).ToListAsync();
            var GoldPlans = await db.Plannns.Where(s => s.PlanTypeID == 2).Select(s => new { s.Level, s.Price, s.Id, s.Description, s.PlanTypeID, s.ImageUrl }).ToListAsync();
            var SilverPlans = await db.Plannns.Where(s => s.PlanTypeID == 1).Select(s => new { s.Level, s.Price, s.Id, s.Description, s.PlanTypeID, s.ImageUrl }).ToListAsync();
            return new
            {
                GoldPlans,
                SilverPlans

            };


        }

        }
}
