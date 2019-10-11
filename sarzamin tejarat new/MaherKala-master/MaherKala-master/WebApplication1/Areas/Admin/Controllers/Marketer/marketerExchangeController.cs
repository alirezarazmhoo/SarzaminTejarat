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
    public class marketerExchangeController : Controller

    {
        DBContext db = new DBContext();

        // GET: Admin/marketerExchange
        public ActionResult Index()
        {
            return View();
        }


        public async Task<ActionResult> MarketerExchange (int item1 , int item2)
        {
            try
            {
                if(item1 == item2)
                {
                    TempData["Error"] = "افراد انتخابی برابر هستند";
                    return RedirectToAction(nameof(Index));

                }


                var marketerone = await db.MarketerUsers.Where(s => s.Id == item1).FirstOrDefaultAsync();
                var totalsubset = await db.MarketerUsers.Where(s => s.Parent_Id == marketerone.Id).ToListAsync();

                var marketertwo = db.MarketerUsers.Where(s => s.Id == item2).FirstOrDefault();

                foreach (var item in totalsubset)
                {
                    item.Parent_Id = marketertwo.Id;

                }
                await db.SaveChangesAsync();
                TempData["Success"] = "عملیات انتقال زیر مجموعه های " + marketerone.Name + marketerone.LastName + " به " + marketertwo.Name + marketertwo.LastName + " انجام شد";

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["Error"] = "خطایی رخ داده";
            }

            return RedirectToAction(nameof(Index));

        }
    }
}