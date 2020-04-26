using System.Web.Mvc;

namespace WebApplication1.Areas.CustomerPaymentManager
{
    public class CustomerPaymentManagerAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "CustomerPaymentManager";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "CustomerPaymentManager_default",
                "CustomerPaymentManager/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}