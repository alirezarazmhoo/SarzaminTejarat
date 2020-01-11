using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Areas.Admin.Controllers.Marketer
{
    public class RequsetForExchangeToMarketerController : Controller
    {

        DBContext db = new DBContext();

        // GET: Admin/RequsetForExchangeToMarketer
        public ActionResult Index()
        {

            return View();
        }
        public async Task<ActionResult> Details(int? id)
        {
            var factors =await db.MarketerFactor.Where(s => s.MarketerUser.Id == id).ToListAsync();
            return View(factors);
        }
        public async Task<ActionResult> Reject (int Id)
        {
            var item = db.RequestForTransfers.Find(Id);
            db.RequestForTransfers.Remove(item);
           await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<ActionResult> Accept(int? UserId,int? RequestId)
        {
            var item =await db.MarketerUsers.Where(s => s.Id == UserId).FirstOrDefaultAsync();
            try
            {
				var commision = await db.Commission.Where(s => s.MarketerUser.Id == UserId).ToListAsync();

				foreach (var item1 in commision)
				{
					var _comsion = db.Commission.Find(item1.Id);
					db.Commission.Remove(_comsion);
				}
				item.Usertype = 0;
				item.PersonalReagentCode = item.IDCardNumber;
				item.IsFirstTime = false;
				item.FactorCounter = 0;
				item.Islazy = false;
				item.SubsetCount = 3; 	
				var Item = db.RequestForTransfers.Find(RequestId);
				db.RequestForTransfers.Remove(Item);
					db.SaveChanges();
				foreach (var item2 in await db.MarketerFactorItem.
					Include(s => s.MarketerFactor).
					Where(s => s.MarketerFactor.MarketerUser.Id
					== UserId).ToListAsync())
                {
					using (SqlConnection conn = new SqlConnection("Data Source=95.216.56.89,2016;Initial Catalog=atrincom123_shop;User Id=atrincom123_shop;Password=26cne3D&"))
					{
						conn.Open();
						SqlCommand cmd = new SqlCommand("RemoveFactor", conn);
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.Add(new SqlParameter("@FactorId", item2.MarketerFactor.Id));
						int res = cmd.ExecuteNonQuery();
						conn.Close();
					}
				}
			}
            catch (Exception )
            {
                TempData["Error"] = "عملیات با مشکل مواجه است لطفا بعدا امتحان کنید";
            }
            return RedirectToRoute("RequestForExchangeToBeMarketer");
        }
    }
}