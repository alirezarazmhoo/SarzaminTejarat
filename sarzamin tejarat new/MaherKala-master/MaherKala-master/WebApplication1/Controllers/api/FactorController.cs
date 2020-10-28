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

        [HttpPost]
        [Route("api/Factor/ShowProductsInfo")]
        public object ShowProductsInfo()
        {
            string productIds = HttpContext.Current.Request.Form["productIds"];
            string[] _ProductId = new string[] { };
            List<Product> products = new List<Product>();
            _ProductId = productIds.Split(',');
            foreach (var item in _ProductId)
            {
                int _id = Int32.Parse(item);
                var productitem = db.Products.Where(s => s.Id == _id).FirstOrDefault();
                if (productitem != null)
                {
                    products.Add(productitem);
                }
            }
            return new
            {
                Message = 0,
                Items = products.Select(p => new {
                    p.Id,
                    UnitPrice = p.Price - p.Discount,
                    p.Qty,
                    ProductName = p.Name,
                    p.Main_Image,
                    p.Thumbnail
                })
            };
        }


        [HttpPost]
        [Route("api/Factor/AdvancedStore")]
        public object AdvancedStore()
        {
            var FactorDetails = HttpContext.Current.Request.Form["FactorDetails"];
            var usertoken = HttpContext.Current.Request.Form["Api_Token"];
            string[] _FactorDetails = new string[] { };
            string[] _FactorDetailsSplit = new string[] { };
            FactorItem factorItem = new FactorItem();
            List<FactorItem> listfactoritem = new List<FactorItem>();
            int productid = 0;
            int productcount = 0;
            int _factorid = 0;
            long Totalsum = 0;
            if (string.IsNullOrEmpty(usertoken))
            {
                return new
                {
                    Message = 1,
                    MessageText = "توکن کاربر خالی است "
                };
            }
            if (string.IsNullOrEmpty(FactorDetails))
            {
                return new
                {
                    Message = 1,
                    MessageText = "سبد خرید خالی است"
                };
            }
            var useritem = db.Users.Where(p => p.Api_Token == usertoken).FirstOrDefault();
            if (useritem == null)
            {
                return new
                {
                    Message = 1,
                    MessageText = "کاربر مورد نظر یافت نشد"
                };
            }
            try
            {
                Product productitem = new Product();
                Factor factor = new Factor();
                factor.Status = false;
                factor.Date = DateTime.Now;
                factor.User = db.Users.Find(useritem.Id);
                factor.Buyer = useritem.Fullname;
                factor.Address = useritem.Address;
                factor.Mobile = useritem.Mobile;
                factor.IsAdminShow = false;
                factor.PostalCode = useritem.PostalCode == null ? "0" : useritem.PostalCode;
                factor.Discount_Amount = 0;
                db.Factors.Add(factor);
                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();
                _factorid = factor.Id;
                _FactorDetails = FactorDetails.Split(',');
                for (int i = 0; i < _FactorDetails.Count(); i++)
                {
                    _FactorDetailsSplit = _FactorDetails[i].Split('.');
                    productid = Int32.Parse(_FactorDetailsSplit[0]);
                    productcount = Int32.Parse(_FactorDetailsSplit[1]);
                    productitem = db.Products.Include("Category").Where(s => s.Id == productid).FirstOrDefault();
                    if(productitem == null)
                    {
                        db.Factors.Remove(db.Factors.Find(factor.Id));
                        db.SaveChanges();

                        return new
                        {
                            Message = 3,
                            MessageText = $"محصول با ای دی {productid} یافت نشد."
                        };
                    }
                    if (productitem != null)
                    {
                        if (productitem.Qty == 0)
                        {
                            db.Factors.Remove(db.Factors.Find(factor.Id));
                            db.SaveChanges();
                            return new
                            {
                                Message = 2,
                                MessageText = $"محصول {productitem.Name} به اتمام رسیده است "
                            };
                        }
                        if (productitem.Qty < productcount)
                        {
                            db.Factors.Remove(db.Factors.Find(factor.Id));
                            db.SaveChanges();
                            return new
                            {
                                Message = 2,
                                MessageText = $"تعداد محصول {productitem.Name}  از تعداد انتخابی کمتر است ، موجودی فعلی {productitem.Qty} عدد است"

                            };
                        }

                        listfactoritem.Add(new FactorItem()
                        {
                            Factor = factor,
                            Product = productitem,
                            ProductName = productitem.Name,
                            Qty = productcount,
                            UnitPrice = (productitem.Price - productitem.Discount) * productcount
                        });
                    }
                }
                if (listfactoritem.Count != 0)
                {
                    foreach (var item in listfactoritem)
                    {
                        Totalsum += item.UnitPrice;
                    }
                    factor = db.Factors.Where(s => s.Id == _factorid).FirstOrDefault();
                    if (factor != null)
                    {
                        factor.TotalPrice = Totalsum;
                    }
                    listfactoritem.ForEach(s => db.FactorItems.Add(s));
                    db.SaveChanges();
                }
                return new
                {
                    Message = 0,
                    factor
                };
            }
            catch (Exception ex)
            {
                db.Factors.Remove(db.Factors.Find(_factorid));
                db.SaveChanges();
                return new
                {
                    Message = 1,
                    MessageText = "عملیات انجام نشد",
                    Error = ex.Message
                };
            }
        }

    }
}
