using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ProductsController : Controller
    {
        DBContext db = new DBContext();
        // GET: Product
        public ActionResult Index()
        {
            int id = Convert.ToInt32(Request["Id"]);
            var data = db.Products.Include("Category").Where(p => p.IsOnlyForMarketer == false).Where(p => p.Id == id).Where(p => p.Status == true).FirstOrDefault();
            var comment = db.Comments.Include("User").Where(p => p.Product.Id == id).Where(p => p.Parent_id == null).OrderByDescending(p => p.Id);
            ViewBag.Product = data;
            ViewBag.comments = new PagedItem<Comment>(comment, "/Products/Index?Id=" + id);

            ViewBag.Random = db.Products.Where(p => p.Category.Id == data.Category.Id).Where(p => p.Id != data.Id).Where(p => p.Status == true).Where(p=>p.IsOnlyForMarketer==false).Where(p => p.Qty > 0).OrderBy(p => Guid.NewGuid()).Take(10).ToList();

            return View();
        }

        [HttpGet]
        public ActionResult Search()
        {
            string name = Request["Name"];
            var res = db.Products.Where(p => p.IsOnlyForMarketer == false).Where(p => p.Name.Contains(name)).OrderByDescending(p => p.Id);
            var data = new PagedItem<Product>(res, "/Products/Search?Name=" + name, 16);
            ViewBag.Data = data;

            return View("Search");
        }
        [HttpGet]
        [Authorize(Roles = "Member")]
        public ActionResult Like()
        {
            var Product_Id = Convert.ToInt32(Request["Product_Id"]);
            var Vote = Convert.ToInt32(Request["Vote"]);
            if (Vote < 1 || Vote > 5)
            {
                throw new Exception();
            }
            var Url = (Request["Url"]);
            string path = Url.Substring(0, Url.IndexOf("?"));
            Url = path + "?Id=" + Product_Id;

            var user = db.Users.Include("Role").Where(p => p.Email == User.Identity.Name).FirstOrDefault();

            if (db.UserProducts.Any(p => p.Product.Id == Product_Id && p.User.Id == user.Id&&p.Product.IsOnlyForMarketer==false))
            {
                return Redirect(Url + "&message=2");
            }

            var Product = db.Products.Include("Category").Where(p => p.IsOnlyForMarketer == false).Where(p => p.Id == Product_Id).FirstOrDefault();
            Product.Like += Vote;
            Product.TotalVotes += 5;
            var data = new UserProduct();
            data.Product = Product;
            data.User = user;
            db.UserProducts.Add(data);
            db.SaveChanges();
            return Redirect(Url + "&message=0");

        }

        [Authorize(Roles = "Member")]
        [HttpGet]
        public ActionResult CommentLike()
        {
            var Comment_Id = Convert.ToInt32(Request["Comment_Id"]);
            var url = "/Products/Index?Id=" + Convert.ToInt32(Request["Product_Id"]);
            var user = db.Users.Where(p => p.Email == User.Identity.Name).FirstOrDefault();
            if (db.UserComments.Any(p => p.Comment.Id == Comment_Id && p.User.Id == user.Id))
            {
                return Redirect(url + "&message=2");
            }

            var comment = db.Comments.Find(Comment_Id);
            comment.Like++;
            var data = new UserComment();
            data.Comment = comment;
            data.User = user;
            db.UserComments.Add(data);
            db.SaveChanges();
            return Redirect(url + "&message=0");
        }
        [Authorize(Roles = "Member")]
        [HttpGet]
        public ActionResult Dislike()
        {
            var url = "/Products/Index?Id=" + Convert.ToInt32(Request["Product_Id"]);
            var Comment_Id = Convert.ToInt32(Request["Comment_Id"]);

            var user = db.Users.Where(p => p.Email == User.Identity.Name).FirstOrDefault();
            if (db.UserComments.Any(p => p.Comment.Id == Comment_Id && p.User.Id == user.Id))
            {
                return Redirect(url + "&message=2");
            }

            var comment = db.Comments.Find(Comment_Id);
            comment.Dislike++;
            var data = new UserComment();
            data.Comment = comment;
            data.User = user;
            db.UserComments.Add(data);
            db.SaveChanges();
            return Redirect(url + "&message=0");

        }
        [HttpPost]
        [Authorize(Roles = "Member")]
        public ActionResult RegisterComment()
        {
            var url = "/Products/Index?Id=" + Convert.ToInt32(Request["Product_Id"]);
            var Product_Id = Convert.ToInt32(Request["Product_Id"]);
            var user = db.Users.Include("Role").Where(p => p.Email == User.Identity.Name).FirstOrDefault();
            var Product = db.Products.Include("Category").Where(p => p.IsOnlyForMarketer == false).Where(p => p.Id == Product_Id).FirstOrDefault();
            var Title = Request["Title"].Trim();
            var Text = Request["Text"].Trim();
            if (Title.Length == 0)
            {
                return Redirect(url + "&message=51");
            }
            if (Title.Length > 20)
            {
                return Redirect(url + "&message=52");
            }
            if (Text.Length == 0)
            {
                return Redirect(url + "&message=53");
            }
            if (Text.Length > 200)
            {
                return Redirect(url + "&message=54");
            }
            string Parent_Id = Request["Parent_Id"];

            var comment = new Comment();
            comment.Status = false;
            comment.Product = Product;
            comment.Text = Text;
            comment.Title = Title;
            comment.User = user;
            comment.Date = DateTime.Now;
            if (Parent_Id == null)
                comment.Parent_id = null;
            else
            {
                int tmp = Convert.ToInt32(Parent_Id);
                if (db.Comments.Any(p => p.Status == true && p.Id == tmp))
                {
                    comment.Parent_id = Convert.ToInt32(Parent_Id);
                }
                else
                {
                    comment.Parent_id = null;
                }

            }

            Product.TotalComment++;
            db.Comments.Add(comment);
            db.SaveChanges();

            return Redirect(url + "&message=100");
        }

        [HttpGet]
        public ActionResult Compare()
        {
            var first = Convert.ToInt32(Request["First"]);
            var secound = Request["Secound"];
            var name = Request["Name"];
            var res1 = db.Products.AsQueryable();
            var res = res1.Where(p => p.IsOnlyForMarketer == false).Where(p => p.Name.Contains(name)).OrderByDescending(p => p.Id);
            var data = new PagedItem<Product>(res, "/Products/Compare?Name=" + name + "&First=" + first, 6);
            ViewBag.Data = data;

            var element1 = db.Products.Include("Category").Where(p => p.Id == first).FirstOrDefault();
            ViewBag.Product1 = element1;
            ViewBag.Product2 = null;
            if (secound != null && secound != "")
            {
                int id = Convert.ToInt32(secound);
                var element2 = db.Products.Where(p => p.Id == id).FirstOrDefault();
                ViewBag.Product2 = element2;
            }
            else
            {
                var expect = db.Products.Include("Category").Where(p => p.Id == first);
                var list = db.Products.Where(p => p.Category.Id == element1.Category.Id).Except(expect).OrderByDescending(p => p.Id);
                var d = new PagedItem<Product>(list, "/Products/Compare?&First=" + first, 6);
                ViewBag.Data = d;
            }

            return View();
        }


    }
}