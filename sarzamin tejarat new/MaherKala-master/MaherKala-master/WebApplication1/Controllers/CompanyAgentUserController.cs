using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
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


        public async Task<ActionResult> SignIn(CompanyAgent companyAgent)

        {
            if(companyAgent.UserName==null || companyAgent.password==null)
            {
                TempData["ErrorLoging"] = "نام کاربری یا رمز عبور خالی است";
                return RedirectToAction(nameof(Index));
            }
            var item =await db.CompanyAgents.Where(s => s.UserName == companyAgent.UserName && s.password == companyAgent.password).FirstOrDefaultAsync();
            if (item == null)
            {
                TempData["ErrorUserLogin"] = "نام کاربری یا رمز عبور صحیح نیست";
                return RedirectToAction(nameof(Index));
            }
            else if(!item.Status)
            {
                TempData["StatusError"] = "شما هنوز توسط ادمین تایید نشده اید";
                return RedirectToAction(nameof(Index));

            }
           

            else
            {

                return RedirectToAction("CompanyAjentHomeController", "CompanyAgentUser", new { userId = item.Id });
            }


          
            

        }


        public ActionResult CompanyAjentHomeController(int? userId)
        {
            if(userId == null)
            {
                return RedirectToAction(nameof(Index));
            }
            var userItem = db.CompanyAgents.Include(s=>s.Company).Where(s => s.Id == userId).FirstOrDefault();
            if(userItem == null)
            {
                TempData["CantEnter"] = "خطا در ورود کاربر";
                return RedirectToAction(nameof(Index));
            }
            if(userItem.CompanyID != null)
            {
                ViewBag.IdUser = userItem.Id;
                ViewBag.CompanyName = userItem.Company.Name;
            var listProducts = db.Products.Where(s => s.CompanyID == userItem.CompanyID).ToList();


                return View(listProducts);

            }



            return View();
        }


        public async Task<ActionResult> QutyRegister(CompanyAjentProduct companyAjentProduct,int IdUser)
        {
            bool ItemExist = db.CompanyAjentProducts.Any(S => S.ProductID == companyAjentProduct.ProductID);
           
            if(companyAjentProduct.Quty == 0)
            {
                TempData["ErrorQuty"] = "تعداد را وارد کنید";
                return RedirectToAction("CompanyAjentHomeController", "CompanyAgentUser", new { userId = IdUser });
            }

                if (ItemExist)
                {
                    var Item = await db.CompanyAjentProducts.Where(s => s.ProductID == companyAjentProduct.ProductID).FirstOrDefaultAsync();
                    Item.Quty = companyAjentProduct.Quty;
                }
                else
                {
                    db.CompanyAjentProducts.Add(companyAjentProduct);
                }
            try
            {

                    await db.SaveChangesAsync();
                var productName = await db.Products.Where(s => s.Id == companyAjentProduct.ProductID).FirstOrDefaultAsync();

                TempData["Success"] = "موفقیت آمیز ،پس از بیست و چهار ساعت تغییرات بر روی تعداد  این کالا اعمال خواهد شد";
            }
             
            
            catch(SqlException ex)
            {
                StringBuilder errorMessages = new StringBuilder();
                for (int i = 0; i < ex.Errors.Count; i++)
                {
                    errorMessages.Append("Index #" + i + "\n" +
                        "Message: " + ex.Errors[i].Message + "\n" +
                        "LineNumber: " + ex.Errors[i].LineNumber + "\n" +
                        "Source: " + ex.Errors[i].Source + "\n" +
                        "Procedure: " + ex.Errors[i].Procedure + "\n");
                }
                TempData["errorMessages"] = errorMessages;
            }

            return RedirectToAction("CompanyAjentHomeController", "CompanyAgentUser", new { userId = IdUser });

        }
    }
}