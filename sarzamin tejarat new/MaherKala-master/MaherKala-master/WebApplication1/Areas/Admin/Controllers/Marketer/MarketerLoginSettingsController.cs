using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Areas.Admin.Controllers.Marketer
{
    public class MarketerLoginSettingsController : Controller
    {
        DBContext DB = new DBContext();
        // GET: Admin/MarketerLoginSettings
        public ActionResult Index()
        {
            var item = DB.MarketerActiveAccountTickets.FirstOrDefault();
            var _Item = DB.MarketerLimitSale.FirstOrDefault();
            if (item == null)
            {
                ViewData["CurrentValue"] = "تیکتی تعریف نشده";
            }
            else
            {
                ViewData["CurrentValue"] = item.Price;
            }
            if(_Item == null)
            {
                ViewData["CurrentLimitSaleValue"] = "مقداری ثبت نشده";
            }
            else
            {
                ViewData["CurrentLimitSaleValue"] = _Item.Price;
                ViewData["CurrentDays"] = _Item.Days;

            }
            if (_Item != null)
            {


                if (_Item.Enable)
                {
                    ViewData["CurrentStatus"] = "فعال";

                }
                else
                {
                    ViewData["CurrentStatus"] = "غیرفعال";
           
                }
            }
            if(_Item == null)
            {
                ViewData["CurrentStatus"] = "نامشخص";
            }
            return View();
        }
        public async Task<ActionResult> add(MarketerActiveAccountTicket marketerActiveAccountTicket)
        {
			int n;
			if (!int.TryParse(Request["Price"], out n))
			{
				TempData["PriceError"] = "ورودی قیمت ها صحیح نیست ، لطفا فقط عدد واردکنید";
				return RedirectToAction(nameof(Index));
			}
			var item = DB.MarketerActiveAccountTickets.FirstOrDefault();
            if (ModelState.IsValid)
            {
		
				if (item == null)
                {
                    DB.MarketerActiveAccountTickets.Add(marketerActiveAccountTicket);
                }
                else
                {
         
                    item.Price = marketerActiveAccountTicket.Price;
                }
                await DB.SaveChangesAsync();

            }


            return RedirectToAction("Index");

        }

        public async Task<ActionResult> addLimitSale(MarketerLimitSale marketerLimitSale)
        {
			int n;
			if (!int.TryParse(Request["Price"], out n))
			{
				TempData["PriceError"] = "ورودی قیمت ها صحیح نیست ، لطفا فقط عدد واردکنید";
				return RedirectToAction(nameof(Index));
			}

			var item = DB.MarketerLimitSale.FirstOrDefault();
          
                if (item == null)
                {

                    DB.MarketerLimitSale.Add(marketerLimitSale);
                }
                else
                {	

                    item.Price = marketerLimitSale.Price;
                    item.Days = marketerLimitSale.Days;
			
			
				item.Enable = marketerLimitSale.Enable;
				
                    item.ActiveDate = DateTime.Now;
                }
                await DB.SaveChangesAsync();

            


            return RedirectToAction("Index");

        }


    }
}