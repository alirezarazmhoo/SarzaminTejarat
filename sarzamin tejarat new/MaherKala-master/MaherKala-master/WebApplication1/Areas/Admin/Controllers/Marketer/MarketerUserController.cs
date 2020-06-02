using SmsIrRestful;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.Models.Errors;
using WebApplication1.Utility;

namespace WebApplication1.Areas.Admin.Controllers.Marketer
{
    public class MarketerUserController : Controller
    {
        // GET: Admin/MarketerUser
        DBContext db = new DBContext();
		SendSms _sendSms = new SendSms();

		// GET: Admin/User
		public ActionResult Index()
        {
            var url = "/Admin/MarketerUser/Index?";
            string name = Request["LastName"];
            string mobile = Request["Mobile"];
            var query = db.MarketerUsers.Where(p=>p.AcceptedByParent  && p.Usertype ==0).AsQueryable();
            if (Request["LastName"] != null && Request["LastName"] != "")
            {
                query = query.Where(p => p.LastName.Contains(name) && p.AcceptedByParent);
                if (url.Contains("?"))
                    url = url + "&LastName=" + name;
                else
                    url = url + "?LastName=" + name;
            }
            if (Request["Mobile"] != null && Request["Mobile"] != "")
            {
                query = query.Where(p => p.Mobile.Contains(mobile) && p.AcceptedByParent);
                if (url.Contains("?"))
                    url = url + "&Mobile=" + mobile;
                else
                    url = url + "?Mobile=" + mobile;
            }
            var ordered = query.OrderByDescending(p => p.Id);
            var paginated = new PagedItem<MarketerUser>(ordered, url);
            ViewBag.Data = paginated;
            return View();
        }
        [HttpGet]
        public ActionResult Deactive(int id)
        {
            var user = db.MarketerUsers.Find(id);
            user.IsAvailable = false;
            db.Configuration.ValidateOnSaveEnabled = false;
            long _Mobile = Int64.Parse(user.Mobile);
            db.SaveChanges();
            string _Token = _sendSms.GetToken(_sendSms.userApiKey, _sendSms.secretKey);
            var ultraFastSend = new UltraFastSend()
            {
                Mobile = _Mobile,
                TemplateId = 27131,
                ParameterArray = new List<UltraFastParameters>()
                  {
                   new UltraFastParameters()
                    {
                   Parameter = "MarketerName" , ParameterValue = String.Format("{0},{1}",user.Name,user.LastName)
                     }
                     }.ToArray()
            };
            UltraFastSendRespone ultraFastSendRespone = new UltraFast().Send(_Token, ultraFastSend);
            if (ultraFastSendRespone.IsSuccessful)
            {
                //SuccessFull
                TempData["SmsResultStatusSuccess"] = SucccessText.SmsSent;
            }
            else
            {
                TempData["SmsResultStatusFalse"] = ErrorsText.SmsNotSendt;
                //Faild
            }
            return RedirectToAction("Index");      
        }
        public ActionResult Active(int id)
        {
			string MessageText = string.Empty;
			var user = db.MarketerUsers.Find(id);
            long _Mobile =Int64.Parse( user.Mobile);
            user.IsAvailable = true;
            if(user.SubsetCount == 0 && user.IsFirstTime && user.Usertype.Equals(0))
            {
             user.SubsetCount = 3;
            }
            db.Configuration.ValidateOnSaveEnabled = false;
            if (user != null && user.Mobile != null && user.Password != null && user.AlreadySmsSent == false)
            {
                string _Token = _sendSms.GetToken(_sendSms.userApiKey, _sendSms.secretKey);

                var ultraFastSend = new UltraFastSend()
                {
                    Mobile = _Mobile,
                    TemplateId = 27112,
                    ParameterArray = new List<UltraFastParameters>()
                  {
               new UltraFastParameters()
                {
                 Parameter = "MobileNumber" , ParameterValue = user.Mobile
                 },
                 new UltraFastParameters()
                {
                 Parameter = "Password" , ParameterValue = user.Password
                 }
                   }.ToArray()
                };
                UltraFastSendRespone ultraFastSendRespone = new UltraFast().Send(_Token, ultraFastSend);

                if (ultraFastSendRespone.IsSuccessful)
                {
                    //SuccessFull
                    TempData["SmsResultStatusSuccess"] = SucccessText.SmsSent;
                    user.AlreadySmsSent = true;
                }
                else
                {
                    TempData["SmsResultStatusFalse"] = ErrorsText.SmsNotSendt;
                    //Faild
                }
            }
            db.SaveChanges();

			return RedirectToAction("Index");
        }
        public ActionResult Details(int id)
        {
            var user = db.MarketerUsers.Find(id);
         var useritem =   db.MarketerUsers.Where(p=>p.Id==id).FirstOrDefault();
            var planitem = db.Plannns.Where(s => s.Id == useritem.PlannnID).FirstOrDefault();
      if(planitem.PlanTypeID == 1)
            {

            ViewBag.plantype ="نقره ای";
                ViewBag.level = planitem.Level;
            }
            if (planitem.PlanTypeID == 2)
            {

                ViewBag.plantype = " طلایی";
                ViewBag.level = planitem.Level;

            }
            //t.Plannn.PlanType.Name;
            ViewBag.Data = user;
            return View();
        }
         public ActionResult Chat(int id)
        {
			var chats = db.Converstions.Where(s => s.MarketerUserID == id).ToList();
			foreach (var item in chats)
			{
				if(item.State==2 && item.IsRead == false)
				{
				item.IsRead = true;
				}

			}

			db.SaveChanges();
			ViewBag.Data = chats;
			ViewBag.userId = id;
			
			ViewBag.UserName = db.MarketerUsers.Where(s => s.Id == id).Select(s => new { s.LastName }).FirstOrDefault();
			return View();
        }
        [HttpPost]
        public ActionResult AddChat()
        {
			Converstions converstions = new Converstions();
			converstions.CreateDate = DateTime.Now;
			converstions.IsRead = false;
			converstions.LastMessage = Request["message"];
			converstions.MarketerUserID = Convert.ToInt32(Request["userId"]);
			if(String.IsNullOrEmpty(converstions.LastMessage))
			{
				TempData["ErrorEmptyMessage"] = "متن پیام خالی است";
				return RedirectToAction("Chat", "MarketerUser", new { area = "Admin", @id = converstions.MarketerUserID });
			}
			converstions.State = 1;
			db.Converstions.Add(converstions);
			db.SaveChanges();
			return RedirectToAction("Chat","MarketerUser",new {area="Admin", @id=converstions.MarketerUserID});
        }
		[HttpPost]
		public ActionResult RemoveChat(int id)
		{
			if(id != 0)
			{
				var item = db.Converstions.Find(id);
				var UserId = db.Converstions.Where(s => s.Id == id).FirstOrDefault();
				db.Converstions.Remove(item);
				db.SaveChanges();
			return RedirectToAction("Chat", "MarketerUser", new { area = "Admin", @id = UserId.MarketerUserID });
			}
			return RedirectToAction("Chat", "MarketerUser", new { area = "Admin", @id = 48 });

		}


