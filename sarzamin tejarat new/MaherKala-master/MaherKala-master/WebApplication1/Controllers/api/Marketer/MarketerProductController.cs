using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebApplication1.Models;
using System.Data.Entity;

namespace WebApplication1.Controllers.api.Marketer
{
	public class MarketerProductController : ApiController
	{
		DBContext db = new DBContext();
		[HttpPost]
		[Route("api/Product/MarketerProduct/GetProduct")]

		public object GetProduct()
		{
			int usertype = 0;
			var RetailerDiscount = db.RetailerFirstFactorDiscounts.FirstOrDefault();
			int id = Convert.ToInt32(HttpContext.Current.Request.Form["Id"]);

			//int id = Convert.ToInt32(HttpContext.Current.Request.QueryString["Id"]);
			var token = HttpContext.Current.Request.Form["Api_Token"];
			var userItem = db.MarketerUsers.Where(s => s.Api_Token == token).FirstOrDefault();
			if (userItem != null)
			{
				usertype = userItem.Usertype;

			}
			if (usertype == 0)
			{

				var data = db.Products.Include("Category").Where(p => p.Status == true && p.IsOnlyForMarketer).Where(p => p.Id == id).Select(p => new {p.Category.Name, p.Color, p.Comments, p.Desc, p.Discount, p.Id, p.Main_Image, p.Images, p.MarketerPrice, ProductName = p.Name, p.ProductPercents, p.Qty, p.Tags, p.Thumbnail, p.TotalVotes }).FirstOrDefault();
				return new
				{
					Data = data,
					Status = 0
				};
			}
	
			else if (usertype == 1)
			{

				if (userItem.IsFirstTime && RetailerDiscount != null)
				{


					float Percentage = RetailerDiscount.Percentage / 100;
					var price = db.Products.Where(p => p.Status == true && p.IsOnlyForRetailer).Where(p => p.Id == id).Select(s => s).FirstOrDefault();
					float result1 = price.RetailerPrice * Percentage;
					float result2 = price.RetailerPrice - result1;

					var _data = db.Products.Where(p => p.Status == true && p.IsOnlyForRetailer).Where(p => p.Id == id).Select(p => new { p.Category.Name, p.Color, p.Comments, p.Desc, p.Discount, p.Id, p.Main_Image, p.Images, RetailerPrice = result2, ProductName = p.Name, p.Qty, p.Tags, p.Thumbnail, p.TotalVotes }).FirstOrDefault();
					return new
					{
						Data = _data,
						Status = 0
					};
				}


				var data = db.Products.Include("Category").Where(p => p.Status == true && p.IsOnlyForRetailer).Where(p => p.Id == id).Select(p => new { p.Category.Name, p.Color, p.Comments, p.Desc, p.Discount, p.Id, p.Main_Image, p.Images, p.RetailerPrice, ProductName = p.Name, p.ProductPercents, p.Qty, p.Tags, p.Thumbnail, p.TotalVotes }).FirstOrDefault();
				return new
				{
					Data = data,
					Status = 0
				};
			}
			else if (usertype == 2)
			{
				var data = db.Products.Include("Category").Where(p => p.Status == true && p.IsOnlyForMultipation).Where(p => p.Id == id).Select(p => new { p.Category.Name, p.Color, p.Comments, p.Desc, p.Discount, p.Id, p.Main_Image, p.Images, p.MultiplicationBuyerPrice, ProductName = p.Name, p.ProductPercents, p.Qty, p.Tags, p.Thumbnail, p.TotalVotes }).FirstOrDefault();

				return new
				{
					Data = data,
					Status = 0
				};
			}
			return new
			{
				status = 0,
				Message = "خطایی رخ داده"
			};


		}

		[HttpPost]
		[Route("api/Product/MarketerProduct/GetProducts/{page}")]

		public object GetProducts(int page)
		{
			var listItems = new object();

			long MaxPrice = 0;
			long MinPrice = 0;
			int usertype = 0;

