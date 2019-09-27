using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.Models;
using PagedList;
using System.Web;
using System.Data.Entity.Validation;

namespace WebApplication1.Controllers.api
{
    public class ProductController : ApiController
    {
        DBContext db = new DBContext();
        [HttpGet]
        public object GetProduct(int id)
        {
            
            var data = db.Products.Include("Category").Where(p=>p.IsOnlyForMarketer==false).Where(p=>p.Status==true).Where(p=>p.Id==id).FirstOrDefault();
            return new {
                Data =data,
                Status=0
            };
        }

        [HttpPost]
        public PagedItem<Product> GetProducts()
        {
            var data = db.Products.Include("Category").Where(p => p.IsOnlyForMarketer == false).Where(p => p.Status == true).AsQueryable();
            if(HttpContext.Current.Request.Form.AllKeys.Contains("status"))
            {
                bool status = Convert.ToBoolean(HttpContext.Current.Request.Form["status"]);
                if(status)
                    data = data.Where(p=>p.Qty>0);
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
                data = data.Where(p=>p.Price>=min);
            }
            if (HttpContext.Current.Request.Form.AllKeys.Contains("maxprice"))
            {
                int max = Convert.ToInt32(HttpContext.Current.Request.Form["maxprice"]);
                data = data.Where(p=>p.Price<=max);
            }
            if (HttpContext.Current.Request.Form.AllKeys.Contains("category_id"))
            {
                List<int> list = new List<int>();
                int temp = Convert.ToInt32(HttpContext.Current.Request.Form["category_id"]);
                list.Add(temp);
                var main = db.Categories.Where(p => p.Parent.Id == temp).ToList();
                for (int i = 0; i < main.Count; i++)
                {
                    var id = main[i].Id;
                    list.AddRange(db.Categories.Where(p => p.Parent.Id == id).Select(p => p.Id).ToList());

                }

                data = data.Where(p => list.Contains(p.Category.Id));
            }

            var result = data.OrderByDescending(p => p.Id);
            return new PagedItem<Product>(result,"");
        }

        [HttpPost]
        [Route("api/Product/NewProduct/GetNewest")]
        public object GetNewestProduct()
        {
            return new {
                Data = db.Products.Include("Category").Where(p => p.IsOnlyForMarketer == false).Where(p => p.Status == true).OrderByDescending(p => p.Id).Take(10).ToList(),
                Status=0
            };
        }

        [HttpPost]
        [Route("api/Product/Special/GetProducts")]
        public object GetSpecialProduct()
        {
            return new {
                Data = db.SpecialProducts.Include("Product").Include("Product.Category").Where(p => p.Product.IsOnlyForMarketer == false).Where(p=>p.ExpireDate>DateTime.Now).Where(p => p.Product.Status == true).Where(p => p.ExpireDate > DateTime.Now).OrderByDescending(p => p.Id).Take(10).ToList(),
                CurrentDate = DateTime.Now.Date,
                Status=0
            };
        }
        [HttpPost]
        [Route("api/Product/Like")]
        public object Like()
        {
            var Product_Id = Convert.ToInt32(HttpContext.Current.Request.Form["Product_Id"]);
            var Vote= Convert.ToInt32(HttpContext.Current.Request.Form["Vote"]);
            if(Vote<1 || Vote>5)
            {
                throw new DbEntityValidationException("خطا در دریافت رای");
            }
            var Token = HttpContext.Current.Request.Form["Api_Token"];
            var user = db.Users.Include("Role").Where(p => p.Api_Token == Token).FirstOrDefault();
            if (db.UserProducts.Any(p => p.Product.Id == Product_Id && p.User.Id == user.Id))
            {
                return new {
                    Message = 1
                };
            }

            var Product = db.Products.Include("Category").Where(p => p.IsOnlyForMarketer == false).Where(p=>p.Id==Product_Id).FirstOrDefault();
            Product.Like+=Vote;
            Product.TotalVotes += 5;
            var data = new UserProduct();
            data.Product = Product;
            data.User = user;
            db.UserProducts.Add(data);
            db.SaveChanges();
            return new { Message=0};

        }

