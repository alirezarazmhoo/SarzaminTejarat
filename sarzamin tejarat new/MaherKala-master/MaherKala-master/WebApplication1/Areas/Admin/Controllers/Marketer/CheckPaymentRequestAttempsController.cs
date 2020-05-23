using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Areas.Admin.Controllers.Marketer
{
    public class CheckPaymentRequestAttempsController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Admin/CheckPaymentRequestAttemps
        public async Task<ActionResult> Index()
        {
            var url = "/Admin/CheckPaymentRequestAttemps/Index";
            var checkPaymentRequestAttemps = db.CheckPaymentRequestAttemps.Include(c => c.CheckPaymentConditaion).Include(c => c.MarketerUser).AsQueryable();
            var res = checkPaymentRequestAttemps.OrderByDescending(p => p.Id);
            ViewBag.Data = new PagedItem<CheckPaymentRequestAttemp>(res, url);
            return View(await checkPaymentRequestAttemps.ToListAsync());


        }

    }
}
