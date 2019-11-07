using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using WebApplication1.Utility;
using WebApplication1.Models;

namespace WebApplication1
{
    public class Global : HttpApplication
    {
        DBContext db = new DBContext();

        void Application_Start(object sender, EventArgs e)
        {
            //var isExist = db.CompanyAjentProducts.Any();
            //if (isExist)
            //{
            //    var listItems = db.CompanyAjentProducts.ToList();

            //    foreach (var item in listItems)
            //    {
            //        var Productitem = db.Products.FirstOrDefault(s => s.Id == item.ProductID);
            //        if (Productitem != null)
            //        {

            //            Productitem.Qty = item.Quty;
            //            Productitem.Name = Productitem.Name;
            //            Productitem.Desc = Productitem.Desc;
            //            Productitem.Thumbnail = Productitem.Thumbnail;
            //            Productitem.Price = Productitem.Price;
            //            Productitem.MarketerPrice = Productitem.MarketerPrice;
            //            Productitem.MultiplicationBuyerPrice = Productitem.MultiplicationBuyerPrice;
            //            Productitem.RetailerPrice = Productitem.RetailerPrice;
            //            Productitem.Discount = Productitem.Discount;
            //            Productitem.Main_Image = Productitem.Main_Image;
            //            Productitem.Images = Productitem.Images;
            //            Productitem.Like = Productitem.Like;
            //            Productitem.TotalVotes = Productitem.TotalVotes;
            //            Productitem.Status = Productitem.Status;
            //            Productitem.TotalComment = Productitem.TotalComment;
            //            Productitem.Color = Productitem.Color;
            //            Productitem.IsOnlyForMarketer = Productitem.IsOnlyForMarketer;
            //            Productitem.Category.Id =2;
            //            Productitem.CompanyID = Productitem.CompanyID;
            //        }
            //    }
            //    db.SaveChanges();
            //}
            //ProductQutyChanger productQutyChanger = new ProductQutyChanger();
            //productQutyChanger.QutyChanger();

            // Code that runs on application startup
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            GlobalConfiguration.Configuration.IncludeErrorDetailPolicy
= IncludeErrorDetailPolicy.Always;
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;


        }
    }
}