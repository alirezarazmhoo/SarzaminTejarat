using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class CompanyAgentUserController : Controller
    {
        DBContext db = new DBContext();

        // GET: CompanyAgentUser
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CompanyAgentUser()
        {
            return View();
        }

        public async Task<ActionResult> Store(CompanyAgent companyAgent)
        {

            if(companyAgent.LastName == null || companyAgent.Mobile == 0 || companyAgent.NationalCode == 0 || companyAgent.password == null || companyAgent.UserName == null || companyAgent.FirstName == null || companyAgent.Adress == null)
            {
                TempData["EmptyFildError"] = "    فیلدهای مورد نیاز خالی است";
                return RedirectToAction(nameof(CompanyAgentUser));



            }




            bool IsAlreadyExist = db.CompanyAgents.Any(s => s.Mobile == companyAgent.Mobile || s.NationalCode == companyAgent.NationalCode || s.UserName == companyAgent.UserName);

            if (IsAlreadyExist)
            {
                TempData["AlreadyExistError"] = "اطلاعات وارد شده تکراری است";
                return RedirectToAction(nameof(CompanyAgentUser));

            }

            try
            {
            companyAgent.Status = false;
                db.CompanyAgents.Add(companyAgent);

            }
            catch (Exception)
            {
                TempData["Error"] = "  انجام عملیات امکان پذیر نیست";

                return RedirectToAction(nameof(CompanyAgentUser));

            }
            finally
            {
              await  db.SaveChangesAsync();
                TempData["Success"] = "اطلاعات شما ثبت شد، پس از تایید شما توسط ادمین اکانت شما فعال می شود";

            }

                return RedirectToAction(nameof(CompanyAgentUser));
        }
    }
}