		public ActionResult LoadBigBuyer(string searchString)
        {
            var url = "/Admin/MarketerUser/LoadBigBuyer";
            //var data = db.Plannns.Include(m => m.PlanType).AsQueryable();
            if(searchString !=null && searchString != "")
            {
                var _data = db.MarketerUsers.Where(s => s.Usertype == 1 && s.AcceptedByParent ).Where(s=> s.Name.Contains(searchString) || s.LastName.Contains(searchString) || s.Mobile == searchString || s.Phone == searchString) .AsQueryable();
                var _res = _data.OrderByDescending(p => p.Id);

                ViewBag.Data = new PagedItem<MarketerUser>(_res, url);
                return View();

            }

            var data = db.MarketerUsers.Where(s => s.Usertype == 1 && s.AcceptedByParent).AsQueryable();
            var res = data.OrderByDescending(p => p.Id);

            ViewBag.Data = new PagedItem<MarketerUser>(res, url);


            return View();
            

        }

        [HttpGet]
        public async Task<ActionResult> BigBuyerInformation(int ? Id)
        {
         
            var Item =await db.MarketerUsers.Where(s => s.Id == Id).FirstOrDefaultAsync();
            ViewBag.Name = Item.Name + " " + Item.LastName;
            var parent = db.MarketerUsers.Where(p => p.Parent_Id == Item.Parent_Id).Select(s => new { s.Name, s.LastName }).FirstOrDefault();
            ViewBag.parent = parent.Name + " " + parent.LastName;
            return View(Item);

        }
        
