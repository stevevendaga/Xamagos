using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Myvshoponline;

namespace Myvshoponline.Controllers
{
    public class ShopCustomersController : Controller
    {
        private MyvshoponlineEntities db = new MyvshoponlineEntities();
        Getdata mydata = new Getdata();
        // GET: ShopCustomers
        public ActionResult Index(string search)
        {
            int id = Convert.ToInt32(Session["ShopID"]);
            ViewBag.SID = id;
            ViewBag.ShopID = id;
            ViewBag.UserID = db.Shops.Where(s => s.ID == id).Select(s => s.User.ID).FirstOrDefault();
            ViewBag.ShopName = db.Shops.Where(s => s.ID == id).Select(s => s.Name).FirstOrDefault();
            ViewBag.CompanyName=db.Shops.Where(s => s.ID == id).Select(s => s.User.CompanyName).FirstOrDefault();
            if (mydata.Is_ShopAdmin((string)Session["username"], (string)Session["UserRole"]) && db.Shops.Find(id).UserID == (int)Session["UserID"] || mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                if (Session["ShopID"] != null && search==null)
                {
                    return View(db.CustomerShops.Where(s => s.ShopID == id).OrderByDescending(s=>s.Datecreated).ToList());
                }
                else
                {
                    return View(db.CustomerShops.Where(s => s.ShopID == id && (s.ShopCustomer.Name.Contains(search) || s.ShopCustomer.PhoneNumber.Contains(search) || s.ShopCustomer.Email.Contains(search))).ToList());
                }
            }
            return Redirect("~/Home/AccessDenied");
        }

        // GET: ShopCustomers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            ShopCustomer shopCustomer = db.ShopCustomers.Find(id);
            if (shopCustomer == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            ViewBag.ShopID = Request.QueryString["sid"];
            return View(shopCustomer);
        }

        // GET: ShopCustomers/Create
        public ActionResult Create(int?v,int?pid,int?quantity,int?cid,string src,string negotiate,int? dcity)
        {
          int id = Convert.ToInt32(Session["ShopID"]);
            if (cid!=null && Session["ShopID"] != null)
            {
               
                if (db.CustomerShops.Where(s => s.ShopID == id && s.ShopCustomerID == cid).Count() < 1)
                {
                    //INSERT SHOPID INTO CUSTOMERS SHOP
                    CustomerShop custs = new CustomerShop();
                    custs.ShopID = id;
                    custs.ShopCustomerID = cid;
                    custs.Datecreated = DateTime.Now;
                    db.CustomerShops.Add(custs);
                    db.SaveChanges();
                }
                return Redirect("~/Users/CustomerDashboard/" + cid);
            }
            if (!string.IsNullOrEmpty(negotiate))
            {
                ViewBag.negotiate = "true";
            }
            else
            {
                ViewBag.negotiate = "";
            }
            if (id != null)
            {
                ViewBag.SID = id;
            }
            else
            {
                ViewBag.SID = 0;
            }
            if (pid != null)
            {
                ViewBag.pid = pid;
            }
            else
            {
                ViewBag.pid = "NIL";
            }
            if (quantity != null)
            {
                ViewBag.quantity = quantity;
            }
            else
            {
                ViewBag.quantity = "NIL";
            }
            if(src!=null)
            {
                ViewBag.src = src;
            }
            else
            {
                ViewBag.src = "NIL";
            }
            
            if (dcity != null)
            {
                ViewBag.dcity = dcity;
            }else
            {
                ViewBag.dcity = "NIL";
            }
            if(v!=null)
            {
                ViewBag.v = v;
            }
            else
            {
                ViewBag.v = "NIL";
            }
            ViewBag.UserID = db.Shops.Where(s => s.ID == id).Select(s => s.User.ID).FirstOrDefault();
            ViewBag.ShopName = db.Shops.Where(s => s.ID == id).Select(s => s.Name).FirstOrDefault();
            ViewBag.ShopID = id;
            ViewBag.CompanyName = db.Shops.Where(s => s.ID == id).Select(s => s.User.CompanyName).FirstOrDefault();
          
            ViewBag.CustomerStatusID = new SelectList(db.CustomerStatus, "ID", "Status");
            return View();
        }

