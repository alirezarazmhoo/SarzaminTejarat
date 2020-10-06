using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.Utility;

namespace WebApplication1.Controllers
{
    public class FactorController : Controller
    {
        // GET: Factor
        DBContext db = new DBContext();

        //[Authorize(Roles = "Member")]
        //[Authorize]
        public ActionResult Index()
        {

            long sum = 0;
            var cashedProducts = Session["ListProduct"] as List<FactorItem>;

            if(cashedProducts == null || cashedProducts.Count == 0)
            {
                return RedirectToAction("../Home/Index");
            }


            foreach (var item in cashedProducts)
            {
                sum += item.UnitPrice * item.Qty;
            }

            ViewBag.Order_Details = cashedProducts;
            ViewBag.OrderTotal =sum;
            return View();
        }
        [HttpGet]
        //[Authorize(Roles = "Member")]
        //[Authorize]
        public ActionResult Shipping()
        {

            if (!User.Identity.IsAuthenticated)
            {
                Session["ReturnUrl"] = "../Factor/Shipping";
                return Redirect("../User/Login");
            }
            var mobile = User.Identity.Name;
            ViewBag.User = db.Users.Where(p => p.Mobile == mobile).FirstOrDefault();         
            Setting s = db.Settings.FirstOrDefault();
            ViewBag.Esfahan = s.TransportationEsfahan;
            ViewBag.Najafabad = s.TransportationNajafabad;
            ViewBag.Other = s.TransportationOther;
            int id = ViewBag.User.Id;
            long sum = 0;
            var cashedProducts = Session["ListProduct"] as List<FactorItem>;
            if (cashedProducts == null)
            {
                return RedirectToAction("../Home/Index");
            }
            foreach (var item in cashedProducts)
            {
                sum += item.UnitPrice * item.Qty;
            
            }
            ViewBag.Order_Details = cashedProducts;
            ViewBag.OrderTotal = sum;
            ViewBag.Count = cashedProducts.Count();   
            if (Request["DiscountPro"] != null)
            {
                var dc = Request["DiscountPro"];
                DiscountCode d = db.DiscountCode.Where(p => p.Code == dc && p.Status == true && p.ExpireDate >= DateTime.Now).FirstOrDefault();
                if (d == null)
                {
                    TempData["Errors"] = "خطا : کد تخفیف اشتباه است یا منقضی شده";
                }
                else
                {
                    ViewBag.Discount_Amount = d.Price;
                    ViewBag.OrderTotal = sum - d.Price;
                    ViewBag.DiscountCode = d.Code;
                }
            }
            return View();
        }

        [HttpGet]
        [Authorize]

