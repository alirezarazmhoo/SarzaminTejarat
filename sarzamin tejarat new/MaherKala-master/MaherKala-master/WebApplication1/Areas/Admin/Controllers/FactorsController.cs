using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Areas.Admin.Controllers
{
    public class FactorsController : Controller
    {
        DBContext db = new DBContext();

        // GET: Admin/Factors
        public ActionResult Index()
        {
            var link = "/Admin/MarketerFactors/Index";
            var buyer = Request["Buyer"];
            var flag = Request["IsAdminShow"];
            var data = db.Factors.Where(p => p.Status == true);
            if (buyer != null)
            {
                data = data.Where(p => p.Buyer.Contains(buyer));
                link += "?Buyer="+buyer;
            }
            if(flag!=null)
            {
                if(flag=="1")
                {
                    data = data.Where(p => p.IsAdminShow == true);
                    if (link.Contains("?"))
                        link = link+ "&IsAdminShow=" + 1;
                    else
                        link = link+ "?IsAdminShow=" + 1;
                }
                else if (flag == "2")
                {
                    data = data.Where(p => p.IsAdminShow == false);
                    if (link.Contains("?"))
                        link = link + "&IsAdminShow=" + 2;
                    else
                        link = link + "?IsAdminShow=" + 2;
                }
            }
            var order = data.OrderByDescending(p => p.Id).OrderBy(p => p.IsAdminShow);
            var paged = new PagedItem<object>(order, link);
            ViewBag.Data = paged;
            return View();
        }
        public ActionResult Detail(int id)
        {
            var data = db.Factors.Include("FactorItems.Product.Category").Include("User").Where(p => p.Id == id).FirstOrDefault();
            data.IsAdminShow = true;
            db.SaveChanges();
            ViewBag.Data = data;

            var d = db.Payments.Where(p => p.Factor.Id == id).OrderByDescending(p => p.Id);
            var paged = new PagedItem<Payment>(d, "/admin/Factors/Detail/" + id, 20);
            ViewBag.Payments = paged;
            return View();
        }
    }
}