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
     

     public async Task<ActionResult> TotalFactors(int? Id , string txtdate1, string txtdate2)
        {
          
       
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var marketerFactor =await db.MarketerFactor.Where(s => s.MarketerUser.Id == Id).ToListAsync();
            var MarketerName = db.MarketerUsers.Where(s => s.Id == Id).FirstOrDefault();
            ViewBag.MarketerName = MarketerName.Name + " " + MarketerName.LastName;

            if(txtdate1 !=null && txtdate2 != null && txtdate1 !="" && txtdate2 !="")
            {




                var url = "/Admin/MarketerFactorManager/TotalFactors/" + Id + "";
                long _totalPrice = 0;
                string[] formats = { "yyyy-MMM-dd" };
                DateTime dt;
             if(!DateTime.TryParse(txtdate1,out dt))
                {
                    ViewData["ErrorDate"] = "فرمت تاریخ صحیح نمی باشد";
                    var _url = "/Admin/MarketerFactorManager/TotalFactors/" + Id + "";
                    var _data = db.MarketerFactor.Where(s => s.MarketerUser.Id == Id).AsQueryable();
                    long _totalPrice2 = 0;
                    var _TotalPrice = db.MarketerFactorItem.Where(p => p.MarketerFactor.MarketerUser.Id == Id).ToList();
                    foreach (var item in _TotalPrice)
                    {
                        _totalPrice2 += item.UnitPrice;

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
                    long _totalPrice2 = 0;
                    var _TotalPrice = db.MarketerFactorItem.Where(p => p.MarketerFactor.MarketerUser.Id == Id).ToList();
                    foreach (var item in _TotalPrice)
                    {
                        _totalPrice2 += item.UnitPrice;

                    }
                    ViewData["TotalFactorPrice"] = _totalPrice2;

                    var result = _data.OrderByDescending(p => p.Id);
                    ViewBag.Data = new PagedItem<MarketerFactor>(result, url);

                    return View();
                }
                    DateTime _txtdate1 = DateChanger.ToGeorgianDateTime(txtdate1);
                DateTime _txtdate2 = DateChanger.ToGeorgianDateTime(txtdate2);
                var data = db.MarketerFactor.Where(s => s.MarketerUser.Id == Id && s.Date >= _txtdate1 && s.Date <= _txtdate2).AsQueryable();
                var TotalPrice = db.MarketerFactorItem.Where(p => p.MarketerFactor.MarketerUser.Id ==
                Id && p.MarketerFactor.Date >= _txtdate1 && p.MarketerFactor.Date <= _txtdate2).ToList();
                foreach (var item in TotalPrice)
                {
                    _totalPrice += item.UnitPrice;

                }
               ViewData["TotalFactorPriceSort"] = _totalPrice;
                var res = data.OrderByDescending(p => p.Id);

                ViewBag.Data = new PagedItem<MarketerFactor>(res, url);
                return View();

            }


            if (marketerFactor != null)

            {
                var url = "/Admin/MarketerFactorManager/TotalFactors/"+Id+"";
                var data = db.MarketerFactor.Where(s => s.MarketerUser.Id == Id).AsQueryable();
                long _totalPrice = 0;
                var TotalPrice = db.MarketerFactorItem.Where(p => p.MarketerFactor.MarketerUser.Id == Id).ToList();
                foreach (var item in TotalPrice)
                {
                    _totalPrice += item.UnitPrice;

                }
                ViewData["TotalFactorPrice"] = _totalPrice;

                var res = data.OrderByDescending(p => p.Id);
                ViewBag.Data = new PagedItem<MarketerFactor>(res, url);
                return View();
                //return View(marketerFactor);
            }
            return RedirectToAction("Index", "MarketerUser");


        }


        public ActionResult ShowSubsets(int? Id,string searchstring)
        {
    
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if( searchstring !=null)
            {
                var _url = "/Admin/MarketerFactorManager/ShowSubsets/" + Id + "";

                var _ListSubsets =  db.MarketerUsers.Where(s => s.Parent_Id == Id).Where(p => p.Name.Contains(searchstring)
 || p.LastName.Contains(searchstring) || p.Mobile.Contains(searchstring)
 || p.Phone.Contains(searchstring) || p.Address.Contains(searchstring) ||
 p.IDCardNumber.Contains(searchstring) ).AsQueryable();
                var _res = _ListSubsets.OrderByDescending(p => p.Id);
                ViewBag.Data = new PagedItem<MarketerUser>(_res, _url);


                return View(_ListSubsets);

            }

            var url = "/Admin/MarketerFactorManager/ShowSubsets/" + Id + "";

            var ListSubsets = db.MarketerUsers.Where(s => s.Parent_Id == Id).AsQueryable();
            var res = ListSubsets.OrderByDescending(p => p.Id);
            ViewBag.Data = new PagedItem<MarketerUser>(res, url);

            return  View(ListSubsets);

        }



    }
}