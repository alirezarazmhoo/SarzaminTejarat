using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebApplication1.Filter;
using WebApplication1.Models;

namespace WebApplication1.Controllers.api.Marketer
{
    public class MarketerFactorController : ApiController
    {
        DBContext db = new DBContext();

     



        [MarketerAuthorize]
        [HttpPost]
        [Route("api/MarketerFactor/Store")]
        public object Store()
        {
            try
            {
                var tr = db.Database.BeginTransaction();
                int pid = Convert.ToInt32(HttpContext.Current.Request.Form["Product_Id"]);
                var product = db.Products.Include("Category").Where(p => p.Id == pid).FirstOrDefault();
                double totalSum = 0;
                var PriceForTranslate = db.PriceForTranslates.FirstOrDefault();

                var pricefortranslate = db.PriceForTranslates.FirstOrDefault();

                if (product.Qty == 0)
                {
                    return new { Message = 1 };

                }

                var token = HttpContext.Current.Request.Form["Api_Token"];
                var factor_id = Convert.ToInt32(HttpContext.Current.Request.Form["Factor_Id"]);

                var marketer = db.MarketerUsers.Where(p => p.Api_Token == token).FirstOrDefault();

                if (factor_id == 0)
                {
                    if (marketer.FactorCounter >= 5)
                    {
                        return new { Message = 4 };
                    }
                    var order = new MarketerFactor();
                    var BuyerAddress = (HttpContext.Current.Request.Form["BuyerAddress"]);
                    var Buyer = (HttpContext.Current.Request.Form["Buyer"]);
                    var BuyerMobile = (HttpContext.Current.Request.Form["BuyerMobile"]);

                    var BuyerPhoneNumber = HttpContext.Current.Request.Form["BuyerPhoneNumber"];
                    var BuyerPostalCode = (HttpContext.Current.Request.Form["BuyerPostalCode"]);
                    if (db.MarketerFactor.Any(p => p.BuyerMobile == BuyerMobile))
                    {
                        var marketerFactor = db.MarketerFactor.FirstOrDefault(p => p.BuyerMobile == BuyerMobile);
                        if (HttpContext.Current.Request.Form.AllKeys.Contains("Buyer"))
                            marketerFactor.Buyer = Buyer;
                        if (HttpContext.Current.Request.Form.AllKeys.Contains("BuyerAddress"))

                            marketerFactor.BuyerAddress = BuyerAddress;
                        if (HttpContext.Current.Request.Form.AllKeys.Contains("BuyerPhoneNumber"))

                            marketerFactor.BuyerPhoneNumber = BuyerPhoneNumber;
                        if (HttpContext.Current.Request.Form.AllKeys.Contains("BuyerPostalCode"))
                            marketerFactor.BuyerPostalCode = BuyerPostalCode;
                        order.MarketerUser = marketer;


                        order.Buyer = marketerFactor.Buyer;
                        order.BuyerAddress = marketerFactor.BuyerAddress;
                        order.BuyerMobile = marketerFactor.BuyerMobile;
                        order.BuyerPhoneNumber = marketerFactor.BuyerPhoneNumber;
                        order.BuyerPostalCode = marketerFactor.BuyerPostalCode;
                    }
                    else
                    {
                        order.MarketerUser = marketer;
                        order.Buyer = Buyer;
                        order.BuyerAddress = BuyerAddress;
                        order.BuyerMobile = BuyerMobile;
                        order.BuyerPhoneNumber = BuyerPhoneNumber;
                        order.BuyerPostalCode = BuyerPostalCode;
                    }
                    order.Status = 1;
                    order.Date = DateTime.Now;


                    order.IsAdminCheck = false;
                    order.IsAdminShow = false;

                    var detail = new MarketerFactorItem();
                    detail.Product = product;
                    detail.ProductName = product.Name;
                    detail.Qty = 1;
                    if(marketer.Usertype ==0)
                    {
                    detail.UnitPrice = product.MarketerPrice - product.Discount;
                        totalSum = detail.UnitPrice;

                    }
                    if (marketer.Usertype == 1)
                    {
                        detail.UnitPrice = product.MultiplicationBuyerPrice - product.Discount;
                        totalSum = detail.UnitPrice;

                    }
                    if (marketer.Usertype == 2)
                    {
                        detail.UnitPrice = product.RetailerPrice - product.Discount;
                        totalSum = detail.UnitPrice;

                    }

                    if (marketer.Usertype == 0 && pricefortranslate !=null)
                    {
                        float x = 0;
                        double y = 0;

                        if (totalSum > PriceForTranslate.Marketergratis)
                        {
                            detail.UnitPrice += 0;
                        }
                        else
                        {

                            x = pricefortranslate.MarketerPriceTranslate / 100;

                            y = detail.UnitPrice * x;
                            detail.UnitPrice += y;



                            detail.UnitPrice += x;

                        }

                    }

                    if (marketer.Usertype == 1 && pricefortranslate != null)
                    {
                        float x = 0;
                        double y = 0;

                        if (totalSum > PriceForTranslate.Buyergratis)
                        {
                            detail.UnitPrice += 0;
                        }
                        else
                        {
                            x = pricefortranslate.BigBuyerPriceTranslate / 100;

                            y = detail.UnitPrice * x;
                            detail.UnitPrice += y;



                            detail.UnitPrice += x;
                        }

                    }
                    if (marketer.Usertype == 2 && pricefortranslate != null)
                    {
                        float x = 0;
                        double y = 0;
                        
                        if (totalSum > PriceForTranslate.Retailergratis)
                        {
                            detail.UnitPrice += 0;
                        }
                        else
                        {
                            x =pricefortranslate.RetailerPriceTranslate / 100;

                            y = detail.UnitPrice * x;
                            detail.UnitPrice += y;
                        }

                    }
                    

                    order.MarketerFactorItems.Add(detail);
                    db.MarketerFactor.Add(order);
                    marketer.FactorCounter++;
                    db.SaveChanges();
                    
                }
                else
                {
                    var order = db.MarketerFactor.FirstOrDefault(p => p.Id == factor_id && p.Status == 1);
                    if (order == null)
                    {
                        return new { Message = 3 };
                    }
                    var res = db.MarketerFactorItem.Where(p => p.Product.Id == product.Id).Where(p => p.MarketerFactor.Id == order.Id).FirstOrDefault();
                    if (res != null)
                    {
                        if (res.Product.Qty - res.Qty <= 0)
                        {
                            return new { Message = 1 };

                        }
                        res.Qty++;
                        db.SaveChanges();


                    }
                    else
                    {
                        var detail = new MarketerFactorItem();
                        detail.Product = product;
                        detail.ProductName = product.Name;
                        detail.Qty = 1;
                        detail.UnitPrice = product.Price - product.Discount;
                        order.MarketerFactorItems.Add(detail);
                        db.SaveChanges();
                    }

                }
                tr.Commit();
            }
            catch (DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors
                                  .SelectMany(x => x.ValidationErrors)
                                  .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join(" - ", errorMessages);
                return new { Message = 2, Details = fullErrorMessage };
            }


