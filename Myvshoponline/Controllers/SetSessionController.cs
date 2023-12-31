using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Myvshoponline.Controllers
{
    public class SetSessionController : Controller
    {
        private MyvshoponlineEntities db = new MyvshoponlineEntities();
        public ActionResult SetVariable(string key, string value)
        {
            int UserID = Convert.ToInt32(value);
            Session["UserID"] = UserID;

            string UserLoggedIn = db.Users.Find(UserID).CompanyName;
            string UserRole = db.Users.Find(UserID).UserRole.Role;
            string Username = db.Users.Find(UserID).Username;

            Session["UserRole"] = UserRole;
            Session["Name"] = UserLoggedIn;
            Session["username"] = Username;
            Session["UserID"] = UserID;

            return this.Json(new { success = true,s= Session["UserID"] });
        }
    
    public JsonResult SetProductSession(string shopid, string productid)
    {
      // Store the product details in TempData
      Session["ProductID"] = productid;
      Session["ShopID"] = shopid;

      var result = new { session = "true" };
      return Json(result, JsonRequestBehavior.AllowGet);
    }

    public void SetObjectSession(string key, string value)
    {
      Session[key] = value;
    }

    public JsonResult SetCartSession(string shopid, string productid,string dcity,string qty, string negotiate)
    {
      // Store the product details in TempData
      Session["CartProductID"] = productid;
      Session["CartShopID"] = shopid;
      Session["dcity"] = dcity;
      Session["qty"] = qty;
      Session["Cart"] = "cart";
      Session["Negotiate"] = negotiate;
      var result = new { session = "true" };
      return Json(result, JsonRequestBehavior.AllowGet);
    }
    //public void SetObjectSession(string key, string value)
    //{
    //  // Ensure that the HttpContext is available
    //  if (HttpContext != null)
    //  {
    //    // Set the value in the session
    //    HttpContext.Session[key] = value;

    //    // Ensure that the session is made active immediately
    //    HttpContext.Session.Timeout = 1; // Set the session timeout to a short duration (in minutes)
    //                                     // Renew the session by updating the session timestamp
    //  }
    //  else
    //  {
    //    // Handle the case where HttpContext is not available (not in a web context)
    //    // You might want to log an error or take appropriate action.
    //  }
    //}

    //public static void generatephone()
    //{
    //        string ph1 = "080";
    //        string ph2 = "070";
    //        string ph3 = "081";
    //        Random r = new Random();
    //        //=== GENERATE
    //        Random random = new Random();
    //    int[] intArr = new int[100];
    //    for (int i=1; i<intArr.Length;i++)
    //    {
    //        int num= random.Next(99999, 99999999);
    //        intArr[i] = num;
    //        Console.WriteLine(num);
    //    }
    //    Console.WriteLine();
    //}
  }
}
