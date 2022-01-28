using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DXTreelist0904.Filters
{
    public class AuthFilter : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Session["OturumYetki"] == null && filterContext.HttpContext.Session["Kullanici"] == null)
            {
                filterContext.Result = new RedirectResult("/Home/Giris");
            }

        }
    }
}