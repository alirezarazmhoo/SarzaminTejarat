using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

using WebApplication1.Controllers.api.Repository;
using WebApplication1.Models;

namespace WebApplication1.Controllers.api.Marketer
{
    [Route("api/MarketerWatingChildUsers")]

    public class MarketerWatingChildUsersController : ApiController
    {
        DBContext db = new DBContext();

        #region ShowChilds
        [Route("api/MarketerWatingChildUsers/GetWaitingUsersChild")]
        [HttpPost]
        public async Task<object> GetWaitingUsersChild()
        {
            var ApiToken = HttpContext.Current.Request.Form["Api_Token"];
            var Userid = 0;
            var Item = db.MarketerUsers.Where(p => p.Api_Token == ApiToken).FirstOrDefault();
            if (Item == null)
            {
                return new { StatusCode = 1, Message = "کاربرموردنظر یافت نشد" };

            }
            if(Item.Usertype != 1)
            {
                return new { StatusCode = 1, Message = "انجام این اکشن فقط برای بازاریاب مجاز است" };

            }
            else
            {
            Userid = Item.Id;
            }
            var ListChildrens = from s in db.MarketerUsers where s.Parent_Id == Userid && s.AcceptedByParent !=true select new { s.Id, s.AccountNumber, s.Address, s.Api_Token, s.CardAccountNumber, s.CertificateNumber, s.Description, s.FactorCounter, s.IBNA, s.IDCardNumber, s.IDCardPhotoAddress, s.IsFirstTime, s.IsMarrid, s.LastName, s.Lat, s.Lng, s.Mobile, s.Name, s.PersonalPicture, s.PersonalReagentCode, s.Phone, s.Usertype,s.AcceptedByParent,s.IsAvailable };

            return await ListChildrens.ToListAsync();
           


        }
        #endregion

        #region Accept A UserChilld
        [Route("api/MarketerWatingChildUsers/AcceptuserChild")]
        [HttpPost]
        public async Task<object> AcceptuserChild()
        {
            var ApiToken = HttpContext.Current.Request.Form["Api_Token"];
            var ParentApiToken = HttpContext.Current.Request.Form["ParentApi_Token"];
            var parentUser = db.MarketerUsers.Where(p => p.Api_Token == ParentApiToken).FirstOrDefault();
            if(parentUser == null)
            {
                return new { StatusCode = 1, Message = "کاربرموردنظر یافت نشد" };
            }
            if(parentUser.SubsetCount == 0)
            {
                return new { StatusCode = 1, Message = "سهمیه شما برای افزایش زیر مجموعه به اتمام رسیده" };
            }
            else
            {
                parentUser.SubsetCount--;
            }

            var Item = db.MarketerUsers.Where(p => p.Api_Token == ApiToken).FirstOrDefault();
            if (Item == null)
            {
                return new { StatusCode = 1, Message = "کاربرموردنظر یافت نشد" };
            }

            try
            {

            Item.AcceptedByParent = true;
              
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                    throw;  
            }
            return  new { StatusCode = 0, Message = "" };

        }
        #endregion


        #region RejectuserChild
        [Route("api/MarketerWatingChildUsers/RejectuserChild")]
        [HttpPost]
        public async Task<object> RejectuserChild()
        {
            var ApiToken = HttpContext.Current.Request.Form["Api_Token"];
            var Item = db.MarketerUsers.Where(p => p.Api_Token == ApiToken).FirstOrDefault();
            if (Item == null)
            {
                return new { StatusCode = 1, Message = "کاربرموردنظر یافت نشد" };

            }
            MarketerUser _User =await db.MarketerUsers.FindAsync(Item.Id);

            if (_User == null)
            {
                return new { StatusCode = 1, Message = "کاربرموردنظر یافت نشد" };

            }
            try
            {
                db.MarketerUsers.Remove(_User);
                
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return new { StatusCode = 0, Message = "" };

        }


        #endregion

    }
}
