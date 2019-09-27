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

        [Authorize(Roles = "Member")]
        public ActionResult Index()
        {


            var email = User.Identity.Name;
            int id = db.Users.Where(p => p.Email == email).FirstOrDefault().Id;
            var order = db.Factors.Where(p => p.User.Id == id).Where(p => p.Status == false).FirstOrDefault();
            List<FactorItem> items = null;
            if (order != null)
                items = db.FactorItems.Include("Product").Where(p => p.Factor.Id == order.Id).ToList();
            ViewBag.Order = order;
            ViewBag.Order_Details = items;
            return View();
        }
        [HttpGet]
        [Authorize(Roles = "Member")]
        public ActionResult Shipping()
        {
            var email = User.Identity.Name;
            ViewBag.User = db.Users.Where(p => p.Email == email).FirstOrDefault();

            Setting s = db.Settings.FirstOrDefault();
            ViewBag.Esfahan = s.TransportationEsfahan;
            ViewBag.Najafabad = s.TransportationNajafabad;
            ViewBag.Other = s.TransportationOther;



            int id = ViewBag.User.Id;
            var order = db.Factors.Include("FactorItems.Product.Category").Where(p => p.User.Id == id).Where(p => p.Status == false).FirstOrDefault();
            List<FactorItem> items = null;
            if (order != null)
                items = db.FactorItems.Include("Product").Where(p => p.Factor.Id == order.Id).ToList();
            ViewBag.Order = order;
            ViewBag.Order_Details = items;

            if (Request["DiscountCode"] != null)
            {
                var dc = Request["DiscountCode"];
                DiscountCode d = db.DiscountCode.Where(p => p.Code == dc && p.Status == true && p.ExpireDate >= DateTime.Now).FirstOrDefault();
                if (d == null)
                {
                    TempData["Errors"] = "کد تخفیف اشتباه است";

                }
                else if (order.ComputeTotalPrice() + order.Discount_Amount - d.Price < 1000)
                {
                    return View();
                }
                else
                {
                    order.Discount_Code = d.Code;
                    order.Discount_Amount = d.Price;
                    db.SaveChanges();
                }
            }

            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Member")]
        public ActionResult Finalize(string Address, string Fullname, string PhoneNumber, string Mobile, string PostalCode, int City_Id)
        {
            if (City_Id != 1 && City_Id != 2 && City_Id != 3)
            {
                TempData["Errors"] = "شهر را انتخاب کنید";

                return RedirectToAction("Shipping");

            }
            if (PostalCode.Length > 20 || PostalCode.Length < 10)
            {
                TempData["Errors"] = "طول کدپستی باید ده رقم باشد";
                return RedirectToAction("Shipping");
            }
            if (PhoneNumber.Length > 15 || PhoneNumber.Length < 7)
            {
                TempData["Errors"] = "طول ارقام تلفن حداقل هفت رقم باشد باشد";
                return RedirectToAction("Shipping");
            }
            if (Mobile.Length > 15 || Mobile.Length < 11)
            {
                TempData["Errors"] = "طول موبایل حداقل باید یازده رقم باشد";
                return RedirectToAction("Shipping");
            }
            if (Address.Length > 1000 || Address.Length < 5)
            {
                TempData["Errors"] = "آدرس را بطور صحیح وارد کنید";
                return RedirectToAction("Shipping");
            }
            if (Fullname.Length > 50 || Fullname.Length < 3)
            {
                TempData["Errors"] = "طول نام گیرنده مجاز نیست";
                return RedirectToAction("Shipping");
            }
            var email = User.Identity.Name;
            int id = db.Users.Where(p => p.Email == email).FirstOrDefault().Id;
            var order = db.Factors.Include("FactorItems.Product.Category").Where(p => p.User.Id == id).Where(p => p.Status == false).FirstOrDefault();
            if (order == null)
            {
                throw new Exception();
            }
            if (order.FactorItems == null || order.FactorItems.Count == 0)
            {
                throw new Exception();
            }

            Setting s = db.Settings.FirstOrDefault();
            int transportation = 0;
            if (City_Id == 1)
            {
                transportation = (int)s.TransportationEsfahan;
            }
            else if (City_Id == 2)
            {
                transportation = (int)s.TransportationNajafabad;
            }
            else if (City_Id == 3)
            {
                transportation = (int)s.TransportationOther;
            }

            order.City_Id = City_Id;
            order.TransportationFee = transportation;
            order.Date = DateTime.Now;
            order.Address = Address;
            order.Buyer = Fullname;
            order.Mobile = Mobile;
            order.PostalCode = PostalCode;
            try
            {

                db.SaveChanges();
            }

            catch (DbEntityValidationException ex)
            {
                ViewBag.User = db.Users.Where(p => p.Email == email).FirstOrDefault();

                int iid = ViewBag.User.Id;
                var oorder = db.Factors.Where(p => p.User.Id == iid).Where(p => p.Status == false).FirstOrDefault();
                List<FactorItem> items = null;
                if (oorder != null)
                    items = db.FactorItems.Include("Product").Where(p => p.Factor.Id == oorder.Id).ToList();
                ViewBag.Order = oorder;
                ViewBag.Order_Details = items;
                var errorMessages = ex.EntityValidationErrors
                   .SelectMany(x => x.ValidationErrors)
                   .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("\n ", errorMessages);
                TempData["Errors"] = fullErrorMessage;
                return View("Shipping");

            }
            return Redirect("/Payment/Index");

            //var setting = db.Settings.First();
            //SendEmail s = new SendEmail(setting);
            //var list = new List<string>();
            //list.Add(email);
            //var price = String.Format("{0:0,0}", order.TotalPrice + 15000);
            //var message = "مبلغ "+price+ " تومان با موفقیت پرداخت شد. "+ "محصول بزودی برای شما ارسال میگردد";
            //s.Send("<h1>فاکتور فروشگاه کتاب دانش</h1><div style='text-align:right'>"+message+"</div>","فاکتور پرداخت",list);
        }



        [HttpPost]
        [Route("Factor/Store")]
        public ActionResult Store()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Redirect("/User/Login?ReturnUrl=/Products/Index?Id=" + Request["Id"]);
            }
            int pid = Convert.ToInt32(Request["Id"]);
            var product = db.Products.Include("Category").Where(p => p.Id == pid).Where(p => p.Status == true).FirstOrDefault();

            var tr = db.Database.BeginTransaction();
            if (product.Qty == 0)
            {
                return Redirect("/Products/Index?Id=" + Request["Id"] + "&" + "message=-1");

            }
            var email = User.Identity.Name;
            var id = db.Users.Where(p => p.Email == email).FirstOrDefault().Id;
            var order = db.Factors.Where(p => p.Status == false).Where(p => p.User.Id == id).FirstOrDefault();
            if (order == null)
            {
                order = new Factor();
                order.Status = false;
                order.Date = DateTime.Now;
                order.TotalPrice = 0;
                order.User = db.Users.Find(id);
                order.Buyer = order.User.Fullname;
                order.Address = order.User.Address;
                order.Mobile = order.User.Mobile;
                order.IsAdminShow = false;
                order.PhoneNumber = order.User.PhoneNumber;
                order.PostalCode = order.User.PostalCode;
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
                        return Redirect("/Products/Index?Id=" + Request["Id"] + "&" + "message=-1");

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
                    detail.UnitPrice = product.Price;
                    order.FactorItems.Add(detail);
                    db.SaveChanges();
                }
            }
            tr.Commit();
            return RedirectToAction("Index");

        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var items = db.FactorItems.Include("Product.Category").Where(p => p.Id == id).FirstOrDefault(); ;
            db.FactorItems.Remove(items);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize]
        public ActionResult ChangeQty()
        {
            var fiid = Convert.ToInt32(Request["Id"]);
            var email = User.Identity.Name;
            int id = db.Users.Where(p => p.Email == email).FirstOrDefault().Id;

            var order = db.Factors.Where(p => p.User.Id == id).Where(p => p.Status == false).FirstOrDefault();

            var Qty = Convert.ToInt32(Request["Qty"]);
            if (Qty > 10 || Qty < 1)
            {
                ModelState.AddModelError("", "تعداد مجاز نیست");
                List<FactorItem> items = null;
                if (order != null)
                    items = db.FactorItems.Include("Product").Where(p => p.Factor.Id == order.Id).ToList();
                ViewBag.Order = order;
                ViewBag.Order_Details = items;
                return View("Index");
            }



            if (order == null)
            {

                ModelState.AddModelError("", "ناموفق");
                List<FactorItem> items = null;
                if (order != null)
                    items = db.FactorItems.Include("Product").Where(p => p.Factor.Id == order.Id).ToList();
                ViewBag.Order = order;
                ViewBag.Order_Details = items;
                return View("Index");
            }

            var data = db.FactorItems.Include("Product.Category").Where(p => p.Factor.Id == order.Id).Where(p => p.Id == fiid).FirstOrDefault();
            if (data == null)
            {
                ModelState.AddModelError("", "ناموفق");
                List<FactorItem> items = null;
                if (order != null)
                    items = db.FactorItems.Include("Product").Where(p => p.Factor.Id == order.Id).ToList();
                ViewBag.Order = order;
                ViewBag.Order_Details = items;
                return View("Index");
            }

            if (data.Product.Qty < Qty)
            {
                ModelState.AddModelError("", "موجودی کافی نیست");
                List<FactorItem> items = null;
                if (order != null)
                    items = db.FactorItems.Include("Product").Where(p => p.Factor.Id == order.Id).ToList();
                ViewBag.Order = order;
                ViewBag.Order_Details = items;
                return View("Index");
            }

            data.Qty = Qty;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Member")]
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
        [Authorize(Roles = "Member")]
        public ActionResult HistoryItems()
        {
            var email = User.Identity.Name;
            int id = db.Users.Where(p => p.Email == email).FirstOrDefault().Id;
            var fid = Convert.ToInt32(Request["Factor_Id"]);

            var data = db.FactorItems.Include("Product").Where(p => p.Factor.Id == fid).OrderByDescending(p => p.Id).ToList();
            ViewBag.Order_Details = data;
            return View();
        }
    }
}
