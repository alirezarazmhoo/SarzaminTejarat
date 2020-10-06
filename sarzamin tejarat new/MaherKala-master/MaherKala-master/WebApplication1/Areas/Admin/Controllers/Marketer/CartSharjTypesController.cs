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
    public class CartSharjTypesController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Admin/CartSharjTypes
        public async Task<ActionResult> Index()
        {
            return View(await db.CartSharjType.ToListAsync());
        }

       
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
     
        public async Task<ActionResult> Create([Bind(Include = "Id,typeCode,Name,Main_Image_Upload")] CartSharjType cartSharjType, HttpPostedFileBase Main_Image)
        {

			if (cartSharjType.Name =="" || Main_Image == null )
			{
				TempData["CartTypeError"] = "فیلدهای مورد نیاز خالی است";
				return RedirectToAction("Create");

			}


			if (ModelState.IsValid)
            {

				var img = Main_Image;
				if (img != null)
				{
					if (!(img.ContentType == "image/jpeg" || img.ContentType == "image/png" || img.ContentType == "image/bmp"))
					{
						TempData["Error"] = "نوع تصویر غیر قابل قبول است";
						return RedirectToAction("Create");
					}
					var name = Guid.NewGuid().ToString().Replace('-', '0') + "." + img.FileName.Split('.')[1];
					var imageUrl = name;
					string path = Server.MapPath(string.Concat("~/Upload/CartSharType/", imageUrl));
					img.SaveAs(path);
					cartSharjType.ImageUrl = imageUrl;
				}
				db.CartSharjType.Add(cartSharjType);
                await db.SaveChangesAsync();
                var item = db.CartSharjType.Where(s => s.Id == cartSharjType.Id).FirstOrDefault();
                item.typeCode = cartSharjType.Id;
                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(cartSharjType);
        }

        // GET: Admin/CartSharjTypes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CartSharjType cartSharjType = await db.CartSharjType.FindAsync(id);
            if (cartSharjType == null)
            {
                return HttpNotFound();
            }
            return View(cartSharjType);
        }


        [HttpPost]

        public async Task<ActionResult> Edit([Bind(Include = "Id,typeCode,Name,Main_Image")] CartSharjType cartSharjType, HttpPostedFileBase Main_Image)
        {
            var item = db.CartSharjType.Where(s => s.Id == cartSharjType.Id).FirstOrDefault();

			if (ModelState.IsValid)
            {


				if (Main_Image != null)
				{
					var img = Main_Image;
					var url = item.ImageUrl;
					if (item.ImageUrl != null)
					{
						System.IO.File.Delete(Server.MapPath(string.Concat("~/Upload/CartSharType/", item.ImageUrl)));

					}


					if (!(img.ContentType == "image/jpeg" || img.ContentType == "image/png" || img.ContentType == "image/bmp"))
					{
						TempData["Error"] = "نوع تصویر غیر قابل قبول است";
						return RedirectToAction("Edit");
					}
					var name = Guid.NewGuid().ToString().Replace('-', '0') + "." + img.FileName.Split('.')[1];
					var imageUrl =  name;
					string path = Server.MapPath(string.Concat("~/Upload/CartSharType/", imageUrl));
					img.SaveAs(path);
					item.ImageUrl = imageUrl;
				}
				item.Name = cartSharjType.Name;

	
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(cartSharjType);
        }

        // GET: Admin/CartSharjTypes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CartSharjType cartSharjType = await db.CartSharjType.FindAsync(id);
            if (cartSharjType == null)
            {
                return HttpNotFound();
            }
            return View(cartSharjType);
        }

		// POST: Admin/CartSharjTypes/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> DeleteConfirmed(int id)
		{

			bool IsHaveCartSharj = db.CartSharjs.Any(p => p.CartType == id);
			if (IsHaveCartSharj)
			{
				TempData["RemoveError"] = "در این نوع چندین کارت شارژ موجود است و قابل حذف نیست";
				return RedirectToAction("Delete");
			}

			var Item = db.CartSharjType.Where(p => p.Id == id).FirstOrDefault();

			if (Item != null)
			{
				if (Item.ImageUrl != null)
				{
					System.IO.File.Delete(Server.MapPath(string.Concat("~/Upload/CartSharType/", Item.ImageUrl)));

				}
			}



			CartSharjType cartSharjType = await db.CartSharjType.FindAsync(id);
			db.CartSharjType.Remove(cartSharjType);
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
