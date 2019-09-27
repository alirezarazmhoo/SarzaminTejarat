using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebApplication1.Filter;
using WebApplication1.Models;

namespace WebApplication1.Controllers.api
{
    public class FactorController : ApiController
    {
        DBContext db = new DBContext();

        [ApiAuthorize]
        [HttpPost]
        [Route("api/Factor/Store")]
        public object Store()
        {
            int pid = Convert.ToInt32(HttpContext.Current.Request.Form["Id"]);
            var product = db.Products.Include("Category").Where(p => p.Id == pid).FirstOrDefault();
            var tr = db.Database.BeginTransaction();

            if (product.Qty == 0)
            {
                return new { Message = 1 };

            }

            var token = HttpContext.Current.Request.Form["Api_Token"];
            var id = db.Users.Where(p => p.Api_Token == token).FirstOrDefault().Id;
            var order = db.Factors.Where(p => p.Status == false).Where(p => p.User.Id == id).FirstOrDefault();
            if (order == null)
            {
                order = new Factor();
                order.Status = false;
                order.Date = DateTime.Now;
                order.User = db.Users.Find(id);
                order.Buyer = order.User.Fullname;
                order.Address = order.User.Address;
                order.Mobile = order.User.Mobile;
                //order.User_PostalCode= order.User.PostalCode;
                order.IsAdminShow = false;
                order.Discount_Amount = 0;

                var detail = new FactorItem();
                detail.Product = product;
                detail.ProductName = product.Name;
                detail.Qty = 1;
                detail.UnitPrice = product.Price - product.Discount;

                order.FactorItems.Add(detail);
                db.Factors.Add(order);
                db.SaveChanges();



            }
            else
            {
                var res = db.FactorItems.Where(p => p.Product.Id == product.Id).Where(p => p.Factor.Id == order.Id).FirstOrDefault();
                if (res != null)
                {
                    if (res.Product.Qty - res.Qty <= 0)
                    {
                        return new { Message = 2 };

                    }
                    res.Qty++;
                    db.SaveChanges();


                }
                else
                {
                    var detail = new FactorItem();
                    detail.Product = product;
                    detail.ProductName = product.Name;
                    detail.Qty = 1;
                    detail.UnitPrice = product.Price - product.Discount;
                    order.FactorItems.Add(detail);
                    db.SaveChanges();
                }
            }
            tr.Commit();


            return new { Message = 0 };

        }
        [ApiAuthorize]
        [HttpPost]
        [Route("api/Factor/Index")]
        public object Index()
        {
            var token = HttpContext.Current.Request.Form["Api_Token"];
            int id = db.Users.Where(p => p.Api_Token == token).FirstOrDefault().Id;
            var order = db.Factors.Include("FactorItems.Product").Where(p => p.User.Id == id).Where(p => p.Status == false).FirstOrDefault();
            if (order == null)
            {
                return new { Message = 1 };
            }
            if (order.FactorItems.Count == 0)
            {
                return new { Message = 1 };
            }


            return new
            {
                Message = 0,
                Date = order.Date,
                Items = order.FactorItems.Select(p => new { p.Id, UnitPrice = p.Product.Price-p.Product.Discount, p.Qty, ProductName = p.Product.Name, p.Product.Main_Image, p.Product.Thumbnail })
                // Items = items
            };
        }

        [HttpPost]
        [Route("api/Factor/History")]
        [ApiAuthorize]

        public object History()
        {
            var token = HttpContext.Current.Request.Form["Api_Token"];
            int id = db.Users.Where(p => p.Api_Token == token).FirstOrDefault().Id;

            var data = db.Factors.Where(p => p.Status == true).Where(p => p.User.Id == id).OrderByDescending(p => p.Id)
                .Select(p => new { p.Id, p.Date, p.TotalPrice }).ToList();
            return new
            {
                Message = 0,
                Data = data
            };
        }


        [HttpPost]
        [Route("api/Factor/HistoryItems")]
        [ApiAuthorize]

        public object HistoryItems()
        {
            var token = HttpContext.Current.Request.Form["Api_Token"];
            int id = db.Users.Where(p => p.Api_Token == token).FirstOrDefault().Id;
            var fid = Convert.ToInt32(HttpContext.Current.Request.Form["Factor_Id"]);

            var data = db.FactorItems.Include("Product").Where(p => p.Factor.Id == fid).OrderByDescending(p => p.Id)
                .Select(p => new { p.Id, p.ProductName, p.Qty, p.UnitPrice, p.Product.Main_Image }).ToList();
            return new
            {
                Message = 0,
                Data = data
            };
        }

        [HttpPost]
        [Route("api/Factor/DeleteProduct")]
        [ApiAuthorize]

        public object DeleteItem()
        {
            var token = HttpContext.Current.Request.Form["Api_Token"];
            var item_id = Convert.ToInt32(HttpContext.Current.Request.Form["Item_Id"]);
            int id = db.Users.Where(p => p.Api_Token == token).FirstOrDefault().Id;
            var factor = db.Factors.Where(p => p.Status == false).Where(p => p.User.Id == id).FirstOrDefault();
            if (factor == null)
            {
                return new { Message = 1 };
            }
            var data = db.FactorItems.Include("Product.Category").Where(p => p.Factor.Id == factor.Id).Where(p => p.Id == item_id).FirstOrDefault();

            if (data == null)
            {
                return new { Message = 1 };
            }
            //db.Products.Include("Category").Where(p => p.Id == data.Product.Id).FirstOrDefault().Qty += data.Qty;
            db.FactorItems.Remove(data);
            db.SaveChanges();
            return new { Message = 0 };
        }

        [HttpPost]
        [Route("api/Factor/ChangeQty")]
        [ApiAuthorize]

        public object ChangeQty()
        {
            var Qty = Convert.ToInt32(HttpContext.Current.Request.Form["Qty"]);
            var token = HttpContext.Current.Request.Form["Api_Token"];
            var fiid = Convert.ToInt32(HttpContext.Current.Request.Form["Id"]);
            int id = db.Users.Where(p => p.Api_Token == token).FirstOrDefault().Id;
            var order = db.Factors.Where(p => p.User.Id == id).Where(p => p.Status == false).FirstOrDefault();
            if (order == null)
            {
                return new { Message = 1 };
            }

            var data = db.FactorItems.Include("Product.Category").Where(p => p.Factor.Id == order.Id).Where(p => p.Id == fiid).FirstOrDefault();
            if (data == null)
            {
                return new { Message = 1 };
            }

            data.Product.Qty += data.Qty;
            if (data.Product.Qty < Qty)
            {
                return new { Message = 2 };
            }

            data.Qty = Qty;
            db.SaveChanges();
            return new { Message = 0 };

        }
    }
}
