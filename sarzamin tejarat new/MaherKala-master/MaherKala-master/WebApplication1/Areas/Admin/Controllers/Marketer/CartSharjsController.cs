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
    public class CartSharjsController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Admin/CartSharjs
        public async Task<ActionResult> Index()
        {
            return View(await db.CartSharjs.ToListAsync());
        }

      


        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]

        public async Task<ActionResult> Create([Bind(Include = "Id,Code,CartType")] CartSharj cartSharj)
        {
            if(cartSharj.Code == null)
            {
                TempData["EmptyCpode"] = "کدکارت شارژ خالی است";
                return RedirectToAction(nameof(Create));
            }


            if (ModelState.IsValid)
            {
                db.CartSharjs.Add(cartSharj);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(cartSharj);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CartSharj cartSharj = await db.CartSharjs.FindAsync(id);
            if (cartSharj == null)
            {
                return HttpNotFound();
            }
            return View(cartSharj);
        }

        // POST: Admin/CartSharjs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CartSharj cartSharj = await db.CartSharjs.FindAsync(id);
            db.CartSharjs.Remove(cartSharj);
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
