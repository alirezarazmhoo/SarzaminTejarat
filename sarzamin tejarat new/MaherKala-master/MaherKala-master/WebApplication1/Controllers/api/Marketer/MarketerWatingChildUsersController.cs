using SmsIrRestful;
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
using WebApplication1.Models;
using WebApplication1.Utility;

namespace WebApplication1.Controllers.api.Marketer
{
    [Route("api/MarketerWatingChildUsers")]

    public class MarketerWatingChildUsersController : ApiController
    {
        DBContext db = new DBContext();
        List<SmsParameters>  _SmsParameters = new List<SmsParameters>();
        SendSms _SendSms = new SendSms();
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
                return new { StatusCode = 200, Message = "کاربرموردنظر یافت نشد" };

            }
            if(Item.Usertype !=0)
            {
                return new { StatusCode = 206, Message = "انجام این اکشن فقط برای بازاریاب مجاز است" };

            }
            else
            {
            Userid = Item.Id;
            }
            var ListChildrens = from s in db.MarketerUsers where s.Parent_Id == Userid && s.AcceptedByParent !=true select new { s.Id, s.AccountNumber, s.Address,userToken= s.Api_Token, s.CardAccountNumber, s.CertificateNumber, s.Description, s.FactorCounter, s.IBNA, s.IDCardNumber, s.IDCardPhotoAddress, s.IsFirstTime, s.IsMarrid, s.LastName, s.Lat, s.Lng, s.Mobile, s.Name, s.PersonalPicture, s.PersonalReagentCode, s.Phone, s.Usertype,s.AcceptedByParent,s.IsAvailable };

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
            SmsParameters smsParameters = new SmsParameters();

            if (string.IsNullOrEmpty(ApiToken) || string.IsNullOrEmpty(ParentApiToken))
			{
				return new { StatusCode = 200, Message = "داده های مورد نظر خالی است " };
			}
			var parentUser = db.MarketerUsers.Where(p => p.Api_Token == ParentApiToken).FirstOrDefault();
            if(parentUser == null)
            {
                return new { StatusCode = 200, Message = "کاربرموردنظر یافت نشد" };
            }
            if(parentUser.SubsetCount == 0)
            {
                return new { StatusCode = 208, Message = "سهمیه شما برای افزایش زیر مجموعه به اتمام رسیده" };
            }
            else
            {
                parentUser.SubsetCount--;
            }

            var Item = db.MarketerUsers.Where(p => p.Api_Token == ApiToken).FirstOrDefault();
            if (Item == null)
            {
                return new { StatusCode = 200, Message = "کاربرموردنظر یافت نشد" };
            }
            try
            {
                Item.AcceptedByParent = true;
				db.Configuration.ValidateOnSaveEnabled = false;
               await db.SaveChangesAsync();
                _SmsParameters.Add(new SmsParameters() { Parameter = "MarketerUserChild", ParameterValue = Item.Name + " " + Item.LastName });
                _SmsParameters.Add(new SmsParameters() { Parameter = "MarketerUserParent", ParameterValue = parentUser.Name + " " + parentUser.LastName });
                _SendSms.CallSmSMethodAdvanced(Convert.ToInt64(Item.Mobile), 29331, _SmsParameters);
                _SendSms.CallSmSMethod(_SendSms.AdminMobile, 29341, "MarketerUser", Item.Name + " " + Item.LastName);
            }
            catch (DbUpdateConcurrencyException)
            {
                    throw;  
            }
       
            return  new { StatusCode = 0, Message = "زیر مجموعه مورد نظر تایید شد" };

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
              return new { StatusCode = 200, Message = "کاربرموردنظر یافت نشد" };
            }
			string UserName = Item.Name;
			string LastName = Item.LastName;
			string Mobile = Item.Mobile;
			string UserPassword = Item.Password;
            long _Mobile =Int64.Parse( Mobile);
            MarketerUser _User =await db.MarketerUsers.FindAsync(Item.Id);
            if (_User == null)
            {
                return new { StatusCode = 200, Message = "کاربرموردنظر یافت نشد" };
            }
            try
            {
                db.MarketerUsers.Remove(_User);
				db.Configuration.ValidateOnSaveEnabled = false;
                await db.SaveChangesAsync();
            }	
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            finally
            {
                _SendSms.CallSmSMethod(_Mobile, 27113, "MarketerName",Item.Name + " "  +  Item.LastName);
            }
            return new { StatusCode = 0, Message = "" };
        }
        #endregion
    }
}
