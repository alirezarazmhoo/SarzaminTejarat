using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.Models.Errors;

namespace WebApplication1.Areas.Admin.Controllers.RetailerAndMultipateBuyer
{
    public class PaySettingsController : Controller
    {
        // GET: Admin/PaySettings
        public ActionResult Index()
        {
            using (DBContext db = new DBContext())
            {
                var MinimumForPayInPerson = db.PaySettings.Where(s => s.Type == PaySettingType.MinimumForPayInPerson).Select(s => new { s.Value }).FirstOrDefault();
                var MaximumForPayInPerson = db.PaySettings.Where(s => s.Type == PaySettingType.MaximumForPayInPerson).Select(s => new { s.Value }).FirstOrDefault();
                var MinimumForCheckPay = db.PaySettings.Where(s => s.Type == PaySettingType.MinimumForCheckPay).Select(s => new { s.Value }).FirstOrDefault();
                var MinimumForCreditPay = db.PaySettings.Where(s => s.Type == PaySettingType.MinimumForCreditPay).Select(s => new { s.Value }).FirstOrDefault();

                ViewBag.MinimumForPayInPerson = MinimumForPayInPerson==null?0: MinimumForPayInPerson.Value;
                ViewBag.MaximumForPayInPerson = MaximumForPayInPerson == null? 0 : MaximumForPayInPerson.Value;
                ViewBag.MinimumForCheckPay = MinimumForCheckPay == null?0: MinimumForCheckPay.Value;
                ViewBag.MinimumForCreditPay = MinimumForCreditPay==null ?0 : MinimumForCreditPay.Value;
            }

            return View();
        }

        public async Task<ActionResult> Add(string MinimumForCheckPay,string MinimumForPayInPerson , string MaximumForPayInPerson ,string MinimumForCreditPay)
        {        
            using (DBContext db = new DBContext())
            {
             
             if (!string.IsNullOrEmpty(MinimumForPayInPerson) && !string.IsNullOrEmpty(MaximumForPayInPerson))
            {
                    if (CheckNumberValidity(MinimumForPayInPerson) == false || CheckNumberValidity(MinimumForPayInPerson)==false)
                    {
                        TempData["Error"] = ErrorsText.IncorrectInputNumber;
                        return RedirectToAction(nameof(Index));
                    }
                    if (Convert.ToInt32( MinimumForPayInPerson) > Convert.ToInt32(MaximumForPayInPerson) )
            {
                TempData["Error"] = ErrorsText.PaySettingError;
                return RedirectToAction(nameof(Index));
            }
            }
            if (!String.IsNullOrEmpty(MinimumForPayInPerson))
             {
                    

               if (CheckNumberValidity(MinimumForPayInPerson) == false)
              {
                TempData["Error"] = ErrorsText.IncorrectInputNumber;
                return RedirectToAction(nameof(Index));
              }
             var _MaximumForPayInPerson = db.PaySettings.Where(s => s.Type == PaySettingType.MaximumForPayInPerson).Select(s => new { s.Value }).FirstOrDefault();

             if (float.Parse(MinimumForPayInPerson) > _MaximumForPayInPerson.Value)
              {
                   TempData["Error"] = ErrorsText.PaySettingError2;
                   return RedirectToAction(nameof(Index));
              }

             }
            if (!String.IsNullOrEmpty(MaximumForPayInPerson))
                {
                    
                    if (CheckNumberValidity(MaximumForPayInPerson)==false){
                        TempData["Error"] = ErrorsText.IncorrectInputNumber;
                        return RedirectToAction(nameof(Index));
                    }
                    var _MinimumForPayInPerson = db.PaySettings.Where(s => s.Type == PaySettingType.MinimumForPayInPerson).Select(s => new { s.Value }).FirstOrDefault();

                    if (float.Parse(MaximumForPayInPerson) < _MinimumForPayInPerson.Value)
                    {
                        TempData["Error"] = ErrorsText.PaySettingError2;
                        return RedirectToAction(nameof(Index));
                    }
                }
            if (!String.IsNullOrEmpty(MinimumForCheckPay))
                {
                    
                    if (CheckNumberValidity(MinimumForCheckPay)== false)
                    {
                        TempData["Error"] = ErrorsText.IncorrectInputNumber;
                        return RedirectToAction(nameof(Index));
                    }

                }
            if (!String.IsNullOrEmpty(MinimumForCreditPay))
                {

                    if (CheckNumberValidity(MinimumForCreditPay) == false)
                    {
                        TempData["Error"] = ErrorsText.IncorrectInputNumber;
                        return RedirectToAction(nameof(Index));
                    }

                }
                try
                {
                    if (!String.IsNullOrEmpty(MinimumForCheckPay))
                    {
                        PaySetting paySetting = await db.PaySettings.Where(s => s.Type == PaySettingType.MinimumForCheckPay).FirstOrDefaultAsync();
                        paySetting.Value = float.Parse(MinimumForCheckPay);
                    }
                    if (!String.IsNullOrEmpty(MinimumForPayInPerson))
                    {
                        PaySetting paySetting = await db.PaySettings.Where(s => s.Type == PaySettingType.MinimumForPayInPerson).FirstOrDefaultAsync();
                        paySetting.Value = float.Parse(MinimumForPayInPerson);
                    }
                    if (!String.IsNullOrEmpty(MaximumForPayInPerson))
                    {
                        PaySetting paySetting = await db.PaySettings.Where(s => s.Type == PaySettingType.MaximumForPayInPerson).FirstOrDefaultAsync();
                        paySetting.Value = float.Parse(MaximumForPayInPerson);
                    }
                    if (!String.IsNullOrEmpty(MinimumForCreditPay))
                    {
                        PaySetting paySetting = await db.PaySettings.Where(s => s.Type == PaySettingType.MinimumForCreditPay).FirstOrDefaultAsync();
                        paySetting.Value = float.Parse(MinimumForCreditPay);
                    }
                    await db.SaveChangesAsync();
                }
                catch(Exception ex)
                {
                    TempData["Error"] = ErrorsText.SystemError;
                }
            return RedirectToAction(nameof(Index));
            }
        }
   
    
           private bool CheckNumberValidity(string Number)
            {
            int n;
            if (!int.TryParse(Number, out n))
            {
                return false;
            }
            else
            {
             return   true;
            }

        }
    
    }
}