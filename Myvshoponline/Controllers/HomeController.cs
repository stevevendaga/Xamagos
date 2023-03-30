using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Myvshoponline;
using System.Net.Mail;
using System.Net;
using Myvshoponline.Models;
using System.Data.Entity;

namespace Myvshoponline.Controllers
{
    public class HomeController : Controller
    {
        MyvshoponlineEntities db = new MyvshoponlineEntities();
        Getdata mydata = new Getdata();
        
        public ActionResult Index()
        {
                string random = Guid.NewGuid().ToString();
                ViewBag.ProductCategories = db.ProductCategories.Where(c => c.Name != "All Categories").ToList();
                ViewBag.ProductCategoryID = new SelectList(db.ProductCategories, "ID", "Name", selectedValue: 4);
                ViewBag.LocationID = new SelectList(db.States, "ID", "Name", selectedValue: 38);
                ViewBag.CountryID = new SelectList(db.CountryRegions, "ID", "Country");
                ViewBag.FeaturedProducts = db.Products.Where(p => p.Shop.ShopStatus == "Active" && p.ProductStatu.Status == "Available").OrderBy(p => p.ID).ToList().Take(78);
                return View();
        }

        public ActionResult ShopDetails(string id, string search,int? q)
        {
            if (!string.IsNullOrEmpty(id) && q==null)
            {
                int count = db.Shops.Where(s => s.ShopURL == id).Count();
                if (count == 1)
                {
                    int ShopID = db.Shops.Where(s => s.ShopURL == id).Select(s => s.ID).FirstOrDefault();
                    Shop shop = db.Shops.Find(ShopID);

                    ViewBag.ProductCategoryID = new SelectList(db.ProductCategories, "ID", "Name");
                    ViewBag.ProductStatusID = new SelectList(db.ProductStatus, "ID", "Status");
                    ViewBag.SocialMediaChannel = new SelectList(db.SocialMediaChannels, "ID", "Channel");
                    ViewBag.BankID = new SelectList(db.Banks, "ID", "Name");

                    if (search != null && id != null)
                    {
                        ViewBag.Products = db.ShopProductCategories.Where(s => s.ShopID == ShopID).ToList().Take(10);
                        ViewBag.search = search;
                        ViewBag.Shopcount = db.Products.Where(p => p.ShopID == ShopID && p.Shop.ShopStatus == "Active" && p.Name.Contains(search)).Count();
                        ViewBag.Shopsearch = db.Products.Where(p => p.ShopID == ShopID && p.Shop.ShopStatus == "Active" && p.Name.Contains(search)).ToList();
                        ViewBag.UserID = db.Shops.Find(ShopID).UserID;
                        var ShopProducts1 = (from p in db.Products
                                             where p.ShopID == ShopID
                                             select new { ProductName = p.Name, ID = p.ID }).ToList();
                        ViewBag.ShopProducts = new SelectList(ShopProducts1, "ID", "ProductName");
                        return View(shop);
                    }
                    int UserID = (int)db.Shops.Find(ShopID).UserID;
                    ViewBag.UserID = db.Shops.Find(ShopID).UserID;
                    ViewBag.Products = db.ShopProductCategories.Where(s => s.ShopID == ShopID).ToList();
                    //Get all products
                    var ShopProducts = (from p in db.Products
                                        where p.ShopID == ShopID
                                        select new { ProductName = p.Name, ID = p.ID }).ToList();
                    ViewBag.ShopProducts = new SelectList(ShopProducts, "ID", "ProductName");

                    ViewBag.CompanyID = UserID;
                    ViewBag.Orders = db.Orders.Where(o => o.Product.Shop.UserID == UserID && o.Product.ShopID == ShopID && o.OnCart == "No").Count();
                    ViewBag.PendingPayment = db.Orders.Where(o => o.Product.Shop.UserID == UserID && o.Product.ShopID == ShopID && o.PaymentStatusID == 0 && o.OnCart == "No").Count();
                    ViewBag.PendingDelivery = db.Orders.Where(o => o.Product.Shop.UserID == UserID && o.Product.ShopID == ShopID && o.PaymentStatusID == 1 && o.DeliveryStatus == null && o.OnCart == "No").Count();
                    ViewBag.Delivered = db.Orders.Where(o => o.Product.Shop.UserID == UserID && o.Product.ShopID == ShopID && o.PaymentStatusID == 1 && o.DeliveryStatus == "Delivered" && o.OnCart == "No").Count();
                    ViewBag.CompanyName = db.Users.Where(u => u.ID == UserID).Select(u => u.CompanyName).FirstOrDefault();
                    return View(shop);
                }
                else
                {
                    return Redirect("~/Home/ShopNotFound");
                }
            }
            else if (q!=null){
                int? pid =(int) q;
                //FETCH PRODUCT DETAILS HERE
                if (id != null && pid != null && db.Shops.Where(s => s.ShopURL == id).Count() > 0)
                {
                    if (db.Products.Where(s => s.ID == pid).Count() == 0)
                    {
                        return Redirect("~/Home/ProductNotFound");
                    }
                    int sid = db.Shops.Where(s => s.ShopURL == id).Select(s => s.ID).FirstOrDefault();
                    return Redirect("~/Products/ProductDetails/" + pid+"?sid=" + sid);
                }
                return Redirect("~/Home/AccessDenied");

            }
           
            else
            {
                return Redirect("~/Home/Index");
            }
        }


