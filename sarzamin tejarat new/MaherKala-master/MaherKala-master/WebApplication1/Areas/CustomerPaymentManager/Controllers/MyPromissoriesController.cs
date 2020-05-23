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
    public class MyPromissoriesController : Controller
    {
        private DBContext db = new DBContext();
        // GET: CustomerPaymentManager/MyPromissories
        public async Task<ActionResult> Index()
        {
            var promissory = db.Promissory.Include(p => p.MarketerUser);
            return View(await promissory.ToListAsync());
        }
    }
}