        // POST: ShopCustomers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,PhoneNumber,Email,Address,Username,Password,DateCreated,CustomerStatusID")] ShopCustomer shopCustomer)
        {
            
            ViewBag.SID = Request.Form["ShopID"];
            ViewBag.v = Request.Form["Visitor"];
            ViewBag.pid = Request.Form["pid"];
            ViewBag.quantity = Request.Form["quantity"];
            ViewBag.src = Request.Form["src"];
            ViewBag.negotiate = Request.Form["negotiate"];
            ViewBag.dcity = Request.Form["dcity"];
            int? ShopID =Convert.ToInt32(Request.Form["ShopID"]);
            string v =Request.Form["Visitor"];
            string pid =Request.Form["pid"];
            string quantity =Request.Form["quantity"];
            string src =Request.Form["src"];
            string negotiate = Request.Form["negotiate"];
            string dcity = Request.Form["dcity"];
            // ViewBag.admin =Convert.ToInt32(Request.Form["admin"]);

            ViewBag.UserID = db.Shops.Where(s => s.ID == ShopID).Select(s => s.User.ID).FirstOrDefault();
            ViewBag.ShopName = db.Shops.Where(s => s.ID == ShopID).Select(s => s.Name).FirstOrDefault();
            ViewBag.CompanyName = db.Shops.Where(s => s.ID == ShopID).Select(s => s.User.CompanyName).FirstOrDefault();
            

            if (ModelState.IsValid)
            {
                //if (db.ShopCustomers.Where(s => s.Email == shopCustomer.Email).Count() > 0)
                //{
                //    ViewBag.EmailExist = "Email already used by another customer!";

                //    return View(shopCustomer);
                //}
                //else if (db.ShopCustomers.Where(s => s.PhoneNumber == shopCustomer.PhoneNumber).Count() > 0)
                //{
                //    ViewBag.PhoneExist = "Phone Number already used by another customer!";
                //    return View(shopCustomer);
                //}
                //else
                //{
                //    string resetkey = Guid.NewGuid().ToString();
                //    shopCustomer.CustomerStatusID = db.CustomerStatus.Where(c => c.Status == "Active").Select(c => c.ID).FirstOrDefault();
                //    shopCustomer.UserRole = db.UserRoles.Where(c => c.Role == "Shop Customer").Select(c => c.ID).FirstOrDefault();
                //    shopCustomer.DateCreated = DateTime.Now;
                //    shopCustomer.resetkey = resetkey;
                //    db.ShopCustomers.Add(shopCustomer);
                //    db.SaveChanges();
                //    // if((string)Session["username"]!=null && ((string)Session["UserRole"] == "Shop Admin" || (string)Session["UserRole"] == "Super Admin") && (int)Session["UserID"] ==(int)shopCustomer.Shop.UserID)               
                //        if (db.CustomerShops.Where(s => s.ShopID ==ShopID  && s.ShopCustomerID == shopCustomer.ID).Count() < 1 && ShopID==0)
                //        {
                //           //INSERT SHOPID INTO CUSTOMERS SHOP
                //            CustomerShop custs = new CustomerShop();
                //            custs.ShopID = ShopID;
                //            custs.ShopCustomerID = shopCustomer.ID;
                //            custs.Datecreated = DateTime.Now;
                //            db.CustomerShops.Add(custs);
                //            db.SaveChanges();
                //        }

                //        string UserLoggedIn = shopCustomer.Name;
                //        string UserRole = db.ShopCustomers.Where(s => s.Email == shopCustomer.Email && s.Password == shopCustomer.Password).Select(s => s.UserRole1.Role).FirstOrDefault();
                //        int UserID = db.ShopCustomers.Where(s => s.Email == shopCustomer.Email && s.Password == shopCustomer.Password).Select(s => s.ID).FirstOrDefault();
                //        Session["CustomerRole"] = UserRole;
                //        Session["CustomerName"] = UserLoggedIn;
                //        Session["Customerusername"] = shopCustomer.Email;
                //        Session["CustomerID"] = UserID;
                //END SAVE RECORDS INTO SHOPCUSTOMERS////
                //SAVE RECORDS INTO USERS TABLE
                if (db.Users.Where(s => s.Email == shopCustomer.Email).Count() > 0)
                {
                    ViewBag.EmailExist = "Email already used by another customer!";

                    return View(shopCustomer);
                }
                else if (db.Users.Where(s => s.PhoneNumber == shopCustomer.PhoneNumber).Count() > 0)
                {
                    ViewBag.PhoneExist = "Phone Number already used by another customer!";
                    return View(shopCustomer);
                }
                else
                {
                    int planid = db.PricingPlans.Where(p => p.PlanName == "FREE PLAN").Select(p => p.ID).FirstOrDefault();
                    int billingid = db.BillingCycles.Where(p => p.Cycle == "Free").Select(p => p.ID).FirstOrDefault();
                    int TotalNumberofdays = (int)db.TrialPeriods.Select(t => t.Period).FirstOrDefault();
                    //Save Records into users here
                    string resetkey = Guid.NewGuid().ToString();
                    User users = new User();
                    users.CompanyName = shopCustomer.Name.TrimEnd().TrimStart();
                    users.PhoneNumber = shopCustomer.PhoneNumber;
                    users.Email = shopCustomer.Email;
                    users.Address = shopCustomer.Address;
                    users.Password = shopCustomer.Password;
                    users.Username = shopCustomer.Email;
                    users.resetkey = shopCustomer.resetkey;
                    users.UserRoleID = db.UserRoles.Where(c => c.Role == "Shop Admin").Select(c => c.ID).FirstOrDefault();
                    users.DateCreated = DateTime.Now;
                    users.resetkey = resetkey;
                    users.PaymentStatus = 0;
                    users.PlanID = planid;
                    users.BillingCycleID = billingid;

                    users.LaunchDate = DateTime.Now;
                    users.PreviousDate = DateTime.Now;
                    users.LaunchYear = DateTime.Now.Year;
                    users.LaunchMonth = DateTime.Now.Month;
                    users.LaunchDay = DateTime.Now.Day;

                    users.PreviousYear = DateTime.Now.Year;
                    users.PreviousMonth = DateTime.Now.Month;
                    users.PreviousDay = DateTime.Now.Day;
                    users.NDays = 0;
                    users.FixedDays = TotalNumberofdays;
                    users.EmailVerify = 1;
                    users.HardToken = Convert.ToString(mydata.Generate_OTP_Numeric());
                    db.Users.Add(users);
                    db.SaveChanges();
                    // if((string)Session["username"]!=null && ((string)Session["UserRole"] == "Shop Admin" || (string)Session["UserRole"] == "Super Admin") && (int)Session["UserID"] ==(int)shopCustomer.Shop.UserID)               
                    if (db.CustomerShops.Where(s => s.ShopID == ShopID && s.UserID == users.ID).Count() < 1 && ShopID == 0)
                    {
                        //INSERT SHOPID INTO CUSTOMERS SHOP
                        CustomerShop custs = new CustomerShop();
                        custs.ShopID = ShopID;
                        custs.ShopCustomerID = shopCustomer.ID;
                        custs.Datecreated = DateTime.Now;
                        db.CustomerShops.Add(custs);
                        db.SaveChanges();
                    }

                    string UserLoggedIn = shopCustomer.Name;
                    string UserRole = db.Users.Where(s => s.Email == shopCustomer.Email && s.Password == shopCustomer.Password).Select(s => s.UserRole.Role).FirstOrDefault();
                    int UserID = db.Users.Where(s => s.Email == shopCustomer.Email && s.Password == shopCustomer.Password).Select(s => s.ID).FirstOrDefault();
                    //Session["CustomerRole"] = UserRole;
                    //Session["CustomerName"] = UserLoggedIn;
                    //Session["Customerusername"] = shopCustomer.Email;
                    //Session["CustomerID"] = UserID;

                    Session["UserRole"] = UserRole;
                    Session["Name"] = UserLoggedIn;
                    Session["username"] = shopCustomer.Email;
                    Session["UserID"] = UserID;

                    //==================SEND MAIL============================//
                    string title = "XAMAGOS welcomes you";
                    string msg = "<a href='https://marketsquare247.com' title='XAMAGOS'> <img src='https://marketsquare247.com/Images/logosquare.png' style='width: 100px; height: 100px' /></a>" + "<hr><strong>" + " Hello " + shopCustomer.Name + "</strong><br><br>";
                    msg += "Congratulations! Your account has been created successfully.";
                    msg += "<br>Enjoy the world of XAMAGOS.";
                    msg += "Best Regards:  XAMAGOS" + "<br>" + "Email: info@marketsqaure247.com";
                   // mydata.SendMail(shopCustomer.Email, title, msg);
                    //==================SEND SMS=========================//
                    string sms = "XAMAGOS welcomes you. Hello " + shopCustomer.Name + ", your account has been created successfully.";
                    sms += "Enjoy the world of XAMAGOS";
                    //mydata.Send_SMS_KudiSMS(shopCustomer.PhoneNumber, sms, "XAMAGOS", DateTime.Now);
                    //==================END SEND SMS=========================//

                    if (!string.IsNullOrEmpty(negotiate) && pid!="NIL")
                    {
                   return Redirect("~/Products/AddtoCart?pid=" + pid + "&sid=" + ShopID + "&qty=" + quantity + "&dcity=" + dcity + "&src=" + src + "&negotiate=true");
                    }
                    else if(string.IsNullOrEmpty(negotiate) && pid != "NIL")
                    {
                        return Redirect("~/Products/AddtoCart?pid=" + pid + "&sid=" + ShopID + "&qty=" + quantity + "&src=" + src + "&dcity=" + dcity);
                    }
                    else
                    {
                        //return Redirect("~/Users/CustomerDashboard/"+shopCustomer.ID);
                        return Redirect("~/Users/BusinessLanding/" + shopCustomer.ID);
                        
                    }


                   
                }
            }
            ViewBag.CustomerStatusID = new SelectList(db.CustomerStatus, "ID", "Status", shopCustomer.CustomerStatusID);
            return View(shopCustomer);
        }


