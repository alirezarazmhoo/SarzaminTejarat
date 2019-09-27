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
    public class MarketerPlansController : Controller
    {

      
        private DBContext db = new DBContext();


        public  ActionResult Index()
        {
            var url = "/Admin/MarketerPlans/Index";
            var data = db.Plannns.Include(m => m.PlanType).AsQueryable();


            var res = data.OrderByDescending(p => p.Id);
            ViewBag.Data = new PagedItem<Plannn>(res, url);



            //var plans = db.Plannns.Include(p => p.PlanType);
            return View();
        }

        // GET: Admin/MarketerPlans/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Plannn plan = await db.Plannns.FindAsync(id);
            if (plan == null)
            {
                return HttpNotFound();
            }
            return View(plan);
        }

        // GET: Admin/MarketerPlans/Create
        public ActionResult Create()
        {
            ViewBag.PlanTypeID = new SelectList(db.PlanTypes, "Id", "Name");
            return View();
        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Plannn plan,HttpPostedFileBase Main_Image)
        {
            if(plan.Level == 0 || plan.Price == 0)
            {
                TempData["RepeatError"] = "فیلدهای مورد نیاز خالی است";
                return RedirectToAction("Create");

            }


            var checkLevel = db.Plannns.Where(p => p.PlanTypeID == plan.PlanTypeID).ToList();
            var max = 0;
            foreach (var item in checkLevel)
            {
                max = item.Level;
                if (item.Level >= max)
                {
                    max = item.Level;
                }


            }
            if (plan.Level == max + 1)
            {


                var CheckPrice = db.Plannns.Where(p => p.PlanType.Id == plan.PlanTypeID);
                CheckPrice.ToList();
                foreach (var item in CheckPrice)
                {
                    if (plan.Price <= item.Price)
                    {
                        TempData["RepeatError"] = "ارزش پلن باید بیشتر از دیگر پلن ها در این سطح باشد";
                        return RedirectToAction("Create");

                    }

                }



                var hasExist = db.Plannns.Where(p => (p.PlanTypeID == plan.PlanTypeID && p.Level == plan.Level && p.Price == plan.Price) || (p.Level == plan.Level && p.PlanTypeID == plan.PlanTypeID)).FirstOrDefault();
                if (hasExist != null)
                {
                    TempData["RepeatError"] = "مشخصات پلن تکراری است";
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
                        var imageUrl = "/Upload/Plans/" + name;
                        string path = Server.MapPath(imageUrl);
                        img.SaveAs(path);
                        plan.ImageUrl = imageUrl;
                    }


                    db.Plannns.Add(plan);
                    await db.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["RepeatError"] = "سطح بندی پلن ها باید به ترتیب باشد";
                return RedirectToAction("Create");

            }

            ViewBag.PlanTypeID = new SelectList(db.PlanTypes, "Id", "Name", plan.PlanTypeID);
            return View(plan);
        }

        // GET: Admin/MarketerPlans/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Plannn plan = await db.Plannns.FindAsync(id);
            if (plan == null)
            {
                return HttpNotFound();
            }
            ViewBag.PlanTypeID = new SelectList(db.PlanTypes, "Id", "Name", plan.PlanTypeID);
            return View(plan);
        }


        [HttpPost]

        public async Task<ActionResult> Edit([Bind(Include = "Id,Level,Description,ImageUrl,Price,PlanTypeID,Main_Image")] Plannn plan, HttpPostedFileBase Main_Image)
        {
           
            var item = db.Plannns.Where(s => s.Id == plan.Id).FirstOrDefault();
            if (ModelState.IsValid)
            {
                if (Main_Image != null)
                {
                    var img = Main_Image;
                    var url = item.ImageUrl;
                    if(item.ImageUrl != null)
                    {

                    System.IO.File.Delete(Server.MapPath(url));
                    }


                    if (!(img.ContentType == "image/jpeg" || img.ContentType == "image/png" || img.ContentType == "image/bmp"))
                    {
                        TempData["Error"] = "نوع تصویر غیر قابل قبول است";
                        return RedirectToAction("Create");
                    }
                    var name = Guid.NewGuid().ToString().Replace('-', '0') + "." + img.FileName.Split('.')[1];
                    var imageUrl = "/Upload/Plans/" + name;
                    string path = Server.MapPath(imageUrl);
                    img.SaveAs(path);
                item.ImageUrl = imageUrl;
                }
  
          
                item.Description = plan.Description;
                item.Level = plan.Level;
                item.PlanTypeID = plan.PlanTypeID;
                item.Price = plan.Price;


              
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.PlanTypeID = new SelectList(db.PlanTypes, "Id", "Name", plan.PlanTypeID);
            return View(plan);
        }


        public async Task<ActionResult> Delete(int? id)
        {
       
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Plannn plan = await db.Plannns.FindAsync(id);
            if (plan == null)
            {
                return HttpNotFound();
            }
            return View(plan);
        }

 
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {

            bool isUserInPlan = db.MarketerUsers.Any(p => p.PlannnID == id);
            if (isUserInPlan)
            {
                TempData["RemoveError"] = "دراین پلن چندین بازاراب وجود دارد و قابل حذف نمی باشد";
                return RedirectToAction("Delete");
            }



            var Item = db.Plannns.Where(p => p.Id == id).FirstOrDefault();
        
            if (Item != null)
            {
                if (Item.ImageUrl != null)
                {
                    System.IO.File.Delete(Server.MapPath(Item.ImageUrl));

                }
            }

            if (Item.Level == 1)
            { 
      
                return RedirectToAction("Index");
            }

            Plannn plan = await db.Plannns.FindAsync(id);
            db.Plannns.Remove(plan);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
