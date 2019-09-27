using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        DBContext db = new DBContext();
        // GET: Admin/Category
        [HttpGet]
        public ActionResult Index(Category category, int? parent_id)
        {
            var categories = db.Categories.Where(x => x.Parent.Id == parent_id).OrderByDescending(x => x.Id);
            if (parent_id != null)
            {
                ViewBag.CategoryTitle = db.Categories.Find(parent_id).Name;
            }
            var url = parent_id != null ? "/Admin/Category/Index?parent_id=" + parent_id : "/Admin/Category/Index?parent_id=null";
            var result = new PagedItem<Category>(categories, url);
            ViewBag.Data = result;
            ViewBag.Parent_Id = parent_id;
            return View();
        }

        public ActionResult back(int id)
        {

            var parent_id = db.Categories.Include("Parent").Where(p => p.Id == id).First();
            if (parent_id.Parent!=null)
            {
                int p = parent_id.Parent.Id;
                return Redirect("/Admin/Category?parent_id=" + p);
            }
            return Redirect("/Admin/Category");

        }

        [HttpPost]
        [Route("/Admin/Category/Store")]
        public ActionResult Store(Category category,int? parent_id)
        {
            if (Request.Files.Count != 1 || Request.Files["image"] == null|| Request.Files["image"].FileName == "")
            {
                TempData["Error"] = "تصویر را انتخاب کنید";
                return RedirectToAction("Index");
            }
            if(category.Name==""|| category.Name == null)
            {
                TempData["Error"] = "نام دسته بندی را انتخاب کنید";
                return RedirectToAction("Index");
            }


            var img = Request.Files["image"];
            var name = Guid.NewGuid().ToString().Replace('-', '0')+"." + img.FileName.Split('.')[1];

            var imageUrl = "/Upload/Categories/" + name;
            string path = Server.MapPath(imageUrl);
            img.SaveAs(path);
            category.Image = imageUrl;

            var image = Image.FromStream(img.InputStream, true, true);
            var thumb = ImageResizer.RezizeImage(image, 150, 150);
            name = Guid.NewGuid().ToString().Replace('-', '0') + "." + img.FileName.Split('.')[1];

            imageUrl = "/Upload/Categories/" + name;
            path = Server.MapPath(imageUrl);
            thumb.Save(path);
            category.Thumbnail = imageUrl;


            if (parent_id!=null)
            {
                category.Parent = db.Categories.Find(parent_id);
            }
            db.Categories.Add(category);
            db.SaveChanges();

            var back = parent_id == null ? "/Admin/Category?parent_id=null" : "/Admin/Category?parent_id=" + parent_id;

          return   Redirect(back);


        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var c = db.Categories.Find(id);
            var parent_id = c.Parent!=null ? c.Parent.Id.ToString() : "null";
            ViewBag.Data = c;
            ViewBag.parent_id = parent_id;
            return View();
        }

        [Route("/Admin/Category/Update")]
        [HttpPost]
        public ActionResult Update(Category category)
        {
            if (category.Name == "" || category.Name == null)
            {
                TempData["Error"] = "نام دسته بندی را انتخاب کنید";
                return Redirect("/Admin/Category/Edit/"+category.Id);
            }
            var update = db.Categories.Include("Parent").Where(p=>p.Id== category.Id).FirstOrDefault();
            update.Name = category.Name;
            var url = update.Parent != null ? "/Admin/Category?parent_id=" + update.Parent.Id : "/Admin/Category?parent_id=null";

            if (Request.Files.Count==1 && Request.Files["image"]!=null&& Request.Files["image"].ContentLength>0)
            {
                var img = Request.Files["image"];
                var name = Guid.NewGuid().ToString().Replace('-', '0') + "." + img.FileName.Split('.')[1];

                var imageUrl = "/Upload/Categories/" + name;
                string path = Server.MapPath(imageUrl);
                var tmp = update.Image;
                var tmp2 = update.Thumbnail;
                update.Image = imageUrl;

                img.SaveAs(path);

                var image = Image.FromStream(img.InputStream, true, true);
                var thumb = ImageResizer.RezizeImage(image, 150, 150);
                name = Guid.NewGuid().ToString().Replace('-', '0') + "." + img.FileName.Split('.')[1];

                imageUrl = "/Upload/Categories/" + name;
                path = Server.MapPath(imageUrl);
                update.Thumbnail=imageUrl;
                thumb.Save(path);
                db.SaveChanges();
                try
                {
                    System.IO.File.Delete(Server.MapPath(tmp));
                    System.IO.File.Delete(Server.MapPath(tmp2));
                }
                catch
                {

                }

                return Redirect(url);
            }

            db.SaveChanges();
            return Redirect(url);
        }

        ///**
        // * Remove the specified resource from storage.
        // *
        // * @param  \App\Category $category
        // * @return \Illuminate\Http\Response
        // */
        [HttpGet]
        [Route("Admin/Category/Delete")]

        public ActionResult Delete(int id)
        {
            //File::delete(public_path().$category->image);
            //int parent_id = Category.Parent.Id;
            var c = db.Categories.Include("Parent").Where(p => p.Id == id).First();
            var tmp = c.Image;
            var tmp2 = c.Thumbnail;
            var back = c.Parent == null ? "/Admin/Category?parent_id=null" :
               "/Admin/Category?parent_id=" + c.Parent.Id;
            db.Categories.Remove(c);
            db.SaveChanges();
            try
            {

                System.IO.File.Delete(Server.MapPath(tmp));
                System.IO.File.Delete(Server.MapPath(tmp2));
            }
            catch { }
           
            return Redirect(back);
        }
       
    }
}