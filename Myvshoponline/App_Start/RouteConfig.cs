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
      routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

      routes.MapRoute(
               name: "Id",
               url: "{id}",
               defaults: new { controller = "Home", action = "ShopDetails", q = UrlParameter.Optional }
           );

      // Default MVC route

      //// Default MVC route with LowerCaseRouteHandler
      //routes.Add(new Route("{controller}/{action}/{id}",
      //    new RouteValueDictionary(new { controller = "Home", action = "Index", id = UrlParameter.Optional }),
      //    new LowerCaseRouteHandler()));

      routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
    }
  }

    public class LowerCaseRouteHandler : MvcRouteHandler
    {
      protected override IHttpHandler GetHttpHandler(RequestContext requestContext)
      {
        requestContext.RouteData.Values["controller"] = requestContext.RouteData.Values["controller"]?.ToString().ToLower();
        requestContext.RouteData.Values["action"] = requestContext.RouteData.Values["action"]?.ToString().ToLower();

        return base.GetHttpHandler(requestContext);
      }
    }

  }