        public ActionResult CreateAdmin(int? id)
        {
            ViewBag.SID = id;
            ViewBag.UserID = db.Shops.Where(s => s.ID == id).Select(s => s.User.ID).FirstOrDefault();
            ViewBag.ShopName = db.Shops.Where(s => s.ID == id).Select(s => s.Name).FirstOrDefault();
            ViewBag.ShopID = id;
            ViewBag.CompanyName = db.Shops.Where(s => s.ID == id).Select(s => s.User.CompanyName).FirstOrDefault();
            ViewBag.CustomerStatusID = new SelectList(db.CustomerStatus, "ID", "Status");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAdmin([Bind(Include = "ID,Name,PhoneNumber,Email,Address,Username,Password,DateCreated,CustomerStatusID")] ShopCustomer shopCustomer)
        {

            ViewBag.SID = Request.Form["ShopID"];
            int? ShopID = Convert.ToInt32(Request.Form["ShopID"]);

            ViewBag.UserID = db.Shops.Where(s => s.ID == ShopID).Select(s => s.User.ID).FirstOrDefault();
            ViewBag.ShopName = db.Shops.Where(s => s.ID == ShopID).Select(s => s.Name).FirstOrDefault();
            ViewBag.CompanyName = db.Shops.Where(s => s.ID == ShopID).Select(s => s.User.CompanyName).FirstOrDefault();


            if (ModelState.IsValid)
            {
                if (db.ShopCustomers.Where(s => s.Email == shopCustomer.Email).Count() > 0)
                {
                    ViewBag.EmailExist = "Email already used by another customer!";

                    return View(shopCustomer);
                }
                else if (db.ShopCustomers.Where(s => s.PhoneNumber == shopCustomer.PhoneNumber).Count() > 0)
                {
                    ViewBag.PhoneExist = "Phone Number already used by another customer!";
                    return View(shopCustomer);
                }
                else
                {
                    string resetkey = Guid.NewGuid().ToString();
                    shopCustomer.CustomerStatusID = db.CustomerStatus.Where(c => c.Status == "Active").Select(c => c.ID).FirstOrDefault();
                    shopCustomer.UserRole = db.UserRoles.Where(c => c.Role == "Shop Customer").Select(c => c.ID).FirstOrDefault();
                    shopCustomer.DateCreated = DateTime.Now;
                    shopCustomer.resetkey = resetkey;
                    db.ShopCustomers.Add(shopCustomer);
                    db.SaveChanges();
                    // if((string)Session["username"]!=null && ((string)Session["UserRole"] == "Shop Admin" || (string)Session["UserRole"] == "Super Admin") && (int)Session["UserID"] ==(int)shopCustomer.Shop.UserID)               
                    if (db.CustomerShops.Where(s => s.ShopID == ShopID && s.ShopCustomerID == shopCustomer.ID).Count() < 1)
                    {
                        //INSERT SHOPID INTO CUSTOMERS SHOP
                        CustomerShop custs = new CustomerShop();
                        custs.ShopID = ShopID;
                        custs.ShopCustomerID = shopCustomer.ID;
                        custs.Datecreated = DateTime.Now;
                        db.CustomerShops.Add(custs);
                        db.SaveChanges();
                    }
                 return Redirect("~/ShopCustomers/Index/" + ShopID);
                }
            }
            ViewBag.CustomerStatusID = new SelectList(db.CustomerStatus, "ID", "Status", shopCustomer.CustomerStatusID);
            return View(shopCustomer);
        }

        // GET: ShopCustomers/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.SID = id;
            ViewBag.UserID = db.Shops.Where(s => s.ID == id).Select(s => s.User.ID).FirstOrDefault();
            ViewBag.ShopName = db.Shops.Where(s => s.ID == id).Select(s => s.Name).FirstOrDefault();
            ViewBag.ShopID = id;
            ViewBag.CompanyName = db.Shops.Where(s => s.ID == id).Select(s => s.User.CompanyName).FirstOrDefault();
            if (mydata.Is_ShopAdmin((string)Session["username"], (string)Session["UserRole"]) && db.Shops.Find(id).UserID == (int)Session["UserID"] )
            {
                if (id == null)
                {
                    return Redirect("~/Home/AccessDenied");
                }
                ShopCustomer shopCustomer = db.ShopCustomers.Find(id);
                if (shopCustomer == null)
                {
                    return Redirect("~/Home/AccessDenied");
                }
                ViewBag.CustomerStatusID = new SelectList(db.CustomerStatus, "ID", "Status", shopCustomer.CustomerStatusID);
                return View(shopCustomer);
            }
            return Redirect("~/Home/AccessDenied");
        }

