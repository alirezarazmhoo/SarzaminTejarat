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
using System.IO;
using WebApplication1.Areas.CustomerPaymentManager.Authorize;

namespace WebApplication1.Areas.CustomerPaymentManager.Controllers
{
    [PaymentAuthorize]
    public class MyCheckPaymentRequestsController : Controller
    {
        private DBContext db = new DBContext();

        // GET: CustomerPaymentManager/MyCheckPaymentRequests
        public async Task<ActionResult> Index()
        {
            IQueryable<CheckPaymentRequestAttemp> queryable = db.CheckPaymentRequestAttemps.Include(c => c.CheckPaymentConditaion).Include(c => c.MarketerUser);
            IQueryable<CheckPaymentRequestAttemp> checkPaymentRequestAttemps = queryable;
            return View(await checkPaymentRequestAttemps.ToListAsync());
        }

        // GET: CustomerPaymentManager/MyCheckPaymentRequests/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
     
            CheckPaymentRequestAttemp checkPaymentRequestAttemp = await db.CheckPaymentRequestAttemps.FindAsync(id);
            CheckPaymentConditaion checkPaymentConditaion = await db.checkPaymentConditaions.Where(s=>s.Id == 
            checkPaymentRequestAttemp.CheckPaymentConditaionId).FirstOrDefaultAsync();

            if (checkPaymentConditaion == null)
            {
                return HttpNotFound();
            }
            else
            {
                ViewBag.Description = checkPaymentConditaion.Description;
                ViewBag.checkprice = checkPaymentConditaion.CheckPrice;
                ViewBag.Interestrate = checkPaymentConditaion.Interestrate;
                ViewBag.InitialPayment = checkPaymentConditaion.InitialPayment;
                ViewBag.Type = checkPaymentConditaion.conditaionType;
                ViewBag.AdminComment = checkPaymentRequestAttemp.AdminComment;
            }
            ViewBag.ImageUrl = await db.CheckPaymentRequestAttempPictures.Where(s => s.checkPaymentRequestAttempId == id).ToListAsync();
            return View(checkPaymentRequestAttemp);
        }

        // GET: CustomerPaymentManager/MyCheckPaymentRequests/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CheckPaymentRequestAttemp checkPaymentRequestAttemp = await db.CheckPaymentRequestAttemps.FindAsync(id);         
            ViewBag.ImageUrl = await db.CheckPaymentRequestAttempPictures.Where(s => s.checkPaymentRequestAttempId == id).ToListAsync();
            if (checkPaymentRequestAttemp == null)
            {
                return HttpNotFound();
            }
            return View();
        }
        // POST: CustomerPaymentManager/MyCheckPaymentRequests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,CheckPaymentRequestAttempStatus,CreatedDate,AdminComment,MarketerUserId,CheckPaymentConditaionId")] CheckPaymentRequestAttemp checkPaymentRequestAttemp, long[] listPic, HttpPostedFileBase[] Images)
        {
            if (listPic != null)
            {
                foreach (var item in listPic)
                {
                    var _item = db.CheckPaymentRequestAttempPictures.Where(s => s.Id == item).FirstOrDefault();
                    if (_item != null)
                    {
                        var findedItem = db.CheckPaymentRequestAttempPictures.Find(item);
                        db.CheckPaymentRequestAttempPictures.Remove(findedItem);
                        System.IO.File.Delete(Server.MapPath("~/" + _item.ImageUrl));
                    }
                }
            }
            foreach (HttpPostedFileBase file in Images)
            {
                if (file != null)
                {
                    var InputFileName = Path.GetFileName(file.FileName);
                    var ServerSavePath = Path.Combine(Server.MapPath("~/Upload/CheckPaymentDocument/") + InputFileName);
                    file.SaveAs(ServerSavePath);
                    if (!(file.ContentType == "image/jpeg" || file.ContentType == "image/png" || file.ContentType == "image/bmp"))
                    {
                        TempData["Error"] = "نوع تصویر غیر قابل قبول است";
                        return View(checkPaymentRequestAttemp);
                    }
                    db.CheckPaymentRequestAttempPictures.Add(new CheckPaymentRequestAttempPictures
                    {
                        checkPaymentRequestAttempId = checkPaymentRequestAttemp.Id,
                        ImageUrl = "Upload/CheckPaymentDocument/" + InputFileName
                    });
                }

            }
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: CustomerPaymentManager/MyCheckPaymentRequests/Delete/5
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

        // POST: CustomerPaymentManager/MyCheckPaymentRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var Pictures = db.CheckPaymentRequestAttempPictures.Where(s => s.checkPaymentRequestAttempId == id && s.ImageUrl != null).ToList();
            if (Pictures.Count != 0)
            {
                foreach (var item in Pictures)
                {
                    System.IO.File.Delete(Server.MapPath("~/" + item.ImageUrl));

                }

            }
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