			var token = HttpContext.Current.Request.Form["Api_Token"];
			var userItem = db.MarketerUsers.Where(s => s.Api_Token == token).FirstOrDefault();
			try
			{
				if (userItem != null)
				{
					usertype = userItem.Usertype;
				}
				if (usertype == 0)
				{
					var data = db.Products.Include("Category").Where(p => p.Status == true).Select(s => new { s.Qty, s.Name, MarketerPrice = s.MarketerPrice, s.Category, s.Id, s.Color, s.Comments, s.Desc, s.Discount, s.Main_Image, s.Status, s.TotalVotes, s.Thumbnail }).AsQueryable();
					var _data = db.Products.Include("Category").Where(p => p.Status == true).AsQueryable();
					if (HttpContext.Current.Request.Form.AllKeys.Contains("status"))
					{
						bool status = Convert.ToBoolean(HttpContext.Current.Request.Form["status"]);
						if (status)
							data = data.Where(p => p.Qty > 0);
						else
							data = data.Where(p => p.Qty == 0);
					}
					if (HttpContext.Current.Request.Form.AllKeys.Contains("name"))
					{
						string name = HttpContext.Current.Request.Form["name"];
						data = data.Where(p => p.Name.Contains(name));
					}
					if (HttpContext.Current.Request.Form.AllKeys.Contains("minprice"))
					{
						int min = Convert.ToInt32(HttpContext.Current.Request.Form["minprice"]);
						data = data.Where(p => p.MarketerPrice >= min);
					}
					if (HttpContext.Current.Request.Form.AllKeys.Contains("maxprice"))
					{
						int max = Convert.ToInt32(HttpContext.Current.Request.Form["maxprice"]);
						data = data.Where(p => p.MarketerPrice <= max);
					}
					if (HttpContext.Current.Request.Form.AllKeys.Contains("category_id"))
					{
						//List<int> list = new List<int>();
						int temp = Convert.ToInt32(HttpContext.Current.Request.Form["category_id"]);
						//list.Add(temp);
						//var main = db.Categories.Where(p => p.Parent.Id == temp).ToList();
						//for (int i = 0; i < main.Count; i++)
						//{
						//    var id = main[i].Id;
						//    list.AddRange(db.Categories.Where(p => p.Parent.Id == id).Select(p => p.Id).ToList());

						//}
						listItems = data.Where(s => s.Category.Id == temp);
						//_data = _data.Where(p => list.Contains(p.Category.Id));
						//var listMinPrice = db.Products.Where(p => list.Contains(p.Category.Id)).ToList();
						//foreach (var item in listMinPrice)
						//{

						//    if(item.MarketerPrice <= min)
						//    {
						//        min = item.MarketerPrice;
						//    }


						//}     long min = 0;




						//MaxPrice = db.Products.Where(p => list.Contains(p.Category.Id)).Max(p => p.MarketerPrice);
					}
					else
					{
						MinPrice = db.Products.Min(p => p.MarketerPrice);
						MaxPrice = db.Products.Max(p => p.MarketerPrice);
					}

					var res = _data.OrderByDescending(p => p.Id);
					var Result = new PagedItem<Product>(res, "");
					if ((HttpContext.Current.Request.Form.AllKeys.Contains("category_id")))
					{
						return new
						{
							//MinPrice = MinPrice,
							//MaxPrice = MaxPrice,

							Result = listItems

						};



					}

					return new
					{


						MinPrice = MinPrice,
						MaxPrice = MaxPrice,
						TotalPage = Result.TotalPage,
						PageNumber = Result.PageNumber,

						Result = _data.OrderByDescending(x => x.Id).Skip(10 * (page - 1)).Take(10).ToList()

					};




				}


				else if (usertype == 2)
				{

					var data = db.Products.Include("Category").Where(p => p.Status == true).Select(s => new { s.Qty, s.Name, MarketerPrice = s.MultiplicationBuyerPrice, s.Category, s.Id, s.Color, s.Comments, s.Desc, s.Discount, s.Main_Image, s.Status, s.TotalVotes, s.Thumbnail }).AsQueryable();
					var _data = db.Products.Include("Category").Where(p => p.Status == true).AsQueryable();

					if (HttpContext.Current.Request.Form.AllKeys.Contains("status"))
					{
						bool status = Convert.ToBoolean(HttpContext.Current.Request.Form["status"]);
						if (status)
							data = data.Where(p => p.Qty > 0);
						else
							data = data.Where(p => p.Qty == 0);
					}
					if (HttpContext.Current.Request.Form.AllKeys.Contains("name"))
					{
						string name = HttpContext.Current.Request.Form["name"];
						data = data.Where(p => p.Name.Contains(name));
					}
					if (HttpContext.Current.Request.Form.AllKeys.Contains("minprice"))
					{
						int min = Convert.ToInt32(HttpContext.Current.Request.Form["minprice"]);
						data = data.Where(p => p.MarketerPrice >= min);
					}
					if (HttpContext.Current.Request.Form.AllKeys.Contains("maxprice"))
					{
						int max = Convert.ToInt32(HttpContext.Current.Request.Form["maxprice"]);
						data = data.Where(p => p.MarketerPrice <= max);
					}
					if (HttpContext.Current.Request.Form.AllKeys.Contains("category_id"))
					{
						//List<int> list = new List<int>();
						int temp = Convert.ToInt32(HttpContext.Current.Request.Form["category_id"]);
						//list.Add(temp);
						//var main = db.Categories.Where(p => p.Parent.Id == temp).ToList();
						//for (int i = 0; i < main.Count; i++)
						//{
						//    var id = main[i].Id;
						//    list.AddRange(db.Categories.Where(p => p.Parent.Id == id).Select(p => p.Id).ToList());

						//}
						listItems = data.Where(s => s.Category.Id == temp);

						//_data = _data.Where(p => list.Contains(p.Category.Id));
						//MinPrice = db.Products.Where(p => list.Contains(p.Category.Id)).Min(p => p.MultiplicationBuyerPrice);
						//MaxPrice = db.Products.Where(p => list.Contains(p.Category.Id)).Max(p => p.MultiplicationBuyerPrice);
					}
					else
					{
						MinPrice = db.Products.Min(p => p.MultiplicationBuyerPrice);
						MaxPrice = db.Products.Max(p => p.MultiplicationBuyerPrice);
					}

					var res = _data.OrderByDescending(p => p.Id);
					var Result = new PagedItem<Product>(res, "");

					if ((HttpContext.Current.Request.Form.AllKeys.Contains("category_id")))
					{
						return new
						{
							//MinPrice = MinPrice,
							//MaxPrice = MaxPrice,

							Result = listItems

						};



					}
					return new
					{

						MinPrice = MinPrice,
						MaxPrice = MaxPrice,
						TotalPage = Result.TotalPage,
						PageNumber = Result.PageNumber,
						Result = data.OrderByDescending(x => x.Id).Skip(10 * (page - 1)).Take(10).ToList()

					};


				}
				else if (usertype == 1)
				{
					var retailerdiscount = db.RetailerFirstFactorDiscounts.FirstOrDefault();
					var dataForFirstTime = new object();

					if (userItem.IsFirstTime == true && retailerdiscount != null)
					{
						float Percentage = retailerdiscount.Percentage / 100;
						dataForFirstTime = db.Products.Include("Category").Where(p => p.Status == true).Select(s => new { s.Qty, s.Name, MarketerPrice = (s.RetailerPrice) - (s.RetailerPrice * Percentage), s.Category, s.Id, s.Color, s.Comments, s.Desc, s.Discount, s.Main_Image, s.Status, s.TotalVotes, s.Thumbnail }).AsQueryable();
					}

					var data = db.Products.Include("Category").Where(p => p.Status == true).Select(s => new { s.Qty, s.Name, MarketerPrice = s.RetailerPrice, s.Category, s.Id, s.Color, s.Comments, s.Desc, s.Discount, s.Main_Image, s.Status, s.TotalVotes, s.Thumbnail }).AsQueryable();

					var _data = db.Products.Include("Category").Where(p => p.Status == true).AsQueryable();

					if (HttpContext.Current.Request.Form.AllKeys.Contains("status"))
					{
						bool status = Convert.ToBoolean(HttpContext.Current.Request.Form["status"]);
						if (status)
							data = data.Where(p => p.Qty > 0);
						else
							data = data.Where(p => p.Qty == 0);
					}
					if (HttpContext.Current.Request.Form.AllKeys.Contains("name"))
					{
						string name = HttpContext.Current.Request.Form["name"];
						data = data.Where(p => p.Name.Contains(name));
					}
					if (HttpContext.Current.Request.Form.AllKeys.Contains("minprice"))
					{
						int min = Convert.ToInt32(HttpContext.Current.Request.Form["minprice"]);
						data = data.Where(p => p.MarketerPrice >= min);
					}
					if (HttpContext.Current.Request.Form.AllKeys.Contains("maxprice"))
					{
						int max = Convert.ToInt32(HttpContext.Current.Request.Form["maxprice"]);
						data = data.Where(p => p.MarketerPrice <= max);
					}
					if (HttpContext.Current.Request.Form.AllKeys.Contains("category_id"))
					{
						//List<int> list = new List<int>();
						int temp = Convert.ToInt32(HttpContext.Current.Request.Form["category_id"]);
						//list.Add(temp);
						//var main = db.Categories.Where(p => p.Parent.Id == temp).ToList();
						//for (int i = 0; i < main.Count; i++)
						//{
						//    var id = main[i].Id;
						//    list.AddRange(db.Categories.Where(p => p.Parent.Id == id).Select(p => p.Id).ToList());

						//}]

						listItems = data.Where(s => s.Category.Id == temp);


						//_data = _data.Where(p => list.Contains(p.Category.Id));
						//MinPrice = db.Products.Where(p => list.Contains(p.Category.Id)).Min(p => p.RetailerPrice);
						//MaxPrice = db.Products.Where(p => list.Contains(p.Category.Id)).Max(p =>  p.RetailerPrice);
					}
					else
					{
						MinPrice = data.Min(p => p.MarketerPrice);
						MaxPrice = data.Max(p => p.MarketerPrice);
					}

					if ((HttpContext.Current.Request.Form.AllKeys.Contains("category_id")))
					{
						return new
						{
							//MinPrice = MinPrice,
							//MaxPrice = MaxPrice,

							Result = listItems

						};



					}



					var res = _data.OrderByDescending(p => p.Id);
					var Result = new PagedItem<Product>(res, "");

					if (userItem.IsFirstTime && retailerdiscount != null)
					{
						return new
						{
							MinPrice = MinPrice,
							MaxPrice = MaxPrice,
							TotalPage = Result.TotalPage,
							PageNumber = Result.PageNumber,

							Result = dataForFirstTime

						};

					}

					return new
					{

						MinPrice = MinPrice,
						MaxPrice = MaxPrice,
						TotalPage = Result.TotalPage,
						PageNumber = Result.PageNumber,

						Result = data.OrderByDescending(x => x.Id).Skip(10 * (page - 1)).Take(10).ToList()

					};



				}

				return new
				{
					status = 500,
					Message = "خطایی رخ داده"
				};
			}
			catch (Exception e)
			{
				return new { e.Message };

			}

		}

