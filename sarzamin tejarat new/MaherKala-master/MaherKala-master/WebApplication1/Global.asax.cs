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
        ProductQutyChanger QutyChanger = new ProductQutyChanger();
        void Application_Start(object sender, EventArgs e)
        {
          

            QutyChanger.QutyChanger();


            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            GlobalConfiguration.Configuration.IncludeErrorDetailPolicy
= IncludeErrorDetailPolicy.Always;
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;


        }
    }
}