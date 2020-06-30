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
using WebApplication1.Models.ViewModel;

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
            return View();


        }

        public async Task<ActionResult> Details(int Id)
		{
            CheckPaymentRequestAttempViewModel checkPaymentRequestAttempViewModel = new CheckPaymentRequestAttempViewModel();
            CheckPaymentRequestAttemp checkPaymentRequestAttemp =await db.CheckPaymentRequestAttemps.Where(s => s.Id == Id).FirstOrDefaultAsync();
            List<CheckPaymentRequestAttempPictures> checkPaymentRequestAttempPictures =await db.CheckPaymentRequestAttempPictures.Where(s => s.checkPaymentRequestAttempId == Id).ToListAsync();
            checkPaymentRequestAttempViewModel.CheckPaymentRequestAttempPictures = checkPaymentRequestAttempPictures;
            checkPaymentRequestAttempViewModel.AdminComment = checkPaymentRequestAttemp.AdminComment;
            checkPaymentRequestAttempViewModel.CheckPaymentConditaion =await db.checkPaymentConditaions.Where(s=>s.Id == checkPaymentRequestAttemp.CheckPaymentConditaionId).FirstOrDefaultAsync();
            checkPaymentRequestAttempViewModel.CheckPaymentConditaionId = checkPaymentRequestAttemp.CheckPaymentConditaionId;
            checkPaymentRequestAttempViewModel.CheckPaymentRequestAttempStatus = checkPaymentRequestAttemp.CheckPaymentRequestAttempStatus;
            checkPaymentRequestAttempViewModel.CreatedDate = checkPaymentRequestAttemp.CreatedDate;
            checkPaymentRequestAttempViewModel.Id = checkPaymentRequestAttemp.Id;
            checkPaymentRequestAttempViewModel.InitializePricePaymentConditaion = checkPaymentRequestAttemp.InitializePricePaymentConditaion;
            checkPaymentRequestAttempViewModel.MarketerUser =await db.MarketerUsers.Where(s=>s.Id == checkPaymentRequestAttemp.MarketerUserId).FirstOrDefaultAsync();
            checkPaymentRequestAttempViewModel.MarketerUserId = checkPaymentRequestAttemp.MarketerUserId;
            return View(checkPaymentRequestAttempViewModel);

		}


    }
}
