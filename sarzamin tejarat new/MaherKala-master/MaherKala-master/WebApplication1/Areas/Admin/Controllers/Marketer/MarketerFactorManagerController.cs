using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Areas.Admin.Utility;
using WebApplication1.Models;

namespace WebApplication1.Areas.Admin.Controllers.Marketer
{
    public class MarketerFactorManagerController : Controller
    {
        DBContext db = new DBContext();
     

     public async Task<ActionResult> TotalFactors(int? Id , string txtdate1, string txtdate2,int? IsAcceptByAdmin)
        { 
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var marketerFactor =await db.MarketerFactor.Where(s => s.MarketerUser.Id == Id).ToListAsync();
            var MarketerName = db.MarketerUsers.Where(s => s.Id == Id).FirstOrDefault();
            ViewBag.MarketerName = MarketerName.Name + " " + MarketerName.LastName;
			ViewBag.userType = MarketerName.Usertype;
            if(txtdate1 !=null && txtdate2 != null && txtdate1 !="" && txtdate2 !="")
            {
                var url = "/Admin/MarketerFactorManager/TotalFactors/" + Id + "";
                double _totalPrice = 0;
                string[] formats = { "yyyy-MMM-dd" };
                DateTime dt;
             if(!DateTime.TryParse(txtdate1,out dt))
                {
                    ViewData["ErrorDate"] = "فرمت تاریخ صحیح نمی باشد";
                    var _url = "/Admin/MarketerFactorManager/TotalFactors/" + Id + "";
                    var _data = db.MarketerFactor.Where(s => s.MarketerUser.Id == Id).AsQueryable();              
					double _totalPrice2 = 0;
                    var _TotalPrice = db.MarketerFactorItem.Where(p => p.MarketerFactor.MarketerUser.Id == Id).ToList();
                    foreach (var item in _TotalPrice)
                    {
                        _totalPrice2 += item.UnitPrice * item.Qty;
                    }
                    ViewData["TotalFactorPrice"] = _totalPrice2;
                    var result = _data.OrderByDescending(p => p.Id);
                    ViewBag.Data = new PagedItem<MarketerFactor>(result, url);
                    return View();
                }
             if(!DateTime.TryParse(txtdate2,out dt))
                {

                    ViewData["ErrorDate"] = "فرمت تاریخ صحیح نمی باشد";

                    var _url = "/Admin/MarketerFactorManager/TotalFactors/" + Id + "";
                    var _data = db.MarketerFactor.Where(s => s.MarketerUser.Id == Id).AsQueryable();
                    double _totalPrice2 = 0;
                    var _TotalPrice = db.MarketerFactorItem.Where(p => p.MarketerFactor.MarketerUser.Id == Id).ToList();
                    foreach (var item in _TotalPrice)
                    {
                        _totalPrice2 += item.UnitPrice * item.Qty;

                    }
                    ViewData["TotalFactorPrice"] = _totalPrice2;

                    var result = _data.OrderByDescending(p => p.Id);
                    ViewBag.Data = new PagedItem<MarketerFactor>(result, url);

                    return View();
                }
                    DateTime _txtdate1 = DateChanger.ToGeorgianDateTime(txtdate1);
                DateTime _txtdate2 = DateChanger.ToGeorgianDateTime(txtdate2.AddDaysToShamsiDate(1));
			
				if(IsAcceptByAdmin == 1 && IsAcceptByAdmin !=null)
				{
					var data3 = db.MarketerFactor.Where(s => s.MarketerUser.Id == Id && s.Date >= _txtdate1 && s.Date <= _txtdate2 && s.IsAdminCheck).AsQueryable();
					foreach (var item in db.MarketerFactorItem.Where(p => p.MarketerFactor.MarketerUser.Id == Id && p.MarketerFactor.Date >= _txtdate1 && p.MarketerFactor.Date <= _txtdate2 && p.MarketerFactor.IsAdminCheck).ToList())
					{
						_totalPrice += item.UnitPrice * item.Qty;

					}
					ViewData["TotalFactorPriceSort"] = _totalPrice;
					var res3 = data3.OrderByDescending(p => p.Id);

					ViewBag.Data = new PagedItem<MarketerFactor>(res3, url);
					return View();
				}
                var data = db.MarketerFactor.Where(s => s.MarketerUser.Id == Id && s.Date >= _txtdate1 && s.Date <= _txtdate2).AsQueryable();  
                foreach (var item in db.MarketerFactorItem.Where(p => p.MarketerFactor.MarketerUser.Id == Id && p.MarketerFactor.Date >= _txtdate1 && p.MarketerFactor.Date <= _txtdate2).ToList())
                {
                    _totalPrice += item.UnitPrice * item.Qty;
                }
               ViewData["TotalFactorPriceSort"] = _totalPrice;
                var res = data.OrderByDescending(p => p.Id);
                ViewBag.Data = new PagedItem<MarketerFactor>(res, url);
                return View();
            }
            if (marketerFactor != null )
            {
				double _totalPrice = 0;
				var url1 = "/Admin/MarketerFactorManager/TotalFactors/" + Id + "";
				if (IsAcceptByAdmin == 1 && IsAcceptByAdmin !=null)
				{
					if (string.IsNullOrEmpty(txtdate1) || string.IsNullOrEmpty(txtdate2))
					{
						var data4 = db.MarketerFactor.Where(s => s.MarketerUser.Id == Id && s.IsAdminCheck).AsQueryable();					
						foreach (var item in db.MarketerFactorItem.Where(p => p.MarketerFactor.MarketerUser.Id == Id && p.MarketerFactor.IsAdminCheck).ToList())
						{
							_totalPrice += item.UnitPrice * item.Qty;
						}
						ViewData["TotalFactorPriceSort"] = _totalPrice;
						var res4 = data4.OrderByDescending(p => p.Id);
						ViewBag.Data = new PagedItem<MarketerFactor>(res4, url1);
						return View();
					}
				}
				var url = "/Admin/MarketerFactorManager/TotalFactors/"+Id+"";
                var data = db.MarketerFactor.Where(s => s.MarketerUser.Id == Id).AsQueryable();       
                foreach (var item in db.MarketerFactorItem.Where(p => p.MarketerFactor.MarketerUser.Id == Id).ToList())
                {
                    _totalPrice += item.UnitPrice * item.Qty;
                }
                ViewData["TotalFactorPrice"] = _totalPrice;
                var res = data.OrderByDescending(p => p.Id);
                ViewBag.Data = new PagedItem<MarketerFactor>(res, url);
                return View();
                //return View(marketerFactor);
            }
            return RedirectToAction("Index", "MarketerUser");
        }
        public ActionResult ShowSubsets(int? Id,string searchstring, string txtdate1, string txtdate2,int? IsAdminCheck)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if(!string.IsNullOrEmpty(searchstring))
            {
                var _url = "/Admin/MarketerFactorManager/ShowSubsets/" + Id + "";

                var _ListSubsets =  db.MarketerUsers.Where(s => s.Parent_Id == Id).
				Where(p => p.Name.Contains(searchstring)
                || p.LastName.Contains(searchstring) 
				|| p.Mobile.Contains(searchstring)
                || p.Phone.Contains(searchstring) 
				|| p.Address.Contains(searchstring)
				||
                p.IDCardNumber.
				Contains(searchstring) )
				.AsQueryable();
                var _res = _ListSubsets.OrderByDescending(p => p.Id);
                ViewBag.Data = new PagedItem<MarketerUser>(_res, _url);
                return View(_ListSubsets);
            }
            if (txtdate1 != null && txtdate2 != null && txtdate1 != "" && txtdate2 != "")
            {
                string[] formats = { "yyyy-MMM-dd" };
                DateTime dt;
                if ((DateTime.TryParse(txtdate1, out dt) && DateTime.TryParse(txtdate2, out dt)) != true )
                {
                    ViewData["ErrorDate"] = "فرمت تاریخ صحیح نمی باشد";
                    
                    double _TotalSumFactor = 0;
                    foreach (var item in db.MarketerFactorItem.Where(p => p.MarketerFactor.MarketerUser.Parent_Id == Id).ToList())
                    {
                        _TotalSumFactor += item.UnitPrice * item.Qty;

                    }
                    ViewData["TotalSumFactor"] = _TotalSumFactor;

                    var _url = "/Admin/MarketerFactorManager/ShowSubsets/" + Id + "";

                    var _ListSubsets = db.MarketerUsers.Where(s => s.Parent_Id == Id).AsQueryable();
                    var _res = _ListSubsets.OrderByDescending(p => p.Id);
                    ViewBag.Data = new PagedItem<MarketerUser>(_res, _url);

                    return View(_ListSubsets);
                }
                DateTime dateTime1 =DateChanger.ToGeorgianDateTime(txtdate1);
                DateTime dateTime2 = DateChanger.ToGeorgianDateTime(txtdate2.AddDaysToShamsiDate(1));
              
                double _Totalsumfactors = 0;

				if(IsAdminCheck == 1 && IsAdminCheck != null)
				{
					foreach (var item in db.MarketerFactorItem.Where(p => p.MarketerFactor.MarketerUser.Parent_Id == Id).Where(p => p.MarketerFactor.Date >= dateTime1 && p.MarketerFactor.Date <= dateTime2 && p.MarketerFactor.IsAdminCheck).ToList())
					{
						_Totalsumfactors += item.UnitPrice * item.Qty;

					}
					ViewData["TotalSumFactor"] = _Totalsumfactors;
					var theurl1 = "/Admin/MarketerFactorManager/ShowSubsets/" + Id + "";
					var theListSubsets1 = db.MarketerUsers.Where(s => s.Parent_Id == Id).AsQueryable();
					var theres1 = theListSubsets1.OrderByDescending(p => p.Id);
					ViewBag.Data = new PagedItem<MarketerUser>(theres1, theurl1);
					return View(theListSubsets1);
				}
                foreach (var item in db.MarketerFactorItem.Where(p => p.MarketerFactor.MarketerUser.Parent_Id == Id).Where(p => p.MarketerFactor.Date >= dateTime1 && p.MarketerFactor.Date <= dateTime2).ToList())
                {
                    _Totalsumfactors += item.UnitPrice * item.Qty;
                }
                ViewData["TotalSumFactor"] = _Totalsumfactors;
                var theurl = "/Admin/MarketerFactorManager/ShowSubsets/" + Id + "";
                var theListSubsets = db.MarketerUsers.Where(s => s.Parent_Id == Id).AsQueryable();
                var theres = theListSubsets.OrderByDescending(p => p.Id);
                ViewBag.Data = new PagedItem<MarketerUser>(theres, theurl);
                return View(theListSubsets);
            }

