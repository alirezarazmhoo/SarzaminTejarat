using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Areas.Admin.Controllers
{
    public class ChartController : Controller
    {
		private DBContext db = new DBContext();

		// GET: Admin/Chart
		public ActionResult Index()
        {
			int MarketerUserCount = db.MarketerUsers.Count();
			ViewBag.MarketerUserCount = MarketerUserCount;


			return View();
        }
		[HttpPost]
		public ActionResult GetList()
		{

			var listUsers = db.MarketerUsers.Select(s => new { MemberId = s.Id, Name = s.Name, ParentId = s.Parent_Id }).ToList();
			return Json(listUsers,JsonRequestBehavior.AllowGet) ;
		}


	}
}