        public ActionResult Finalize(string Address,int? City_Id )
        {

            var Price = Request["Price"];
            var Discount = Request["Discount"];
            if (City_Id != 1 && City_Id != 2 && City_Id != 3)
            {
                TempData["Errors"] = "شهر را انتخاب کنید";
                return RedirectToAction("Shipping");
            }
            var email = User.Identity.Name;
            int id = db.Users.Where(p => p.Email == email).FirstOrDefault().Id;

            try
            {
                return Redirect($"/Payment/Index?Address={Address}&City_Id={City_Id}&Price={Price}&Discount={Discount}&UserId={id}");
            }
            catch (DbEntityValidationException)
            {
           
                TempData["Errors"] = "عملیات انجام نشد";
                return View("Shipping");
            }
        }
        [HttpPost]
        [Route("Factor/Store")]
        public JsonResult Store(int Id)
        {
            List<FactorItem> detail = new List<FactorItem>();
            List<FactorItem> listFactorItems = new List<FactorItem>();

    
 
            int pid = Id;
            var product = db.Products.Include("Category").Where(p => p.Id == pid).Where(p => p.Status == true).FirstOrDefault();
            if (product.Qty == 0)
            {
	     	return Json(new { success = false, responseText = "محصول مورد نظر به اتمام رسیده است" } , JsonRequestBehavior.AllowGet);
            }
            if (Session["ListProduct"] == null)
            {
                detail = new List<FactorItem>();
                detail.Add(new FactorItem()
                {
                    Product = product,
                    ProductName = product.Name,
                    Qty = 1,
                    UnitPrice = product.Price - product.Discount
                });
                Session["ListProduct"] = detail;
            }

            else
            {
                var cashedProducts = Session["ListProduct"] as List<FactorItem>;
                if (cashedProducts.Any(s => s.Product.Id == pid))
                {
                    string error = $"محصول ({product.Name}) قبلا به سبد خرید اضافه شده است .";
                    return Json(new { success = false, responseText = error }, JsonRequestBehavior.AllowGet);

                }
                foreach (var item in cashedProducts)
                {
                    listFactorItems.Add(item);
                }
                listFactorItems.Add(new FactorItem()
                {
                    Product = product,
                    ProductName = product.Name,
                    Qty = 1,
                    UnitPrice = product.Price - product.Discount
                });
                Session["ListProduct"] = listFactorItems;
            }
            //var tr = db.Database.BeginTransaction();
            //if (product.Qty == 0)
            //{
            //    return Redirect("/Products/Index?Id=" + Request["Id"] + "&" + "message=-1");

            //}
            //var email = User.Identity.Name;
            //var id = db.Users.Where(p => p.Email == email).FirstOrDefault();
            //var order = db.Factors.Where(p => p.Status == false).Where(p => p.User.Id == id.Id).FirstOrDefault();
            //if (order == null)
            //{
            //    order = new Factor();
            //    order.Status = false;
            //    order.Date = DateTime.Now;
            //    order.TotalPrice = 0;
            //    order.User = db.Users.Find(id.Id);
            //    order.Buyer = order.User.Fullname;
            //    order.Address = order.User.Address;
            //    order.Mobile = order.User.Mobile;
            //    order.IsAdminShow = false;
            //    order.PhoneNumber = order.User.PhoneNumber;
            //    order.PostalCode = order.User.PostalCode;
            //    order.Discount_Amount = 0;
            //    var detail = new FactorItem();
            //    detail.Product = product;
            //    detail.ProductName = product.Name;
            //    detail.Qty = 1;
            //    detail.UnitPrice = product.Price - product.Discount;

            //    order.FactorItems.Add(detail);
            //    db.Factors.Add(order);
            //    db.Configuration.ValidateOnSaveEnabled = false;
            //    db.SaveChanges();
            //}
            //else
            //{
            //    var res = db.FactorItems.Where(p => p.Product.Id == product.Id).Where(p => p.Factor.Id == order.Id).FirstOrDefault();
            //    if (res != null)
            //    {
            //        if (res.Product.Qty - res.Qty <= 0)
            //        {
            //            return Redirect("/Products/Index?Id=" + Request["Id"] + "&" + "message=-1");

            //        }
            //        res.Qty++;
            //        db.SaveChanges();

            //    }
            //    else
            //    {
            //        var detail = new FactorItem();
            //        detail.Product = product;
            //        detail.ProductName = product.Name;
            //        detail.Qty = 1;
            //        detail.UnitPrice = product.Price;
            //        order.FactorItems.Add(detail);
            //        db.SaveChanges();
            //    }
            //}
            //tr.Commit();
            return Json(new { success = true, responseText = "محصول به سبد خرید افزوده شد"  }, JsonRequestBehavior.AllowGet);


        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var cashedProducts = Session["ListProduct"] as List<FactorItem>;
            cashedProducts.Remove(cashedProducts.Where(s => s.Product.Id == id).FirstOrDefault());
            Session["ListProduct"] = cashedProducts;
            if (cashedProducts.Count() == 0)
            {
                return RedirectToAction("../Home/Index");
            }
            else
            {
                return RedirectToAction("Index");
            }
     
        }
        [HttpPost]
        public ActionResult ChangeQty(int Id)
        {
            int fiid =Id;
            var cashedProducts = Session["ListProduct"] as List<FactorItem>;
            List<FactorItem> productitems = new List<FactorItem>();
            string coloritem = string.Empty;
            var Qty = Convert.ToInt32(Request["Qty"]);
            if (Qty > 10 || Qty < 1)
            {
                TempData["Basketerror"] = "تعداد مجاز نیست";

                ViewBag.Order_Details = cashedProducts;
                return RedirectToAction("Index");
            }

            if (cashedProducts.Where(s => s.Product.Id == fiid).FirstOrDefault() == null)
            {
                TempData["Basketerror"] = "تغییر تعداد موفقیت آمیز نبود";
                ViewBag.Order_Details = cashedProducts;
                return RedirectToAction("Index");
            }

            if (db.Products.Where(s => s.Id == fiid).FirstOrDefault().Qty < Qty)
            {
                TempData["Basketerror"] = $"موجودی انبار برای {cashedProducts.Where(s => s.Product.Id == fiid).FirstOrDefault().ProductName} کافی نیست";

                ViewBag.Order_Details = cashedProducts;
                return RedirectToAction("Index");
            }
            var casheditem = cashedProducts.Where(s => s.Product.Id == fiid).FirstOrDefault();
            coloritem = casheditem.Color;
            cashedProducts.Remove(casheditem);
            var product = db.Products.Include("Category").Where(p => p.Id == fiid).Where(p => p.Status == true).FirstOrDefault();
            foreach (var item2 in cashedProducts)
            {
                productitems.Add(item2);
            }
            productitems.Add(new FactorItem()
            {
                Product = product,
                ProductName = product.Name,
                Qty = Qty,
                UnitPrice = product.Price - product.Discount,
                Color = coloritem
            });
            Session["ListProduct"] = productitems;

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize]
        public ActionResult History()
        {
            var email = User.Identity.Name;

            int id = db.Users.Where(p => p.Email == email).FirstOrDefault().Id;

            var data = db.Factors.Where(p => p.Status == true).Where(p => p.User.Id == id).OrderByDescending(p => p.Id);
            var d = new PagedItem<object>(data, "/Factor/History");
            ViewBag.Data = d;
            return View();
        }