        [HttpPost]
        [Route("api/Comment/Like")]
        public object CommentLike()
        {
            var Comment_Id = Convert.ToInt32(HttpContext.Current.Request.Form["Comment_Id"]);
            var Token = HttpContext.Current.Request.Form["Api_Token"];
            var user = db.Users.Where(p => p.Api_Token == Token).FirstOrDefault();
            if (db.UserComments.Any(p => p.Comment.Id == Comment_Id && p.User.Id == user.Id))
            {
                return new
                {
                    Message = 1
                };
            }

            var comment = db.Comments.Find(Comment_Id);
            if(comment.Status==true)
            {

            comment.Like++;
            var data = new UserComment();
            data.Comment = comment;
            data.User = user;
            db.UserComments.Add(data);
            db.SaveChanges();
            }

            return new { Message = 0 };
        }
        [HttpPost]
        [Route("api/Comment/Dislike")]
        public object Dislike()
        {
            var Comment_Id = Convert.ToInt32(HttpContext.Current.Request.Form["Comment_Id"]);
            var Token = HttpContext.Current.Request.Form["Api_Token"];
            var user = db.Users.Where(p => p.Api_Token == Token).FirstOrDefault();
            if (db.UserComments.Any(p => p.Comment.Id == Comment_Id && p.User.Id == user.Id))
            {
                return new
                {
                    Message = 1
                };
            }

            var comment = db.Comments.Find(Comment_Id);
            if (comment.Status == true)
            {
                comment.Dislike++;
                var data = new UserComment();
                data.Comment = comment;
                data.User = user;
                db.UserComments.Add(data);
                db.SaveChanges();
            }
            return new { Message = 0 };

        }
        [HttpPost]
        [Route("api/Product/RegisterComment")]
        public object RegisterComment()
        {
            var Product_Id = Convert.ToInt32(HttpContext.Current.Request.Form["Product_Id"]);
            var Token = HttpContext.Current.Request.Form["Api_Token"];
            var user = db.Users.Include("Role").Where(p => p.Api_Token == Token).FirstOrDefault();
            var Product = db.Products.Include("Category").Where(p => p.IsOnlyForMarketer == false).Where(p=>p.Id ==Product_Id).FirstOrDefault();
            var Title = (HttpContext.Current.Request.Form["Title"]);
            var Text = (HttpContext.Current.Request.Form["Text"]);

            var comment = new Comment();
            comment.Product = Product; ;
            comment.Status = false;
            comment.Text = Text;
            comment.Title = Title;
            comment.User = user;
            comment.Date = DateTime.Now;
            Product.TotalComment++;
            db.Comments.Add(comment);
            db.SaveChanges();

            return new {
                Message=0
            };
        }
        [HttpPost]
        [Route("api/Product/ShowComment")]
        public object ShowComment()
        {
            var Product_Id = Convert.ToInt32(HttpContext.Current.Request.Form["Product_Id"]);
            var Comments= db.Comments.Include("User").Where(p => p.Product.Id == Product_Id).Where(p => p.Product.IsOnlyForMarketer == false).Select(p=>new  { p.Date,p.Dislike,p.Id,p.Like,p.Status,p.Text,p.Title,p.User.Fullname}).OrderByDescending(p=>p.Id);
            var data = new PagedItem<object>(Comments, "");
            return data;

        }
        [HttpPost]
        [Route("api/Product/DynamicCategories")]
        public object getDynamic()
        {
            var c = db.Settings.First();
            var First = db.Categories.Find(c.FirstCategory);
            var Secound= db.Categories.Find(c.SecoundCategory);

            var f = db.Products.Where(p => p.IsOnlyForMarketer == false).Where(p => p.Status == true).Where(p => p.Category.Id == First.Id).OrderByDescending(p => p.Id).Take(12).ToList();
            var s = db.Products.Where(p => p.IsOnlyForMarketer == false).Where(p => p.Status == true).Where(p => p.Category.Id == Secound.Id).OrderByDescending(p => p.Id).Take(12).ToList();
            return new {
                FirstCategory = First,
                FirstProducts = f,
                SecoundCategory = Secound,
                SecoundProducts = s
            };

        }

    }
}

