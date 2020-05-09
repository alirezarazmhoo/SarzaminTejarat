using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Areas.Admin.Controllers.Marketer
{

    public class MarketerFactorsController : Controller
    {
        DBContext db = new DBContext();
        Utility.Utility utility = new Utility.Utility();

        // GET: Admin/Marketer
        public ActionResult Index()
        {
            var link = "/Admin/MarketerFactors/Index";
            var buyer = Request["Buyer"];
            var flag = Request["IsAdminShow"];
            var marketer = Request["Marketer"];
            var data = db.MarketerFactor.Include("MarketerUser").Where(p => p.Status == 0 || p.Status == 2);
            if (buyer != null)
            {
                data = data.Where(p => p.Buyer.Contains(buyer));
                link += "?Buyer=" + buyer;
            }
            if (marketer != null)
            {
                data = data.Where(p => p.MarketerUser.Name.Contains(marketer) || p.MarketerUser.LastName.Contains(marketer));
                link += "?Marketer=" + marketer;
            }
            if (flag != null)
            {
                if (flag == "1")
                {
                    data = data.Where(p => p.IsAdminShow == true);
                    if (link.Contains("?"))
                        link = link + "&IsAdminShow=" + 1;
                    else
                        link = link + "?IsAdminShow=" + 1;
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
            var order = data.OrderBy(p => p.IsAdminShow).OrderByDescending(p => p.Id);
            var paged = new PagedItem<object>(order, link);
            ViewBag.Data = paged;
            return View();
        }

        public ActionResult Confirm(int id)
        {
            var data = db.MarketerFactor.Include("MarketerUser").Include("MarketerFactorItems.Product.Category").Where(p=>p.Id==id).Where(p => p.Status == 0 || p.Status == 2).FirstOrDefault();
            var user = data.MarketerUser;
            double sum = 0;
            if (user.IsFirstTime == true)
            {
                Commission c = new Commission();
                c.Detail = "سه درصد پورسانت ثبت اولین فاکتور";
                int p =(int) data.TotalPrice * 3 / 100;
                c.Date = DateTime.Now;
                c.Formula = data.TotalPrice+" * (0.03)";
                c.Money = p;
                c.MarketerFactor = data;
                c.MarketerUser = user;
                db.Commission.Add(c);
                user.IsFirstTime = false;
            }
            int total = 0;
            foreach (var item in data.MarketerFactorItems)
            {
                int pid = item.Product.Id;
                var qty = item.Qty;
                var productCommision = db.ProductPresent.Where(p => p.Product.Id == pid)
                    .Where(p => p.Min <= qty && p.Max >= qty).FirstOrDefault();
                if(productCommision == null)
                {
                    TempData["Message"] = "کمیسیون پیدا نشد";
                    return Redirect(Request.UrlReferrer.ToString());
                }
                var percent = productCommision.Percent;
                 sum = item.Qty * item.UnitPrice;
                var res = (int)(sum * percent / 100);
                total += res;
            }
            var commision = new Commission();
            commision.Date = DateTime.Now;
            commision.Detail = "کمیسیون فاکتور شماره " + data.Id + " بمیزان یک درصد";
            commision.Formula = "";
            commision.Money = total;
            commision.MarketerUser = data.MarketerUser;
            commision.MarketerFactor = data;
            db.Commission.Add(commision);
            data.IsAdminCheck = true;
			if(user.Usertype ==0)
			{

            #region AddMarketerPoint
            utility.AddMarketerPoint(user.Id, sum);
            #endregion
            #region CheckTheMarketerPlan
   //       var userplanId=  utility.CheckMarketerPlan(user.Id);
   //         if(userplanId !=0)
   //         {
   //         user.PlannnID = userplanId;
			//	//CheckPlanRegisterDate
			//	var PlanRegisterDateItem = db.PlanDateregister.Where(s => s.IDCardNumber == user.IDCardNumber).FirstOrDefault();
			//	if (PlanRegisterDateItem != null)
			//	{
			//		//var _userItem = db.MarketerUsers.Where(s => s.Id == user.Id).FirstOrDefault();
			//				PlanRegisterDateItem.RegisterDate = DateTime.Now;
			//	}	//End
			//}
			}
			#endregion
			db.SaveChanges();
            return Redirect(Request.UrlReferrer.ToString());
        }
        public ActionResult Detail(int id)
        {
            var data = db.MarketerFactor.Include("MarketerFactorItems.Product.Category").Include("MarketerUser").Where(p => p.Id == id).FirstOrDefault();
            var commision = db.Commission.FirstOrDefault(p => p.MarketerFactor.Id == data.Id);
            data.IsAdminShow = true;
            db.SaveChanges();
            ViewBag.Data = data;
            ViewBag.Commision = commision;
            return View();
        }
    }
}