        //public ActionResult Search(string search, string StateID,int? CountryID, int? ProductCategoryID)
        //{
        //    ViewBag.ProductCategoryID = new SelectList(db.ProductCategories, "ID", "Name", selectedValue: 4);
        //    ViewBag.LocationID = new SelectList(db.States, "ID", "Name", selectedValue: 38);
        //    ViewBag.CountryID = new SelectList(db.CountryRegions, "ID", "Country");
        //    if (ProductCategoryID != null)
        //    {
        //        ViewBag.CategoryName = db.ProductCategories.Find(ProductCategoryID).Name;
        //        ViewBag.Searchitem = search;
        //        string random = Guid.NewGuid().ToString();
        //        ViewBag.ProductCategoryID = new SelectList(db.ProductCategories, "ID", "Name");
        //        ViewBag.LocationID = new SelectList(db.States, "ID", "Name");
        //        if (db.Shops.Where(s => s.CountryID == CountryID).Select(s => s.CountryRegion.ID).FirstOrDefault() == CountryID && db.ProductCategories.Where(s => s.ID == ProductCategoryID).Select(s => s.Name).FirstOrDefault() == "All Categories")
        //        {
        //            var searchresult = db.Products.Where(s => s.Name.Contains(search) && s.ProductStatu.Status == "Available" && s.Shop.ShopStatus == "Active" && s.Shop.CountryID==CountryID);
        //            return View(searchresult.ToList());
        //        }
        //        else if (db.Shops.Where(s => s.CountryID == CountryID).Select(s => s.CountryRegion.ID).FirstOrDefault() == CountryID && db.ProductCategories.Where(s => s.ID == ProductCategoryID).Select(s=>s.ID).FirstOrDefault()==ProductCategoryID)
        //        {
        //            var searchresult = db.Products.Where(s => s.Name.Contains(search) && s.ProductCategoryID == ProductCategoryID && s.ProductStatu.Status == "Available" && s.Shop.ShopStatus == "Active" && s.Shop.CountryID == CountryID);
        //            return View(searchresult.ToList());
        //        }
        //        else if (db.Shops.Where(s => s.CountryID == CountryID).Select(s => s.CountryRegion.ID).FirstOrDefault() == CountryID  && db.ProductCategories.Where(s => s.ID == ProductCategoryID).Select(s => s.Name).FirstOrDefault() == "All Categories" && StateID!=null)
        //        {
        //            var searchresult = db.Products.Where(s => s.Name.Contains(search) && s.Shop.StateID == StateID  && s.Shop.CountryID==CountryID  && s.ProductStatu.Status == "Available" && s.Shop.ShopStatus == "Active");
        //            return View(searchresult.ToList());
        //        }
        //        else
        //        {
        //            var searchresult = db.Products.Where(s => s.Name.Contains(search) && s.ProductCategoryID == ProductCategoryID && s.Shop.StateID == StateID && s.Shop.CountryID==CountryID && s.ProductStatu.Status == "Available" && s.Shop.ShopStatus == "Active");
        //            return View(searchresult.ToList());
        //        }
        //    }
        //    else
        //    {
        //        var searchresult = db.Products.Where(s => s.Name.Contains(search) && s.ProductStatu.Status == "Available" && s.Shop.ShopStatus == "Active");
        //        return View(searchresult.ToList());
        //    }

