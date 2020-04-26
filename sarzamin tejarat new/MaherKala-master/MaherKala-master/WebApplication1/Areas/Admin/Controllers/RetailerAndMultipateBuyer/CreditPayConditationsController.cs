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
    public class CreditPayConditationsController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Admin/CreditPayConditations
        public ActionResult Index()
        {
            var url = "/Admin/CreditPayConditations/Index";
            var data = db.creditPayConditations.AsQueryable();
            var res = data.OrderByDescending(p => p.Id);
            ViewBag.Data = new PagedItem<CreditPayConditations>(res, url);
            return View();
        }

  

        // GET: Admin/CreditPayConditations/Create
        public ActionResult Create()
        {
            return View();
        }

 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Description,CheckPrice,Interestrate,InitialPayment,conditaionType")] CreditPayConditations creditPayConditations)
        {
            if (ModelState.IsValid)
            {
                db.creditPayConditations.Add(creditPayConditations);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(creditPayConditations);
        }

        // GET: Admin/CreditPayConditations/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CreditPayConditations creditPayConditations = await db.creditPayConditations.FindAsync(id);
            if (creditPayConditations == null)
            {
                return HttpNotFound();
            }
            return View(creditPayConditations);
        }

        // POST: Admin/CreditPayConditations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Description,CheckPrice,Interestrate,InitialPayment,conditaionType")] CreditPayConditations creditPayConditations)
        {
            if (ModelState.IsValid)
            {
                db.Entry(creditPayConditations).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(creditPayConditations);
        }

        // GET: Admin/CreditPayConditations/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CreditPayConditations creditPayConditations = await db.creditPayConditations.FindAsync(id);
            if (creditPayConditations == null)
            {
                return HttpNotFound();
            }
            return View(creditPayConditations);
        }

        // POST: Admin/CreditPayConditations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CreditPayConditations creditPayConditations = await db.creditPayConditations.FindAsync(id);
            db.creditPayConditations.Remove(creditPayConditations);
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