            return new { Message = 0 };

        }

        [HttpGet]
        [Route("api/MarketerFactor/GetBuyerInfo")]

        public object GetBuyerInfo(string BuyerMobile)
        {
            MarketerFactor result = null;
            result = db.MarketerFactor.FirstOrDefault(p => p.BuyerMobile == BuyerMobile);
            return new
            {
                Data = result,
                Message = 0
            };
        }


        [MarketerAuthorize]
        [HttpPost]
        [Route("api/MarketerFactor/Index")]
        public object Index()
        {
            var token = HttpContext.Current.Request.Form["Api_Token"];
            int id = db.MarketerUsers.Where(p => p.Api_Token == token).FirstOrDefault().Id;
            var order = db.MarketerFactor.Include("MarketerFactorItems.Product").Where(p => p.MarketerUser.Id == id).Where(p => p.Status == 1)
                .Select(p => new
                { p.Id, p.Buyer, p.BuyerAddress, p.BuyerMobile, p.BuyerPhoneNumber, p.BuyerPostalCode, p.Date, Items = p.MarketerFactorItems.Select(a => new { Id = a.Id, Product_Id = a.Product.Id, a.Qty, UnitPrice = a.Product.Price - a.Product.Discount, a.ProductName, a.Product.Images, a.Product.Thumbnail, a.Product.Name, a.Product.Desc, CategoriName = a.Product.Category.Name, a.Product.Price, a.Product.Discount, a.Product.Tags, a.Product.Like, a.Product.Main_Image, a.Product.Status, a.Product.TotalVotes, a.Product.TotalComment, a.Product.Color, a.Product.IsOnlyForMarketer }) }
            ).ToList();





            if (order == null)
            {
                return new { Message = 1 };
            }



            return new
            {
                Message = 0,
                Items = order
                // Items = items
            };
        }

        //[MarketerAuthorize]
        [HttpPost]
        [Route("api/MarketerFactor/History")]

        public object History()
        {

            var token = HttpContext.Current.Request.Form["Api_Token"];
            int id = db.MarketerUsers.Where(p => p.Api_Token == token).FirstOrDefault().Id;