        //}
        public ActionResult Search(string search, int? ProductCategoryID)
        {
            ViewBag.ProductCategoryID = new SelectList(db.ProductCategories, "ID", "Name", selectedValue: 4);
            ViewBag.LocationID = new SelectList(db.States, "ID", "Name", selectedValue: 38);
            ViewBag.CountryID = new SelectList(db.CountryRegions, "ID", "Country");
            if (ProductCategoryID != null)
            {
                ViewBag.CategoryName = db.ProductCategories.Find(ProductCategoryID).Name;
                ViewBag.Searchitem = search;
                 string CategoryName = ViewBag.CategoryName;
                string random = Guid.NewGuid().ToString();
                ViewBag.ProductCategoryID = new SelectList(db.ProductCategories, "ID", "Name");
                ViewBag.LocationID = new SelectList(db.States, "ID", "Name");
               
                if (CategoryName != "All Categories")
                {
                    var searchresult = db.Products.Where(s => s.Name.Contains(search) && s.ProductStatu.Status == "Available" && s.Shop.ShopStatus == "Active" && s.ProductCategoryID==ProductCategoryID );
                    return View(searchresult.ToList());
                }
                else
                {
                    var searchresult = db.Products.Where(s => s.Name.Contains(search) && s.ProductStatu.Status == "Available" && s.Shop.ShopStatus == "Active");
                    return View(searchresult.ToList());
                }
            }
            else
            {
                return View();
            }

        }


        public ActionResult FindItem(string item, string CountryID, string StateID, int? ProductCategoryID)
        {
            ViewBag.ProductCategoryID = new SelectList(db.ProductCategories, "ID", "Name", selectedValue: 4);
            ViewBag.LocationID = new SelectList(db.States, "ID", "Name", selectedValue: 38);
            ViewBag.CountryID = new SelectList(db.CountryRegions, "ID", "Country");
            if (ProductCategoryID != null)
            {
                ViewBag.CategoryName = db.ProductCategories.Find(ProductCategoryID).Name;
                ViewBag.Searchitem = item;
                string CategoryName = ViewBag.CategoryName;
                string random = Guid.NewGuid().ToString();
                ViewBag.ProductCategoryID = new SelectList(db.ProductCategories, "ID", "Name");
                ViewBag.LocationID = new SelectList(db.States, "ID", "Name");
                int NewCountryID = db.CountryRegions.Where(s => s.Country == CountryID).Select(s => s.ID).FirstOrDefault();
                string NewStateID = Convert.ToString(db.States.Where(s => s.Name == StateID).Select(s => s.ID).FirstOrDefault());

                if (CategoryName != "All Categories")
                {
                    var searchresult = db.Products.Where(s => s.Name.Contains(item) && s.ProductStatu.Status == "Available" && s.Shop.ShopStatus == "Active" && s.ProductCategoryID == ProductCategoryID && s.Shop.CountryID == NewCountryID && s.Shop.StateID == NewStateID);
                    return View(searchresult.ToList());
                }
                else
                {
                    var searchresult = db.Products.Where(s => s.Name.Contains(item) && s.ProductStatu.Status == "Available" && s.Shop.ShopStatus == "Active" && s.Shop.CountryID == NewCountryID);
                    return View(searchresult.ToList());
                }
            }
            else
            {
                return View();
            }

        }