		[HttpPost]
		[Route("api/Product/MarketerProduct/GetProductsLim")]

		public object GetProductsLim()
		{
			long MaxPrice = 0;
			long MinPrice = 0;
			int usertype = 0;

			var token = HttpContext.Current.Request.Form["Api_Token"];
			var userItem = db.MarketerUsers.Where(s => s.Api_Token == token).FirstOrDefault();
			if (userItem != null)
			{
				usertype = userItem.Usertype;
			}
			if (usertype == 0)
			{
				MinPrice = db.Products.Min(p => p.MarketerPrice);
				MaxPrice = db.Products.Max(p => p.MarketerPrice);

			}
			if (usertype == 1)
			{
				MinPrice = db.Products.Min(p => p.MultiplicationBuyerPrice);
				MaxPrice = db.Products.Max(p => p.MultiplicationBuyerPrice);

			}
			if (usertype == 2)
			{
				MinPrice = db.Products.Min(p => p.RetailerPrice);
				MaxPrice = db.Products.Max(p => p.RetailerPrice);

			}

			return new
			{
				MinPrice = MinPrice,
				MaxPrice = MaxPrice,
			};
		}

		[HttpPost]
		[Route("api/Product/MarketerProduct/GetCartSharjs")]

		public object GetCartSharjs()
		{
			var Type =Convert.ToInt32( HttpContext.Current.Request.Form["Type"]);
			
