using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        public async Task<ActionResult> Reject (int? id)
        {
            var item = db.RequestForTransfers.Find(id);
            db.RequestForTransfers.Remove(item);

           await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<ActionResult> Accept(int? marketerId,int? UserId)
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

                var marketerfactoritems = await db.MarketerFactorItem.Where(s => s.MarketerFactor.MarketerUser.Id == UserId).ToListAsync();

                foreach (var item2 in marketerfactoritems)
                {
                    var _factoritem = db.MarketerFactorItem.Find(item2.Id);
                    db.MarketerFactorItem.Remove(item2);


                }
                var marketerFactors = await db.MarketerFactor.Where(s => s.MarketerUser.Id == UserId).ToListAsync();
                foreach (var item3 in marketerFactors)
                {
                    var _factor = db.MarketerFactor.Find(item.Id);
                    db.MarketerFactor.Remove(_factor);
                }
                await db.SaveChangesAsync();
            }
            catch (Exception)
            {
                TempData["Error"] = "عملیات با مشکل مواجه است لطفا بعدا امتحان کنید";
            }
            return RedirectToRoute("RequestForExchangeToBeMarketer");

        }





    }
}