        [HttpGet]
        [Authorize]
        public ActionResult HistoryItems()
        {
            var email = User.Identity.Name;
            int id = db.Users.Where(p => p.Email == email).FirstOrDefault().Id;
            var fid = Convert.ToInt32(Request["Factor_Id"]);

            var data = db.FactorItems.Include("Product").Where(p => p.Factor.Id == fid).OrderByDescending(p => p.Id).ToList();
            ViewBag.Order_Details = data;
            return View();
        }

        [HttpPost]
        public ActionResult ChangeColor()
		{
            var fiid = Convert.ToInt32(Request["Id"]);
            var Color = Request["Color"];
            int qtyitem = 0;
            var cashedProducts = Session["ListProduct"] as List<FactorItem>;
            List<FactorItem> productitems = new List<FactorItem>();
            var casheditem = cashedProducts.Where(s => s.Product.Id == fiid).FirstOrDefault();
            if (cashedProducts.Where(s => s.Product.Id == fiid).FirstOrDefault() == null)
            {
                TempData["Basketerror"] = "تغییر رنگ محصول موفقیت آمیز نبود";
                ViewBag.Order_Details = cashedProducts;
                return RedirectToAction("Index");
            }
            qtyitem = casheditem.Qty;
            cashedProducts.Remove(casheditem);
            var product = db.Products.Include("Category").Where(p => p.Id == fiid).Where(p => p.Status == true).FirstOrDefault();
            foreach (var item2 in cashedProducts)
            {
                productitems.Add(item2);
            }
            productitems.Add(new FactorItem()
            {
                Product = product,
                ProductName = product.Name,
                Qty = qtyitem,
                UnitPrice = product.Price - product.Discount,
                Color = Color
            }) ;
            Session["ListProduct"] = productitems;


            return RedirectToAction("Index");


        }


    }
}
