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
    public class ListCheckPaymentRequestAttempsController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Admin/ListCheckPaymentRequestAttemps
        public async Task<ActionResult> Index()
        {
            var checkPaymentRequestAttemps = db.CheckPaymentRequestAttemps.Include(c => c.CheckPaymentConditaion).Include(c => c.MarketerUser);
            return View(await checkPaymentRequestAttemps.ToListAsync());
        }

        // GET: Admin/ListCheckPaymentRequestAttemps/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CheckPaymentRequestAttemp checkPaymentRequestAttemp = await db.CheckPaymentRequestAttemps.FindAsync(id);
            if (checkPaymentRequestAttemp == null)
            {
                return HttpNotFound();
            }
            return View(checkPaymentRequestAttemp);
        }


        // GET: Admin/ListCheckPaymentRequestAttemps/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CheckPaymentRequestAttemp checkPaymentRequestAttemp = await db.CheckPaymentRequestAttemps.FindAsync(id);
            if (checkPaymentRequestAttemp == null)
            {
                return HttpNotFound();
            }
            return View(checkPaymentRequestAttemp);
        }

        // POST: Admin/ListCheckPaymentRequestAttemps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CheckPaymentRequestAttemp checkPaymentRequestAttemp = await db.CheckPaymentRequestAttemps.FindAsync(id);
            db.CheckPaymentRequestAttemps.Remove(checkPaymentRequestAttemp);
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
