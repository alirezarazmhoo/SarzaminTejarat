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

namespace WebApplication1.Areas.Admin.Controllers.RetailerAndMultipateBuyer
{
    public class ChecksController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Admin/Checks
        public async Task<ActionResult> Index()
        {
            var url = "/Admin/Checks/Index";
            var data =db.Checks.Include(c => c.Bank).Include(c => c.MarketerUser).AsQueryable();

            var res = data.OrderByDescending(p => p.Id);
            ViewBag.Data = new PagedItem<Check>(res, url);
            return View(await data.ToListAsync());
        }



        // GET: Admin/Checks/Create
        public ActionResult Create()
        {
            ViewBag.BankId = new SelectList(db.Banks, "Id", "Name");
            ViewBag.MarketerUserId = new SelectList(db.MarketerUsers.Where(s=>s.Usertype==1 || s.Usertype == 2), "Id", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,MarketerUserId,CheckStatus,BankId,Date,Price,CheckCode")] Check check,string Date)
        {
            if(Date != null)
            {
                string[] formats = { "yyyy-MMM-dd" };
                DateTime dt;
                if (!DateTime.TryParse(Date.ToString(), out dt))
                {
                    TempData["ErrorDatee"] = "فرمت تاریخ صحیح نمی باشد";
                return RedirectToAction(nameof(Create));
                }

            }
            if (ModelState.IsValid)
            {
            check.Date= Utility.DateChanger.ToGeorgianDateTime(Date);
                db.Checks.Add(check);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
    
                ViewBag.BankId = new SelectList(db.Banks, "Id", "Name", check.BankId);
            ViewBag.MarketerUserId = new SelectList(db.MarketerUsers, "Id", "Name", check.MarketerUserId);
            return View(check);
        }

        // GET: Admin/Checks/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Check check = await db.Checks.FindAsync(id);
            if (check == null)
            {
                return HttpNotFound();
            }
            ViewBag.BankId = new SelectList(db.Banks, "Id", "Name", check.BankId);
            ViewBag.MarketerUserId = new SelectList(db.MarketerUsers, "Id", "Name", check.MarketerUserId);
            return View(check);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,MarketerUserId,CheckStatus,BankId,Date,Price,CheckCode")] Check check, string Date)
        {
            if (Date != null)
            {
                string[] formats = { "yyyy-MMM-dd" };
                DateTime dt;
                if (!DateTime.TryParse(Date.ToString(), out dt))
                {
                    TempData["ErrorDatee"] = "فرمت تاریخ صحیح نمی باشد";
                    return RedirectToAction(nameof(Create));
                }

            }
            if (ModelState.IsValid)
            {
                check.Date = Utility.DateChanger.ToGeorgianDateTime(Date);

                db.Entry(check).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.BankId = new SelectList(db.Banks, "Id", "Name", check.BankId);
            ViewBag.MarketerUserId = new SelectList(db.MarketerUsers, "Id", "Name", check.MarketerUserId);
            return View(check);
        }

        // GET: Admin/Checks/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Check check = await db.Checks.FindAsync(id);
            if (check == null)
            {
                return HttpNotFound();
            }
            return View(check);
        }

        // POST: Admin/Checks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Check check = await db.Checks.FindAsync(id);
            db.Checks.Remove(check);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
