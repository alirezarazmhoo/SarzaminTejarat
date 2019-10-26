using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Areas.Admin.Controllers.Marketer
{
    public class RetailerDiscountController : Controller
    {
        DBContext db = new DBContext();

        // GET: Admin/RetailerDiscount
        public async Task<ActionResult> Index()
        {
            var item =await db.RetailerFirstFactorDiscounts.FirstOrDefaultAsync();

            if(item != null)
            {
                ViewBag.CurrentPercentage = item.Percentage;
            }


            return View();
        }



        public async Task<ActionResult> Create(RetailerFirstFactorDiscount retailerFirstFactorDiscount)
        {
            var item = await db.RetailerFirstFactorDiscounts.FirstOrDefaultAsync();
            try
            {


                if (item == null)
                {
                    db.RetailerFirstFactorDiscounts.Add(retailerFirstFactorDiscount);

                }
                else
                {
                    item.Percentage = retailerFirstFactorDiscount.Percentage;

                }
             await  db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
               ViewData["Error"] = "امکان ثبت وجود ندارد";
                return RedirectToAction(nameof(Index));

            }


        }

    }
}