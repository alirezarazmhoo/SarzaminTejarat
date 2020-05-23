using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Areas.CustomerPaymentManager.Authorize;
using WebApplication1.Models;

namespace WebApplication1.Areas.CustomerPaymentManager.Controllers
{
    [PaymentAuthorize]
    public class MyCreditPaymentRequestsController : Controller
    {
        private DBContext db = new DBContext();

        // GET: CustomerPaymentManager/MyCreditPaymentRequests
        public async Task<ActionResult> Index()
        {
            var creditPaymentRequestAttemps = db.CreditPaymentRequestAttemps.Include(c => c.CreditPayConditations).Include(c => c.MarketerUser);
            return View(await creditPaymentRequestAttemps.ToListAsync());
        }

        // GET: CustomerPaymentManager/MyCreditPaymentRequests/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CreditPaymentRequestAttemp creditPaymentRequestAttemp = await db.CreditPaymentRequestAttemps.FindAsync(id);
            CreditPayConditations creditPayConditations = await db.creditPayConditations.Where(s => s.Id ==
            creditPaymentRequestAttemp.CreditPayConditationsId).FirstOrDefaultAsync();

            if (creditPayConditations == null)
            {
                return HttpNotFound();
            }
            else
            {
                ViewBag.Description = creditPayConditations.Description;
                ViewBag.checkprice = creditPayConditations.CheckPrice;
                ViewBag.Interestrate = creditPayConditations.Interestrate;
                ViewBag.InitialPayment = creditPayConditations.InitialPayment;
                ViewBag.Type = creditPayConditations.conditaionType;
                ViewBag.AdminComment = creditPaymentRequestAttemp.AdminComment;
            }
            ViewBag.ImageUrl = await db.CreditPaymentRequestAttempPictures.Where(s => s.creditPaymentRequestAttempId == id).ToListAsync();
            return View(creditPaymentRequestAttemp);
        }

        // GET: CustomerPaymentManager/MyCreditPaymentRequests/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CreditPaymentRequestAttemp creditPaymentRequestAttemp = await db.CreditPaymentRequestAttemps.FindAsync(id);
            ViewBag.ImageUrl = await db.CreditPaymentRequestAttempPictures.Where(s => s.creditPaymentRequestAttempId == id).ToListAsync();
            if (creditPaymentRequestAttemp == null)
            {
                return HttpNotFound();
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,CreditPaymentRequestAttempStatus,CreatedDate,AdminComment,MarketerUserId,CreditPayConditationsId")] CreditPaymentRequestAttemp creditPaymentRequestAttemp, long[] listPic, HttpPostedFileBase[] Images)
        {
            if (listPic != null)
            {
                foreach (var item in listPic)
                {
                    var _item = db.CreditPaymentRequestAttempPictures.Where(s => s.Id == item).FirstOrDefault();
                    if (_item != null)
                    {
                        var findedItem = db.CreditPaymentRequestAttempPictures.Find(item);
                        db.CreditPaymentRequestAttempPictures.Remove(findedItem);
                        System.IO.File.Delete(Server.MapPath("~/" + _item.ImageUrl));
                    }
                }
            }
            foreach (HttpPostedFileBase file in Images)
            {
                if (file != null)
                {
                    var InputFileName = Path.GetFileName(file.FileName);
                    var ServerSavePath = Path.Combine(Server.MapPath("~/Upload/CreditPaymentDocument/") + InputFileName);
                    file.SaveAs(ServerSavePath);
                    if (!(file.ContentType == "image/jpeg" || file.ContentType == "image/png" || file.ContentType == "image/bmp"))
                    {
                        TempData["Error"] = "نوع تصویر غیر قابل قبول است";
                        return View(creditPaymentRequestAttemp);
                    }
                    db.CreditPaymentRequestAttempPictures.Add(new CreditPaymentRequestAttempPictures
                    {
                        creditPaymentRequestAttempId = creditPaymentRequestAttemp.Id,
                        ImageUrl = "Upload/CreditPaymentDocument/" + InputFileName
                    });
                }

            }
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
        // GET: CustomerPaymentManager/MyCreditPaymentRequests/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CreditPaymentRequestAttemp creditPaymentRequestAttemp = await db.CreditPaymentRequestAttemps.FindAsync(id);
            if (creditPaymentRequestAttemp == null)
            {
                return HttpNotFound();
            }
            return View(creditPaymentRequestAttemp);
        }

        // POST: CustomerPaymentManager/MyCreditPaymentRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var Pictures = db.CreditPaymentRequestAttempPictures.Where(s => s.creditPaymentRequestAttempId == id && String.IsNullOrEmpty(s.ImageUrl)==false).ToList();
            if (Pictures.Count != 0)
            {
                foreach (var item in Pictures)
                {
                    System.IO.File.Delete(Server.MapPath("~/" + item.ImageUrl));

                }

            }
            CreditPaymentRequestAttemp creditPaymentRequestAttemp = await db.CreditPaymentRequestAttemps.FindAsync(id);
            db.CreditPaymentRequestAttemps.Remove(creditPaymentRequestAttemp);
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
