using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Areas.Admin.Controllers.Marketer
{
    public class CommisionController : Controller
    {
        DBContext db = new DBContext();
        // GET: Admin/Commision
        public ActionResult Index()
        {
            var link = "/Admin/Commision/Index";
            var buyer = Request["Buyer"];
            var marketer = Request["Marketer"];
            var data = db.Commission.Include("MarketerUser").Include("MarketerFactor").AsQueryable();
            if (buyer != null)
            {
                data = data.Where(p => p.MarketerFactor.Buyer.Contains(buyer));
                link += "?Buyer=" + buyer;
            }
            if (marketer != null)
            {
                data = data.Where(p => p.MarketerUser.Name.Contains(marketer) || p.MarketerUser.LastName.Contains(marketer));
                link += "?Marketer=" + marketer;
            }
            
            var commision = data.OrderByDescending(p => p.Id);
            var paged = new PagedItem<object>(commision, link);
            ViewBag.Data = paged;
            return View();
        }
    }
}