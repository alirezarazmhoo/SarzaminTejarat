using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class CategoriesController : Controller
    {
        // GET: Categories
        DBContext db = new DBContext();
        public ActionResult Index()
        {
            int id = Convert.ToInt32(Request["Category_Id"]);
            var pid = db.Categories.Include("Parent").Where(p => p.Id == id).FirstOrDefault().Parent.Id;
            var cateogries = db.Categories.Where(p => p.Parent.Id == pid).ToList();
            ViewBag.Categories = cateogries;
            var list = db.Categories.Where(c => c.Parent.Id == id).ToList();
            if (list.Count == 0)
            {
                var url = "/Categories/Index?Category_Id=" + id;
                var products = db.Products.Include("Category").Where(p => p.Status == true).Where(p => p.Category.Id == id);

                var name = Request["Name"];
                if (name != null && name != "")
                {
                    products = products.Where(p => p.Name.Contains(name));
                    url += "&Name=" + name;
                }
                var tmp = Request["PriceFrom"];
                if (tmp != null && tmp != "")
                {
                    long from = Convert.ToInt64(Request["PriceFrom"]);
                    products = products.Where(p => p.Price - p.Discount >= from);
                    url += "&PriceFrom=" + from;
                }
                tmp = Request["PriceTo"];
                if (tmp != null && tmp != "")
                {
                    long? to = Convert.ToInt64(Request["PriceTo"]);
                    products = products.Where(p => p.Price - p.Discount <= to);
                    url += "&PriceTo=" + to;

                }

                var res = products.OrderByDescending(p => p.Id);
                long MaxPrice = 0;
                try
                {
                    var temp = db.Products.Where(p => p.Category.Id == id).Max(p => p.Price);
                    MaxPrice = temp;

                }
                catch (Exception)
                {

                }
                ViewBag.MaxPrice = MaxPrice;
                ViewBag.Data = new PagedItem<Product>(res, url, 16);
                ViewBag.Category = db.Categories.Include("Parent").Where(p => p.Id == id).FirstOrDefault();
                return View("ProductsList");
            }
            ViewBag.Data = list;
            return View("CategoriesList");
        }
        [HttpPost]
        public object AjaxData()
        {

            int id = Convert.ToInt32(Request["Category_Id"]);

            var products = db.Products.Include("Category").Where(p => p.Status == true).Where(p => p.Category.Id == id);

            var name = Request["Name"];
            if (name != null && name != "")
            {
                products = products.Where(p => p.Name.Contains(name));
            }
            int flag = Convert.ToInt32(Request["changeCategory"]);
            if (flag == 0)
            {

                var tmp = Request["PriceFrom"];
                if (tmp != null && tmp != "")
                {
                    long from = Convert.ToInt64(Request["PriceFrom"]);
                    products = products.Where(p => p.Price - p.Discount >= from);
                }
                tmp = Request["PriceTo"];
                if (tmp != null && tmp != "")
                {
                    long? to = Convert.ToInt64(Request["PriceTo"]);
                    products = products.Where(p => p.Price - p.Discount <= to);

                }
            }


            var res = products.OrderByDescending(p => p.Id);
            long MaxPrice = 0;
            long MinPrice = 0;

            if (db.Products.Where(p => p.Category.Id == id).Count() > 0)
            {

                MaxPrice = db.Products.Where(p => p.Category.Id == id).Max(p => p.Price);
                MinPrice = db.Products.Where(p => p.Category.Id == id).Min(p => p.Price);
            }

            JavaScriptSerializer js = new JavaScriptSerializer();
            var paged = new PagedItem<Product>(res, "", 16);
            long pf = -1;
            long pt = -1;

            if ((Request["PriceFrom"]) != null || (Request["PriceFrom"]) != "")
            {
                pf = Convert.ToInt64(Request["PriceFrom"]);
            }
            if ((Request["PriceTo"]) != null || (Request["PriceTo"]) != "")
            {
                pf = Convert.ToInt64(Request["PriceFrom"]);
            }
            var obj = new { Links = paged.GetAjaxLinks(id), Data = paged, MaxPrice = MaxPrice, MinPrice = MinPrice, PriceFrom = pf, PriceTo = pt };

            var result = js.Serialize(obj);
            return result;
        }
    }
}