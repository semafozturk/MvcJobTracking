using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DXTreelist0904.Filters
{
        public class AuthFilterYetki : FilterAttribute, IAuthorizationFilter
        {
            public void OnAuthorization(AuthorizationContext filterContext)
            {
            if (filterContext.HttpContext.Session["OturumYetki"].ToString() != null)
            {
                if (filterContext.HttpContext.Session["OturumYetki"].ToString() == "1")
                {
                    filterContext.Result = new RedirectResult("/Admin/Index");
                }
                else if (filterContext.HttpContext.Session["OturumYetki"].ToString() == "0")
                {
                    filterContext.Result = new RedirectResult("/Kullanici/Index");
                }
            }
               
            }
        }
    }