			//if (Type == 0)
			//{
			//	return new
			//	{
			//	statusCode =3 ,
			//	Message = "نوع کارت شارژ خالی است"

			//	};
			//}

			var items = db.CartSharjs.Select(s => new { s.Id,s.Price,s.SharjPrice,s.CartType}).Where(s=>s.CartType == Type).ToList();
			return new
			{
				items = items
			};
		}

		[HttpGet]
		[Route("api/Product/MarketerProduct/GetCartTypeOfSharjs")]

		public object GetCartTypeOfSharjs()
		{
			//var items = db.CartSharjType.Where(from o in CartSharj).ToList();
			var query = from c in db.CartSharjType where (from o in db.CartSharjs select o.CartType).Contains(c.typeCode) select c;
			return query.ToList();
		}

		[HttpPost]
		[Route("api/Product/MarketerProduct/GetProducts2/{page}")]

		public object GetProducts2(int page)
		{
			long MaxPrice = 0;
			long MinPrice = 0;
			int usertype = 0;
			
				var token = HttpContext.Current.Request.Form["Api_Token"];
			var userItem = db.MarketerUsers.Where(s => s.Api_Token == token).FirstOrDefault();
			if (userItem != null)
			{
				usertype = userItem.Usertype;

			}
			if (userItem == null)
			{
				return new { StatusCode = 200, Message = "کاربر مورد نظر یافت نشد!" };

			}
			
