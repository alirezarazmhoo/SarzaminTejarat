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
    public class PriceForTranslateController : Controller
    {
        DBContext db = new DBContext();

        // GET: Admin/PriceForTranslate
        public ActionResult Index()
        {
            var Item = db.PriceForTranslates.FirstOrDefault();
            if(Item != null)
            {
                ViewBag.CurrentMarketerPrice = Item.MarketerPriceTranslate;
                ViewBag.CurrentMarketerGrtis = Item.Marketergratis;
                ViewBag.CurrentBigBuyerPrice = Item.BigBuyerPriceTranslate;
                ViewBag.CurrentBigBuyerGrtis = Item.Buyergratis;
                ViewBag.CurrentRetailerPrice = Item.RetailerPriceTranslate;
                ViewBag.CurrentRetailerGrtis = Item.Retailergratis;
           
                return View();

            }


            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddTransfore(PriceForTranslate priceForTranslate,int? MarketerDisactive,int? BigBuyerDisactive,int? RetailerDisactive)
        {
       

            var Item =await db.PriceForTranslates.FirstOrDefaultAsync();


            if (MarketerDisactive == 1)
            {
                Item.Marketergratis = 0;
                Item.MarketerPriceTranslate = 0;

            }
            if(BigBuyerDisactive == 2)
            {
                Item.BigBuyerPriceTranslate = 0;
                Item.Buyergratis = 0;

            }

            if(RetailerDisactive == 3)
            {
                Item.RetailerPriceTranslate = 0;
                Item.Retailergratis = 0;

            }



            if (Item != null)
            {

                if (priceForTranslate.BigBuyerPriceTranslate != 0)
                {
                    Item.BigBuyerPriceTranslate = priceForTranslate.BigBuyerPriceTranslate;
                }
                if(priceForTranslate.Buyergratis != 0)
                {
                    Item.Buyergratis = priceForTranslate.Buyergratis;
                }
                if (priceForTranslate.Marketergratis !=0)
                {

                    Item.Marketergratis = priceForTranslate.Marketergratis;
                }
                if(priceForTranslate.MarketerPriceTranslate !=0)
                {

                    Item.MarketerPriceTranslate = priceForTranslate.MarketerPriceTranslate;
                }
                if(priceForTranslate.Retailergratis != 0)
                {

                    Item.Retailergratis = priceForTranslate.Retailergratis;
                }
               
                if(priceForTranslate.RetailerPriceTranslate !=0)
                {

                    Item.RetailerPriceTranslate = priceForTranslate.RetailerPriceTranslate;
                }

                
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
          
                db.PriceForTranslates.Add(priceForTranslate);
              await  db.SaveChangesAsync();
            return RedirectToAction("Index");

        }
             
    }
}