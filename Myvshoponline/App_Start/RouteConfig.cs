using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Myvshoponline
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
               name: "Id",
               url: "{id}",
               defaults: new { controller = "Home", action = "ShopDetails", q = UrlParameter.Optional }
           );
          //  routes.MapRoute(
          //    name: "Product",
          //    url: "{id}/{pid}",
          //    defaults: new { controller = "Home", action = "ProductDetails" }
          //);
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );


           
        }
    }
}