        public async Task<ActionResult> ActiveBigBuyer(int id)
        {
            var user =await db.MarketerUsers.FindAsync(id);
            if (user != null)
            {

                user.IsAvailable = true;
                db.Configuration.ValidateOnSaveEnabled = false;

                await db.SaveChangesAsync();
            }
            else
            {
                return Json("UserNotFound!");
            }
          
            return RedirectToAction("LoadBigBuyer");
        }
        public async Task<ActionResult> DeactiveBigBuyer(int id)
        {
            var user =await db.MarketerUsers.FindAsync(id);
            if(user !=null)
            {
            user.IsAvailable = false;
                db.Configuration.ValidateOnSaveEnabled = false;

                await db.SaveChangesAsync();
            }
                
            return RedirectToAction("LoadBigBuyer");
        }

        public ActionResult Loadretailer(string searchString)
        {
            var url = "/Admin/MarketerUser/Loadretailer";

            if (searchString != null && searchString != "")
            {
                var _data = db.MarketerUsers.Where(s => s.Usertype == 2 && s.AcceptedByParent).Where(s => s.Name.Contains(searchString) || s.LastName.Contains(searchString) || s.Mobile == searchString || s.Phone == searchString).AsQueryable();
                var _res = _data.OrderByDescending(p => p.Id);

                ViewBag.Data = new PagedItem<MarketerUser>(_res, url);
                return View();

            }

            var data = db.MarketerUsers.Where(s => s.Usertype == 2 && s.AcceptedByParent).AsQueryable();
            var res = data.OrderByDescending(p => p.Id);

            ViewBag.Data = new PagedItem<MarketerUser>(res, url);


            return View();

        }
        public async Task<ActionResult> Activeretailer(int id)
        {
            var user = await db.MarketerUsers.FindAsync(id);
            if (user != null)
            {

                user.IsAvailable = true;
                db.Configuration.ValidateOnSaveEnabled = false;

                await db.SaveChangesAsync();
            }
            else
            {
                return Json("UserNotFound!");
            }

            return RedirectToAction("Loadretailer");
        }


        public async Task<ActionResult> Deactiveretailer(int id)
        {
            var user = await db.MarketerUsers.FindAsync(id);
            if (user != null)
            {
                user.IsAvailable = false;
                db.Configuration.ValidateOnSaveEnabled = false;

                await db.SaveChangesAsync();
            }

            return RedirectToAction("Loadretailer");
        }
        [HttpGet]
        public async Task<ActionResult> RetailerInformation(int? Id)
        {

            var Item = await db.MarketerUsers.Where(s => s.Id == Id).FirstOrDefaultAsync();
            ViewBag.Name = Item.Name + " " + Item.LastName;
            var parent = db.MarketerUsers.Where(p => p.Parent_Id == Item.Parent_Id).Select(s => new { s.Name, s.LastName }).FirstOrDefault();
            ViewBag.parent = parent.Name + " " + parent.LastName;
            return View(Item);

        }


    
        public async Task<ActionResult> ActiveCheckPayment(int? Id)
        {
            MarketerUser marketerUser =await db.MarketerUsers.FindAsync(Id);
            if (marketerUser != null)
            {      
                marketerUser.CanCheckPayment = true;
                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();
            }
            if (marketerUser.Usertype == 2)
            {
                return RedirectToAction("Loadretailer");
            }
            else
            {
                return RedirectToAction("LoadBigBuyer");

            }
        }
        public async Task<ActionResult> DeActiveCheckPayment(int? Id)
        {
            MarketerUser marketerUser = await db.MarketerUsers.FindAsync(Id);
            if (marketerUser != null)
            {
                marketerUser.CanCheckPayment = false;
                db.Configuration.ValidateOnSaveEnabled = false;

                await db.SaveChangesAsync();

            }
            if (marketerUser.Usertype == 2)
            {
            return RedirectToAction("Loadretailer");
            }
            else
            {
                return RedirectToAction("LoadBigBuyer");

            }
        }


    }
}