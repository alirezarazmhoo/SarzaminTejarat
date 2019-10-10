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
    }
}