            var query = db.MarketerFactor.Include("MarketerFactorItems.Product").Where(p => p.Status == 0 || p.Status == 2).Where(p => p.MarketerUser.Id == id);
            //var Product = db.MarketerFactorItem.Where(p => p.MarketerFactor.Status == 0 || p.MarketerFactor.Status == 2).Where(p => p.MarketerFactor.MarketerUser.Id == id);


            var sdate = HttpContext.Current.Request.Form["StartDate"];
            var edate = HttpContext.Current.Request.Form["EndDate"];

            if (sdate != null)
            {
                System.DateTime sdateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
                sdateTime = sdateTime.AddMilliseconds(Convert.ToInt64(sdate));

                query = query.Where(p => p.Date >= sdateTime);
            }
            if (edate != null)
            {
                System.DateTime edateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
                edateTime = edateTime.AddMilliseconds(Convert.ToInt64(edate));

                query = query.Where(p => p.Date <= edateTime);
            }
            var data = query.Select(p => new { p.Id, p.Date, p.Buyer, p.BuyerAddress, p.BuyerMobile, p.BuyerPhoneNumber, p.BuyerPostalCode, p.Status, p.TotalPrice, Items = p.MarketerFactorItems.Select(a => new { Id = a.Id, Product_Id = a.Product.Id, a.Qty, UnitPrice = a.Product.Price - a.Product.Discount, a.ProductName, a.Product.Images, a.Product.Thumbnail, a.Product.Name, a.Product.Desc, CategoriName = a.Product.Category.Name, a.Product.Price, a.Product.Discount, a.Product.Tags, a.Product.Like, a.Product.Main_Image, a.Product.Status, a.Product.TotalVotes, a.Product.TotalComment, a.Product.Color, a.Product.IsOnlyForMarketer }) }).OrderByDescending(p => p.Id);

            //var Items = Product.Select(s => new { s.Product.Images,s.Product.Color,CategoriName=s.Product.Category.Name,s.Product.Desc,s.Product.Discount , s.Product.IsOnlyForMarketer, s.Product.Like, s.Product.Main_Image, s.Product.Name, s.Product.Price, s.Product.Qty, s.Product.Status, s.Product.Tags, s.Product.Thumbnail, s.Product.TotalComment, s.Product.TotalVotes }).ToList();
            return new
            {
                Message = 0,
                Data = new PagedItem<object>(data, ""),

            };
        }

        [MarketerAuthorize]
        [HttpPost]
        [Route("api/MarketerFactor/HistoryItems")]

        public object HistoryItems()
        {
            var token = HttpContext.Current.Request.Form["Api_Token"];
            int id = db.MarketerUsers.Where(p => p.Api_Token == token).FirstOrDefault().Id;
            var fid = Convert.ToInt32(HttpContext.Current.Request.Form["Factor_Id"]);

            var data = db.MarketerFactorItem.Include("Product").Where(p => p.MarketerFactor.Id == fid).OrderByDescending(p => p.Id)
                .Select(p => new { p.Id, p.ProductName, p.Qty, p.UnitPrice, p.Product.Main_Image, p.Product.Thumbnail }).ToList();
            return new
            {
                Message = 0,
                Data = data
            };
        }

        [HttpPost]
        [Route("api/MarketerFactor/DeleteProduct")]
        [MarketerAuthorize]

        public object DeleteItem()
        {
            var token = HttpContext.Current.Request.Form["Api_Token"];
            var fid = Convert.ToInt32(HttpContext.Current.Request.Form["Factor_Id"]);
            var item_id = Convert.ToInt32(HttpContext.Current.Request.Form["Item_Id"]);
            int id = db.MarketerUsers.Where(p => p.Api_Token == token).FirstOrDefault().Id;
            var factor = db.MarketerFactor.Where(p => p.Status == 1).Where(p => p.MarketerUser.Id == id).Where(p => p.Id == fid).FirstOrDefault();
            if (factor == null)
            {
                return new { Message = 1 };
            }
            var data = db.MarketerFactorItem.Include("Product.Category").Include("MarketerFactor").Where(p => p.MarketerFactor.Id == factor.Id).Where(p => p.Id == item_id).FirstOrDefault();

            if (data == null)
            {
                return new { Message = 1 };
            }
            //db.Products.Include("Category").Where(p => p.Id == data.Product.Id).FirstOrDefault().Qty += data.Qty;
            db.MarketerFactorItem.Remove(data);
            if (db.MarketerFactorItem.Where(p => p.MarketerFactor.Id == factor.Id).Count() == 1)
            {
                db.MarketerFactor.Remove(factor);
            }

            db.SaveChanges();

            return new { Message = 0 };
        }

        [HttpPost]
        [Route("api/MarketerFactor/Finalize")]
        [MarketerAuthorize]

