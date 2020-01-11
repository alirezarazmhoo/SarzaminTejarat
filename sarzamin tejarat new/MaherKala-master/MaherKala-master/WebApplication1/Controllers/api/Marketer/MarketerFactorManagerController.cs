using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WebApplication1.Models;
namespace WebApplication1.Controllers.api.Marketer
{
    public class MarketerFactorManagerController : ApiController
    {
        DBContext db = new DBContext();     
        [Route("api/MarketerFactorManager/GetUserTotalFactor")]
        [HttpPost]
       public async Task<object> GetUserTotalFactor()
        {
            var ApiToken = HttpContext.Current.Request.Form["Api_Token"];
            var date1 = HttpContext.Current.Request.Form["date1"];
            var date2 = HttpContext.Current.Request.Form["date2"];
            if((date1 !="" && date2 != "") && (date1 !=null && date2 !=null))
            {
                DateTime dt;
                if (!DateTime.TryParse(date1, out dt))
                {
                    return new { StatusCode = 700, Message = "فرمت تاریخ صحیح نیست" };
                }
                if (!DateTime.TryParse(date2, out dt))
                {
                    return new { StatusCode = 700, Message = "فرمت تاریخ صحیح نیست" };
                }
                var convertdate1 = WebApplication1.Areas.Admin.Utility.DateChanger.ToGeorgianDateTime(date1);
                var convertdate2 = WebApplication1.Areas.Admin.Utility.DateChanger.ToGeorgianDateTime(date2);
                var _totalFactorItem = await db.MarketerFactorItem.Where(s => s.MarketerFactor.MarketerUser.Api_Token == ApiToken && s.MarketerFactor.Date >= convertdate1 && s.MarketerFactor.Date <= convertdate2 && s.MarketerFactor.IsAdminCheck).ToListAsync();
                if (_totalFactorItem == null)
                {
                    return new { StatusCode = 200, Message = "کاربر مورد نظر یافت نشد!" };
                }
                double _TotalPrice = 0;
                foreach (var item in _totalFactorItem)
                {
                    _TotalPrice += item.UnitPrice * item.Qty;
                }
                return new { totalprice = _TotalPrice} ;
            } 
            var totalFactorItem =await db.MarketerFactorItem.Where(s => s.MarketerFactor.MarketerUser.Api_Token == ApiToken && s.MarketerFactor.IsAdminCheck ).ToListAsync();
            if (totalFactorItem == null)
            {           
                    return new { StatusCode = 200, Message = "کاربر مورد نظر یافت نشد!" };          
            }
            double TotalPrice = 0;
            foreach (var item in totalFactorItem)
            {
                TotalPrice += item.UnitPrice * item.Qty;
            }
            return new { totalprice = TotalPrice } ;
        }
        [Route("api/MarketerFactorManager/GetAllUsersTotalFactor")]
        [HttpPost]
        public async Task<object> GetAllUsersTotalFactor()
        {
            var ApiToken = HttpContext.Current.Request.Form["Api_Token"];
            var date1 = HttpContext.Current.Request.Form["date1"];
            var date2 = HttpContext.Current.Request.Form["date2"];
            if ((date1 != "" && date2 != "") && (date1 != null && date2 != null))
            {
                DateTime dt;
                if (!DateTime.TryParse(date1, out dt))
                {
                    return new { StatusCode = 1, Message = "فرمت تاریخ صحیح نیست" };
                }
                if (!DateTime.TryParse(date2, out dt))
                {
                    return new { StatusCode = 1, Message = "فرمت تاریخ صحیح نیست" };
                }
                var convertdate1 = WebApplication1.Areas.Admin.Utility.DateChanger.ToGeorgianDateTime(date1);
                var convertdate2 = WebApplication1.Areas.Admin.Utility.DateChanger.ToGeorgianDateTime(date2);
                var _userItem = await db.MarketerUsers.Where(s => s.Api_Token == s.Api_Token).FirstOrDefaultAsync();
                if (_userItem == null)
                {
                    return new { StatusCode = 1, Message = "کاربر مورد نظر یافت نشد!" };
                }
                var _totalFactors = await db.MarketerFactorItem.Where(p => p.MarketerFactor.MarketerUser.Parent_Id 
                == _userItem.Id && p.MarketerFactor.Date >=convertdate1 && p.MarketerFactor.Date <= convertdate2
				&& p.MarketerFactor.IsAdminCheck).ToListAsync();
                double _totalPrice = 0;
                foreach (var item in _totalFactors)
                {
                    _totalPrice += item.UnitPrice * item.Qty;
                }
                return new { totalprice = _totalPrice };
            }
            var userItem =await db.MarketerUsers.Where(s => s.Api_Token == ApiToken ).FirstOrDefaultAsync();
            if (userItem == null)
            {
                return new { StatusCode = 1, Message = "کاربر مورد نظر یافت نشد!" };
            }
            var totalFactors =await db.MarketerFactorItem.Where(p => p.MarketerFactor.MarketerUser.Parent_Id == userItem.Id && p.MarketerFactor.IsAdminCheck).ToListAsync();
            double totalPrice = 0;
            foreach (var item in totalFactors)
            {
                totalPrice += item.UnitPrice *item.Qty;
            }        
            return new { totalprice = totalPrice };
        }
        #region GetTotalSellForAMarketerInthisMonth
        [Route("api/MarketerFactorManager/GetTotalSellForAMarketerInthisMonth")]
        [HttpPost]
        public async Task<object> GetTotalSellForAMarketer()
        {
            var ApiToken = HttpContext.Current.Request.Form["Api_Token"];
            var thismonth = DateTime.Now.Month;
            var thisYear = DateTime.Now.Year;
            if(!(db.MarketerUsers.Any(s=>s.Api_Token == ApiToken) ))
            {
                return new { StatusCode = 200, Message = "کاربر مورد نظر یافت نشد!" };
            }        
            var ListSell = from s in db.MarketerFactorItem
		 where s.MarketerFactor.MarketerUser.Api_Token ==ApiToken 
		 && s.MarketerFactor.Date.Year.Equals(thisYear) 
		 && s.MarketerFactor.Date.Year == thisYear && 
		 s.MarketerFactor.IsAdminCheck
		 select new { s.UnitPrice,s.Qty};
           await ListSell.ToListAsync();
            double TotalSell = 0; 
            foreach (var item in ListSell)
            {
                TotalSell += item.UnitPrice * item.Qty;
            }
            var ListSellInAllyears = from s in db.MarketerFactorItem where s.MarketerFactor.MarketerUser.Api_Token==ApiToken && s.MarketerFactor.IsAdminCheck select new { s.UnitPrice, s.Qty };
            await ListSell.ToListAsync();
            double TotalSellInAllYears = 0;
            foreach (var item in ListSell)
            {
                TotalSellInAllYears += item.UnitPrice * item.Qty;
            }
            return new { TotalSellInMonth= TotalSell,TotalSellInAllYears= TotalSellInAllYears } ;
        }
        #endregion
        [Route("api/MarketerFactorManager/ShowPricePointInThisMonth")]
        [HttpPost]
        public async Task<object> ShowPricePointInThisMonth()
        {
            var ApiToken = HttpContext.Current.Request.Form["Api_Token"];
            var thismonth = DateTime.Now.Month;
            var thisYear = DateTime.Now.Year;
            var Item = db.MarketerUsers.Any(s => s.Api_Token == ApiToken);
            if (!Item)
            {
                return new { StatusCode = 200, Message = "کاربر مورد نظر یافت نشد!" };
            }
            var PricePoint =await db.MarketerUserPoints.Where(s => s.MarketerUser.Api_Token == ApiToken
            && s.SavedDate.Year == thisYear && s.SavedDate.Month == thismonth).Select(p=>p.UserPoits).FirstOrDefaultAsync();
            return new { PricePoint= PricePoint };
        }
        [Route("api/MarketerFactorManager/HazineErsal")]
        [HttpPost]
        public async Task<object> HazineErsal()
        {
            var ApiToken = HttpContext.Current.Request.Form["Api_Token"];
            var userItem =await db.MarketerUsers.Where(s => s.Api_Token == ApiToken).FirstOrDefaultAsync();
            if(userItem.Usertype == 0)
            {
               var PriceFortransfare0 =await db.PriceForTranslates.Select(s => new { Darsad = s.MarketerPriceTranslate, Takhfif = s.Marketergratis }).FirstOrDefaultAsync();
                return new
                {
                    PriceFortransfare = PriceFortransfare0
                };
            }
            if (userItem.Usertype == 1)
            {
               var PriceFortransfare1 =await db.PriceForTranslates.Select(s => new { Darsad = s.BigBuyerPriceTranslate, Takhfif = s.Buyergratis }).FirstOrDefaultAsync();
                return new
                {
                    PriceFortransfare = PriceFortransfare1
                };
            }
            if (userItem.Usertype == 2)
            {
               var PriceFortransfare2 =await db.PriceForTranslates.Select(s => new { Darsad = s.RetailerPriceTranslate, Takhfif = s.Retailergratis }).FirstOrDefaultAsync();
                return new
                {
                    PriceFortransfare = PriceFortransfare2
                };
            }

            return new { StatusCode = 200, Message = "خطا" };

        }

