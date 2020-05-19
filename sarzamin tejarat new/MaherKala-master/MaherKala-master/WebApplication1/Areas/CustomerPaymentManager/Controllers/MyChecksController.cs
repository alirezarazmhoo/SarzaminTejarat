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
using WebApplication1.Areas.CustomerPaymentManager.Authorize;

namespace WebApplication1.Areas.CustomerPaymentManager.Controllers
{
    [PaymentAuthorize]
    public class MyChecksController : Controller
    {
        private DBContext db = new DBContext();

        // GET: CustomerPaymentManager/MyChecks
        public async Task<ActionResult> Index()
        {
           var UserId = Session["UserInfo"];

            var checks = db.Checks.Where(s=>s.MarketerUserId ==(Int32) UserId).Include(c => c.Bank).Include(c => c.MarketerUser);
            return View(await checks.ToListAsync());
        }


    }
}