        public object Finalize()
        {
            return null;
            //var tr = db.Database.BeginTransaction();
            //var token = HttpContext.Current.Request.Form["Api_Token"];
            //var fid = Convert.ToInt32(HttpContext.Current.Request.Form["Factor_Id"]);

            //var usr = db.MarketerUsers.Where(p => p.Api_Token == token).FirstOrDefault();
            //int id = usr.Id;
            //var factor = db.MarketerFactor.Include("MarketerFactorItems.Product.Category").Where(p => p.Id == fid).Where(p => p.Status == 1).Where(p => p.MarketerUser.Id == id).FirstOrDefault();
            //if (factor == null)
            //{
            //    return new { Message = 1 };
            //}
            //List<object> Empty = new List<object>();
            //foreach (var item in factor.MarketerFactorItems)
            //{
            //    item.UnitPrice = item.Product.Price - item.Product.Discount;
            //    item.ProductName = item.Product.Name;
            //    if (item.Product.Qty < item.Qty)
            //        Empty.Add(new { Detail = "محصول " + item.Product.Name + " به تعداد انتخابی شما وجود ندارد" });
            //}
            //if (Empty.Count > 0)
            //    return new { Message = 2, Empty };
            //factor.TotalPrice = factor.ComputeTotalPrice() + 15000;
            //factor.Status = 0;
            //factor.Date = DateTime.Now;
            //usr.FactorCounter--;
            //db.SaveChanges();
            //tr.Commit();
            //return new { Message = 0 };
        }

        [HttpPost]
        [Route("api/MarketerFactor/ChangeQty")]
        [MarketerAuthorize]

        public object ChangeQty()
        {
            var Qty = Convert.ToInt32(HttpContext.Current.Request.Form["Qty"]);
            var token = HttpContext.Current.Request.Form["Api_Token"];
            var item_id = Convert.ToInt32(HttpContext.Current.Request.Form["Item_Id"]);
            var factor_id = Convert.ToInt32(HttpContext.Current.Request.Form["Factor_Id"]);
            int id = db.MarketerUsers.Where(p => p.Api_Token == token).FirstOrDefault().Id;
            var order = db.MarketerFactor.Where(p => p.MarketerUser.Id == id).Where(p => p.Status == 1).Where(p => p.Id == factor_id).FirstOrDefault();
            if (order == null)
            {
                return new { Message = 1 };
            }
            var data = db.MarketerFactorItem.Include("Product.Category").Where(p => p.MarketerFactor.Id == factor_id && p.Id == item_id).FirstOrDefault();
            if (data == null)
            {
                return new { Message = 11 };
            }

            //data.Product.Qty += data.Qty;
            if (data.Product.Qty < Qty)
            {
                return new { Message = 2 };
            }

            data.Qty = Qty;
            //data.Product.Qty -= Qty;
            db.SaveChanges();
            return new { Message = 0 };

        }

        [HttpPost]
        [Route("api/MarketerFactor/GetReturned")]
        [MarketerAuthorize]

        public object GetReturned()
        {
            var token = HttpContext.Current.Request.Form["Api_Token"];

            var usr = db.MarketerUsers.Where(p => p.Api_Token == token).FirstOrDefault();
            var id = usr.Id;

            var money = db.MarketerFactor.Where(p => p.MarketerUser.Id == id).Where(a => a.Status == 2).Select(c => c.TotalPrice).DefaultIfEmpty(0).Sum();
            return new { Message = 0, Price = money };
        }

        [HttpPost]
        [Route("api/MarketerFactor/GetPaid")]
        //[MarketerAuthorize]

        public object GetPaid()
        {
            var token = HttpContext.Current.Request.Form["Api_Token"];

            var usr = db.MarketerUsers.Where(p => p.Api_Token == token).FirstOrDefault();
            var id = usr.Id;

            //var money = db.MarketerFactor.Where(p => p.MarketerUser.Id == id).Where(a => a.Status == 0).Sum(p => p.TotalPrice);
            //var money = db.MarketerFactor.Where(p => p.MarketerUser.Id == id || p.Status == 0).Sum(p => p.TotalPrice);

            //var money = db.MarketerFactor.Where(p => p.MarketerUser.Id == id && p.Status == 0);
            //if(money != null)
            //{
            //    money.Sum(p => p.TotalPrice);
            //}
            long _totalPrice = 0;
            var money = db.MarketerFactor.Where(p => p.MarketerUser.Id == id);
            foreach (var item in money)
            {
                if (item.Status == 0)
                {
                    _totalPrice += item.TotalPrice;
                }

            }





            return new { Message = 0, Price = _totalPrice };



            //return new { Message = 0, Price = money };
        }

  
  


}
}
