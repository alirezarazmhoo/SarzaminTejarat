using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Areas.Admin.Controllers
{
    public class PaymentsController : Controller
    {
        DBContext db = new DBContext();
        // GET: Admin/Payments
        public ActionResult Index()
        {
            var url = "/admin/Payments/index";
            var data = db.Payments.Include("User").Include("Factor").OrderByDescending(p => p.Id);
            var paged = new PagedItem<Payment>(data, url);
            ViewBag.Data = paged;
            return View();
        }
    }
}