        // POST: ShopCustomers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,PhoneNumber,Email,Address,Username,Password,DateCreated,CustomerStatusID")] ShopCustomer shopCustomer)
        {
            int ShopID = Convert.ToInt32(Request.Form["ShopID"]);

            if (ModelState.IsValid)
            {
                db.Entry(shopCustomer).State = EntityState.Modified;
                db.SaveChanges();
                return Redirect("~/ShopCustomers/Index/" + ShopID);
            }
            ViewBag.CustomerStatusID = new SelectList(db.CustomerStatus, "ID", "Status", shopCustomer.CustomerStatusID);
            ViewBag.ShopID = new SelectList(db.Shops, "ID", "Name", ShopID);
            return View(shopCustomer);
        }

        // GET: ShopCustomers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (mydata.Is_ShopAdmin((string)Session["username"], (string)Session["UserRole"]) && db.Shops.Find(id).UserID == (int)Session["UserID"] || mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                if (id == null)
                {
                    return Redirect("~/Home/AccessDenied");
                }
                ShopCustomer shopCustomer = db.ShopCustomers.Find(id);
                if (shopCustomer == null)
                {
                    return Redirect("~/Home/AccessDenied");
                }
                return View(shopCustomer);
            }
            return Redirect("~/Home/AccessDenied");
        }

        // POST: ShopCustomers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ShopCustomer shopCustomer = db.ShopCustomers.Find(id);
            db.ShopCustomers.Remove(shopCustomer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

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