			if (IsAdminCheck == 1 && IsAdminCheck != null)
			{
				
				double TotalSumFactor1 = 0;
				foreach (var item in db.MarketerFactorItem.Where(p => p.MarketerFactor.MarketerUser.Parent_Id == Id).Where(s=>s.MarketerFactor.IsAdminCheck).ToList())
				{
					TotalSumFactor1 += item.UnitPrice * item.Qty;
				}
				ViewData["TotalSumFactor"] = TotalSumFactor1;

				var url1 = "/Admin/MarketerFactorManager/ShowSubsets/" + Id + "";

				var ListSubsets1 = db.MarketerUsers.Where(s => s.Parent_Id == Id).AsQueryable();
				var res1 = ListSubsets1.OrderByDescending(p => p.Id);
				ViewBag.Data = new PagedItem<MarketerUser>(res1, url1);

				return View(ListSubsets1);
			}
				var TotalFactors = db.MarketerFactorItem.Where(p => p.MarketerFactor.MarketerUser.Parent_Id == Id).ToList();
            double TotalSumFactor = 0;
            foreach (var item in TotalFactors)
            {
                TotalSumFactor += item.UnitPrice * item.Qty;
            }
            ViewData["TotalSumFactor"] = TotalSumFactor;

            var url = "/Admin/MarketerFactorManager/ShowSubsets/" + Id + "";

            var ListSubsets = db.MarketerUsers.Where(s => s.Parent_Id == Id).AsQueryable();
            var res = ListSubsets.OrderByDescending(p => p.Id);
            ViewBag.Data = new PagedItem<MarketerUser>(res, url);

            return  View(ListSubsets);

        }



    }
}