using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Areas.Admin.Controllers.Marketer
{
    public class SendMessageController : Controller
    {
        DBContext db = new DBContext();

        // GET: Admin/SendMessage
        public ActionResult ForMarketer()
        {
            var Messages = db.SendMessages.Where(s => s.UserType == 0).ToList();

            return View(Messages);
        }


        public ActionResult CreateForMarketer()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> CreateForMarketer(SendMessage sendMessage)
        {
            if(sendMessage.LastMessage == null)
            {
                TempData["EmptyContent"] = "متن پیام خالی است";
                return RedirectToAction(nameof(CreateForMarketer));
            }
            sendMessage.UserType = 0;
            sendMessage.createDate = DateTime.Now;
            sendMessage.isRead = false;
            db.SendMessages.Add(sendMessage);
           await db.SaveChangesAsync();
            return RedirectToAction(nameof(ForMarketer));
        }
        public ActionResult RemoveForMarketer(int? id)
        {
            var item = db.SendMessages.Where(s => s.id == id).FirstOrDefault();

            return View(item);
          

        }

        [HttpPost]
        public async Task<ActionResult> RemoveForMarketer(int id)
        {
            var item = db.SendMessages.Find(id);
            db.SendMessages.Remove(item);
          await  db.SaveChangesAsync();
            return RedirectToAction(nameof(ForMarketer));
        }


        public ActionResult ForBigBuyer()
        {
            var Messages = db.SendMessages.Where(s => s.UserType == 1).ToList();

            return View(Messages);
        }


        public ActionResult CreateForBigBuyer()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateForBigBuyer(SendMessage sendMessage)
        {
            if (sendMessage.LastMessage == null)
            {
                TempData["EmptyContent"] = "متن پیام خالی است";
                return RedirectToAction(nameof(CreateForBigBuyer));
            }
            sendMessage.UserType = 1;
            sendMessage.createDate = DateTime.Now;
            sendMessage.isRead = false;
            db.SendMessages.Add(sendMessage);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(ForBigBuyer));
        }
        public ActionResult RemoveForBigBuyer(int? id)
        {
            var item = db.SendMessages.Where(s => s.id == id).FirstOrDefault();

            return View(item);


        }
        [HttpPost]
        public async Task<ActionResult> RemoveForBigBuyer(int id)
        {
            var item = db.SendMessages.Find(id);
            db.SendMessages.Remove(item);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(ForBigBuyer));
        }
        public ActionResult Forretailer()
        {
            var items = db.SendMessages.Where(s => s.UserType == 2).ToList();
            return View(items);
        }
        public ActionResult CreateForRetailer()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> CreateForRetailer(SendMessage sendMessage)
        {
            if (sendMessage.LastMessage == null)
            {
                TempData["EmptyContent"] = "متن پیام خالی است";
                return RedirectToAction(nameof(CreateForRetailer));
            }
            sendMessage.UserType = 2;
            sendMessage.createDate = DateTime.Now;
            sendMessage.isRead = false;
            db.SendMessages.Add(sendMessage);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Forretailer));
        }

        public ActionResult RemoveForRetailer(int? id)
        {
            var item = db.SendMessages.Where(s => s.id == id).FirstOrDefault();

            return View(item);
        }
        [HttpPost]
        public async Task<ActionResult> RemoveForRetailer(int id)
        {
            var item = db.SendMessages.Find(id);
            db.SendMessages.Remove(item);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Forretailer));
        }

    }
}