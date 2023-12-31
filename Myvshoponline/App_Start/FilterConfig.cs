using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Myvshoponline
{
  public class FilterConfig
  {
    public static void RegisterGlobalFilters(GlobalFilterCollection filters)
    {
      //Register GUID here
      //filters.Add(new  AddGuidToUrlFilter());
      //Filter for internal redirection
      //filters.Add(new InternalRedirectActionFilter());
      filters.Add(new HandleErrorAttribute());

    }

  }

  //Internally redirect to each controller actions
  public class InternalRedirectActionFilter : IAuthorizationFilter
  {
    public void OnAuthorization(AuthorizationContext filterContext)
    {

      // Logic to handle internal redirection
      string controllerName = filterContext.RouteData.Values["controller"].ToString();
      string actionName = filterContext.RouteData.Values["action"].ToString();

      // Construct a new URL without controller and action names
      string rewrittenUrl = "~/" + controllerName + "/" + actionName;

      // Perform internal redirection with the rewritten URL
      filterContext.Result = new RedirectResult(rewrittenUrl);
    }

    public class AddGuidToUrlFilter : ActionFilterAttribute
    {
      public const string ModificationKey = "UrlModificationApplied";

      public override void OnActionExecuting(ActionExecutingContext filterContext)
      {
        // Check if the modification has already been applied globally
        if (HttpContext.Current.Session[ModificationKey] == null)
        {
          // Generate a new random number
          string randomNumber = Guid.NewGuid().ToString();

          // Append the random number to the URL
          string modifiedUrl = filterContext.HttpContext.Request.Url.ToString() + "?" + randomNumber;

          // Redirect to the modified URL
          filterContext.Result = new RedirectResult(modifiedUrl);

          // Set a session variable to indicate that the modification has been applied globally
          HttpContext.Current.Session[ModificationKey] = true;
        }
      }
    }

  }
}