		#region SearchFactorByDate

		[Route("api/MarketerFactorManager/GetUserTotalFactorByDate")]
		[HttpPost]
		public object GetUserTotalFactorByDate()
		{
			var ApiToken = HttpContext.Current.Request.Form["Api_Token"];
			var date1 = HttpContext.Current.Request.Form["date1"];
			var date2 = HttpContext.Current.Request.Form["date2"];
			if ((date1 != "" && date2 != "") && (date1 != null && date2 != null))
			{

				DateTime dt;
				if (!DateTime.TryParse(date1, out dt))
				{
					return new { StatusCode = 700, Message = "فرمت تاریخ صحیح نیست" };

				}
				if (!DateTime.TryParse(date2, out dt))
				{
					return new { StatusCode = 700, Message = "فرمت تاریخ صحیح نیست" };
				}
				var convertdate1 = WebApplication1.Areas.Admin.Utility.DateChanger.ToGeorgianDateTime(date1);
				var convertdate2 = WebApplication1.Areas.Admin.Utility.DateChanger.ToGeorgianDateTime(date2);

				var _totalFactorItem =  db.MarketerFactor.Include("MarketerFactorItems.Product").Where(s => s.MarketerUser.Api_Token == ApiToken && s.Date >= convertdate1 && s.Date <= convertdate2).Select(p => new { p.Id, p.Buyer, p.BuyerAddress, p.BuyerMobile, p.BuyerPhoneNumber, p.BuyerPostalCode, p.Date, Items = p.MarketerFactorItems.Select(a => new { Id = a.Id, Product_Id = a.Product.Id, a.Qty, UnitPrice = a.Product.MarketerPrice - a.Product.Discount, a.ProductName, a.Product.Images, a.Product.Thumbnail, a.Product.Name, a.Product.Desc, CategoriName = a.Product.Category.Name, MarketerPrice = a.Product.MarketerPrice, a.Product.Discount, a.Product.Tags, a.Product.Like, a.Product.Main_Image, a.Product.Status, a.Product.TotalVotes, a.Product.TotalComment, a.Product.Color, a.Product.IsOnlyForMarketer }) }).AsQueryable();
				if (_totalFactorItem == null)
				{
					return new { StatusCode = 800, Message = "بدون فاکتور" };
				}

				var res = _totalFactorItem.OrderByDescending(s => s.Id);
				var Result = new PagedItem<object>(res, "");
				return new
				{
					Result = Result
				};
			}
			return new
			{
				Message = "فاکتوری یافت نشده"
			};
		}
		[Route("api/MarketerFactorManager/RemoveDraftFactor")]
		[HttpPost]
		public object RemoveDraftFactor()
		{	
			var FactorID =Convert.ToInt32( HttpContext.Current.Request.Form["FactorID"]);
			if(FactorID == 0)
			{
				return new { StatusCode = 706, Message = "ای دی فاکتور صفر است" };
			}
			if (db.MarketerFactor.Any(s => s.Status == 1) && db.MarketerFactor.Any(s=>s.Id == FactorID))
			{
				using (SqlConnection conn = new SqlConnection("Data Source=95.216.56.89,2016;Initial Catalog=atrincom123_shop;User Id=atrincom123_shop;Password=26cne3D&"))
				{
					conn.Open();
					SqlCommand cmd = new SqlCommand("RemoveFactor", conn);
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.Add(new SqlParameter("@FactorId", FactorID));
					int res = cmd.ExecuteNonQuery();
					conn.Close();
					if (res > 0)
					{
						return new
						{
							status = 0,
							Message = "فاکتور مورد نظر حذف گردید"
						};
					}
					else
					{
						return new
						{
							status = 360,
							Message = "عملیات با شکست مواجه شد"
						};
					}
				}
			}
			else
			{
				return new { StatusCode = 361, Message = "این فاکتور اجازه حذف ندارد" };
			}
		}
		#endregion
	}
}
