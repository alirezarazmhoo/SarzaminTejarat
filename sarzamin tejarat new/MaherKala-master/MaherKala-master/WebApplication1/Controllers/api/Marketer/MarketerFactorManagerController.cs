using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using WebApplication1.Models;
using WebApplication1.Models.Errors;
using WebApplication1.Models.PayOffFactor;

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
				return new { totalprice = _TotalPrice };
			}
			var totalFactorItem = await db.MarketerFactorItem.Where(s => s.MarketerFactor.MarketerUser.Api_Token == ApiToken && s.MarketerFactor.IsAdminCheck).ToListAsync();
			if (totalFactorItem == null)
			{
				return new { StatusCode = 200, Message = "کاربر مورد نظر یافت نشد!" };
			}
			double TotalPrice = 0;
			foreach (var item in totalFactorItem)
			{
				TotalPrice += item.UnitPrice * item.Qty;
			}
			return new { totalprice = TotalPrice };
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
				== _userItem.Id && p.MarketerFactor.Date >= convertdate1 && p.MarketerFactor.Date <= convertdate2
				&& p.MarketerFactor.IsAdminCheck).ToListAsync();
				double _totalPrice = 0;
				foreach (var item in _totalFactors)
				{
					_totalPrice += item.UnitPrice * item.Qty;
				}
				return new { totalprice = _totalPrice };
			}
			var userItem = await db.MarketerUsers.Where(s => s.Api_Token == ApiToken).FirstOrDefaultAsync();
			if (userItem == null)
			{
				return new { StatusCode = 1, Message = "کاربر مورد نظر یافت نشد!" };
			}
			var totalFactors = await db.MarketerFactorItem.Where(p => p.MarketerFactor.MarketerUser.Parent_Id == userItem.Id && p.MarketerFactor.IsAdminCheck).ToListAsync();
			double totalPrice = 0;
			foreach (var item in totalFactors)
			{
				totalPrice += item.UnitPrice * item.Qty;
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
			if (!(db.MarketerUsers.Any(s => s.Api_Token == ApiToken)))
			{
				return new { StatusCode = 200, Message = "کاربر مورد نظر یافت نشد!" };
			}
			var ListSell = from s in db.MarketerFactorItem
						   where s.MarketerFactor.MarketerUser.Api_Token == ApiToken
						   && s.MarketerFactor.Date.Year.Equals(thisYear)
						   && s.MarketerFactor.Date.Year == thisYear &&
						   s.MarketerFactor.IsAdminCheck
						   select new { s.UnitPrice, s.Qty };
			await ListSell.ToListAsync();
			double TotalSell = 0;
			foreach (var item in ListSell)
			{
				TotalSell += item.UnitPrice * item.Qty;
			}
			var ListSellInAllyears = from s in db.MarketerFactorItem where s.MarketerFactor.MarketerUser.Api_Token == ApiToken && s.MarketerFactor.IsAdminCheck select new { s.UnitPrice, s.Qty };
			await ListSell.ToListAsync();
			double TotalSellInAllYears = 0;
			foreach (var item in ListSell)
			{
				TotalSellInAllYears += item.UnitPrice * item.Qty;
			}
			return new { TotalSellInMonth = TotalSell, TotalSellInAllYears = TotalSellInAllYears };
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
			var PricePoint = await db.MarketerUserPoints.Where(s => s.MarketerUser.Api_Token == ApiToken
			 && s.SavedDate.Year == thisYear && s.SavedDate.Month == thismonth).Select(p => p.UserPoits).FirstOrDefaultAsync();
			return new { PricePoint = PricePoint };
		}
		[Route("api/MarketerFactorManager/HazineErsal")]
		[HttpPost]
		public async Task<object> HazineErsal()
		{
			var ApiToken = HttpContext.Current.Request.Form["Api_Token"];

			var userItem = await db.MarketerUsers.Where(s => s.Api_Token == ApiToken).FirstOrDefaultAsync();
			if (userItem == null)
			{
				return new { StatusCode = 302, Message = "کاربرمورد نظر یافت نشد" };

			}
			if (userItem.Usertype == 0)
			{
				var PriceFortransfare0 = await db.PriceForTranslates.Select(s => new { Darsad = s.MarketerPriceTranslate, Rayegan = s.Marketergratis }).FirstOrDefaultAsync();
				return new
				{
					PriceFortransfare = PriceFortransfare0
				};
			}
			if (userItem.Usertype == 1)
			{
				var PriceFortransfare1 = await db.PriceForTranslates.Select(s => new { Darsad = s.BigBuyerPriceTranslate, Rayegan = s.Buyergratis }).FirstOrDefaultAsync();
				return new
				{
					PriceFortransfare = PriceFortransfare1
				};
			}
			if (userItem.Usertype == 2)
			{
				var PriceFortransfare2 = await db.PriceForTranslates.Select(s => new { Darsad = s.RetailerPriceTranslate, Rayegan = s.Retailergratis }).FirstOrDefaultAsync();
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

				var _totalFactorItem = db.MarketerFactor.Include("MarketerFactorItems.Product").Where(s => s.MarketerUser.Api_Token == ApiToken && s.Date >= convertdate1 && s.Date <= convertdate2).Select(p => new { p.Id, p.Buyer, p.BuyerAddress, p.BuyerMobile, p.BuyerPhoneNumber, p.BuyerPostalCode, p.Date, Items = p.MarketerFactorItems.Select(a => new { Id = a.Id, Product_Id = a.Product.Id, a.Qty, UnitPrice = a.Product.MarketerPrice - a.Product.Discount, a.ProductName, a.Product.Images, a.Product.Thumbnail, a.Product.Name, a.Product.Desc, CategoriName = a.Product.Category.Name, MarketerPrice = a.Product.MarketerPrice, a.Product.Discount, a.Product.Tags, a.Product.Like, a.Product.Main_Image, a.Product.Status, a.Product.TotalVotes, a.Product.TotalComment, a.Product.Color, a.Product.IsOnlyForMarketer }) }).AsQueryable();
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
			var FactorID = Convert.ToInt32(HttpContext.Current.Request.Form["FactorID"]);
			if (db.MarketerFactor.Find(FactorID) == null)
			{
				return new
				{
					StatusCode = 500,
					Message = "فاکتور مورد نظر یافت شند"
				};
			}
			MarketerFactor marketerFactor = db.MarketerFactor.Find(FactorID);
			db.MarketerFactor.Remove(marketerFactor);
			db.SaveChanges();
			return new
			{
				StatusCode = 200,
				Message = "Success"
			};
		}
		#endregion

		[Route("api/MarketerFactorManager/AddBasketPay")]
		[HttpPost]
		public async Task<IHttpActionResult> AddBasketPay(MainModel MainModel)
		{
			int customerId = 0;
			double totalSum = 0;
			float x = 0;
			double y = 0;
			long FactorPrice = 0;
			MarketerFactor _marketerFactor = new MarketerFactor();
			MarketerFactorItem _marketerFactorItem = new MarketerFactorItem();
			MarketerFactorItem _ExistingmarketerFactorItem = new MarketerFactorItem();
			List<MarketerFactorItem> ListMarketerFactorItems = new List<MarketerFactorItem>();
			var PriceForTranslate = db.PriceForTranslates.FirstOrDefault();
			if (string.IsNullOrEmpty(MainModel.Api_Token))
			{
				return new System.Web.Http.Results.ResponseMessageResult(
				Request.CreateErrorResponse(HttpStatusCode.InternalServerError, new HttpError(ErrorsText.EmptyToken)));
			}
			MarketerUser _MarketerUser = await db.MarketerUsers.Where(s => s.Api_Token == MainModel.Api_Token).FirstOrDefaultAsync();
			if (_MarketerUser == null)
			{
				return new System.Web.Http.Results.ResponseMessageResult(
			   Request.CreateErrorResponse(HttpStatusCode.InternalServerError, new HttpError(ErrorsText.MarketerNotFound)));
			}
			if (!CheckNumberValidity(MainModel.customer_id))
			{
				return new System.Web.Http.Results.ResponseMessageResult(
				  Request.CreateErrorResponse(HttpStatusCode.InternalServerError, new HttpError(ErrorsText.CustomerIdisIncorrect)));
			}
			customerId = Convert.ToInt32(MainModel.customer_id);
			Customer customerItem = await db.Customers.Where(s => s.Id == customerId).FirstOrDefaultAsync();
			MarketerUser UserId = db.MarketerUsers.Where(s => s.Api_Token == MainModel.Api_Token).FirstOrDefault();
			if (UserId.Usertype == 0)
			{
				if (customerItem == null)
				{
					return new System.Web.Http.Results.ResponseMessageResult(
					Request.CreateErrorResponse(HttpStatusCode.InternalServerError, new HttpError(ErrorsText.UserNotFound)));
				}
			}
			try
			{
				if (UserId.Usertype == 0)
				{
					_marketerFactor.Buyer = customerItem.FullName;
					_marketerFactor.BuyerAddress = customerItem.Address;
					_marketerFactor.BuyerMobile = customerItem.Mobile;
					_marketerFactor.BuyerPhoneNumber = customerItem.Phone;
					_marketerFactor.BuyerPostalCode = customerItem.PostalCode;
				}
				else
				{
					_marketerFactor.Buyer = string.Empty;
					_marketerFactor.BuyerAddress = string.Empty;
					_marketerFactor.BuyerMobile = string.Empty;
					_marketerFactor.BuyerPhoneNumber = string.Empty;
					_marketerFactor.BuyerPostalCode = string.Empty;
				}
				_marketerFactor.Date = DateTime.Now;
				_marketerFactor.IsAdminCheck = false;
				_marketerFactor.IsAdminShow = false;
				_marketerFactor.Status = 1;
				_marketerFactor.MarketerUserId = _MarketerUser.Id;
				_marketerFactor.TotalPrice = 0;
				db.MarketerFactor.Add(_marketerFactor);
				db.Configuration.ValidateOnSaveEnabled = false;

				await db.SaveChangesAsync();
				JavaScriptSerializer js = new JavaScriptSerializer();
				data[] ListProducts = js.Deserialize<data[]>(MainModel.data);

				for (int i = 0; i < ListProducts.Count(); i++)
				{
					int _productId = ListProducts[i].Product_Id;
					Product productItem = await db.Products.Where(s => s.Id == _productId).FirstOrDefaultAsync();
					if (productItem == null)
					{
						db.MarketerFactor.Remove(db.MarketerFactor.Find(_marketerFactor.Id));
						await db.SaveChangesAsync();
						return new System.Web.Http.Results.ResponseMessageResult(
						Request.CreateErrorResponse(HttpStatusCode.InternalServerError, new HttpError(ErrorsText.ProductNotFound)));
					}
					else if (productItem.Qty == 0)
					{
						db.MarketerFactor.Remove(db.MarketerFactor.Find(_marketerFactor.Id));
						await db.SaveChangesAsync();
						return new System.Web.Http.Results.ResponseMessageResult(
						Request.CreateErrorResponse(HttpStatusCode.InternalServerError, new HttpError("کالای " + productItem.Name + " به اتمام رسیده است")));
					}
					if (ListProducts[i].count == 0)
					{
						db.MarketerFactor.Remove(db.MarketerFactor.Find(_marketerFactor.Id));
						await db.SaveChangesAsync();
						return new System.Web.Http.Results.ResponseMessageResult(
					   Request.CreateErrorResponse(HttpStatusCode.InternalServerError, new HttpError(ErrorsText.CountIsZero)));
					}
					else if (productItem.Qty < ListProducts[i].count)
					{
						db.MarketerFactor.Remove(db.MarketerFactor.Find(_marketerFactor.Id));
						await db.SaveChangesAsync();
						return new System.Web.Http.Results.ResponseMessageResult(
						Request.CreateErrorResponse(HttpStatusCode.InternalServerError, new HttpError("تعداد " + ListProducts[i].count + " عدد برای کالای " + productItem.Name + "  موجود نمی باشد تعداد موجودی فعلی  " + productItem.Qty + " عدد می باشد ")));
					}
				}

				for (int i = 0; i < ListProducts.Count(); i++)
				{

					int _productId = ListProducts[i].Product_Id;
					Product productItem = await db.Products.Where(s => s.Id == _productId).FirstOrDefaultAsync();
					switch (_MarketerUser.Usertype)
					{
						case 0:
							_marketerFactorItem.UnitPrice = productItem.MarketerPrice - productItem.Discount;
							break;
						case 1:
							_marketerFactorItem.UnitPrice = productItem.RetailerPrice - productItem.Discount;

							break;
						case 2:
							_marketerFactorItem.UnitPrice = productItem.MultiplicationBuyerPrice - productItem.Discount;
							break;
						default:
							return new System.Web.Http.Results.ResponseMessageResult(
					   Request.CreateErrorResponse(HttpStatusCode.InternalServerError, new HttpError(ErrorsText.UndefindedUserType)));
					}
					ListMarketerFactorItems.Add(new MarketerFactorItem { Qty = ListProducts[i].count, MarketerFactorId = _marketerFactor.Id, ProductId = _productId, ProductName = productItem.Name, UnitPrice = _marketerFactorItem.UnitPrice });
					productItem.Qty = productItem.Qty - ListProducts[i].count;
					FactorPrice += _marketerFactorItem.UnitPrice * ListProducts[i].count;
				}
				switch (_MarketerUser.Usertype)
				{
					case 0:
						if (PriceForTranslate != null)
						{
							if (FactorPrice > PriceForTranslate.Marketergratis)
							{
								_marketerFactorItem.UnitPrice += 0;
							}
							else
							{
								x = PriceForTranslate.MarketerPriceTranslate / 100;
								y = _marketerFactorItem.UnitPrice * x;
								FactorPrice += Convert.ToInt64(y);
							}
						}
						break;
					case 1:
						if (PriceForTranslate != null)
						{
							if (totalSum > PriceForTranslate.Retailergratis)
							{
								_marketerFactorItem.UnitPrice += 0;
							}
							else
							{
								x = PriceForTranslate.RetailerPriceTranslate / 100;
								y = _marketerFactorItem.UnitPrice * x;
								FactorPrice += Convert.ToInt64(y);
							}
						}
						break;
					case 2:
						if (PriceForTranslate != null)
						{
							if (totalSum > PriceForTranslate.Buyergratis)
							{
								_marketerFactorItem.UnitPrice += 0;
							}
							else
							{
								x = PriceForTranslate.BigBuyerPriceTranslate / 100;
								y = _marketerFactorItem.UnitPrice * x;
								FactorPrice += Convert.ToInt64(y);
							}
						}
						break;
				}
				if (ListMarketerFactorItems.Count != 0)
					ListMarketerFactorItems.ForEach(s => db.MarketerFactorItem.Add(s));
				MarketerFactor marketerFactorItem = db.MarketerFactor.Find(_marketerFactor.Id);
				if (marketerFactorItem != null)
				{
					marketerFactorItem.TotalPrice = FactorPrice;
					marketerFactorItem.OriginalPrice = FactorPrice;
				}
				await db.SaveChangesAsync();
			}
			catch (DbUpdateException ex)
			{
				return new System.Web.Http.Results.ResponseMessageResult(
					 Request.CreateErrorResponse(HttpStatusCode.InternalServerError, new HttpError(ex.Message)));
			}
			catch (DbEntityValidationException ex)
			{
				return new System.Web.Http.Results.ResponseMessageResult(
							  Request.CreateErrorResponse(HttpStatusCode.InternalServerError, new HttpError(ex.Message)));
			}
			FactorIdModel factorIdModel = new FactorIdModel();
			factorIdModel.factor_id = _marketerFactor.Id;
			return Ok(factorIdModel);
		}
		public class data
		{
			public int count { get; set; }
			public int Product_Id { get; set; }

		}

		public class MainModel
		{
			public string customer_id { get; set; }
			public string factor_id { get; set; }
			public string Api_Token { get; set; }
			public string data { get; set; }
		}
		public static object GetPropertyValue(object src, string propName)
		{
			if (src == null) throw new ArgumentException("Value cannot be null.", "src");
			if (propName == null) throw new ArgumentException("Value cannot be null.", "propName");

			if (propName.Contains("."))//complex type nested
			{
				var temp = propName.Split(new char[] { '.' }, 2);
				return GetPropertyValue(GetPropertyValue(src, temp[0]), temp[1]);
			}
			else
			{
				var prop = src.GetType().GetProperty(propName);
				return prop != null ? prop.GetValue(src, null) : null;
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
				return true;
			}

		}


		[Route("api/MarketerFactorManager/Applydiscountcode")]
		public async Task<IHttpActionResult> Applydiscountcode(ApplydiscountcodeModel applydiscountcodeModel)
		{

			int n;
			if (!Int32.TryParse(applydiscountcodeModel.FactorId, out n) || !Int32.TryParse(applydiscountcodeModel.CodeNumber, out n))
			{
				return new System.Web.Http.Results.ResponseMessageResult(
			   Request.CreateErrorResponse(HttpStatusCode.InternalServerError, new HttpError(ErrorsText.InvalidId)));
			}
			int _FactorId = Int32.Parse(applydiscountcodeModel.FactorId);
			int _CodeNumber = Int32.Parse(applydiscountcodeModel.CodeNumber);

			if (_FactorId == 0)
			{
				return new System.Web.Http.Results.ResponseMessageResult(
				Request.CreateErrorResponse(HttpStatusCode.InternalServerError, new HttpError(ErrorsText.EmptyFactorId)));
			}
			if (_CodeNumber == 0)
			{
				return new System.Web.Http.Results.ResponseMessageResult(
				Request.CreateErrorResponse(HttpStatusCode.InternalServerError, new HttpError(ErrorsText.EmptyDiscountCode)));
			}
			if (db.MarketerFactor.Find(_FactorId) == null)
			{
				return new System.Web.Http.Results.ResponseMessageResult(
			  Request.CreateErrorResponse(HttpStatusCode.InternalServerError, new HttpError(ErrorsText.FactorNotFound)));
			}
			PaymentCodes paymentCodes = db.PaymentCodes.Where(s => s.CodeNumber == _CodeNumber).FirstOrDefault();
			if (paymentCodes == null)
			{
				return new System.Web.Http.Results.ResponseMessageResult(
				Request.CreateErrorResponse(HttpStatusCode.InternalServerError, new HttpError(ErrorsText.WrongDiscountCode)));
			}
			MarketerUser marketerUser1 = db.MarketerUsers.Where(s => s.Id == paymentCodes.MarketerUserId).FirstOrDefault();
			if (paymentCodes.IsUsed)
			{
				return new System.Web.Http.Results.ResponseMessageResult(
			   Request.CreateErrorResponse(HttpStatusCode.InternalServerError, new HttpError(ErrorsText.expiredDiscountCode)));
			}
			MarketerUser marketerUser = db.MarketerUsers.Where(s => s.Api_Token == applydiscountcodeModel.Api_Token).FirstOrDefault();
			if (marketerUser == null)
			{
				return new System.Web.Http.Results.ResponseMessageResult(
				Request.CreateErrorResponse(HttpStatusCode.InternalServerError, new HttpError(ErrorsText.UserNotFound)));
			}
			if (marketerUser.IDCardNumber.Equals(marketerUser1.IDCardNumber) == false)
			{
				return new System.Web.Http.Results.ResponseMessageResult(
				Request.CreateErrorResponse(HttpStatusCode.InternalServerError, new HttpError(ErrorsText.OwnerDiscountCodeError)));
			}
			else
			{
				MarketerFactor marketerFactor = await db.MarketerFactor.Where(s => s.Id == _FactorId).FirstOrDefaultAsync();
				marketerFactor.TotalPrice -= paymentCodes.Price;
				if (marketerFactor.TotalPrice < 0)
				{
					marketerFactor.TotalPrice = 0;
				}
				paymentCodes.IsUsed = true;
				await db.SaveChangesAsync();
				return Ok(marketerFactor);

			}
		}

		public class ApplydiscountcodeModel
		{
			public string FactorId { get; set; }
			public string CodeNumber { get; set; }
			public string Api_Token { get; set; }

		}
		[Route("api/MarketerFactorManager/payOffFactor")]
		[HttpPost]
		public async Task<IHttpActionResult> payOffFactor(PayOffFactorModel payOffFactorModel)
		{
			int n;
			if (!Int32.TryParse(payOffFactorModel.FactorId, out n))
			{
				return new System.Web.Http.Results.ResponseMessageResult(
			   Request.CreateErrorResponse(HttpStatusCode.InternalServerError, new HttpError(ErrorsText.InvalidId)));
			}
			int _FactorId = Int32.Parse(payOffFactorModel.FactorId);
			if (_FactorId == 0)
			{
				return new System.Web.Http.Results.ResponseMessageResult(
				Request.CreateErrorResponse(HttpStatusCode.InternalServerError, new HttpError(ErrorsText.EmptyFactorId)));
			}
			if (!db.MarketerFactor.Any(s => s.Id == _FactorId))
			{
				return new System.Web.Http.Results.ResponseMessageResult(
				Request.CreateErrorResponse(HttpStatusCode.InternalServerError, new HttpError(ErrorsText.FactorNotFound)));
			}
			Random random = new Random();
			int randomNumber = random.Next(0, 1000000000);
			payOffFactorModel.StatusCode = 200;
			payOffFactorModel.TrackingCode = randomNumber.ToString();
			MarketerFactor marketerFactor = await db.MarketerFactor.Where(s => s.Id == _FactorId).FirstOrDefaultAsync();
			marketerFactor.TrackingCode = randomNumber.ToString();
			marketerFactor.Status = 2;
			db.Configuration.ValidateOnSaveEnabled = false;
			await db.SaveChangesAsync();
			return Ok(payOffFactorModel);
		}


		[Route("api/MarketerFactorManager/Validationdiscountcode")]
		[HttpPost]
		public async Task<IHttpActionResult> Validationdiscountcode(ApplydiscountcodeModel  applydiscountcodeModel)
		{

			int n;
			if (!Int32.TryParse(applydiscountcodeModel.FactorId, out n) || !Int32.TryParse(applydiscountcodeModel.CodeNumber, out n))
			{
				return new System.Web.Http.Results.ResponseMessageResult(
			   Request.CreateErrorResponse(HttpStatusCode.InternalServerError, new HttpError(ErrorsText.InvalidId)));
			}
			int _FactorId = Int32.Parse(applydiscountcodeModel.FactorId);
			int _CodeNumber = Int32.Parse(applydiscountcodeModel.CodeNumber);

			if (_FactorId == 0)
			{
				return new System.Web.Http.Results.ResponseMessageResult(
				Request.CreateErrorResponse(HttpStatusCode.InternalServerError, new HttpError(ErrorsText.EmptyFactorId)));
			}
			if (_CodeNumber == 0)
			{
				return new System.Web.Http.Results.ResponseMessageResult(
				Request.CreateErrorResponse(HttpStatusCode.InternalServerError, new HttpError(ErrorsText.EmptyDiscountCode)));
			}
			if (db.MarketerFactor.Find(_FactorId) == null)
			{
				return new System.Web.Http.Results.ResponseMessageResult(
			  Request.CreateErrorResponse(HttpStatusCode.InternalServerError, new HttpError(ErrorsText.FactorNotFound)));
			}
			PaymentCodes paymentCodes = db.PaymentCodes.Where(s => s.CodeNumber == _CodeNumber).FirstOrDefault();
			if (paymentCodes == null)
			{
				return new System.Web.Http.Results.ResponseMessageResult(
				Request.CreateErrorResponse(HttpStatusCode.InternalServerError, new HttpError(ErrorsText.WrongDiscountCode)));
			}
			MarketerUser marketerUser1 = db.MarketerUsers.Where(s => s.Id == paymentCodes.MarketerUserId).FirstOrDefault();
			if (paymentCodes.IsUsed)
			{
				return new System.Web.Http.Results.ResponseMessageResult(
			   Request.CreateErrorResponse(HttpStatusCode.InternalServerError, new HttpError(ErrorsText.expiredDiscountCode)));
			}
			MarketerUser marketerUser = db.MarketerUsers.Where(s => s.Api_Token == applydiscountcodeModel.Api_Token).FirstOrDefault();
			if (marketerUser == null)
			{
				return new System.Web.Http.Results.ResponseMessageResult(
				Request.CreateErrorResponse(HttpStatusCode.InternalServerError, new HttpError(ErrorsText.UserNotFound)));
			}
			if (marketerUser.IDCardNumber.Equals(marketerUser1.IDCardNumber) == false)
			{
				return new System.Web.Http.Results.ResponseMessageResult(
				Request.CreateErrorResponse(HttpStatusCode.InternalServerError, new HttpError(ErrorsText.OwnerDiscountCodeError)));
			}
			else
			{
				return new System.Web.Http.Results.ResponseMessageResult(
						Request.CreateResponse(HttpStatusCode.Accepted, new HttpError(ErrorsText.CodeAccepted)));
			}
		}
	}
}
