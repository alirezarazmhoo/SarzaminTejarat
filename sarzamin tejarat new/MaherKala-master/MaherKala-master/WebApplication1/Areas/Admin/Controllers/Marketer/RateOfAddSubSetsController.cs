using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Areas.Admin.Controllers.Marketer
{
    public class RateOfAddSubSetsController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Admin/RateOfAddSubSets
        public ActionResult Index()
        {
            var RateOfAddSubSet = db.pricePointForAddSubsets.Select(p => p.MinimumPrice).FirstOrDefault();
            ViewBag.rateOfAddSubSet = RateOfAddSubSet;

            var url = "/Admin/RateOfAddSubSets/Index";
            var data = db.RateOfAddSubSets.AsQueryable();

            //return View(await db.RateOfAddSubSets.ToListAsync());
            var res = data.OrderByDescending(p => p.Id);
            ViewBag.Data = new PagedItem<RateOfAddSubSet>(res, url);
            return  View();
        }

        // GET: Admin/RateOfAddSubSets/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RateOfAddSubSet rateOfAddSubSet = await db.RateOfAddSubSets.FindAsync(id);
            if (rateOfAddSubSet == null)
            {
                return HttpNotFound();
            }
            return View(rateOfAddSubSet);
        }

        // GET: Admin/RateOfAddSubSets/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/RateOfAddSubSets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Price,AddSubsetCounts")] RateOfAddSubSet rateOfAddSubSet)
        {
            if (ModelState.IsValid)
            {
                db.RateOfAddSubSets.Add(rateOfAddSubSet);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(rateOfAddSubSet);
        }

        // GET: Admin/RateOfAddSubSets/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RateOfAddSubSet rateOfAddSubSet = await db.RateOfAddSubSets.FindAsync(id);
            if (rateOfAddSubSet == null)
            {
                return HttpNotFound();
            }
            return View(rateOfAddSubSet);
        }

        // POST: Admin/RateOfAddSubSets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Price,AddSubsetCounts")] RateOfAddSubSet rateOfAddSubSet)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rateOfAddSubSet).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(rateOfAddSubSet);
        }

        // GET: Admin/RateOfAddSubSets/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RateOfAddSubSet rateOfAddSubSet = await db.RateOfAddSubSets.FindAsync(id);
            if (rateOfAddSubSet == null)
            {
                return HttpNotFound();
            }
            return View(rateOfAddSubSet);
        }

        // POST: Admin/RateOfAddSubSets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            RateOfAddSubSet rateOfAddSubSet = await db.RateOfAddSubSets.FindAsync(id);
            db.RateOfAddSubSets.Remove(rateOfAddSubSet);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
        //Add limitPricePoint For  Add User
        [HttpPost]
       
        public async Task<ActionResult> CreatelimitPricePoint([Bind(Include = "Id,MinimumPrice")] PricePointForAddSubset pricePointForAddSubset)
        {
      
            if (ModelState.IsValid)
            {
                var Item = db.pricePointForAddSubsets.FirstOrDefault();
                //if(Item.MinimumPrice != 0)
                //{
                //ViewBag.NowPrice = Item.MinimumPrice;

                //}
                //if(Item.MinimumPrice == 0)
                //{
                //    ViewBag.NowPrice = 0;
                //}
                if(Item != null)
                {

                Item.MinimumPrice = pricePointForAddSubset.MinimumPrice;
                }
                else
                {
                    db.pricePointForAddSubsets.Add(pricePointForAddSubset);
                }

                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
    

    }
}
