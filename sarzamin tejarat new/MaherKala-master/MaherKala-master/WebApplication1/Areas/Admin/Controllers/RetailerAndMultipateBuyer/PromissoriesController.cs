﻿using System;
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
    public class PromissoriesController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Admin/Promissories
        public async Task<ActionResult> Index()
        {
			var url = "/Admin/Promissories/Index";
			var data = db.Promissory.Include(c => c.MarketerUser).AsQueryable();
			var res = data.OrderByDescending(p => p.Id);
			ViewBag.Data = new PagedItem<Promissory>(res, url);
			return View(await data.ToListAsync());
		}

        // GET: Admin/Promissories/Create
        public ActionResult Create()
        {
            ViewBag.MarketerUserId = new SelectList(db.MarketerUsers.Where(s=>s.Usertype !=0), "Id", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Price,Date,Creditor,MarketerUserId,CommittedAddress,PlaceOfPayment")] Promissory promissory, string Date)
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
				promissory.Date = Utility.DateChanger.ToGeorgianDateTime(Date);
				db.Promissory.Add(promissory);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.MarketerUserId = new SelectList(db.MarketerUsers.Where(s => s.Usertype != 0), "Id", "Name", promissory.MarketerUserId);
            return View(promissory);
        }

        // GET: Admin/Promissories/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Promissory promissory = await db.Promissory.FindAsync(id);
            if (promissory == null)
            {
                return HttpNotFound();
            }
            ViewBag.MarketerUserId = new SelectList(db.MarketerUsers.Where(s => s.Usertype != 0), "Id", "Name", promissory.MarketerUserId);
            return View(promissory);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Price,Date,Creditor,MarketerUserId,CommittedAddress,PlaceOfPayment")] Promissory promissory, string Date)
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
				promissory.Date = Utility.DateChanger.ToGeorgianDateTime(Date);
				db.Entry(promissory).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.MarketerUserId = new SelectList(db.MarketerUsers.Where(s => s.Usertype != 0), "Id", "Name", promissory.MarketerUserId);
            return View(promissory);
        }

        // GET: Admin/Promissories/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Promissory promissory = await db.Promissory.FindAsync(id);
            if (promissory == null)
            {
                return HttpNotFound();
            }
            return View(promissory);
        }

        // POST: Admin/Promissories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Promissory promissory = await db.Promissory.FindAsync(id);
            db.Promissory.Remove(promissory);
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
