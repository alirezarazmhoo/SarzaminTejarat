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
    public class CompaniesController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Admin/Companies
        public async Task<ActionResult> Index()
        {
            return View(await db.Companies.Where(s=>!s.Name.Contains("بدون شرکت")).ToListAsync());
        }
        // GET: Admin/Companies/Create
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Address,NationalCode,PhoneNumber,cityName,subCityName")] Company company)
        {
            if (ModelState.IsValid)
            {
                db.Companies.Add(company);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(company);
        }
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = await db.Companies.FindAsync(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Address,NationalCode,PhoneNumber,cityName,subCityName")] Company company)
        {
            if (ModelState.IsValid)
            {
                db.Entry(company).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(company);
        }
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = await db.Companies.FindAsync(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // POST: Admin/Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
			if(db.Products.Any(s=>s.CompanyID == id) || db.CompanyAgents.Any(s=>s.CompanyID == id))
			{
				TempData["ErrorRemoveCompany"] = "اطلاعات این شرکت در بخش های دیگر استفاده شده و قابل حذف نمی باشد";

            return RedirectToAction("Index");
			}



			Company company = await db.Companies.FindAsync(id);
            db.Companies.Remove(company);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public ActionResult awaitingList()
        {
            return View(db.CompanyAgents.Include(s=>s.Company).ToList());
        }
		public async Task<ActionResult> Active(int Id)
		{
			var UserItem =await db.CompanyAgents.Where(s => s.Id == Id).FirstOrDefaultAsync();

			if(UserItem != null)
			{
				UserItem.Status = true;
				await db.SaveChangesAsync();
			}
			return RedirectToAction(nameof(awaitingList));
		}
        public async Task<ActionResult> DeActive(int Id)
		{
			var UserItem = await db.CompanyAgents.Where(s => s.Id == Id).FirstOrDefaultAsync();

			if (UserItem != null)
			{
				UserItem.Status = false;
				await db.SaveChangesAsync();
			}
			return RedirectToAction(nameof(awaitingList));
		}
		public async Task<ActionResult> DeleteAjent(int Id)
		{
			db.CompanyAgents.Remove(db.CompanyAgents.Find(Id));
			await db.SaveChangesAsync();
			return RedirectToAction(nameof(awaitingList));
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