				#region ForMarketer
				if (userItem.Usertype == 0)
				{
					var data = db.Products.Include("Category").Where(p => p.Status == true && p.IsOnlyForMarketer).Select(s => new { s.Qty, s.Name, MarketerPrice = s.MarketerPrice, s.Category, s.Id, s.Color, s.Comments, s.Desc, s.Discount, s.Main_Image, s.Status, s.TotalVotes, s.Thumbnail }).AsQueryable();

					if (HttpContext.Current.Request.Form.AllKeys.Contains("status"))
					{
						bool status = Convert.ToBoolean(HttpContext.Current.Request.Form["status"]);
						if (status)
							data = data.Where(p => p.Qty > 0);
						else
							data = data.Where(p => p.Qty == 0);
					}
					if (HttpContext.Current.Request.Form.AllKeys.Contains("name"))
					{
						string name = HttpContext.Current.Request.Form["name"];
						data = data.Where(p => p.Name.Contains(name));
					}
					if (HttpContext.Current.Request.Form.AllKeys.Contains("minprice"))
					{
						int min = Convert.ToInt32(HttpContext.Current.Request.Form["minprice"]);
						data = data.Where(p => p.MarketerPrice >= min);
					}
					if (HttpContext.Current.Request.Form.AllKeys.Contains("maxprice"))
					{
						int max = Convert.ToInt32(HttpContext.Current.Request.Form["maxprice"]);
						data = data.Where(p => p.MarketerPrice <= max);
					}
					if (HttpContext.Current.Request.Form.AllKeys.Contains("category_id"))
					{

						int temp = Convert.ToInt32(HttpContext.Current.Request.Form["category_id"]);
						bool IsExist = db.Products.Any(s => s.CategoryID == temp);

						if (IsExist)
						{
							List<int> list = new List<int>();
							list.Add(temp);
							var main = db.Categories.Where(p => p.Parent.Id == temp).ToList();
							for (int i = 0; i < main.Count; i++)
							{
								var id = main[i].Id;
								list.AddRange(db.Categories.Where(p => p.Parent.Id == id).Select(p => p.Id).ToList());

							}

							data = data.Where(p => list.Contains(p.Category.Id));
							if (list.Count != 0)
							{
								MinPrice = db.Products.Where(p => list.Contains(p.Category.Id)).Min(p => p.MarketerPrice);
								MaxPrice = db.Products.Where(p => list.Contains(p.Category.Id)).Max(p => p.MarketerPrice);
							}
						}

						else
						{

							return new
							{
								MinPrice = 0,
								MaxPrice = 0,

								Result = 0
							};

						}

					}
					else
					{
						MinPrice = db.Products.Min(p => p.MarketerPrice);
						MaxPrice = db.Products.Max(p => p.MarketerPrice);
					}

					var res = data.OrderByDescending(p => p.Id);
					var Result = new PagedItem<object>(res,"");
					return new
					{
						MinPrice = MinPrice,
						MaxPrice = MaxPrice,
						Result.TotalPage,
						PageNumber = page,
						Result.TotalItemCount,

						data = data.OrderByDescending(x => x.Id).Skip(10 * (page - 1)).Take(10).ToList()
					};
				}
				#endregion

