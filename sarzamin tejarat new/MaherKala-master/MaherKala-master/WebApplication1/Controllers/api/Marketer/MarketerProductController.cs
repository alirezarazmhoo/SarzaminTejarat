using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebApplication1.Models;

namespace WebApplication1.Controllers.api.Marketer
{
    public class MarketerProductController : ApiController
    {
        DBContext db = new DBContext();
        [HttpGet]
        [Route("api/Product/MarketerProduct/GetProduct")]

        public object GetProduct()
        {
            int id = Convert.ToInt32(HttpContext.Current.Request.QueryString["Id"]);
            var data = db.Products.Include("Category").Where(p => p.Status == true).Where(p => p.Id == id).FirstOrDefault();
            return new
            {
                Data = data,
                Status = 0
            };
        }

        [HttpPost]
        [Route("api/Product/MarketerProduct/GetProducts")]

        public object GetProducts()
        {
            long MaxPrice = 0;
            long MinPrice = 0;
            var data = db.Products.Include("Category").Where(p => p.Status == true).AsQueryable();
            if (HttpContext.Current.Request.Form.AllKeys.Contains("status"))
            {
                bool status = Convert.ToBoolean(HttpContext.Current.Request.Form["status"]);
                if (status)
                    data = data.Where(p => p.Qty > 0);
                else
                    data = data.Where(p => p.Qty == 0);
            }
            if (HttpContext.Current.Request.Form.AllKeys.Contains("name"))
            {
                string name = HttpContext.Current.Request.Form["name"];
                data = data.Where(p => p.Name.Contains(name));
            }
            if (HttpContext.Current.Request.Form.AllKeys.Contains("minprice"))
            {
                int min = Convert.ToInt32(HttpContext.Current.Request.Form["minprice"]);
                data = data.Where(p => p.Price >= min);
            }
            if (HttpContext.Current.Request.Form.AllKeys.Contains("maxprice"))
            {
                int max = Convert.ToInt32(HttpContext.Current.Request.Form["maxprice"]);
                data = data.Where(p => p.Price <= max);
            }
            if (HttpContext.Current.Request.Form.AllKeys.Contains("category_id"))
            {
                List<int> list = new List<int>();
                int temp = Convert.ToInt32(HttpContext.Current.Request.Form["category_id"]);
                list.Add(temp);
                var main = db.Categories.Where(p => p.Parent.Id == temp).ToList();
                for (int i = 0; i < main.Count; i++)
                {
                    var id = main[i].Id;
                    list.AddRange(db.Categories.Where(p => p.Parent.Id == id).Select(p => p.Id).ToList());

                }

                data = data.Where(p => list.Contains(p.Category.Id));
                MinPrice = db.Products.Where(p => list.Contains(p.Category.Id)).Min(p => p.Price);
                MaxPrice = db.Products.Where(p => list.Contains(p.Category.Id)).Max(p => p.Price);
            }
            else
            {
                MinPrice = db.Products.Min(p => p.Price);
                MaxPrice = db.Products.Max(p => p.Price);
            }

            var res = data.OrderByDescending(p => p.Id);
            var Result = new PagedItem<Product>(res, "");
            return new
            {
                MinPrice = MinPrice,
                MaxPrice = MaxPrice,
                Result
            };
        }

        [HttpPost]
        [Route("api/Product/MarketerProduct/GetProductsLim")]

        public object GetProductsLim()
        {
            long MaxPrice = 0;
            long MinPrice = 0;
            MinPrice = db.Products.Min(p => p.Price);
            MaxPrice = db.Products.Max(p => p.Price);
            return new
            {
                MinPrice = MinPrice,
                MaxPrice = MaxPrice,
            };
        }
    }
}
