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
using WebApplication1.Models.Errors;

namespace WebApplication1.Areas.Admin.Controllers.RetailerAndMultipateBuyer
{
    public class CheckPaymentConditaionsController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Admin/CheckPaymentConditaions
        public ActionResult Index()
        {
            var url = "/Admin/CheckPaymentConditaions/Index";
            var data = db.checkPaymentConditaions.AsQueryable();
            var res = data.OrderByDescending(p => p.Id);
            ViewBag.Data = new PagedItem<CheckPaymentConditaion>(res, url);
            return View();
        }



        // GET: Admin/CheckPaymentConditaions/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Description,CheckPrice,Interestrate,InitialPayment,conditaionType")] CheckPaymentConditaion checkPaymentConditaion)
        {
            if (ModelState.IsValid)
            {
                db.checkPaymentConditaions.Add(checkPaymentConditaion);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ModelState.AddModelError(string.Empty, ErrorsText.IncorrectInputNumber);

            return View(checkPaymentConditaion);
        }

        // GET: Admin/CheckPaymentConditaions/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CheckPaymentConditaion checkPaymentConditaion = await db.checkPaymentConditaions.FindAsync(id);
            if (checkPaymentConditaion == null)
            {
                return HttpNotFound();
            }
            return View(checkPaymentConditaion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Description,CheckPrice,Interestrate,InitialPayment,conditaionType")] CheckPaymentConditaion checkPaymentConditaion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(checkPaymentConditaion).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ModelState.AddModelError(string.Empty, ErrorsText.IncorrectInputNumber);
            return View(checkPaymentConditaion);
        }

        // GET: Admin/CheckPaymentConditaions/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CheckPaymentConditaion checkPaymentConditaion = await db.checkPaymentConditaions.FindAsync(id);
            if (checkPaymentConditaion == null)
            {
                return HttpNotFound();
            }
            return View(checkPaymentConditaion);
        }

        // POST: Admin/CheckPaymentConditaions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CheckPaymentConditaion checkPaymentConditaion = await db.checkPaymentConditaions.FindAsync(id);
            db.checkPaymentConditaions.Remove(checkPaymentConditaion);
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
