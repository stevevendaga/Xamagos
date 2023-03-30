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