				#region ForBigBuyer

				if (userItem.Usertype == 2)
				{
					var data = db.Products.Include("Category").Where(p => p.Status == true && p.IsOnlyForMultipation).Select(s => new { s.Qty, s.Name, MarketerPrice = s.MultiplicationBuyerPrice, s.Category, s.Id, s.Color, s.Comments, s.Desc, s.Discount, s.Main_Image, s.Status, s.TotalVotes, s.Thumbnail }).AsQueryable();

					if (HttpContext.Current.Request.Form.AllKeys.Contains("status"))
					{
						bool status = Convert.ToBoolean(HttpContext.Current.Request.Form["status"]);
						if (status)
							data = data.Where(p => p.Qty > 0);
						else
							data = data.Where(p => p.Qty == 0);
					}
					if (HttpContext.Current.Request.Form.AllKeys.Contains("name"))
					{
						string name = HttpContext.Current.Request.Form["name"];
						data = data.Where(p => p.Name.Contains(name));
					}
					if (HttpContext.Current.Request.Form.AllKeys.Contains("minprice"))
					{
						int min = Convert.ToInt32(HttpContext.Current.Request.Form["minprice"]);
						data = data.Where(p => p.MarketerPrice >= min);
					}
					if (HttpContext.Current.Request.Form.AllKeys.Contains("maxprice"))
					{
						int max = Convert.ToInt32(HttpContext.Current.Request.Form["maxprice"]);
						data = data.Where(p => p.MarketerPrice <= max);
					}
					if (HttpContext.Current.Request.Form.AllKeys.Contains("category_id"))
					{
						int temp = Convert.ToInt32(HttpContext.Current.Request.Form["category_id"]);
						bool IsExist = db.Products.Any(s => s.CategoryID == temp);

						if (IsExist)
						{

							List<int> list = new List<int>();

							list.Add(temp);
							var main = db.Categories.Where(p => p.Parent.Id == temp).ToList();
							for (int i = 0; i < main.Count; i++)
							{
								var id = main[i].Id;
								list.AddRange(db.Categories.Where(p => p.Parent.Id == id).Select(p => p.Id).ToList());

							}

							data = data.Where(p => list.Contains(p.Category.Id));
							MinPrice = db.Products.Where(p => list.Contains(p.Category.Id)).Min(p => p.MarketerPrice);
							MaxPrice = db.Products.Where(p => list.Contains(p.Category.Id)).Max(p => p.MarketerPrice);
						}

						else
						{

							return new
							{
								MinPrice = 0,
								MaxPrice = 0,

								Result = 0
							};

						}


					}
					else
					{
						MinPrice = db.Products.Min(p => p.MarketerPrice);
						MaxPrice = db.Products.Max(p => p.MarketerPrice);
					}

					var res = data.OrderByDescending(p => p.Id);
					var Result = new PagedItem<object>(res, "");
					return new
					{
						MinPrice = MinPrice,
						MaxPrice = MaxPrice,
						Result.TotalPage,
						PageNumber = page,
						Result.TotalItemCount,
						data= data.OrderByDescending(x => x.Id).Skip(10 * (page - 1)).Take(10).ToList()

					};
				}

