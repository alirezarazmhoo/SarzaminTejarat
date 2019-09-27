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
    public class MarketerImprovePlansController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Admin/MarketerImprovePlans
        public  ActionResult Index()
        {

            var url = "/Admin/MarketerImprovePlans/Index";
            var data = db.MarketerImprovePlans.Include(m => m.CanGoPlanType).AsQueryable();

           
            var res = data.OrderByDescending(p => p.Id);
            ViewBag.Data = new PagedItem<MarketerImprovePlan>(res, url);
           
            //var marketerImprovePlans = db.MarketerImprovePlans.Include(m => m.CanGoPlanType).Include(m => m.CurrentPlanType);
            return View();
        }

        // GET: Admin/MarketerImprovePlans/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MarketerImprovePlan marketerImprovePlan = await db.MarketerImprovePlans.FindAsync(id);
            if (marketerImprovePlan == null)
            {
                return HttpNotFound();
            }
            return View(marketerImprovePlan);
        }

        // GET: Admin/MarketerImprovePlans/Create
        public ActionResult Create()
        {
            ViewBag.CanGoPlanTypeId = new SelectList(db.PlanTypes, "Id", "Name");
            ViewBag.CurrentPlanTypeId = new SelectList(db.PlanTypes, "Id", "Name");
            return View();
        }

        // POST: Admin/MarketerImprovePlans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,CurrentPlanTypeId,CurrentLevel,CanGoPlanTypeId,LevelCanGo,Price")] MarketerImprovePlan marketerImprovePlan)
        {
            

            var isRepeat = db.MarketerImprovePlans.Any(s => s.CurrentPlanTypeId
            == marketerImprovePlan.CurrentPlanTypeId && s.CurrentLevel ==
            marketerImprovePlan.CurrentLevel && s.CanGoPlanTypeId ==
            marketerImprovePlan.CanGoPlanTypeId && s.LevelCanGo ==
            marketerImprovePlan.LevelCanGo);
            if (isRepeat)
            {
                TempData["ErrorImprovePlan"] = "تعرفه مورد نظر تکراری است";
                return RedirectToAction("Create");

            }
            if (marketerImprovePlan.CurrentPlanTypeId > marketerImprovePlan.CanGoPlanTypeId)
            {
                TempData["ErrorImprovePlan"] = "پلن فعلی نباید بیشتر از پلن قابل ارتقا باشد";
                return RedirectToAction("Create");
            }
            if (marketerImprovePlan.CurrentPlanTypeId ==
                marketerImprovePlan.CanGoPlanTypeId && marketerImprovePlan.LevelCanGo ==
                marketerImprovePlan.CurrentLevel)
            {
                TempData["ErrorImprovePlan"] = "سطوح انتخابی یکسان است";
                return RedirectToAction("Create");
            }
            if(marketerImprovePlan.CurrentPlanTypeId == marketerImprovePlan.CanGoPlanTypeId
                && marketerImprovePlan.CurrentLevel > marketerImprovePlan.LevelCanGo)
            {
                TempData["ErrorImprovePlan"] = "سطح فعلی نباید بیشتر از سطح قابل ارتقا باشد";
                return RedirectToAction("Create");
            }

            var CheckCurrentplan = db.Plannns.Where(s => s.PlanTypeID == marketerImprovePlan.CurrentPlanTypeId).
                Any(p => p.Level == marketerImprovePlan.CurrentLevel);
            if (!CheckCurrentplan)
            {
                TempData["ErrorImprovePlan"] = "سطح فعلی در پلن های تعریف شده وجود ندارد";
                return RedirectToAction("Create");
            }

            var CheckCanGoplan = db.Plannns.Where(s => s.PlanTypeID == marketerImprovePlan.CanGoPlanTypeId).
                Any(p => p.Level == marketerImprovePlan.LevelCanGo);

            if (!CheckCanGoplan)
            {
                TempData["ErrorImprovePlan"] = "سطح قابل ارتقا در پلن های تعریف شده وجود ندارد";
                return RedirectToAction("Create");
            }
            if(marketerImprovePlan.Price == 0)
            {
                TempData["ErrorImprovePlan"] = "قیمت را تعیین کنید";
                return RedirectToAction("Create");
            }

            if (ModelState.IsValid)
            {
            

                db.MarketerImprovePlans.Add(marketerImprovePlan);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CanGoPlanTypeId = new SelectList(db.PlanTypes, "Id", "Name", marketerImprovePlan.CanGoPlanTypeId);
            ViewBag.CurrentPlanTypeId = new SelectList(db.PlanTypes, "Id", "Name", marketerImprovePlan.CurrentPlanTypeId);
            return View(marketerImprovePlan);
        }

        // GET: Admin/MarketerImprovePlans/Edit/5
        //public async Task<ActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    MarketerImprovePlan marketerImprovePlan = await db.MarketerImprovePlans.FindAsync(id);
        //    if (marketerImprovePlan == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.CanGoPlanTypeId = new SelectList(db.PlanTypes, "Id", "Name", marketerImprovePlan.CanGoPlanTypeId);
        //    ViewBag.CurrentPlanTypeId = new SelectList(db.PlanTypes, "Id", "Name", marketerImprovePlan.CurrentPlanTypeId);
        //    return View(marketerImprovePlan);
        //}

        // POST: Admin/MarketerImprovePlans/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,CurrentPlanTypeId,CurrentLevel,CanGoPlanTypeId,LevelCanGo,Price")] MarketerImprovePlan marketerImprovePlan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(marketerImprovePlan).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CanGoPlanTypeId = new SelectList(db.PlanTypes, "Id", "Name", marketerImprovePlan.CanGoPlanTypeId);
            ViewBag.CurrentPlanTypeId = new SelectList(db.PlanTypes, "Id", "Name", marketerImprovePlan.CurrentPlanTypeId);
            return View(marketerImprovePlan);
        }

        // GET: Admin/MarketerImprovePlans/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MarketerImprovePlan marketerImprovePlan = await db.MarketerImprovePlans.FindAsync(id);
            if (marketerImprovePlan == null)
            {
                return HttpNotFound();
            }
            return View(marketerImprovePlan);
        }

        // POST: Admin/MarketerImprovePlans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            MarketerImprovePlan marketerImprovePlan = await db.MarketerImprovePlans.FindAsync(id);
            db.MarketerImprovePlans.Remove(marketerImprovePlan);
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
    }
}
