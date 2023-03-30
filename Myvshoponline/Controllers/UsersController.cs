using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Myvshoponline;
using System.IO;
using System.Text;
using System.Net.Mail;
using System.Drawing;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Web.Helpers;

namespace Myvshoponline.Controllers
{
    public class UsersController : Controller
    {
        private MyvshoponlineEntities db = new MyvshoponlineEntities();
        Getdata mydata = new Getdata();
        // GET: Users
        public ActionResult Index()
        {
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                var users = db.Users.Include(u => u.UserRole);
                return View(users.OrderByDescending(p => p.DateCreated).ToList());
            }
            return Redirect("~/Home/AccessDenied");
        }

        public ActionResult UpgradedUsers()
        {
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                var users = db.Users.Include(u => u.UserRole);
                return View(users.Where(p => p.PaymentStatus == 1).OrderByDescending(p => p.DatePaid).ToList());
            }
            return Redirect("~/Home/AccessDenied");
        }
        public ActionResult BusinessLanding(int? id)
        {
            //EXPIRY CHECK
            //if (count == 1)
            //{
            //    int lYear = (int)db.Registrations.Where(k => k.ID == id).Select(k => k.LaunchYear).FirstOrDefault();
            //    int lmonth = (int)db.Registrations.Where(k => k.ID == id).Select(k => k.LaunchMonth).FirstOrDefault();
            //    int lday = (int)db.Registrations.Where(k => k.ID == id).Select(k => k.LaunchDay).FirstOrDefault();

            //    int pYear = (int)db.Registrations.Where(k => k.ID == id).Select(k => k.PreviousYear).FirstOrDefault();
            //    int pmonth = (int)db.Registrations.Where(k => k.ID == id).Select(k => k.PreviousMonth).FirstOrDefault();
            //    int pday = (int)db.Registrations.Where(k => k.ID == id).Select(k => k.PreviousDay).FirstOrDefault();

            //    DateTime lauchd = new DateTime(lYear, lmonth, lday);
            //    DateTime prevd = new DateTime(pYear, pmonth, pday);
            //    TimeSpan diff = lauchd.Subtract(prevd);
            //    ViewBag.diff = diff.Days;
            //    ViewBag.FixedDays = db.Registrations.Find(id).FixedDays;
            //    ViewBag.Ndays = db.Registrations.Find(id).NDays;

            //    if (ViewBag.diff > ViewBag.FixedDays)
            //    {

            //    }
            //    else
            //    {
            //        var r = db.Set<Registration>().Find(id);
            //        r.NDays = ViewBag.diff;
            //        r.LaunchDate = DateTime.Now;
            //        r.LaunchYear = DateTime.Now.Year;
            //        r.LaunchMonth = DateTime.Now.Month;
            //        r.LaunchDay = DateTime.Now.Day;
            //        db.SaveChanges();
            //    }
            //}


            if (id != null && db.Users.Where(s => s.ID == id).Count() == 1)
            {
                // int UserID = Convert.ToInt32(mydata.DecodeFrom64(""+id));
                ViewBag.s = db.Shops.Where(s => s.UserID == id);
                ViewBag.ShopID = new SelectList(ViewBag.s, "ID", "Name");
                int UserID = (int)id;
                ViewBag.CompanyID = id;
                string Name = (string)Session["Name"];

                ViewBag.CompanyName = db.Users.Where(u => u.ID == UserID).Select(u => u.CompanyName).FirstOrDefault();
                ViewBag.Shops = db.Shops.Where(s => s.UserID == UserID && s.ShopStatus == "Active").OrderByDescending(s => s.DateCreated).ToList();
                ViewBag.ShopsAdmin = db.Shops.Where(s => s.UserID == UserID).OrderByDescending(s => s.DateCreated).ToList();
                var user = db.Users.Where(c => c.ID == UserID).ToList();
                return View(user);
            }
            else
            {
                return Redirect("~/Home/AccessDenied");
            }

        }

        public ActionResult AdminLanding(int? id)
        {
            //EXPIRY CHECK
            //if (count == 1)
            //{
            //    int lYear = (int)db.Registrations.Where(k => k.ID == id).Select(k => k.LaunchYear).FirstOrDefault();
            //    int lmonth = (int)db.Registrations.Where(k => k.ID == id).Select(k => k.LaunchMonth).FirstOrDefault();
            //    int lday = (int)db.Registrations.Where(k => k.ID == id).Select(k => k.LaunchDay).FirstOrDefault();

            //    int pYear = (int)db.Registrations.Where(k => k.ID == id).Select(k => k.PreviousYear).FirstOrDefault();
            //    int pmonth = (int)db.Registrations.Where(k => k.ID == id).Select(k => k.PreviousMonth).FirstOrDefault();
            //    int pday = (int)db.Registrations.Where(k => k.ID == id).Select(k => k.PreviousDay).FirstOrDefault();

            //    DateTime lauchd = new DateTime(lYear, lmonth, lday);
            //    DateTime prevd = new DateTime(pYear, pmonth, pday);
            //    TimeSpan diff = lauchd.Subtract(prevd);
            //    ViewBag.diff = diff.Days;
            //    ViewBag.FixedDays = db.Registrations.Find(id).FixedDays;
            //    ViewBag.Ndays = db.Registrations.Find(id).NDays;

            //    if (ViewBag.diff > ViewBag.FixedDays)
            //    {

            //    }
            //    else
            //    {
            //        var r = db.Set<Registration>().Find(id);
            //        r.NDays = ViewBag.diff;
            //        r.LaunchDate = DateTime.Now;
            //        r.LaunchYear = DateTime.Now.Year;
            //        r.LaunchMonth = DateTime.Now.Month;
            //        r.LaunchDay = DateTime.Now.Day;
            //        db.SaveChanges();
            //    }
            //}


            if (id != null)
            {
                // int UserID = Convert.ToInt32(mydata.DecodeFrom64(""+id));
                ViewBag.s = db.Shops.Where(s => s.UserID == id);
                ViewBag.ShopID = new SelectList(ViewBag.s, "ID", "Name");
                int UserID = (int)id;
                ViewBag.CompanyID = id;
                string Name = (string)Session["Name"];
                ViewBag.CompanyName = db.Users.Where(u => u.ID == UserID).Select(u => u.CompanyName).FirstOrDefault();
                ViewBag.Shops = db.Shops.Where(s => s.UserID == UserID && s.ShopStatus == "Active").OrderByDescending(s => s.DateCreated).ToList();
                ViewBag.ShopsAdmin = db.Shops.Where(s => s.UserID == UserID).OrderByDescending(s => s.DateCreated).ToList();
                var user = db.Users.Where(c => c.ID == UserID).ToList();
                if (user.Count() == 0)
                {
                    return Redirect("~/Home/AccessDenied");
                }
                return View(user);
            }
            else
            {
                return Redirect("~/Home/AccessDenied");
            }

        }
        public ActionResult BuyPlan(int? id)
        {
            //EXPIRY CHECK
            //int count = (int)db.Registrations.Where(c => c.ID == id).Select(c => c.PaymentStatus).FirstOrDefault();
            //if (count == 1)
            //{
            //    int lYear = (int)db.Registrations.Where(k => k.ID == id).Select(k => k.LaunchYear).FirstOrDefault();
            //    int lmonth = (int)db.Registrations.Where(k => k.ID == id).Select(k => k.LaunchMonth).FirstOrDefault();
            //    int lday = (int)db.Registrations.Where(k => k.ID == id).Select(k => k.LaunchDay).FirstOrDefault();

            //    int pYear = (int)db.Registrations.Where(k => k.ID == id).Select(k => k.PreviousYear).FirstOrDefault();
            //    int pmonth = (int)db.Registrations.Where(k => k.ID == id).Select(k => k.PreviousMonth).FirstOrDefault();
            //    int pday = (int)db.Registrations.Where(k => k.ID == id).Select(k => k.PreviousDay).FirstOrDefault();

            //    DateTime lauchd = new DateTime(lYear, lmonth, lday);
            //    DateTime prevd = new DateTime(pYear, pmonth, pday);
            //    TimeSpan diff = lauchd.Subtract(prevd);
            //    ViewBag.diff = diff.Days;
            //    ViewBag.FixedDays = db.Registrations.Find(id).FixedDays;
            //    ViewBag.Ndays = db.Registrations.Find(id).NDays;

            //    if (ViewBag.diff > ViewBag.FixedDays)
            //    {

            //    }
            //    else
            //    {
            //        var r = db.Set<Registration>().Find(id);
            //        r.NDays = ViewBag.diff;
            //        r.LaunchDate = DateTime.Now;
            //        r.LaunchYear = DateTime.Now.Year;
            //        r.LaunchMonth = DateTime.Now.Month;
            //        r.LaunchDay = DateTime.Now.Day;
            //        db.SaveChanges();
            //    }
            //}

            //ViewBag.Regid = id;
            //var registrations = db.Registrations.Where(c => c.ID == id);
            //ViewBag.TotalCandidates = db.Registrations.Count();
            //ViewBag.PaymentStatus = db.Registrations.Where(c => c.ID == id).Select(c => c.PaymentStatus).FirstOrDefault();
            //ViewBag.RegSubjects = db.RegisteredSubjects.Where(c => c.RegistrationID == id).ToList();
            //ViewBag.RegSubjectsCount = db.RegisteredSubjects.Where(c => c.RegistrationID == id).Count();
            //ViewBag.SubjectExistCount = db.RegisteredSubjects.Where(c => c.RegistrationID == id).Select(c => c.SubjectID).Count();
            //return View(registrations.ToList());
            if (string.IsNullOrEmpty((string)Session["username"]))
            {
                return Redirect("~/Home/AccessDenied");
            }
            if (id != null)
            {
                ViewBag.p = db.PricingPlans.Where(p => p.PlanName != "FREE PLAN");
                ViewBag.PlanID = new SelectList(ViewBag.p, "ID", "PlanName", id);
                ViewBag.b = db.BillingCycles.Where(p => p.Cycle != "Free");
                ViewBag.BillingID = new SelectList(ViewBag.b, "ID", "Cycle");
                int UserID = Convert.ToInt32(Session["UserID"]);
                ViewBag.CompanyName = db.Users.Where(u => u.ID == UserID).Select(u => u.CompanyName).FirstOrDefault();
                PricingPlan plan = db.PricingPlans.Find(id);
                return View(plan);
            }
            else
            {
                return Redirect("~/Home/AccessDenied");
            }
        }
        public ActionResult CustomerDashboard(int? id)
        {
            if (string.IsNullOrEmpty((string)Session["Name"]))
            {
                return Redirect("~/Home/AccessDenied");
            }
            if (id != null)
            {
                // int UserID = Convert.ToInt32(mydata.DecodeFrom64(""+id));
                //ViewBag.s = db.CustomerShops.Where(s => s.ShopCustomerID == id);
                //ViewBag.ShopID = new SelectList(ViewBag.s, "ID", "Name");
                int UserID = (int)id;
                ViewBag.CompanyID = id;
                //ViewBag.Orders = db.Orders.Where(o => o.ShopCustomerID == id && o.OnCart == "No").Count();
                //string Name = (string)Session["CustomerName"];
                //ViewBag.CompanyName = db.ShopCustomers.Where(u => u.ID == UserID).Select(u => u.Name).FirstOrDefault();

                //ViewBag.Shops = db.CustomerShops.Where(s => s.ShopCustomerID == UserID).Select(s=>s.Shop).ToList();
                //var user = db.ShopCustomers.Where(c => c.ID == UserID).ToList();
                //if (user.Count() == 0)
                //{
                //    return Redirect("~/Home/AccessDenied");
                //}

                ViewBag.Orders = db.Orders.Where(o => o.UserID == id && o.OnCart == "No").Count();
                string Name = (string)Session["Name"];
                ViewBag.CompanyName = db.Users.Where(u => u.ID == UserID).Select(u => u.CompanyName).FirstOrDefault();

                ViewBag.Shops = db.CustomerShops.Where(s => s.UserID == UserID).Select(s => s.Shop).ToList();
                var user = db.Users.Where(c => c.ID == UserID).ToList();
                if (user.Count() == 0)
                {
                    return Redirect("~/Home/AccessDenied");
                }
                return View(user);
            }
            else
            {
                return Redirect("~/Home/AccessDenied");
            }

        }
        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                if (id == null)
                {
                    return Redirect("~/Home/AccessDenied");
                }
                User user = db.Users.Find(id);
                if (user == null)
                {
                    return Redirect("~/Home/AccessDenied");
                }
                return View(user);
            }
            return Redirect("~/Home/AccessDenied");
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            ViewBag.UserRoleID = new SelectList(db.UserRoles, "ID", "Role");
            ViewBag.SexID = new SelectList(db.Sexes, "Sex1", "Sex1");
            ViewBag.StateID = new SelectList(db.States, "Name", "Name");
            return View();
        }
        public ActionResult VerificationLinkSent(int? id)
        {
            if (id != null)
            {
                ViewBag.email = db.Users.Find(id).Email;
                ViewBag.vid = id;
                return View();
            }
            return View();
        }


        public JsonResult Verify_RegisterOTP(int otp_register, int UserID)
        {
            if (Convert.ToInt32(Session["OTP_Register"]) == otp_register)
            {
                var user = db.Set<User>().Find(UserID);
                user.EmailVerify = 1;
                db.SaveChanges();
                string UserLoggedIn = db.Users.Find(UserID).CompanyName;
                string UserRole = db.Users.Find(UserID).UserRole.Role;
                Session["UserRole"] = UserRole;
                Session["Name"] = UserLoggedIn;
                Session["username"] = user.Username;
                Session["UserID"] = UserID;
                //==================SEND MAIL============================//
                string title = "Xamagos welcomes you";
                string msg = "<a href='https://xamagos.com' title='Xamagos'> <img src='https://xamagos.com/Images/logosquare.png' style='width: 160px; height: 50px;background-color:#17A2B8' /></a>" + "<hr><strong>" + " Hello " + user.CompanyName + "</strong><br><br>";
                msg += "Congratulations! Your account has been created successfully.";
                msg += "<br>Enjoy the world of Xamagos.";
                msg += "Best Regards:  Xamagos" + "<br>" + "Email: support@xamagos.com";
                mydata.SendMail(user.Email, title, msg);
                //==================SEND SMS=========================//
                //string sms ="Market Square247 welcomes you.Hello " + user.CompanyName + ", your account has been created successfully.";
                string sms = "Xamagos welcomes you. Your account has been created successfully. Enjoy the world of Xamagos";
                //sms+= "Email:info@marketsqaure247.com";
                mydata.Send_SMS_KudiSMS(user.PhoneNumber, sms, "XAMAGOS", DateTime.Now);
                //==================END SEND SMS=========================//

                //string title = "marketsquare247.com welcomes you!";
                //string msg = "<a href='https://marketsquare247.com' title='Market Square247'> <img src='https://marketsquare247.com/Images/logosquare.png' style='width: 100px; height: 100px' /></a>" + "<hr>" + " Hi " + user.CompanyName + "<br>" +
                //"Congratulations!" + "<br/><br>" +
                //"Your account has been created successfully. <hr>" +
                //"Do you know that you can advertize your products on social media using MarketSquare247?" +
                //HttpUtility.HtmlDecode(db.EmailMessages.Where(s => s.Subject == "Advertize your products/services on social media using MarketSquare247 social media API").Select(s => s.Message).FirstOrDefault()) +
                //"<hr>Your Login Details <br> Your Email:" + user.Email + "<br/>" +
                //"Your Password:" + user.Password + "<hr/>" +
                //"Regards:  marketsquare247.com" + "<br>" +
                //"Email: info@marketsqaure247.com";
                // mydata.SendMail(user.Email, title, msg);
                var result = from p in db.Users
                             where p.ID == UserID
                             select new { ID = p.ID, OTPExist = "true" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var result = from p in db.Users
                             where p.ID == UserID
                             select new { ID = p.ID, OTPExist = "false" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult Resend_VerifyRegisterOTP(int UserID)
        {
            int RegisterOTP_Numeric = mydata.Generate_OTP_Numeric();
            Session["OTP_Register"] = RegisterOTP_Numeric;
            var user = db.Set<User>().Find(UserID);
            //user.resetkey =Convert.ToString(RegisterOTP_Numeric);
            //db.SaveChanges();
            //==================SEND MAIL============================//
            string title = "Xamagos - New Verification Code";
            string msg = "<a href='https://xamagos.com' title='Xamagos'> <img src='https://xamagos.com/Images/logosquare.png' style='width: 160px; height: 50px;background-color:#17A2B8' /></a>" + "<hr><strong>" + " Hello " + user.CompanyName + "</strong><br><br>";
            msg += "Please enter the code <b>" + RegisterOTP_Numeric + "</b> to confirm your email on Xamagos.";
            msg += "<br>This code expires in 10 minutes, do not share it with others.";
            msg += "Regards:  Xamagos" + "<br>" + "Email: support@xamagos.com";
            mydata.SendMail(user.Email, title, msg);
            //==================SEND SMS=========================//
            //string sms = "New verification code.Please enter the code sent to your email " + user.Email + " to verify your email on Market Square247.";
            string sms = "New verification code.Please enter the code sent to your email to verify your email on Xamagos. The code expires in 10 minutes.";
            mydata.Send_SMS_KudiSMS(user.PhoneNumber, sms, "XAMAGOS", DateTime.Now);
            //==================END SEND SMS=========================//
            var result = from p in db.Users
                         where p.ID == UserID
                         select new { ID = p.ID, OTPSent = "Yes" };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult VerificationLinkSent(int? id, string verifykey, string verify)
        //{
        //    if (id != null)
        //    {
        //        ViewBag.email = db.Users.Find(id).Email;
        //    }
        //    if (verifykey != null)
        //    {
        //        int UserID = db.Users.Where(v => v.resetkey == verifykey).Select(v => v.ID).FirstOrDefault();
        //        var user = db.Set<User>().Find(UserID);
        //        user.EmailVerify = 1;
        //        db.SaveChanges();
        //        string UserLoggedIn = db.Users.Find(UserID).CompanyName;
        //        string UserRole = db.Users.Find(UserID).UserRole.Role;
        //        Session["UserRole"] = UserRole;
        //        Session["Name"] = UserLoggedIn;
        //        Session["username"] = user.Username;
        //        // Session["password"]=Password;
        //        Session["UserID"] = UserID;
        //        //Send Mail
        //        string title = "marketsquare247.com welcomes you!";
        //        string msg = "<a href='https://marketsquare247.com' title='Market Square247'> <img src='https://marketsquare247.com/Images/logosquare.png' style='width: 100px; height: 100px' /></a>" + "<hr>" + " Hi " + user.CompanyName + "<br>" +
        //        "Congratulations!" + "<br/><br>" +
        //        "Your account has been created successfully. <hr>" +
        //        "Do you know that you can advertize your products on social media using MarketSquare247?" +
        //        HttpUtility.HtmlDecode(db.EmailMessages.Where(s => s.Subject == "Advertize your products/services on social media using MarketSquare247 social media API").Select(s => s.Message).FirstOrDefault()) +
        //        "<hr>Your Login Details <br> Your Email:" + user.Email + "<br/>" +
        //        "Your Password:" + user.Password + "<hr/>" +
        //        "Regards:  marketsquare247.com" + "<br>" +
        //        "Email: info@marketsqaure247.com";
        //        mydata.SendMail(user.Email, title, msg);


        //        //return Redirect("~/Users/VerificationLinkSent/?verify=true");
        //        return Redirect("~/Users/BusinessLanding/" + UserID + "?ax=1");
        //    }
        //    return View();
        //}

        //"Do you know that you can earn money by refering other customers?<p>Simply hit the share button on your dashboard." +
        // "This implies that anybody that registered with your referal code you stand a chance of earning more that 5NGN+" +
        // "<br><strong> The more referals the more money will hit your account.</strong>" +

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CompanyName,PhoneNumber,Email,Username,Password,Address,State,Gender,DateCreated,UserRoleID,ReferalID")] User user)
        {
            if (ModelState.IsValid)
            {
                if (db.Users.Where(s => s.CompanyName == user.CompanyName).Count() > 0)
                {
                    ViewBag.CompanyExist = "This Business name has been taken by another user!";
                }
                else
                {
                    if (db.Users.Where(s => s.Email == user.Email).Count() > 0)
                    {
                        ViewBag.EmailExist = "This Email is already used by another user!";
                        int UserID = db.Users.Where(s => s.Email == user.Email).Select(s => s.ID).FirstOrDefault();
                        if (db.Users.Where(s => s.Email == user.Email).Select(s => s.EmailVerify).FirstOrDefault() == null)
                        {
                            return Redirect("~/Users/VerificationLinkSent/?id=" + UserID + "&ax=1");
                        }
                    }
                    else
                    {
                        int RegisterOTP_Numeric = mydata.Generate_OTP_Numeric();
                        Session["OTP_Register"] = RegisterOTP_Numeric;
                        string resetkey = Guid.NewGuid().ToString();
                        int TotalNumberofdays = (int)db.TrialPeriods.Select(t => t.Period).FirstOrDefault();
                        int planid = db.PricingPlans.Where(p => p.PlanName == "FREE PLAN").Select(p => p.ID).FirstOrDefault();
                        int billingid = db.BillingCycles.Where(p => p.Cycle == "Free").Select(p => p.ID).FirstOrDefault();
                        user.DateCreated = DateTime.Now;
                        user.PaymentStatus = 0;
                        //Trial Days
                        //plan information
                        user.CompanyName = user.CompanyName.TrimEnd().TrimStart();
                        user.PlanID = planid;
                        user.BillingCycleID = billingid;

                        user.LaunchDate = DateTime.Now;
                        user.PreviousDate = DateTime.Now;
                        user.LaunchYear = DateTime.Now.Year;
                        user.LaunchMonth = DateTime.Now.Month;
                        user.LaunchDay = DateTime.Now.Day;

                        user.PreviousYear = DateTime.Now.Year;
                        user.PreviousMonth = DateTime.Now.Month;
                        user.PreviousDay = DateTime.Now.Day;
                        user.NDays = 0;
                        user.FixedDays = TotalNumberofdays;
                        //==End of expiry
                        user.resetkey = resetkey;
                        user.HardToken = Convert.ToString(mydata.Generate_OTP_Numeric());
                        // user.resetkey =Convert.ToString(RegisterOTP_Numeric);
                        user.Username = user.Email;
                        user.CompanyName = user.CompanyName.ToUpper();
                        user.UserRoleID = db.UserRoles.Where(u => u.Role == "Shop Admin").Select(u => u.ID).FirstOrDefault();
                        db.Users.Add(user);
                        db.SaveChanges();
                        string UserLoggedIn = db.Users.Where(s => s.Username == user.Username && s.Password == user.Password).Select(s => s.CompanyName).FirstOrDefault();
                        string UserRole = db.Users.Where(s => s.Username == user.Username && s.Password == user.Password).Select(s => s.UserRole.Role).FirstOrDefault();
                        int UserID = db.Users.Where(s => s.Username == user.Username && s.Password == user.Password).Select(s => s.ID).FirstOrDefault();
                        //Session["UserRole"] = UserRole;
                        //Session["Name"] = UserLoggedIn;
                        //Session["username"] = user.Username;
                        //// Session["password"]=Password;
                        //Session["UserID"] = UserID;

                        // string RegisterOTP_Char = mydata.Generate_OTP_Char();

                        //==================SEND MAIL============================//
                        string title = "XAMAGOS - Email Verification!";
                        string msg = "<a href='https://marketsquare247.com' title='Xamagos'> <img src='https://xamagos.com/Images/logosquare.png' style='width: 160px; height: 50px;background-color:#17A2B8' /></a>" + "<hr><strong>" + " Hello " + user.CompanyName + "</strong><br><br>";
                        msg += "Please enter the code <b>" + RegisterOTP_Numeric + "</b> to confirm your email on XAMAGOS.";
                        msg += "<br>This code expires in 10 minutes, do not share it with others.";
                        msg += "Regards:  XAMAGOS" + "<br>" + "Email: support@xamagos.com";
                        mydata.SendMail(user.Email, title, msg);
                        //==================SEND SMS=========================//
                        //string sms = "Please enter the code sent to your email " + user.Email + " to verify your account email on Market Square247.";
                        string sms = "Please enter the code sent to your email to verify your account on XAMAGOS. The code expires in 10 minutes.";
                        mydata.Send_SMS_KudiSMS(user.PhoneNumber, sms, "XAMAGOS", DateTime.Now);
                        //==================END SEND SMS=========================//

                        // string link = "https://marketsquare247.com/Users/VerificationLinkSent?verifykey=" + user.resetkey;
                        //Code to get a particular message
                        //var getmessage = HttpUtility.HtmlDecode(db.EmailMessages.Where(s => s.msgKey == "Account_Verification").Select(s => s.Message).FirstOrDefault());
                        //string msg = "<a href='https://marketsquare247.com' title='Market Square247'> <img src='https://marketsquare247.com/Images/logosquare.png' style='width: 100px; height: 100px' /></a>" + "<hr><strong>" + " Hello " + user.CompanyName + "</strong><br>" +
                        // "Click on the link below to verify your email." + "<br/><br>" +
                        // "<a href='"+ link +"'>"+ "VERIFY MY EMAIL" + "</a><br/><br>" +
                        // //"Email:" + user.Email + "<br/>" +
                        // //"Password:" + user.Password + "<hr/>" +
                        // "Regards:  marketsquare247.com" + "<br>" +
                        // "Email: info@marketsqaure247.com";
                        // mydata.SendMail(user.Email, title, msg);
                        //==================END SEND MAIL=========================//


                        //Create Business Folder
                        if (!Directory.Exists("~/BusinessImages/" + user.CompanyName))
                        {
                            Directory.CreateDirectory(Server.MapPath("~/BusinessImages/" + user.CompanyName));
                        }
                        //return Redirect("~/Account/Login/?ax=1");
                        // return Redirect("~/Users/BusinessLanding/"+UserID+"?ax=1");
                        return Redirect("~/Users/VerificationLinkSent/?id=" + user.ID + "&ax=1");

                    }
                }
            }

            ViewBag.UserRoleID = new SelectList(db.UserRoles, "ID", "Role", user.UserRoleID);
            return View(user);
        }

        public JsonResult SignUpAjax(string name, string phone,string email,string sex,string state, string address, string password)
        {
              if (db.Users.Where(s => s.Email == email && s.EmailVerify==1).Count() ==1)
                {
                    int UserID = db.Users.Where(s => s.Email == email).Select(s => s.ID).FirstOrDefault();
                    //ViewBag.EmailExist = "Email already used!";
                    var result = from p in db.Users
                                 where p.Email == email
                                 select new { EmailExist = "true", userid = p.ID };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }


            else if (db.Users.Where(s => s.Email == email && s.EmailVerify == null).Count() == 1)
            {
                //return Redirect("~/Users/VerificationLinkSent/?id=" + UserID + "&ax=1");
                var result = from p in db.Users
                             where p.Email == email
                             select new { GotoVerification = "true", userid = p.ID };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else if (db.Users.Where(s => s.PhoneNumber == phone).Count() >0)
            {
                //return Redirect("~/Users/VerificationLinkSent/?id=" + UserID + "&ax=1");
                var result = from p in db.Users
                             where p.PhoneNumber == phone
                             select new { PhoneExist = "true", userid = p.ID };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            else
            {
                int RegisterOTP_Numeric = mydata.Generate_OTP_Numeric();
                Session["OTP_Register"] = RegisterOTP_Numeric;
                string resetkey = Guid.NewGuid().ToString();
                int TotalNumberofdays = (int)db.TrialPeriods.Select(t => t.Period).FirstOrDefault();
                int planid = db.PricingPlans.Where(p => p.PlanName == "FREE PLAN").Select(p => p.ID).FirstOrDefault();
                int billingid = db.BillingCycles.Where(p => p.Cycle == "Free").Select(p => p.ID).FirstOrDefault();
                User user = new User();
                user.DateCreated = DateTime.Now;
                user.PaymentStatus = 0;
                //Trial Days
                //plan information
                var newname = name.TrimEnd().TrimStart();
                user.PlanID = planid;
                user.BillingCycleID = billingid;

                user.LaunchDate = DateTime.Now;
                user.PreviousDate = DateTime.Now;
                user.LaunchYear = DateTime.Now.Year;
                user.LaunchMonth = DateTime.Now.Month;
                user.LaunchDay = DateTime.Now.Day;

                user.PreviousYear = DateTime.Now.Year;
                user.PreviousMonth = DateTime.Now.Month;
                user.PreviousDay = DateTime.Now.Day;
                user.NDays = 0;
                user.FixedDays = TotalNumberofdays;
                //==End of expiry
                user.resetkey = resetkey;
                user.HardToken = Convert.ToString(mydata.Generate_OTP_Numeric());
                // user.resetkey =Convert.ToString(RegisterOTP_Numeric);
                user.Email = email;
                user.Username = email;
                user.Password = Crypto.HashPassword(password);
                //user.Password = password;
                user.State = state;
                user.Gender = sex;
                user.Address = address;
                user.PhoneNumber = phone;
                user.CompanyName = newname.ToUpper();
                user.UserRoleID = db.UserRoles.Where(u => u.Role == "Shop Admin").Select(u => u.ID).FirstOrDefault();
                db.Users.Add(user);
                db.SaveChanges();
                string UserLoggedIn = db.Users.Where(s => s.Username == email && s.Password == password).Select(s => s.CompanyName).FirstOrDefault();
                string UserRole = db.Users.Where(s => s.Username == email && s.Password == password).Select(s => s.UserRole.Role).FirstOrDefault();
                int UserID = db.Users.Where(s => s.Username == email && s.Password == password).Select(s => s.ID).FirstOrDefault();
                //Session["UserRole"] = UserRole;
                //Session["Name"] = UserLoggedIn;
                //Session["username"] = user.Username;
                //// Session["password"]=Password;
                //Session["UserID"] = UserID;

                // string RegisterOTP_Char = mydata.Generate_OTP_Char();

                //==================SEND MAIL============================//
                string title = "XAMAGOS - Email Verification!";
                string msg = "<a href='https://xamagos.com' title='Xamagos'> <img src='https://xamagos.com/Images/logosquare.png' style='width: 160px; height: 50px;background-color:#17A2B8' /></a>" + "<hr><strong>" + " Hello " + user.CompanyName + "</strong><br><br>";
               msg += "Please enter the code <b>" + RegisterOTP_Numeric + "</b> to confirm your email on XAMAGOS.";
                msg += "<br>This code expires in 10 minutes, do not share it with others.";
                msg += "Regards:  XAMAGOS" + "<br>" + "Email: support@xamagos.com";
              //  mydata.SendMail(user.Email, title, msg);
                //==================SEND SMS=========================//
                //string sms = "Please enter the code sent to your email " + user.Email + " to verify your account email on Market Square247.";
                string sms = "Please enter the code sent to your email to verify your account on XAMAGOS. The code expires in 10 minutes.";
               // mydata.Send_SMS_KudiSMS(user.PhoneNumber, sms, "MARKTSQR247", DateTime.Now);
                //==================END SEND SMS=========================//

                // string link = "https://marketsquare247.com/Users/VerificationLinkSent?verifykey=" + user.resetkey;
                //Code to get a particular message
                //var getmessage = HttpUtility.HtmlDecode(db.EmailMessages.Where(s => s.msgKey == "Account_Verification").Select(s => s.Message).FirstOrDefault());
                //string msg = "<a href='https://marketsquare247.com' title='Market Square247'> <img src='https://marketsquare247.com/Images/logosquare.png' style='width: 100px; height: 100px' /></a>" + "<hr><strong>" + " Hello " + user.CompanyName + "</strong><br>" +
                // "Click on the link below to verify your email." + "<br/><br>" +
                // "<a href='"+ link +"'>"+ "VERIFY MY EMAIL" + "</a><br/><br>" +
                // //"Email:" + user.Email + "<br/>" +
                // //"Password:" + user.Password + "<hr/>" +
                // "Regards:  marketsquare247.com" + "<br>" +
                // "Email: info@marketsqaure247.com";
                // mydata.SendMail(user.Email, title, msg);
                //==================END SEND MAIL=========================//


                //Create Business Folder
                if (!Directory.Exists("~/BusinessImages/" + user.CompanyName+ user.HardToken))
                {
                    Directory.CreateDirectory(Server.MapPath("~/BusinessImages/" + user.CompanyName + user.HardToken));
                }
                //return Redirect("~/Account/Login/?ax=1");
                // return Redirect("~/Users/BusinessLanding/"+UserID+"?ax=1");
                //return Redirect("~/Users/VerificationLinkSent/?id=" + user.ID + "&ax=1");
                var result = from p in db.Users
                             where p.Email == email
                             select new { GotoVerificationSent = "true", userid = p.ID };
                return Json(result, JsonRequestBehavior.AllowGet);
            }       
        }



        public ActionResult CreateDescription()
        {
            if (!string.IsNullOrEmpty((string)Session["username"]))
            {
                int UserID = (int)Session["UserID"];
                var user = db.Users.Find(UserID);
                return View(user);
            }
            else
            {
                return Redirect("~/Home/AccessDenied");
            }
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult CreateDescription(string Description)
        {
            if (!string.IsNullOrEmpty((string)Session["username"]))
            {
                int UserID = (int)Session["UserID"];


                if (ModelState.IsValid)
                {
                    var user = db.Set<User>().Find(UserID);
                    user.Description = Description;
                    db.SaveChanges();
                    return Redirect("~/Users/BusinessLanding/" + UserID);
                }

            }

            return View();
        }
        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                User user = db.Users.Find(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                ViewBag.UserRoleID = new SelectList(db.UserRoles, "ID", "Role", user.UserRoleID);
                return View(user);
            }
            return Redirect("~/Home/AccessDenied");
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CompanyName,PhoneNumber,Email,Username,Password,Address,State,Gender,DateCreated,UserRoleID")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserRoleID = new SelectList(db.UserRoles, "ID", "Role", user.UserRoleID);
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                if (id == null)
                {
                    return Redirect("~/Home/AccessDenied");
                }
                User user = db.Users.Find(id);
                if (user == null)
                {
                    return Redirect("~/Home/AccessDenied");
                }
                return View(user);
            }
            return Redirect("~/Home/AccessDenied");
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult CheckBusinessName(string businessname)
        {
            var result = (from r in db.Users
                          where r.CompanyName == businessname
                          select new { Text = r.CompanyName }).Count();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UploadLogo(HttpPostedFileBase[] files)
        {
            Random random = new Random();
            //   Random generator = new Random();
            int rand1 = random.Next(99999, 99999999);
            int rand2 = random.Next(99999, 99999999);
            StringBuilder builder = new StringBuilder();
            int size = 550;
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                ViewBag.Random = builder.Append(ch);
            }

            int UserID = (int)Session["UserID"];

            string businessname = db.Users.Find(UserID).CompanyName;
            string hardtoken = db.Users.Find(UserID).HardToken;
            string DocumentName = UserID + ".jpg";
            string efilepath;
            foreach (HttpPostedFileBase file in files)
            {
                if (file != null)
                {
                    efilepath = Server.MapPath("~/BusinessImages/" + businessname+hardtoken + "/" + "\\" + DocumentName);
                    string ext = Path.GetExtension(efilepath);

                    //if file extension is supported, save file and update database
                    if (ext == ".jpg" || ext == ".jpeg" || ext == ".png" || ext == ".gif" || ext == ".ico")
                    {
                        string newName = System.IO.Path.GetFileNameWithoutExtension(efilepath);
                        newName = newName + ".jpg";

                        efilepath = Server.MapPath("~/BusinessImages/" + businessname+hardtoken + "/" + "\\" + newName);
                        file.SaveAs(efilepath);
                        mydata.ResizePicture(efilepath);


                    }
                    //else
                    //{
                    //    ViewBag.Message = "Oops! invalid file format";
                    //    return Redirect("/StudentBioDatas/Edit/" + BioDataID);
                    //}
                }

            }
            return Redirect("~/Users/MyProfile/" + UserID + "?pp=" + ViewBag.Random + "&ckp=" + ViewBag.Random);
        }

        public ActionResult Get_Billing(int PlanID, int BillingID)
        {
            //decimal PlanAmount = Convert.ToDecimal(db.PricingPlans.Find(PlanID).Amount);
            //int BillingValue =(int) db.BillingCycles.Find(BillingID).Value;

            var result = (from r in db.PricingPlans
                          from s in db.BillingCycles
                          where r.ID == PlanID && s.ID == BillingID && s.Cycle!="Free"
                          select new { Amount = r.Amount * s.Value, Cycle = s.Cycle, PlanName = r.PlanName, Value = s.Value }).Distinct();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Get_PlanFeatures(int PlanID)
        {
            //decimal PlanAmount = Convert.ToDecimal(db.PricingPlans.Find(PlanID).Amount);
            //int BillingValue =(int) db.BillingCycles.Find(BillingID).Value;

            var result = (from r in db.PlanFeatures
                          from k in db.PricingPlans
                          where k.ID == PlanID && r.PricingPlanID == PlanID && k.PlanName!="FREE PLAN"
                          select new { Feature = r.Feature, PlanName = k.PlanName }).Distinct();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Get_PlanAmount(int PlanID)
        {
            var result = (from k in db.PricingPlans
                          where k.ID == PlanID
                          select new { PlanAmount = k.Amount, PlanName = k.PlanName }).Distinct();
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Update_Payment_Company_Plan(string refno, decimal amount, int planid, int billingid, int extraperiod, decimal discountpercentage, decimal extrabonusamount, decimal expectedamount, decimal extrabonuspercentage, decimal discountamount)

        {
            int UserID = (int)Session["UserID"];
            int BillingDays = (int)db.BillingCycles.Find(billingid).Value;
            //GET DAYS REMAINING FOR OLD PLAN
            int totaldays = (int)db.Users.Find(UserID).FixedDays;
            int Numberused = (int)db.Users.Find(UserID).NDays;
            int existingPlanID=(int) db.Users.Find(UserID).PlanID;
            string PlanName = db.PricingPlans.Find(existingPlanID).PlanName;
            int DaysRemaining = totaldays - Numberused;
            //int TotalNumberofdays = (BillingDays * 30) + DaysRemaining + extraperiod;
            int TotalNumberofdays;
            if (PlanName != "FREE PLAN")
            {
                TotalNumberofdays = (BillingDays * 30) + DaysRemaining + extraperiod;
            }
            else
            {
                TotalNumberofdays = (BillingDays * 30) + extraperiod;

            }
            var update = db.Set<User>().Find(UserID);
            update.ReferenceNo = refno;
            update.Amount = amount / 100;
            update.PaymentStatus = 1;
            update.DatePaid = DateTime.Now;
            update.ReferenceNo = refno;
            update.EmailVerify = 1;
            //plan information
            update.PlanID = planid;
            update.BillingCycleID = billingid;

            update.LaunchDate = DateTime.Now;
            update.PreviousDate = DateTime.Now;
            update.LaunchYear = DateTime.Now.Year;
            update.LaunchMonth = DateTime.Now.Month;
            update.LaunchDay = DateTime.Now.Day;

            update.PreviousYear = DateTime.Now.Year;
            update.PreviousMonth = DateTime.Now.Month;
            update.PreviousDay = DateTime.Now.Day;
            update.NDays = 0;
            update.FixedDays = TotalNumberofdays;
            db.SaveChanges();

            //SET ALL STATUS IN USERBONUS TABLE TO ZERO
            if (db.UserBonus.Where(s => s.UserID == UserID).Count() > 0)
            {
                mydata.UpdateUserBonusStatus(UserID);
            }

            //UPDATE USERBONUS TABLE
            UserBonu userbonus = new UserBonu();
            userbonus.UserID = UserID;
            userbonus.PercentageDiscount = discountpercentage;
            userbonus.ExpectedAmount = expectedamount;
            userbonus.DiscountAmount = discountamount;
            userbonus.PercentageExtraBonus = extrabonuspercentage;
            userbonus.ExtraBonusAmount = extrabonusamount;
            userbonus.ExtraPeriod = extraperiod;
            userbonus.DateCreated = DateTime.Now;
            userbonus.Status = 1;
            db.UserBonus.Add(userbonus);
            db.SaveChanges();
            mydata.Update_ShopStatus_to_Active_on_Renewal(UserID);

            string email = update.Email;
            string Name = update.CompanyName;
            string msg = "<a href='https://xamagos.com' title='Xamagos'> <img src='https://xamagos.com/Images/logosquare.png' style='width: 160px; height: 50px;background-color:#17A2B8' /></a><hr>Hello! " + Name + "<br/>" +
                " Your transaction details <br/>" +
                " Reference Number: " + refno + "<br/>" +
                " Amount Paid: " + amount / 100 + "<br/>" +
                "Plan: " + update.PricingPlan.PlanName + "<br/>" +
                "Billing Cycle: " + update.BillingCycle.Cycle + "<br/>" +
                " Website: https://www.xamagos.com <br/>" +
                "From Market Square";
            mydata.SendMail(email, "Xamagos - Payment Made Successfully", msg);

            return Redirect("/Users/AccountUpgrade/?id=" + UserID);
        }

        public ActionResult AssignUserRole(int? u)
        {
            if ((string)Session["UserRole"] != "Super Admin")
            {
                return Redirect("~/Account/Login");
            }
            else
            {
                var reg = from k in db.Users
                          select new
                          {
                              user = k.CompanyName + " - " + k.Email,
                              ID = k.ID
                          };
                ViewBag.Users = new SelectList(reg, "ID", "user");
                ViewBag.UserRole = new SelectList(db.UserRoles, "ID", "Role");
                return View();
            }
        }



        [HttpPost]
        public ActionResult AssignUserRole()
        {
            int RegID = Convert.ToInt32(Request.Form["Users"]);
            int Role = Convert.ToInt32(Request.Form["UserRole"]);
            var assign = db.Set<User>().Find(RegID);
            assign.UserRoleID = Role;
            db.SaveChanges();

            var reg = from k in db.Users
                      select new
                      {
                          user = k.CompanyName + " - " + k.Email,
                          ID = k.ID
                      };
            ViewBag.Users = new SelectList(reg, "ID", "user");
            ViewBag.UserRole = new SelectList(db.UserRoles, "ID", "Role");
            return View();
        }

        public ActionResult Bonus(int? id)
        {
            if ((string)Session["UserRole"] != "Super Admin")
            {
                return Redirect("~/Account/Login");
            }
            ViewBag.DiscountPercentage = db.Settings.Select(s => s.DiscountPercentage).FirstOrDefault();
            ViewBag.BonusPercentage = db.Settings.Select(s => s.BonusPercentage).FirstOrDefault();
            return View();
        }

        [HttpPost]
        public ActionResult Bonus()
        {

            decimal discount = Convert.ToDecimal(Request.Form["discount"]);
            decimal bonus = Convert.ToDecimal(Request.Form["bonus"]);
            int ID = db.Settings.Select(s => s.ID).FirstOrDefault();
            var setting = db.Set<Setting>().Find(ID);
            setting.DiscountPercentage = discount;
            setting.BonusPercentage = bonus;
            db.SaveChanges();

            ViewBag.DiscountPercentage = db.Settings.Select(s => s.DiscountPercentage).FirstOrDefault();
            ViewBag.BonusPercentage = db.Settings.Select(s => s.BonusPercentage).FirstOrDefault();

            return View();
        }

        public ActionResult FlutterwavePayment()
        {
            return View();
        }

        public ActionResult enable_payment_account_activation()
        {
            int ID = db.Settings.Select(s => s.ID).FirstOrDefault();
            var setting = db.Set<Setting>().Find(ID);
            setting.Payment_Account_Activation = 1;
            db.SaveChanges();
            var result = 1;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult disable_payment_account_activation()
        {
            int ID = db.Settings.Select(s => s.ID).FirstOrDefault();
            var setting = db.Set<Setting>().Find(ID);
            setting.Payment_Account_Activation = 0;
            db.SaveChanges();
            var result = 1;
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult display_logomobile()
        {
            int ID = db.Settings.Select(s => s.ID).FirstOrDefault();
            var setting = db.Set<Setting>().Find(ID);
            setting.LogoMobile = 1;
            db.SaveChanges();
            var result = 1;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult hide_logomobile()
        {
            int ID = db.Settings.Select(s => s.ID).FirstOrDefault();
            var setting = db.Set<Setting>().Find(ID);
            setting.LogoMobile = 0;
            db.SaveChanges();
            var result = 1;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Update_Payment_Store_Activation(string refno, decimal amount, int shopid)
        {
            int UserID = (int)Session["UserID"];
            var update = db.Set<User>().Find(UserID);
            update.ReferenceNo = refno;
            update.Amount = amount / 100;
            update.PaymentStatus = 1;
            update.DatePaid = DateTime.Now;
            db.SaveChanges();
            mydata.Update_ShopStatus_to_Active_on_Renewal(UserID);
            string shopname = db.Shops.Find(shopid).Name;
            string email = db.Shops.Find(shopid).Email;
            string phone = db.Shops.Find(shopid).PhoneNumber;
            //==================SEND MAIL============================//
            string title = "Xamagos-Account Activation";
            string msg = "<a href='https://marketsquare247.com' title='Xamagos'><img src='https://xamagos.com/Images/logosquare.png' style='width: 160px; height: 50px;background-color:#17A2B8' /></a>" + "<hr><strong>" + " Hello " + shopname + "</strong><br><br>";
            msg += "Your store has been activated successfully";
            msg += "<br><br>Enjoy the world of Xamagos.";
            // msg += "Regards:  Xamagos" + "<br>" + "Email: support@xamagos.com";
            mydata.SendMail(email, title, msg);

            //==================SEND SMS=========================//
            string sms = "Xamagos-Store Activation. Your store has been activated successfully. Enjoy the world of Xamagos.";
            mydata.Send_SMS_KudiSMS(phone, sms, "XAMAGOS", DateTime.Now);
            //==================END SEND SMS=========================//
            var result = from p in db.Users
                         where p.ID == UserID
                         select new { shopid = shopid };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Check_HardToken(int userid, string token)
        {
            int Count = db.Users.Where(s => s.ID == userid && s.HardToken == token).Count();
            var result = from p in db.Users
                         where p.ID == userid
                         select new { ID = p.ID, count = Count };
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        public ActionResult Dashboard(int? id)
        {
            //EXPIRY CHECK
            //if (count == 1)
            //{
            //    int lYear = (int)db.Registrations.Where(k => k.ID == id).Select(k => k.LaunchYear).FirstOrDefault();
            //    int lmonth = (int)db.Registrations.Where(k => k.ID == id).Select(k => k.LaunchMonth).FirstOrDefault();
            //    int lday = (int)db.Registrations.Where(k => k.ID == id).Select(k => k.LaunchDay).FirstOrDefault();

            //    int pYear = (int)db.Registrations.Where(k => k.ID == id).Select(k => k.PreviousYear).FirstOrDefault();
            //    int pmonth = (int)db.Registrations.Where(k => k.ID == id).Select(k => k.PreviousMonth).FirstOrDefault();
            //    int pday = (int)db.Registrations.Where(k => k.ID == id).Select(k => k.PreviousDay).FirstOrDefault();

            //    DateTime lauchd = new DateTime(lYear, lmonth, lday);
            //    DateTime prevd = new DateTime(pYear, pmonth, pday);
            //    TimeSpan diff = lauchd.Subtract(prevd);
            //    ViewBag.diff = diff.Days;
            //    ViewBag.FixedDays = db.Registrations.Find(id).FixedDays;
            //    ViewBag.Ndays = db.Registrations.Find(id).NDays;

            //    if (ViewBag.diff > ViewBag.FixedDays)
            //    {

            //    }
            //    else
            //    {
            //        var r = db.Set<Registration>().Find(id);
            //        r.NDays = ViewBag.diff;
            //        r.LaunchDate = DateTime.Now;
            //        r.LaunchYear = DateTime.Now.Year;
            //        r.LaunchMonth = DateTime.Now.Month;
            //        r.LaunchDay = DateTime.Now.Day;
            //        db.SaveChanges();
            //    }
            //}
            if (Session["UserID"] == null)
            {
                return Redirect("~/Home/AccessDenied");
            }

            if (id != null && db.Users.Where(s => s.ID == id).Count() == 1)
            {
                // int UserID = Convert.ToInt32(mydata.DecodeFrom64(""+id));
                ViewBag.s = db.Shops.Where(s => s.UserID == id);
                ViewBag.ShopID = new SelectList(ViewBag.s, "ID", "Name");
                int UserID = (int)id;
                ViewBag.CompanyID = id;
                string Name = (string)Session["Name"];

                ViewBag.CompanyName = db.Users.Where(u => u.ID == UserID).Select(u => u.CompanyName).FirstOrDefault();
                ViewBag.Shops = db.Shops.Where(s => s.UserID == UserID && s.ShopStatus == "Active").OrderByDescending(s => s.DateCreated).ToList();
                ViewBag.ShopsAdmin = db.Shops.Where(s => s.UserID == UserID).OrderByDescending(s => s.DateCreated).ToList();
                var user = db.Users.Where(c => c.ID == UserID).ToList();
                return View(user);
            }
            else
            {
                return Redirect("~/Home/AccessDenied");
            }

        }

        public ActionResult Manageshop(int? id)
        {
            //EXPIRY CHECK
            //if (count == 1)
            //{
            //    int lYear = (int)db.Registrations.Where(k => k.ID == id).Select(k => k.LaunchYear).FirstOrDefault();
            //    int lmonth = (int)db.Registrations.Where(k => k.ID == id).Select(k => k.LaunchMonth).FirstOrDefault();
            //    int lday = (int)db.Registrations.Where(k => k.ID == id).Select(k => k.LaunchDay).FirstOrDefault();

            //    int pYear = (int)db.Registrations.Where(k => k.ID == id).Select(k => k.PreviousYear).FirstOrDefault();
            //    int pmonth = (int)db.Registrations.Where(k => k.ID == id).Select(k => k.PreviousMonth).FirstOrDefault();
            //    int pday = (int)db.Registrations.Where(k => k.ID == id).Select(k => k.PreviousDay).FirstOrDefault();

            //    DateTime lauchd = new DateTime(lYear, lmonth, lday);
            //    DateTime prevd = new DateTime(pYear, pmonth, pday);
            //    TimeSpan diff = lauchd.Subtract(prevd);
            //    ViewBag.diff = diff.Days;
            //    ViewBag.FixedDays = db.Registrations.Find(id).FixedDays;
            //    ViewBag.Ndays = db.Registrations.Find(id).NDays;

            //    if (ViewBag.diff > ViewBag.FixedDays)
            //    {

            //    }
            //    else
            //    {
            //        var r = db.Set<Registration>().Find(id);
            //        r.NDays = ViewBag.diff;
            //        r.LaunchDate = DateTime.Now;
            //        r.LaunchYear = DateTime.Now.Year;
            //        r.LaunchMonth = DateTime.Now.Month;
            //        r.LaunchDay = DateTime.Now.Day;
            //        db.SaveChanges();
            //    }
            //}


            if (id != null && db.Users.Where(s => s.ID == id).Count() == 1)
            {
                // int UserID = Convert.ToInt32(mydata.DecodeFrom64(""+id));
                ViewBag.s = db.Shops.Where(s => s.UserID == id);
                ViewBag.ShopID = new SelectList(ViewBag.s, "ID", "Name");
                int UserID = (int)id;
                ViewBag.CompanyID = id;
                string Name = (string)Session["Name"];

                ViewBag.CompanyName = db.Users.Where(u => u.ID == UserID).Select(u => u.CompanyName).FirstOrDefault();
                ViewBag.Shops = db.Shops.Where(s => s.UserID == UserID && s.ShopStatus == "Active").OrderByDescending(s => s.DateCreated).ToList();
                ViewBag.ShopsAdmin = db.Shops.Where(s => s.UserID == UserID).OrderByDescending(s => s.DateCreated).ToList();
                var user = db.Users.Where(c => c.ID == UserID).ToList();
                return View(user);
            }
            else
            {
                return Redirect("~/Home/AccessDenied");
            }

        }

        public ActionResult MyProfile(int? id)
        {
            //EXPIRY CHECK
            //if (count == 1)
            //{
            //    int lYear = (int)db.Registrations.Where(k => k.ID == id).Select(k => k.LaunchYear).FirstOrDefault();
            //    int lmonth = (int)db.Registrations.Where(k => k.ID == id).Select(k => k.LaunchMonth).FirstOrDefault();
            //    int lday = (int)db.Registrations.Where(k => k.ID == id).Select(k => k.LaunchDay).FirstOrDefault();

            //    int pYear = (int)db.Registrations.Where(k => k.ID == id).Select(k => k.PreviousYear).FirstOrDefault();
            //    int pmonth = (int)db.Registrations.Where(k => k.ID == id).Select(k => k.PreviousMonth).FirstOrDefault();
            //    int pday = (int)db.Registrations.Where(k => k.ID == id).Select(k => k.PreviousDay).FirstOrDefault();

            //    DateTime lauchd = new DateTime(lYear, lmonth, lday);
            //    DateTime prevd = new DateTime(pYear, pmonth, pday);
            //    TimeSpan diff = lauchd.Subtract(prevd);
            //    ViewBag.diff = diff.Days;
            //    ViewBag.FixedDays = db.Registrations.Find(id).FixedDays;
            //    ViewBag.Ndays = db.Registrations.Find(id).NDays;

            //    if (ViewBag.diff > ViewBag.FixedDays)
            //    {

            //    }
            //    else
            //    {
            //        var r = db.Set<Registration>().Find(id);
            //        r.NDays = ViewBag.diff;
            //        r.LaunchDate = DateTime.Now;
            //        r.LaunchYear = DateTime.Now.Year;
            //        r.LaunchMonth = DateTime.Now.Month;
            //        r.LaunchDay = DateTime.Now.Day;
            //        db.SaveChanges();
            //    }
            //}


            if (id != null && db.Users.Where(s => s.ID == id).Count() == 1)
            {
                // int UserID = Convert.ToInt32(mydata.DecodeFrom64(""+id));
                ViewBag.s = db.Shops.Where(s => s.UserID == id);
                ViewBag.ShopID = new SelectList(ViewBag.s, "ID", "Name");
                int UserID = (int)id;
                ViewBag.CompanyID = id;
                string Name = (string)Session["Name"];

                ViewBag.CompanyName = db.Users.Where(u => u.ID == UserID).Select(u => u.CompanyName).FirstOrDefault();
                ViewBag.Shops = db.Shops.Where(s => s.UserID == UserID && s.ShopStatus == "Active").OrderByDescending(s => s.DateCreated).ToList();
                ViewBag.ShopsAdmin = db.Shops.Where(s => s.UserID == UserID).OrderByDescending(s => s.DateCreated).ToList();
                var user = db.Users.Where(c => c.ID == UserID).ToList();
                return View(user);
            }
            else
            {
                return Redirect("~/Home/AccessDenied");
            }

        }
         
        public ActionResult AccountUpgrade(int? id)
        {
            //EXPIRY CHECK
            //if (count == 1)
            //{
            //    int lYear = (int)db.Registrations.Where(k => k.ID == id).Select(k => k.LaunchYear).FirstOrDefault();
            //    int lmonth = (int)db.Registrations.Where(k => k.ID == id).Select(k => k.LaunchMonth).FirstOrDefault();
            //    int lday = (int)db.Registrations.Where(k => k.ID == id).Select(k => k.LaunchDay).FirstOrDefault();

            //    int pYear = (int)db.Registrations.Where(k => k.ID == id).Select(k => k.PreviousYear).FirstOrDefault();
            //    int pmonth = (int)db.Registrations.Where(k => k.ID == id).Select(k => k.PreviousMonth).FirstOrDefault();
            //    int pday = (int)db.Registrations.Where(k => k.ID == id).Select(k => k.PreviousDay).FirstOrDefault();

            //    DateTime lauchd = new DateTime(lYear, lmonth, lday);
            //    DateTime prevd = new DateTime(pYear, pmonth, pday);
            //    TimeSpan diff = lauchd.Subtract(prevd);
            //    ViewBag.diff = diff.Days;
            //    ViewBag.FixedDays = db.Registrations.Find(id).FixedDays;
            //    ViewBag.Ndays = db.Registrations.Find(id).NDays;

            //    if (ViewBag.diff > ViewBag.FixedDays)
            //    {

            //    }
            //    else
            //    {
            //        var r = db.Set<Registration>().Find(id);
            //        r.NDays = ViewBag.diff;
            //        r.LaunchDate = DateTime.Now;
            //        r.LaunchYear = DateTime.Now.Year;
            //        r.LaunchMonth = DateTime.Now.Month;
            //        r.LaunchDay = DateTime.Now.Day;
            //        db.SaveChanges();
            //    }
            //}


            if (id != null && db.Users.Where(s => s.ID == id).Count() == 1)
            {
                // int UserID = Convert.ToInt32(mydata.DecodeFrom64(""+id));
                ViewBag.s = db.Shops.Where(s => s.UserID == id);
                ViewBag.ShopID = new SelectList(ViewBag.s, "ID", "Name");
                int UserID = (int)id;
                ViewBag.CompanyID = id;
                string Name = (string)Session["Name"];

                ViewBag.CompanyName = db.Users.Where(u => u.ID == UserID).Select(u => u.CompanyName).FirstOrDefault();
                ViewBag.Shops = db.Shops.Where(s => s.UserID == UserID && s.ShopStatus == "Active").OrderByDescending(s => s.DateCreated).ToList();
                ViewBag.ShopsAdmin = db.Shops.Where(s => s.UserID == UserID).OrderByDescending(s => s.DateCreated).ToList();
                var user = db.Users.Where(c => c.ID == UserID).ToList();
                return View(user);
            }
            else
            {
                return Redirect("~/Home/AccessDenied");
            }

        }

        public ActionResult ChangePW(string oldpw, string pw,int userid)
        {
            int Count = db.Users.Where(s => s.ID == userid && s.Password == oldpw).Count();
            if (Count == 1)
            {
                //update password
                var update = db.Set<User>().Find(userid);
                update.Password = pw;
                update.PasswordUpdated = DateTime.Now;
                db.SaveChanges();
                var result = from p in db.Users
                             where p.ID == userid
                             select new { ID = p.ID, count = Count,PasswordChanged="true"};
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var result = from p in db.Users
                             where p.ID == userid
                             select new { ID = p.ID, count = Count, PasswordChanged ="false"};
                return Json(result, JsonRequestBehavior.AllowGet);
            }
           
        }

        public ActionResult AccountVerification(int? id)
        {
            //EXPIRY CHECK
            //if (count == 1)
            //{
            //    int lYear = (int)db.Registrations.Where(k => k.ID == id).Select(k => k.LaunchYear).FirstOrDefault();
            //    int lmonth = (int)db.Registrations.Where(k => k.ID == id).Select(k => k.LaunchMonth).FirstOrDefault();
            //    int lday = (int)db.Registrations.Where(k => k.ID == id).Select(k => k.LaunchDay).FirstOrDefault();

            //    int pYear = (int)db.Registrations.Where(k => k.ID == id).Select(k => k.PreviousYear).FirstOrDefault();
            //    int pmonth = (int)db.Registrations.Where(k => k.ID == id).Select(k => k.PreviousMonth).FirstOrDefault();
            //    int pday = (int)db.Registrations.Where(k => k.ID == id).Select(k => k.PreviousDay).FirstOrDefault();

            //    DateTime lauchd = new DateTime(lYear, lmonth, lday);
            //    DateTime prevd = new DateTime(pYear, pmonth, pday);
            //    TimeSpan diff = lauchd.Subtract(prevd);
            //    ViewBag.diff = diff.Days;
            //    ViewBag.FixedDays = db.Registrations.Find(id).FixedDays;
            //    ViewBag.Ndays = db.Registrations.Find(id).NDays;

            //    if (ViewBag.diff > ViewBag.FixedDays)
            //    {

            //    }
            //    else
            //    {
            //        var r = db.Set<Registration>().Find(id);
            //        r.NDays = ViewBag.diff;
            //        r.LaunchDate = DateTime.Now;
            //        r.LaunchYear = DateTime.Now.Year;
            //        r.LaunchMonth = DateTime.Now.Month;
            //        r.LaunchDay = DateTime.Now.Day;
            //        db.SaveChanges();
            //    }
            //}


            if (id != null && db.Users.Where(s => s.ID == id).Count() == 1)
            {
                // int UserID = Convert.ToInt32(mydata.DecodeFrom64(""+id));
                ViewBag.s = db.Shops.Where(s => s.UserID == id);
                ViewBag.ShopID = new SelectList(ViewBag.s, "ID", "Name");
                int UserID = (int)id;
                ViewBag.CompanyID = id;
                string Name = (string)Session["Name"];

                ViewBag.CompanyName = db.Users.Where(u => u.ID == UserID).Select(u => u.CompanyName).FirstOrDefault();
                ViewBag.Shops = db.Shops.Where(s => s.UserID == UserID && s.ShopStatus == "Active").OrderByDescending(s => s.DateCreated).ToList();
                ViewBag.ShopsAdmin = db.Shops.Where(s => s.UserID == UserID).OrderByDescending(s => s.DateCreated).ToList();
                var user = db.Users.Where(c => c.ID == UserID).ToList();
                return View(user);
            }
            else
            {
                return Redirect("~/Home/AccessDenied");
            }

        }




    ///END HASHING PASSWORD

    public ActionResult LoginAjax(string email, string password)
    {
      string Username = email;
      string Password = password;
      //If user exist but email not verify redirect 
      //if (mydata.Is_Password_Verified(db.Users.Where(s =>(s.Email == Username || s.PhoneNumber == Username)).Select(s => s.Password).FirstOrDefault(), password))
      //{
      //  var result = from p in db.Users
      //               where p.Email == email
      //               select new { match = "true", userid = p.ID };
      //  return Json(result, JsonRequestBehavior.AllowGet);
      //}
      //==================================//
      int checkEmail = db.Users.Where(r => (r.Email == Username || r.PhoneNumber == Username)).Count();
      if (checkEmail > 0)
      {
        if (mydata.Is_Password_Verified(db.Users.Where(s => (s.Email == Username || s.PhoneNumber == Username)).Select(s => s.Password).FirstOrDefault(), password))
        {
          int UserID = db.Users.Where(s => (s.Email == Username || s.PhoneNumber == Username)).Select(s => s.ID).FirstOrDefault();
          int emailverify = Convert.ToInt32(db.Users.Where(s => (s.Email == Username || s.PhoneNumber == Username)).Select(s => s.EmailVerify).FirstOrDefault());
          if (emailverify == 1)
          {
            string UserLoggedIn = db.Users.Where(s => (s.Email == Username || s.PhoneNumber == Username) && s.EmailVerify == 1).Select(s => s.CompanyName).FirstOrDefault();
            string UserRole = db.Users.Where(s => (s.Email == Username || s.PhoneNumber == Username) && s.EmailVerify == 1).Select(s => s.UserRole.Role).FirstOrDefault();

            Session["UserRole"] = UserRole;
            Session["Name"] = UserLoggedIn;
            Session["username"] = Username;
            // Session["password"]=Password;
            Session["UserID"] = UserID;
            // return Redirect("~/Users/BusinessLanding/"+mydata.EncodePasswordToBase64(""+UserID));
            if (UserRole == "Super Admin" || UserRole == "Site Admin" || UserRole == "Marketer" || UserRole == "Delivery Agent")
            {
              //return Redirect("~/Users/AdminLanding/" + UserID);
              var result = from p in db.Users
                           where p.Email == email
                           select new { gotoAdminLanding = "true", userid = p.ID };
              return Json(result, JsonRequestBehavior.AllowGet);
            }
            //return Redirect("~/Users/Dashboard/" + UserID);
            var result2 = from p in db.Users
                          where p.Email == email
                          select new { gotoUserDashboard = "true", userid = p.ID };
            return Json(result2, JsonRequestBehavior.AllowGet);
          }
          else
          {
            //return Redirect("~/Users/VerificationLinkSent/?id=" + UserID + "&ax=1");
            var result3 = from p in db.Users
                          where p.Email == email
                          select new { gotoUserVerifiEmail = "true", userid = p.ID };
            return Json(result3, JsonRequestBehavior.AllowGet);
          }

        }

        else
        {
          var result4 = from p in db.Users
                        select new { UserNotExist = "true" };
          return Json(result4, JsonRequestBehavior.AllowGet);
        }

      }
      else
      {
        var result4 = from p in db.Users
                      select new { UserNotExist = "true" };
        return Json(result4, JsonRequestBehavior.AllowGet);
      }
    }


    //public ActionResult LoginAjax(string email, string password)
    //{
    //  string Username = email;
    //  string Password = mydata.EncodePasswordToBase64(password);
    //  If user exist but email not verify redirect 
    //  int UserID1 = db.Users.Where(s => s.Password == Password && (s.Email == Username || s.PhoneNumber == Username)).Select(s => s.ID).FirstOrDefault();

    //  ==================================//
    //  int UserLogin = db.Users.Where(r => r.Password == Password && (r.Email == Username || r.PhoneNumber == Username)).Count();
    //  if (UserLogin > 0)
    //  {
    //    int UserID = db.Users.Where(s => s.Password == Password && (s.Email == Username || s.PhoneNumber == Username)).Select(s => s.ID).FirstOrDefault();
    //    int emailverify = Convert.ToInt32(db.Users.Where(s => s.Password == Password && (s.Email == Username || s.PhoneNumber == Username)).Select(s => s.EmailVerify).FirstOrDefault());
    //    if (emailverify == 1)
    //    {
    //      string UserLoggedIn = db.Users.Where(s => s.Password == Password && (s.Email == Username || s.PhoneNumber == Username) && s.EmailVerify == 1).Select(s => s.CompanyName).FirstOrDefault();
    //      string UserRole = db.Users.Where(s => s.Password == Password && (s.Email == Username || s.PhoneNumber == Username) && s.EmailVerify == 1).Select(s => s.UserRole.Role).FirstOrDefault();

    //      Session["UserRole"] = UserRole;
    //      Session["Name"] = UserLoggedIn;
    //      Session["username"] = Username;
    //       Session["password"]=Password;
    //      Session["UserID"] = UserID;
    //       return Redirect("~/Users/BusinessLanding/"+mydata.EncodePasswordToBase64(""+UserID));
    //      if (UserRole == "Super Admin" || UserRole == "Site Admin" || UserRole == "Marketer" || UserRole == "Delivery Agent")
    //      {
    //        return Redirect("~/Users/AdminLanding/" + UserID);
    //        var result = from p in db.Users
    //                     where p.Email == email
    //                     select new { gotoAdminLanding = "true", userid = p.ID };
    //        return Json(result, JsonRequestBehavior.AllowGet);
    //      }
    //      return Redirect("~/Users/Dashboard/" + UserID);
    //      var result2 = from p in db.Users
    //                    where p.Email == email
    //                    select new { gotoUserDashboard = "true", userid = p.ID };
    //      return Json(result2, JsonRequestBehavior.AllowGet);
    //    }
    //    else
    //    {
    //      return Redirect("~/Users/VerificationLinkSent/?id=" + UserID + "&ax=1");
    //      var result3 = from p in db.Users
    //                    where p.Email == email
    //                    select new { gotoUserVerifiEmail = "true", userid = p.ID };
    //      return Json(result3, JsonRequestBehavior.AllowGet);
    //    }

    //  }

    //  else
    //  {
    //    var result4 = from p in db.Users
    //                  select new { UserNotExist = "true" };
    //    return Json(result4, JsonRequestBehavior.AllowGet);
    //  }

    //}


    protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
