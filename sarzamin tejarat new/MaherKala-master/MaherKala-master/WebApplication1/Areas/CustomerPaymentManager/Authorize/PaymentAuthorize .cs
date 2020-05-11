using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Areas.CustomerPaymentManager.Authorize
{
	public class PaymentAuthorize:AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            // If you are authorized
            if (this.AuthorizeCore(filterContext.HttpContext))
            {
                base.OnAuthorization(filterContext);
            }
            else
            {
                // else redirect to your Area  specific login page
                filterContext.Result = new RedirectResult("~/CustomerPaymentManager/Login/Index");
            }
        }
    }
}