				#endregion



				#region ForRetailer

				if (userItem.Usertype == 1)
				{
					var data = db.Products.Include("Category").Where(p => p.Status == true && p.IsOnlyForRetailer).Select(s => new { s.Qty, s.Name, MarketerPrice = s.RetailerPrice, s.Category, s.Id, s.Color, s.Comments, s.Desc, s.Discount, s.Main_Image, s.Status, s.TotalVotes, s.Thumbnail }).AsQueryable();

					if (HttpContext.Current.Request.Form.AllKeys.Contains("status"))
					{
						bool status = Convert.ToBoolean(HttpContext.Current.Request.Form["status"]);
						if (status)
							data = data.Where(p => p.Qty > 0);
						else
							data = data.Where(p => p.Qty == 0);
					}
					if (HttpContext.Current.Request.Form.AllKeys.Contains("name"))
					{
						string name = HttpContext.Current.Request.Form["name"];
						data = data.Where(p => p.Name.Contains(name));
					}
					if (HttpContext.Current.Request.Form.AllKeys.Contains("minprice"))
					{
						int min = Convert.ToInt32(HttpContext.Current.Request.Form["minprice"]);
						data = data.Where(p => p.MarketerPrice >= min);
					}
					if (HttpContext.Current.Request.Form.AllKeys.Contains("maxprice"))
					{
						int max = Convert.ToInt32(HttpContext.Current.Request.Form["maxprice"]);
						data = data.Where(p => p.MarketerPrice <= max);
					}
					if (HttpContext.Current.Request.Form.AllKeys.Contains("category_id"))
					{
						int temp = Convert.ToInt32(HttpContext.Current.Request.Form["category_id"]);
						bool IsExist = db.Products.Any(s => s.CategoryID == temp);

						if (IsExist)
						{
							List<int> list = new List<int>();

							list.Add(temp);
							var main = db.Categories.Where(p => p.Parent.Id == temp).ToList();
							for (int i = 0; i < main.Count; i++)
							{
								var id = main[i].Id;
								list.AddRange(db.Categories.Where(p => p.Parent.Id == id).Select(p => p.Id).ToList());
							}

							data = data.Where(p => list.Contains(p.Category.Id));
							MinPrice = db.Products.Where(p => list.Contains(p.Category.Id)).Min(p => p.MarketerPrice);
							MaxPrice = db.Products.Where(p => list.Contains(p.Category.Id)).Max(p => p.MarketerPrice);
						}
						else
						{
							return new
							{
								MinPrice = 0,
								MaxPrice = 0,

								Result = 0
							};
						}
					}
					else
					{
						MinPrice = db.Products.Min(p => p.MarketerPrice);
						MaxPrice = db.Products.Max(p => p.MarketerPrice);
					}

					var res = data.OrderByDescending(p => p.Id);
					var Result = new PagedItem<object>(res, "");
					return new
					{
						MinPrice = MinPrice,
						MaxPrice = MaxPrice,
						Result.TotalPage,
						PageNumber = page,
						Result.TotalItemCount,

						data = data.OrderByDescending(x => x.Id).Skip(10 * (page - 1)).Take(10).ToList()
					};
				}
			

			#endregion

			else
			{
				return new { message = "Error" };
			}


		
				
			



		}

	}

}