        public ActionResult BargainMarket(string item, string CountryID, string StateID, int? ProductCategoryID)
        {
            ViewBag.ProductCategoryID = new SelectList(db.ProductCategories, "ID", "Name", selectedValue: 4);
            ViewBag.LocationID = new SelectList(db.States, "ID", "Name", selectedValue: 38);
            ViewBag.CountryID = new SelectList(db.CountryRegions, "ID", "Country");
            if (ProductCategoryID != null)
            {
                ViewBag.CategoryName = db.ProductCategories.Find(ProductCategoryID).Name;
                ViewBag.Searchitem = item;
                string CategoryName = ViewBag.CategoryName;
                string random = Guid.NewGuid().ToString();
                ViewBag.ProductCategoryID = new SelectList(db.ProductCategories, "ID", "Name");
                ViewBag.LocationID = new SelectList(db.States, "ID", "Name");
                int NewCountryID = db.CountryRegions.Where(s => s.Country == CountryID).Select(s => s.ID).FirstOrDefault();
                string NewStateID = Convert.ToString(db.States.Where(s => s.Name == StateID).Select(s => s.ID).FirstOrDefault());
                if (CategoryName != "All Categories")
                {
                    var searchresult = db.Products.Where(s => s.Name.Contains(item) && s.ProductStatu.Status == "Available" && s.Shop.ShopStatus == "Active" && s.ProductCategoryID == ProductCategoryID && s.Shop.CountryID == NewCountryID && s.Shop.StateID == NewStateID && s.Negotiable == "Yes");
                    return View(searchresult.ToList());
                }
                else
                {
                    var searchresult = db.Products.Where(s => s.Name.Contains(item) && s.ProductStatu.Status == "Available" && s.Shop.ShopStatus == "Active" && s.Shop.CountryID == NewCountryID && s.Negotiable=="Yes");
                    return View(searchresult.ToList());
                }
            }
            else
            {
                return View();
            }

        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult AccessDenied()
        {
            ViewBag.Message = "Access Denied.";

            return View();
        }
        public ActionResult ShopNotFound()
        {
            ViewBag.Message = "SHOP NOT FOUND.";

            return View();
        }
        public ActionResult TermsandConditions()
        {
            ViewBag.Message = "Your Terms and Conditions description page.";

            return View();
        }

        public ActionResult Privacy()
        {
            ViewBag.Message = "Your Privacy Policy description page.";

            return View();
        }

        public ActionResult CustomerLogin(string returnUrl, int? ax, int?sid,int?pid,int?quantity,int?v,string src,string negotiate,int?dcity)
        {
            if(sid!=null)
            {
                ViewBag.SID = sid;
            }
            ViewBag.SID = sid;
            ViewBag.pid = pid;
            ViewBag.quantity = quantity;
            ViewBag.v = v;
            ViewBag.src = src;
            if (dcity != null)
            {
                ViewBag.dcity = dcity;
            }
            else
            {
                ViewBag.dcity = "NIL";
            }
            if (!string.IsNullOrEmpty(negotiate))
            {
                ViewBag.negotiate = negotiate;
            }
            else
            {
                ViewBag.negotiate ="";
            }

            if (ax != null)
            {
                ViewBag.ax = ax;
                ViewBag.AccountCreated = "Your account has been created successfully. Enter your login detail below to proceed.";
            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult CustomerLogin()
        {
            string Username = Request.Form["Username"];
            string Password = Request.Form["Password"];          

            int UserLogin = db.Users.Where(r => r.Email == Username || r.PhoneNumber==Username && r.Password == Password).Count();
            if (UserLogin > 0)
            {
                string UserLoggedIn = db.Users.Where(s => s.Email == Username || s.PhoneNumber == Username && s.Password == Password).Select(s => s.CompanyName).FirstOrDefault();
                string UserRole = db.Users.Where(s => s.Email == Username || s.PhoneNumber == Username && s.Password == Password).Select(s => s.UserRole.Role).FirstOrDefault();
                int UserID = db.Users.Where(s => s.Email == Username || s.PhoneNumber == Username && s.Password == Password).Select(s => s.ID).FirstOrDefault();
                Session["UserRole"] = UserRole;
                Session["Name"] = UserLoggedIn;
                Session["username"] = Username;
                Session["UserID"] = UserID;

                if (Request.Form["pid"] != null)
                {
                    int ShopID = Convert.ToInt32(Request.Form["shopid"]);
                    ViewBag.SID = Request.Form["shopid"];
                    ViewBag.v = Request.Form["Visitor"];
                    ViewBag.pid = Request.Form["pid"];
                    ViewBag.quantity = Request.Form["quantity"];

                    int? v = Convert.ToInt32(Request.Form["Visitor"]);
                    int? pid = Convert.ToInt32(Request.Form["pid"]);
                    int? quantity = Convert.ToInt32(Request.Form["quantity"]);
                    string src = Request.Form["src"];
                    string negotiate = Request.Form["negotiate"];
                    string dcity = Request.Form["dcity"];
                  
                    if (pid != null && quantity != null)
                    {
                        if (!string.IsNullOrEmpty(negotiate))
                        {
                        return Redirect("~/Products/AddtoCart?pid=" + pid + "&sid=" + ShopID + "&qty=" + quantity + "&src=" + src + "&dcity=" + dcity + "&negotiate=true");
                        }
                        else
                        {
                           
                       return Redirect("~/Products/AddtoCart?pid=" + pid + "&sid=" + ShopID + "&qty=" + quantity + "&src=" + src + "&dcity=" + dcity);
                           
                        }
                    }
                }
                if (Request.Form["shopid"]!=null)
                {
                    int ShopID = Convert.ToInt32(Request.Form["shopid"]);

                    if (db.CustomerShops.Where(s => s.ShopID == ShopID && s.UserID == UserID).Count()< 1)
                    {
                        //INSERT SHOPID INTO CUSTOMERS SHOP
                        CustomerShop custs = new CustomerShop();
                        custs.ShopID = ShopID;
                        custs.ShopCustomerID = UserID;
                        custs.Datecreated = DateTime.Now;
                        db.CustomerShops.Add(custs);
                        db.SaveChanges();
                    }
                }
                return Redirect("~/Users/CustomerDashboard/" + UserID);
            }
            else
            {
                ViewBag.UserNotExist = " Access denied, try again";
                return View();
            }


        }
        // POST: /Account/LogOff
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            Session["UserID"] = null;
            Session["Name"] = null;
            Session["username"] = null;
            Session["password"] = null;
            return Redirect("~/Account/Login");
        }

        public ActionResult LogOffCustomer()
        {
            Session["CustomerRole"] = null;
            Session["CustomerName"] = null;
            Session["Customerusername"] = null;
            Session["CustomerID"] = null;
            return Redirect("~/Home/CustomerLogin");
        }
        


        ////Forgot Password
        //public ActionResult ForgotPassword()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult ForgotPassword(string Email)
        //{
        //    string message = "";
        //    string resetCode = Guid.NewGuid().ToString();
            

        //    var user = db.Users.Where(a => a.Email == Email).FirstOrDefault();
        //    var verifyUrl = "http://myvshoponline.com/Home/ResetPassword/" + resetCode;
            
        //    if (user != null)
        //    {                
        //        //send Email for Reset password 

        //        string subject = "Reset Password";

        //        string body = "Hi <br/>We have got request for your reset password. Please click on the link below to reset your password.'" + "' <br/><br/><a href='" + verifyUrl + "' target='_blank'> Reset Password Link </a> ";
                
        //        SendMail(Email, subject, body);

        //        //user.ResetPasswordCode = resetCode;
        //        user.Password = Convert.ToString(resetCode);
        //        db.Configuration.ValidateOnSaveEnabled = false;                
        //        db.SaveChanges();
        //        message = "Reset Password Link has been sent to your Email, Logon to your Email to continue the process.";

        //    }

        
        //    else
        //    {
        //        message = "Account not found";
                
        //    }

        //    ViewBag.Message = message;
            

        //    return View();

        //}



     
        public JsonResult AutoCompleteSearch(string term)
        {
            var result = (from r in db.Products
                          where r.Name.ToLower().Contains(term)
                          select new { label = r.Name, Id = r.ID }).Distinct();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AutoLoadCountries(string term)
        {
            var result = (from r in db.CountryRegions
                          where r.Country.ToLower().Contains(term)
                          select new { label = r.Country, Id = r.ID }).Distinct();
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult AutoLoadStates(string term)
        {
            var result = (from r in db.States
                          where r.Name.ToLower().Contains(term)
                          select new { label = r.Name, Id = r.ID }).Distinct();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Insert_Email_NewsLetter(string email)
        {
            var result = email;
            DateTime date = DateTime.Now;
            //GET CORRECT ANSWER
           mydata.Insert_Email_NewsLetter(email, date);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }


        [HttpPost]
        public ActionResult ForgotPassword(string Email)
        {
            var user = db.Users.Where(a => a.Email == Email).FirstOrDefault();
            int usercount = db.Users.Where(a => a.Email == Email).Count();
            if (usercount > 0)
            {

                int RegisterOTP_Numeric = mydata.Generate_OTP_Numeric();
                Session["OTP_Resetpw"] = RegisterOTP_Numeric;
                //==================SEND MAIL============================//
                string title = "Password Reset!";
                string msg = "<a href='https://xamagos.com' title='Xamagos'> <img src='https://xamagos.com/Images/logosquare.png' style='width: 160px; height: 50px;background-color:#17A2B8' /></a>" + "<hr><strong>" + " Hello " + user.CompanyName + "</strong><br><br>";
                msg += "Please enter the code <b>" + RegisterOTP_Numeric + "</b> to reset your password on Xamagos.";
                msg += "<br>This code expires in 10 minutes, do not share it with others.";
                msg += "Regards:  Xamagos" + "<br>" + "Email: support@xamagos.com";
                mydata.SendMail(user.Email, title, msg);

                //string title = "Password Reset";
                //string msg = "<a href='http://marketsquare247.com' title='Market Square'> <img src='http://marketsquare247.com/Images/logosquare.png' style='width: 65px; height: 35px' /></a>" + "<hr>"+" Dear " + user.CompanyName + "<br><br>" +
                //"Recently a request was submitted to reset your password on Market Square. If you did not request this, please ignore this email." + "<br/>" +
                //"To reset your password, please visit the url below:" + "<br>" +
                //"http://marketsquare247.com/Home/ResetPassword?key=" + user.resetkey + "<br/><br/>" +
                //"When you visit the link above, you will have the opportunity to choose a new password." + "<hr/>" +
                //"From:  Market Square" +"<br/>"+
                //"Email: info@marketsqaure247.com";
                //mydata.SendMail(Email, title, msg);
                ViewBag.MailSent = "An OTP has been sent to your email. Enter OTP to reset your password.";
                return Redirect("~/Home/ResetPassword?q=" + ViewBag.MailSent+"&em="+Email);

            }
            else
            {
                ViewBag.EmailNotExist = "Account not found!";
                return View();
            }
             
        }

        public ActionResult ResetPassword(string q,string em)
        {
          
            return View();
    }


        [HttpPost]
        public ActionResult ResetPassword()
        {

            int OTP = Convert.ToInt32(Request.Form["OTP"]);
            string Email = Request.Form["email"];
            string Password = Request.Form["Password"];
            if (Convert.ToInt32(Session["OTP_Resetpw"]) == OTP)
            {
                int usercount = db.Users.Where(a => a.Email == Email).Count();
                if (usercount > 0)
                {
                    int userID = db.Users.Where(a => a.Email == Email).Select(a => a.ID).FirstOrDefault();
                    var u = db.Set<User>().Find(userID);
                    u.Password = Password;
                    db.SaveChanges();
                    ViewBag.PasswordReset = "Password updated successfully";
                    return View();
                }
                else
                {
                    ViewBag.NotFound = "Account not found!";
                    return View();
                }
            }
            else
            {
                ViewBag.NotFound = "Invalid OTP!";
                return View();
            }
        }


 public ActionResult ForgotPasswordCustomer()
        {
            return View();
    }


    [HttpPost]
    public ActionResult ForgotPasswordCustomer(string Email)
    {
        var user = db.ShopCustomers.Where(a => a.Email == Email).FirstOrDefault();
        int usercount = db.Users.Where(a => a.Email == Email).Count();
        if (usercount > 0)
        {
                int RegisterOTP_Numeric = mydata.Generate_OTP_Numeric();
                Session["OTP_Resetpw_buyer"] = RegisterOTP_Numeric;
                //==================SEND MAIL============================//
                string title = "Password Reset!";
                string msg = "<a href='https://marketsquare247.com' title='Market Square247'> <img src='https://marketsquare247.com/Images/logosquare.png' style='width: 120px; height: 60px' /></a>" + "<hr><strong>" + " Hello " + user.Name + "</strong><br><br>";
                msg += "Please enter the code <b>" + RegisterOTP_Numeric + "</b> to reset your password on Market Square247.";
                msg += "<br>This code expires in 10 minutes, do not share it with others.";
                msg += "Regards:  Market Square247" + "<br>" + "Email: info@marketsqaure247.com";
                mydata.SendMail(Email, title, msg);
                //string msg = "<a href='http://marketsquare247.com' title='Market Square'> <img src='http://marketsquare247.com/Images/logosquare.png' style='width: 65px; height: 35px' /></a>" + "<hr>" + " Dear " + user.Name + ",<br><br>" +
                //"Recently a request was submitted to reset your password on Market Square. If you did not request this, please ignore this email." + "<br/>" +
                //"To reset your password, please visit the url below:" + "<br><br>" +
                //"<a href='http://marketsquare247.com/Home/ResetPasswordCustomer?key=" + user.resetkey + "'</a><br><br>" +
                //"When you visit the link above, you will have the opportunity to choose a new password." + "<hr>" +
                //"From:  Market Square" + "<br>" +
                //"Email: info@marketsqaure247.com";
                //mydata.SendMail(Email, title, msg);

                ViewBag.MailSent = "An OTP has been sent to your email. Enter OTP to reset your password.";
                return Redirect("~/Home/ResetPasswordCustomer?q=" + ViewBag.MailSent + "&em=" + Email);


            }
            else
        {
            ViewBag.EmailNotExist = "Account not found!";
            return View();
        }
 
    }

    public ActionResult ResetPasswordCustomer(string q,string em)
    {
        return View();
    }


    [HttpPost]
    public ActionResult ResetPasswordCustomer()
    {
            int OTP = Convert.ToInt32(Request.Form["OTP"]);
            string Email = Request.Form["Email"];
            string Password = Request.Form["Password"];
            if (Convert.ToInt32(Session["OTP_Resetpw_buyer"]) == OTP)
            {
                int usercount = db.ShopCustomers.Where(a => a.Email == Email).Count();
                if (usercount > 0)
                {
                    int userID = db.ShopCustomers.Where(a => a.Email == Email).Select(a => a.ID).FirstOrDefault();
                    var u = db.Set<ShopCustomer>().Find(userID);
                    u.Password = Password;
                    db.SaveChanges();
                    ViewBag.PasswordReset = "Password updated successfully";
                    return View();
                }
                else
                {
                    ViewBag.NotFound = "Account not found!";
                    return View();
                }
            }
            else
            {
                ViewBag.NotFound = "Invalid OTP!";
                return View();
            }

        }
        public ActionResult Promotion()
        {
            return View();
        }
        public ActionResult Pricing()
        {
            return View();
        }

        public ActionResult ReverseGeocoding()
        {
            return View();
        }

        public ActionResult Goecoding()
        {
            return View();
        }
        public ActionResult Geolocation()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult GoeolocationUserPosition()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult NearbyStores()
        {
            return View();
        }

        public ActionResult StoreLocator()
        {
            return View();
        }

        public ActionResult PlacesSearch()
        {
            return View();
        }

        public ActionResult ProductNotFound()
        {
            return View();
        }


    }
}
