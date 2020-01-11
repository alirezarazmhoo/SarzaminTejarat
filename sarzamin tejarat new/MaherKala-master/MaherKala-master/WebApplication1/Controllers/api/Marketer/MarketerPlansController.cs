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
            var GoldPlans = await db.Plannns.Where(s => s.PlanTypeID == 2).Select(s => new { s.Level, s.Price, s.Id, s.Description, s.PlanTypeID, s.ImageUrl }).OrderBy(s=>s.Level).ToListAsync();
            var SilverPlans = await db.Plannns.Where(s => s.PlanTypeID == 1).Select(s => new { s.Level, s.Price, s.Id, s.Description, s.PlanTypeID, s.ImageUrl }).OrderBy(s => s.Level).ToListAsync();
            return new
            {
                GoldPlans,
                SilverPlans

            };


        }
		[Route("api/MarketerPlans/FindNextPlan")]
		[HttpPost]
		public async Task<object> FindNextPlan()
		{

			var token = HttpContext.Current.Request.Form["Api_Token"];

			if (token == null)
			{
				return new
				{
					StatusCode = 1,
					Message = "توکن خالی است"
				};
			}

			var UserItem =await db.MarketerUsers.Where(s => s.Api_Token == token).FirstOrDefaultAsync();
			if (UserItem == null)
			{
				return new
				{
					StatusCode = 101,
					Message = "کاربر مورد نظر یافت نشد"
				};
			}
			var FindPlan =await db.Plannns.Where(s => s.Id == UserItem.PlannnID).FirstOrDefaultAsync();
			if(FindPlan ==null)
			{
				return new
				{
					StatusCode = 102,
					Message = "پلن مورد نظر یافت نشد"
				};
			}
			var FindFinalPlan =await db.Plannns.Where(s => s.PlanTypeID == FindPlan.PlanTypeID  ).ToListAsync();
			int max = 0;
			foreach (var item in FindFinalPlan)
			{
				if(item.Level >= max)
				{
					max = item.Level;
				}
			}
			if(FindPlan.Level == max && FindPlan.PlanTypeID ==1)
			{

				var FindNextPlanGold =await db.Plannns.Where(s => s.PlanTypeID == 2).FirstOrDefaultAsync();
				return new
				{
					Price =  FindNextPlanGold.Price
				};
			}
			else
			{

			var FindNextPlanSilver =await db.Plannns.Where(s => s.PlanTypeID == FindPlan.PlanTypeID && s.Level == FindPlan.Level + 1).FirstOrDefaultAsync();
				return new
				{
					Price =  FindNextPlanSilver.Price
				};
			}
		}

	}
}
