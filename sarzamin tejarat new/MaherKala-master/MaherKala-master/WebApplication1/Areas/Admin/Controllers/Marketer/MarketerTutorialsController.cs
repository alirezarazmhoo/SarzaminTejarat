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

namespace WebApplication1.Areas.Admin.Controllers.Marketer
{
    public class MarketerTutorialsController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Admin/MarketerTutorials
        public ActionResult Index()
        {
            var url = "/Admin/MarketerTutorials/Index";
            var data = db.MarketerTutorials.Include(m => m.MarketerTutorialFiles).AsQueryable();


            var res = data.OrderByDescending(p => p.Id);
            ViewBag.Data = new PagedItem<MarketerTutorial>(res, url);

            return View();

            //return View(await db.MarketerTutorials.Include(m=>m.MarketerTutorialFiles).ToListAsync());
        }

        // GET: Admin/MarketerTutorials/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.IdPost = id;
            MarketerTutorial marketerTutorial = await db.MarketerTutorials.FindAsync(id);
            if (marketerTutorial == null)
            {
                return HttpNotFound();
            }
            return View(marketerTutorial);
        }

        // GET: Admin/MarketerTutorials/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/MarketerTutorials/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       
        public async Task<ActionResult> Create(MarketerTutorial marketerTutorial, HttpPostedFileBase[] Main_TextFile, HttpPostedFileBase[] Images)
        {
            if (ModelState.IsValid)
            {
             marketerTutorial.Description =   marketerTutorial.Description.Trim();
                

                db.MarketerTutorials.Add(marketerTutorial);


                if (Images != null)
                {


                    foreach (HttpPostedFileBase file in Images)
                    {
                        
                       
                      

                        if (file != null)
                        {
                            var InputFileName = Path.GetFileName(file.FileName);
                            var ServerSavePath = Path.Combine(Server.MapPath("~/Upload/Tutotial/") + InputFileName);
                            //Save file to server folder  
                            file.SaveAs(ServerSavePath);
                            //assigning file uploaded status to ViewBag for showing message to user.  

                            if (!(file.ContentType == "image/jpeg" || file.ContentType == "image/png" || file.ContentType == "image/bmp"))
                            {
                                TempData["Error"] = "نوع تصویر غیر قابل قبول است";
                                return RedirectToAction("Create");
                            }

                            db.MarketerTutorialFiles.Add(new MarketerTutorialFiles
                            {
                                MarketerTutorialID = marketerTutorial.Id,
                                ImageUrl = "Upload/Tutotial/" + InputFileName
                            });
                        }

                    }
                }
                foreach (HttpPostedFileBase TextFile in Main_TextFile)
                {

                    if (TextFile != null)
                    {
                        var InputFileName = Path.GetFileName(TextFile.FileName);
                        var ServerSavePath = Path.Combine(Server.MapPath("~/Upload/Tutotial/") + InputFileName);

                        var fileExt = Path.GetExtension(TextFile.FileName).Substring(1);

                        var supportedTypes = new[] { "txt", "doc", "docx", "pdf", "xls", "xlsx" };

                        TextFile.SaveAs(ServerSavePath);


                        if (!supportedTypes.Contains(fileExt))
                        {
                            TempData["Error"] = "فایل انتخابی در فرمت صحیح قرار ندارد";
                            return RedirectToAction("Create");
                        }

                        db.MarketerTutorialFiles.Add(new MarketerTutorialFiles
                        {
                            MarketerTutorialID = marketerTutorial.Id,
                            FileUrl = "Upload/Tutotial/" + InputFileName
                        });




                    }
                }



                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(marketerTutorial);
        }

        // GET: Admin/MarketerTutorials/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            ViewBag.IdPost = id;
            MarketerTutorial marketerTutorial = await db.MarketerTutorials.FindAsync(id);
            if (marketerTutorial == null)
            {
                return HttpNotFound();
            }
            return View(marketerTutorial);
        }

        // POST: Admin/MarketerTutorials/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Description")] MarketerTutorial marketerTutorial ,long[] listPic ,long[] listFile, HttpPostedFileBase[] Main_TextFile, HttpPostedFileBase[] Images)
        {

            if(listPic != null)
            {
                foreach (var item in listPic)
                {
                    var _item = db.MarketerTutorialFiles.Where(s => s.Id == item).FirstOrDefault();
                    if (_item != null)
                    {
                 
                        var findedItem = db.MarketerTutorialFiles.Find(item);
                   

                        db.MarketerTutorialFiles.Remove(findedItem);
                        System.IO.File.Delete(Server.MapPath("~/" + _item.ImageUrl));
  
                    }

                }
            }
            if(listFile != null)
            {
                foreach (var item in listFile)
                {
                    var _item = db.MarketerTutorialFiles.Where(s => s.Id == item).FirstOrDefault();
                    if (_item != null)
                    {

                        var findedItem = db.MarketerTutorialFiles.Find(item);


                        db.MarketerTutorialFiles.Remove(findedItem);
                        System.IO.File.Delete(Server.MapPath("~/" + _item.FileUrl));

                    }

                }

            }


            if (ModelState.IsValid)
            {

                if (Images != null)
                {


                    foreach (HttpPostedFileBase file in Images)
                    {




                        if (file != null)
                        {
                            var InputFileName = Path.GetFileName(file.FileName);
                            var ServerSavePath = Path.Combine(Server.MapPath("~/Upload/Tutotial/") + InputFileName);
                            //Save file to server folder  
                            file.SaveAs(ServerSavePath);
                            //assigning file uploaded status to ViewBag for showing message to user.  

                            if (!(file.ContentType == "image/jpeg" || file.ContentType == "image/png" || file.ContentType == "image/bmp"))
                            {
                                TempData["Error"] = "نوع تصویر غیر قابل قبول است";
                                return RedirectToAction("Create");
                            }

                            db.MarketerTutorialFiles.Add(new MarketerTutorialFiles
                            {
                                MarketerTutorialID = marketerTutorial.Id,
                                ImageUrl = "Upload/Tutotial/" + InputFileName
                            });
                        }

                    }
                }
 
                foreach (HttpPostedFileBase TextFile in Main_TextFile)
                {

                    if (TextFile != null)
                    {
                        var InputFileName = Path.GetFileName(TextFile.FileName);
                        var ServerSavePath = Path.Combine(Server.MapPath("~/Upload/Tutotial/") + InputFileName);

                        var fileExt = Path.GetExtension(TextFile.FileName).Substring(1);

                        var supportedTypes = new[] { "txt", "doc", "docx", "pdf", "xls", "xlsx" };

                        TextFile.SaveAs(ServerSavePath);


                        if (!supportedTypes.Contains(fileExt))
                        {
                            TempData["Error"] = "فایل انتخابی در فرمت صحیح قرار ندارد";
                            return RedirectToAction("Create");
                        }

                        db.MarketerTutorialFiles.Add(new MarketerTutorialFiles
                        {
                            MarketerTutorialID = marketerTutorial.Id,
                            FileUrl = "Upload/Tutotial/" + InputFileName
                        });

                    }
                }


                marketerTutorial.Description = marketerTutorial.Description.Trim();

                db.Entry(marketerTutorial).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(marketerTutorial);
        }

        // GET: Admin/MarketerTutorials/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.IdPost = id;
            MarketerTutorial marketerTutorial = await db.MarketerTutorials.FindAsync(id);
            //MarketerTutorialFiles marketerTutorialFiles = db.MarketerTutorialFiles.Include(m => m.MarketerTutorial).Where(s => s.MarketerTutorialID == id || s.MarketerTutorial.Id ==id).FirstOrDefault();
            if (marketerTutorial == null  )
            {
                return HttpNotFound();
            }
            return View(marketerTutorial);
        }

        // POST: Admin/MarketerTutorials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var Pictures = db.MarketerTutorialFiles.Where(s => s.MarketerTutorialID == id && s.ImageUrl !=null).ToList();
            if(Pictures.Count != 0)
            {
                foreach (var item in Pictures)
                {
                    System.IO.File.Delete(Server.MapPath("~/" + item.ImageUrl));

                }

            }
            var Files = db.MarketerTutorialFiles.Where(s => s.MarketerTutorialID == id && s.FileUrl != null).ToList();
            if(Files.Count != 0)
            {
                foreach (var item in Files)
                {
                    System.IO.File.Delete(Server.MapPath("~/" + item.FileUrl));

                }
            }

            MarketerTutorial marketerTutorial = await db.MarketerTutorials.FindAsync(id);



            db.MarketerTutorials.Remove(marketerTutorial);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

     
        public ActionResult RemoveFiles(int? id)
        {
            var item = db.MarketerTutorialFiles.Where(s => s.Id == id).FirstOrDefault();
            if (item != null)
            {

                var idpost = item.MarketerTutorialID;

                var post = db.MarketerTutorials.Where(s => s.Id == idpost).FirstOrDefault();
                var findedItem = db.MarketerTutorialFiles.Find(id);
                ViewData["idpost"] = idpost;
            
                    db.MarketerTutorialFiles.Remove(findedItem);
                    System.IO.File.Delete(Server.MapPath("~/" + item.ImageUrl));
                    db.SaveChanges();

                
            return View("Edit", post);
            }
            //return RedirectToAction("Edit", "MarketerTutorials", idpost );
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
