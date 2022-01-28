using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DXTreelist0904 {
    public class RouteConfig {
        public static void RegisterRoutes(RouteCollection routes) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{resource}.ashx/{*pathInfo}");

            routes.MapRoute(
                name: "Admin", // Route name
                url: "{controller}/{action}/{id}", // URL with parameters
                defaults: new { controller = "Admin", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }
    }
}