using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Areas.Admin.Controllers
{
    public class SpecialProductController : Controller
    {
        DBContext db = new DBContext();
        // GET: Admin/SpecialProduct
        public ActionResult Index()
        {
            var products = db.Products;
            ViewBag.Inside = db.SpecialProducts.Include("Product.Category").ToList();
            var data = db.SpecialProducts.Include("Product").Select(p => p.Product).AsQueryable();
            var r = db.Products.Include("Category").Except(data);

            int? category_id = Convert.ToInt32(Request["Category_id"]);
            string name = Request["Name"];
            if (category_id!=null && category_id!=0)
            {
                r = r.Where(p => p.Category.Id == category_id);
            }
            if (name != null&&name!="")
            {
                r = r.Where(p => p.Name.Contains(name));
            }
            var res = r.OrderByDescending(p => p.Id);
            ViewBag.Outside = new PagedItem<Product>(res, "/Admin/SpecialProduct/Index");
            var categories = db.Database.SqlQuery<Category>("select * from Categories c where Id Not In (select Parent_Id from Categories where Parent_Id Is Not Null) and  c.Parent_Id Is Not null");
            ViewBag.Categories = categories;

            return View();
        }
        [HttpPost]
        public ActionResult Increase()
        {
            SpecialProduct s = new Models.SpecialProduct();
            var pid = Convert.ToInt32(Request["Product_Id"]);
            s.Product = db.Products.Include("Category").Where(w=>w.Id==pid).FirstOrDefault();
            string date = Request["Date"];
            var array = date.Split('/');
            PersianCalendar p = new PersianCalendar();
            DateTime dt = new DateTime(Convert.ToInt32(array[0]), Convert.ToInt32(array[1]), Convert.ToInt32(array[2]),p).Date;
            s.ExpireDate = dt;
            db.SpecialProducts.Add(s);
            db.SaveChanges();
            return RedirectToAction("Index");

            
        }
        public ActionResult Decrease(int id)
        {
            var p = db.SpecialProducts.Find(id);
            db.SpecialProducts.Remove(p);
            db.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}