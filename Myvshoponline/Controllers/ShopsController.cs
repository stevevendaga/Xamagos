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
using System.Globalization;

namespace Myvshoponline.Controllers
{
    public class ShopsController : Controller
    {
        private MyvshoponlineEntities db = new MyvshoponlineEntities();
        Getdata mydata = new Getdata();
        TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
    // GET: Shops
    public ActionResult Index(string shopurl)
        {

            if (shopurl != null)
            {
                int countexistence = db.Shops.Where(s => s.ShopURL == shopurl && s.ShopStatus=="Active").Count();
                if (countexistence > 0)
                {
                    int shopid = db.Shops.Where(s => s.ShopURL == shopurl).Select(s => s.ID).FirstOrDefault();
                    return Redirect("~/Shops/ShopDetails/" + shopid);
                }
                else
                {
                    return Redirect("~/Home/ShopNotFound");
                }
            }
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                return View(db.Shops.ToList());
            }
            else
            {
             return Redirect("~/Home/AccessDenied");
            }
        }
        
        // GET: Shops/Details/5
        public ActionResult Details(int? id)
        {
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                if (id == null)
                {
                    return Redirect("~/Home/AccessDenied");
                }
                Shop shop = db.Shops.Find(id);
                if (shop == null)
                {
                    return Redirect("~/Home/AccessDenied");
                }
                return View(shop);
            }
            return Redirect("~/Home/AccessDenied");
        }

        public ActionResult ShopDetails(string search)
        {
            ViewBag.ProductCategoryID = new SelectList(db.ProductCategories, "ID", "Name");
            ViewBag.ProductStatusID = new SelectList(db.ProductStatus, "ID", "Status");
            ViewBag.SocialMediaChannel = new SelectList(db.SocialMediaChannels, "ID", "Channel");
            ViewBag.BankID = new SelectList(db.Banks, "ID", "Name");
            int id = Convert.ToInt32(Session["ShopID"]);
            if (Session["ShopID"] == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            Shop shop = db.Shops.Find(id);
            if (shop == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            if (search != null && Session["ShopID"] != null)
            {
                ViewBag.Products = db.ShopProductCategories.Where(s => s.ShopID == id).ToList().Take(10);
                ViewBag.search = search;
                ViewBag.Shopcount = db.Products.Where(p => p.ShopID == id && p.Shop.ShopStatus == "Active" && p.Name.Contains(search)).Count();
                ViewBag.Shopsearch = db.Products.Where(p => p.ShopID == id && p.Shop.ShopStatus == "Active" && p.Name.Contains(search)).ToList();
                ViewBag.UserID = db.Shops.Find(id).UserID;
                var ShopProducts1 = (from p in db.Products
                                    where p.ShopID == id
                                    select new { ProductName = p.Name, ID = p.ID }).ToList();
                ViewBag.ShopProducts = new SelectList(ShopProducts1, "ID", "ProductName");
                return View(shop);
            }
            int UserID =(int) db.Shops.Find(id).UserID;
            ViewBag.UserID = db.Shops.Find(id).UserID;
            ViewBag.Products = db.ShopProductCategories.Where(s => s.ShopID == id).ToList();
            //Get all products
            var ShopProducts=(from p in db.Products
                                 where p.ShopID==id
                                 select new {ProductName=p.Name,ID=p.ID}).ToList();
            ViewBag.ShopProducts = new SelectList(ShopProducts, "ID", "ProductName");

            ViewBag.CompanyID = UserID;
            ViewBag.Orders = db.Orders.Where(o => o.Product.Shop.UserID == UserID && o.Product.ShopID==id && o.OnCart == "No").Count();
            ViewBag.PendingPayment = db.Orders.Where(o => o.Product.Shop.UserID == UserID && o.Product.ShopID == id && o.PaymentStatusID == 0 && o.OnCart == "No").Count();
            ViewBag.PendingDelivery = db.Orders.Where(o => o.Product.Shop.UserID == UserID && o.Product.ShopID == id && o.PaymentStatusID == 1 && o.DeliveryStatus == null && o.OnCart == "No").Count();
            ViewBag.Delivered = db.Orders.Where(o => o.Product.Shop.UserID == UserID && o.Product.ShopID == id && o.PaymentStatusID == 1 && o.DeliveryStatus == "Delivered" && o.OnCart == "No").Count();
            ViewBag.CompanyName = db.Users.Where(u => u.ID == UserID).Select(u => u.CompanyName).FirstOrDefault();

            return View(shop);
        }



        // GET: Shops/Create
        public ActionResult Create()
        {
            if (!string.IsNullOrEmpty((string)Session["username"]))
            {
                int UserID = (int)Session["UserID"];
                ViewBag.CompanyName = db.Users.Find(UserID).CompanyName;
                ViewBag.StateID = new SelectList(db.States, "Name", "Name");
               ViewBag.CountryID = new SelectList(db.CountryRegions, "ID", "Country");
                ViewBag.UserID = new SelectList(db.Users, "ID", "CompanyName");
                return View();
            }
            else
            {
                return Redirect("~/Home/AccessDenied");
            }
        }

        // POST: Shops/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,UserID,PhoneNumber,Email,Address,DateCreated,ShopURL,Description,EnableOnlineOrder,ShopStatus,EnableOnlinePayment,CountryID")] Shop shop)
        {
            if (!string.IsNullOrEmpty((string)Session["username"]))
            {
                int UserID = (int)Session["UserID"];
                string PricingPlan = db.Users.Find(UserID).PricingPlan.PlanName;
                int NumberofShops =(int) db.PricingPlans.Where(p => p.PlanName == PricingPlan).Select(p => p.NumberofShops).FirstOrDefault();
                int TotalNumberofShops = db.Shops.Where(s => s.UserID == UserID).Count();
                
                    if (db.Shops.Where(s => s.Name == shop.Name && s.UserID == UserID).Count() > 0)
                    {
                        ViewBag.ShopExist = "This shop already exist!";
                        ViewBag.CompanyName = db.Users.Find(UserID).CompanyName;
                        ViewBag.StateID = new SelectList(db.States, "ID", "Name", shop.StateID);
                        ViewBag.CountryID = new SelectList(db.CountryRegions, "ID", "Country",shop.CountryID);
                        ViewBag.UserID = new SelectList(db.Users, "ID", "CompanyName", shop.UserID);
                        return View(shop);
                    }
                    else
                    {
                    //if (db.Shops.Where(s => s.ShopURL == shop.ShopURL).Count() > 0)
                    //{
                    //    ViewBag.ShopURL = "This shop URL has been taken by another user!";
                    //    ViewBag.CompanyName = db.Users.Find(UserID).CompanyName;
                    //    ViewBag.StateID = new SelectList(db.States, "ID", "Name", shop.StateID);
                    //    ViewBag.UserID = new SelectList(db.Users, "ID", "CompanyName", shop.UserID);
                    //    return View(shop);
                    //}
                    //else
                    //{
                            if (TotalNumberofShops < NumberofShops)
                            {
                                shop.EnableOnlineOrder = "Yes";
                                shop.EnableOnlinePayment = "Yes";
                                shop.ShopStatus = "Inactive";
                                shop.UserID = UserID;
                                shop.Name = shop.Name.TrimEnd().TrimStart();
                                shop.DateCreated = DateTime.Now;
                                string State = Request.Form["StateID"];
                                int StateID = db.States.Where(s => s.Name == State).Select(s => s.ID).FirstOrDefault();
                                string Country = Request.Form["CountryID"];
                                int CountryID = db.CountryRegions.Where(s => s.Country == Country).Select(s => s.ID).FirstOrDefault();
                                shop.StateID =Convert.ToString(StateID);
                                shop.CountryID =CountryID;
                                db.Shops.Add(shop);
                                db.SaveChanges();
                                var updateurl = db.Set<Shop>().Find(shop.ID);
                                 updateurl.ShopURL = shop.ID.ToString();
                                db.SaveChanges();
                    //=================ADD DELIVERY LOCATIONS =====================//
                    
                    foreach (var item in db.DeliveryCities.Where(s=>s.FromStateID==StateID).ToList())
                    {
                        mydata.Insert_Delevery_Cities((int)item.StateID, shop.ID,DateTime.Now);
                    }

                    //============================END DELIVERY LOCATIONS =============//
                            //}
                            //else
                            //{
                            //    ViewBag.ShopLimitExceeded = "Maximun number of shops reached for your current plan";
                            //    ViewBag.CompanyName = db.Users.Find(UserID).CompanyName;
                            //    ViewBag.StateID = new SelectList(db.States, "ID", "Name", shop.StateID);
                            //ViewBag.CountryID = new SelectList(db.CountryRegions, "ID", "Country", shop.CountryID);
                            //ViewBag.UserID = new SelectList(db.Users, "ID", "CompanyName", shop.UserID);
                            //    return View(shop);
                            //}
                            //Create Shop Folder in the business's directory
                            string businessname = db.Users.Where(s => s.ID == shop.UserID).Select(s => s.CompanyName).FirstOrDefault();
                            if (!Directory.Exists("~/BusinessImages/" + businessname + "/" + shop.Name))
                            {
                                Directory.CreateDirectory(Server.MapPath("~/BusinessImages/" + businessname + "/" + shop.Name));
                            }
                       }
                        return Redirect("~/Shops/ShopDetails/"+shop.ID);
                    }
                
            }
            ViewBag.StateID = new SelectList(db.States, "ID", "Name", shop.StateID);
            ViewBag.CountryID = new SelectList(db.CountryRegions, "ID", "Country", shop.CountryID);
            ViewBag.UserID = new SelectList(db.Users, "ID", "CompanyName", shop.UserID);
            return View(shop);
        }

        public JsonResult CreateAjax(string name, string description, string phone, string email, int countryid, string state, string city, string street, string shortname)
        {
            
                int UserID = (int)Session["UserID"];
                string PricingPlan = db.Users.Find(UserID).PricingPlan.PlanName;
                int NumberofShops = (int)db.PricingPlans.Where(p => p.PlanName == PricingPlan).Select(p => p.NumberofShops).FirstOrDefault();
                int TotalNumberofShops = db.Shops.Where(s => s.UserID == UserID).Count();

                if (db.Shops.Where(s => s.Name == name && s.UserID == UserID).Count() > 0)
                {
                    var result = from p in db.Shops
                                 select new { ShopExist = "true" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else if(db.Shops.Where(s=>s.ShopURL==shortname).Count()==1)
                {
                    var result = from p in db.Shops
                                 select new { ShortnameExist = "true" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                 }
                else
                {
                    if (TotalNumberofShops < NumberofShops)
                    {
                        Shop shop = new Shop();
                        shop.Name = name.TrimEnd().TrimStart();
                        shop.Description = description;
                        shop.PhoneNumber = phone;
                        shop.Email = email;
                      //  int CountryID = db.CountryRegions.Where(s => s.Country == country).Select(s => s.ID).FirstOrDefault();
                        shop.CountryID = countryid;
                      if (db.States.Where(s => s.Name == state).Count() > 0)
                       {
                        int StateID = db.States.Where(s => s.Name == state).Select(s => s.ID).FirstOrDefault();
                        string address = street + " " + city;
                        shop.Address = address;
                        shop.StateID = Convert.ToString(StateID);
                        }
                        else
                        {
                        string address = street + " " + city + " " + state;
                        shop.Address = address;
                        shop.StateID ="0";
                         }
                        shop.ShopURL = shortname;
                        shop.EnableOnlineOrder = "Yes";
                        shop.EnableOnlinePayment = "Yes";
                        shop.ShopStatus = "Inactive";
                        shop.UserID = UserID;
                        shop.DateCreated = DateTime.Now;
                        db.Shops.Add(shop);
                        db.SaveChanges();
          //=================ADD DELIVERY LOCATIONS =====================//
          //foreach (var item in db.DeliveryCities.Where(s => s.FromStateID == StateID).ToList())
          //{
          //    mydata.Insert_Delevery_Cities((int)item.StateID, shop.ID, DateTime.Now);
          //}
                    //Set session
                    Session["ShopID"] = shop.ID;
                    //foreach (var item in db.States.Where(s=>s.Name!= "All Locations within Nigeria").ToList())
                    //{
                    //    mydata.Insert_Delevery_Cities((int)item.ID, shop.ID, DateTime.Now);
                    //}

                    //Create Shop Folder in the business's directory
                    string businessname = db.Users.Where(s => s.ID == UserID).Select(s => s.CompanyName).FirstOrDefault();
                        string hardtoken = db.Users.Where(s => s.ID == UserID).Select(s => s.HardToken).FirstOrDefault();
                        if (!Directory.Exists("~/BusinessImages/" + businessname + hardtoken + "/" + shop.Name))
                        {
                            Directory.CreateDirectory(Server.MapPath("~/BusinessImages/" + businessname + hardtoken + "/" + shop.Name));
                        }
                    }
                    var result = from p in db.Shops
                                 select new { shopid = p.ID, RecordSaved = "true" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
        }
        // GET: Shops/Edit/5
        public ActionResult Edit(int? id)
        {
            if (mydata.Is_ShopAdmin((string)Session["username"], (string)Session["UserRole"]) && db.Shops.Find(id).UserID==(int)Session["UserID"] || mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                if (id == null)
                {
                    return Redirect("~/Home/AccessDenied");
                }
                Shop shop = db.Shops.Find(id);
                if (shop == null)
                {
                    return Redirect("~/Home/AccessDenied");
                }
                ViewBag.CountryID = new SelectList(db.CountryRegions, "ID", "Country", shop.CountryID);
                ViewBag.StateID = new SelectList(db.States, "ID", "Name", shop.StateID);
                ViewBag.UserID = new SelectList(db.Users, "ID", "CompanyName", shop.UserID);
                return View(shop);
            }
            return Redirect("~/Home/AccessDenied");
        }

        // POST: Shops/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,UserID,PhoneNumber,Email,Address,DateCreated,StateID,ShopURL,Description,EnableOnlineOrder,ShopStatus,CountryID,EnableOnlinePayment")] Shop shop)
        {
            int UserID = (int)Session["UserID"];
            if (ModelState.IsValid)
            {
                shop.UserID = UserID;
                //string State = Request.Form["StateID"];
                //int StateID = db.States.Where(s => s.Name == State).Select(s => s.ID).FirstOrDefault();
                //string Country = Request.Form["CountryID"];
                //int CountryID = db.CountryRegions.Where(s => s.Country == Country).Select(s => s.ID).FirstOrDefault();
                //shop.StateID = Convert.ToString(StateID);
                //shop.CountryID = CountryID;
                string state = Request.Form["StateID"];
                int StateID = db.States.Where(s => s.Name == state).Select(s => s.ID).FirstOrDefault();
                shop.StateID =Convert.ToString(StateID);
                db.Entry(shop).State = EntityState.Modified;
                db.SaveChanges();
                return Redirect("~/Users/Manageshop/"+UserID);
            }
            ViewBag.CountryID = new SelectList(db.CountryRegions, "ID", "Country", shop.CountryID);
            ViewBag.StateID = new SelectList(db.States, "ID", "Name", shop.StateID);
            ViewBag.UserID = new SelectList(db.Users, "ID", "CompanyName", shop.UserID);
            return View(shop);
        }

        // GET: Shops/Delete/5
        public ActionResult Delete(int? id)
        {
            if (mydata.Is_ShopAdmin((string)Session["username"], (string)Session["UserRole"]) && db.Shops.Find(id).UserID == (int)Session["UserID"] || mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                if (id == null)
                {
                    return Redirect("~/Home/AccessDenied");
                }
                Shop shop = db.Shops.Find(id);
                if (shop == null)
                {
                    return Redirect("~/Home/AccessDenied");
                }

                db.Shops.Remove(shop);
                db.SaveChanges();
                return Redirect("~/Users/Manageshop/" + shop.UserID);
            }
            return Redirect("~/Home/AccessDenied");
        }

        // POST: Shops/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Shop shop = db.Shops.Find(id);
            db.Shops.Remove(shop);
            db.SaveChanges();
            return Redirect("~/Users/Manageshop");
        }

        public ActionResult SelectOnlineOrderStatus(int shopid)
        {
            var result = (from r in db.Shops
                          where r.ID == shopid
                          select new { Status = r.EnableOnlineOrder }).Distinct();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EnableOnlineOrder(int shopid)
        {
            mydata.EnableOnlineOrder(shopid);
            //var shop = db.Set<Shop>().Find(shopid);
            //shop.EnableOnlineOrder = "Yes";
            //db.SaveChanges();
            var result = 1;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DisableOnlineOrder(int shopid)
        {
            mydata.DisableOnlineOrder(shopid);
            //var shop = db.Set<Shop>().Find(shopid);
            //shop.EnableOnlineOrder = "No";
            //db.SaveChanges();
            var result = 1;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EnableOnlinePayment(int shopid)
        {
            mydata.EnableOnlinePayment(shopid);
            //var shop = db.Set<Shop>().Find(shopid);
            //shop.EnableOnlineOrder = "Yes";
            //db.SaveChanges();
            var result = 1;
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult DisableOnlinePayment(int shopid)
        {
            mydata.DisableOnlinePayment(shopid);
            //var shop = db.Set<Shop>().Find(shopid);
            //shop.EnableOnlinePayment = "No";
            //db.SaveChanges();
            var result = 1;
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult EnableShop(int shopid)
        {

            var shop = db.Set<Shop>().Find(shopid);
            shop.ShopStatus = "Active";
            db.SaveChanges();
            var result = 1;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DisableShop(int shopid)
        {

            var shop = db.Set<Shop>().Find(shopid);
            shop.ShopStatus = "Inactive";
            db.SaveChanges();
            var result = 1;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public JsonResult GetCountryName(int CountryID)
        {
            var result = (from r in db.CountryRegions.Where(s => s.ID == CountryID)
                          select new { CountryName = r.Country}).Distinct();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStates(int CountryID)
        {

            var result = (from r in db.States
                          select new { ID = r.ID, StateName = r.Name}).Distinct();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckState(string state)
        {
            int count = db.States.Where(s => s.Name == state).Count();
            var result = (from r in db.States
                          select new {count=count}).Distinct();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckCountry(string country)
        {
            int count = db.CountryRegions.Where(s => s.Country == country).Count();
            var result = (from r in db.CountryRegions
                          select new { count = count}).Distinct();
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        public JsonResult GetChannelPrice(int id)
        {

            var result = (from r in db.SocialMediaChannels
                          where r.ID==id
                          select new { ChannelAmount = r.CostPerDay }).Distinct();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Update_Payment_Promote_Popular_Store(string refno, decimal amount, int ShopID,int noofdays, string shopemail,string shopname,string enddate)

        {
            //Insert into popular store
            PopularStore store = new PopularStore();
            store.ShopID = ShopID;
            store.PopularStoreStatusID = db.PopularStoreStatus.Where(s => s.Status == "Active").Select(s => s.ID).FirstOrDefault();
            store.PaymentStatus = 1;
            store.RefNo = refno;
            store.Amount = amount;
            store.DatePaid = DateTime.Now;
            store.DaysActive = 0;
            store.TotalDaysPaid = noofdays;
            store.LaunchYear = DateTime.Now.Year;
            store.LaunchMonth = DateTime.Now.Month;
            store.LaunchDay = DateTime.Now.Day;
            store.PreviousYear = DateTime.Now.Year;
            store.PreviousMonth = DateTime.Now.Month;
            store.PreviousDay = DateTime.Now.Day;
            store.EndDate = enddate;
            db.PopularStores.Add(store);
            db.SaveChanges();
            string msg = "<a href='https://xamagos.com' title='Xamagos'> <img src='https://xamagos.com/Images/logosquare.png' style='width: 160px; height: 50px;background-color:#17A2B8' /></a><hr>Hello! " + shopname + "<br/>" +
                " You have successfully promoted your store. <br> Your transaction details <br/>" +
                " Store: " + shopname + "<br/>" +
                " Duration: " + noofdays + " days <br/>" +
                " Reference Number: " + refno + "<br/>" +
                " Amount Paid: " + amount / 100 + "<br/>" +
                " Website: https://xamagos.com <br/>" +
                "From Xamagos";
            mydata.SendMail(shopemail, "Payment made successfully", msg);

            return Redirect("/Hone/Index/");
        }


        
public void Update_Payment_Promote_Social_Media(string refno, decimal amount, int ShopID, int noofdays, string shopemail, string shopname, string enddate,int socialmediachannelID,string country,string locations,string audience,int productid)
    {
            //Insert into SocialMediaAds
            SocialMediaAd store = new SocialMediaAd();
            store.ShopID = ShopID;
            store.SocialMediaChannelID = socialmediachannelID;
            store.AdsStatus = db.PopularStoreStatus.Where(s => s.Status == "Pending").Select(s => s.ID).FirstOrDefault();
            store.PaymentStatus = 1;
            store.RefNo = refno;
            store.AmountPaid = amount;
            store.DatePaid = DateTime.Now;
            store.DaysActive = 0;
            store.TotalDaysPaid = noofdays;
            store.Country = country;
            store.Locations = locations;
            store.TargetPeople = audience;
            store.ProductID = productid;
            //store.LaunchYear = DateTime.Now.Year;
            //store.LaunchMonth = DateTime.Now.Month;
            //store.LaunchDay = DateTime.Now.Day;
            //store.PreviousYear = DateTime.Now.Year;
            //store.PreviousMonth = DateTime.Now.Month;
            //store.PreviousDay = DateTime.Now.Day;
            store.EndDate = enddate;
            db.SocialMediaAds.Add(store);
            db.SaveChanges();
        }

        public JsonResult Get_ShopsForNearbyStores()
        {
            var Count = db.Shops.Where(b => b.ShopStatus == "Active").Count();
            var result = (from r in db.Shops
                          where r.ShopStatus == "Active"
                          select new { ShopName = r.Name, ShopAddress=r.Address+","+r.StateID + " State " +r.CountryRegion.Country, shopid=r.ID }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult StoreLocator(string location)
        {
            //var result = (from l in db.Shops
            //              where l.Address.Contains(location.TrimEnd().TrimStart()) && l.ShopStatus=="Active"
            //              select new { ShopName = l.Name, ShopAddress = l.Address, ID = l.ID, UserID=l.UserID,CompanyName=l.User.CompanyName}).Distinct();
            //return Json(result, JsonRequestBehavior.AllowGet);

            var result = (from l in db.Shops
                          where l.Name.Contains(location.TrimEnd().TrimStart()) || l.ShopURL.Contains(location.TrimEnd().TrimStart()) && l.ShopStatus == "Active"
                          select new { ShopName = l.Name, ShopAddress = l.Address, ID = l.ID, UserID = l.UserID, CompanyName = l.User.CompanyName }).Distinct();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Send_EmailVerifyOTP(int ShopID)
        {
            int RegisterOTP_Numeric = mydata.Generate_OTP_Numeric();
            Session["OTP_ShopEmailVerifyOTP"] = RegisterOTP_Numeric;
            var shop = db.Set<Shop>().Find(ShopID);
            //shop.PhoneVerify = Convert.ToString(RegisterOTP_Numeric);
            //db.SaveChanges();
            //==================SEND MAIL============================//
            string title = "Xamagos - Store Email Verification";
            string msg = "<center><a href='https://xamagos.com' title='Xamagos'> <img src='https://xamagos.com/Images/logosquare.png' style='width: 70px; height: 30px;background-color:#17A2B8' /></a></center><br>" + "<strong>" + " Hello " + textInfo.ToTitleCase(shop.User.CompanyName.ToLower()) + "</strong>, <br><br>";
            msg += "Your verification code is: <b>" + RegisterOTP_Numeric + "</b> <br>Please enter this code to verify your XAMAGOS store email (" + shop.Name + "). ";
            msg += "<br>This code expires in 10 minutes. <br> Do not share this code with anyone. Thank you!.";
            msg += "<br><hr>";
            msg += "<a href='https://xamagos.com'>www.xamagos.com</a> &nbsp;&nbsp;<a href='https://tinyurl.com/23a8r43k'>Get support</a>";

            mydata.SendMail(shop.Email, title, msg);
            var result = from p in db.Shops
                         where p.ID == ShopID
                         select new { ID = p.ID, OTPSent = "Yes" };
            return Json(result, JsonRequestBehavior.AllowGet);

        }


        public JsonResult Verify_EmailOTP_Shop(int ShopID,string emailotp_shop_received)
        {
            if (Convert.ToString(Session["OTP_ShopEmailVerifyOTP"]) == emailotp_shop_received)
            {
                int Payment_Closed = (int)db.Settings.Select(s => s.Payment_Account_Activation).FirstOrDefault();
                var shop = db.Set<Shop>().Find(ShopID);
                string PhoneVerify = db.Set<Shop>().Find(ShopID).PhoneVerify;
                shop.EmailVerify ="Yes";
                if (shop.PhoneVerify == "Yes")
                {
                    if (Payment_Closed == 0)
                    {
                      shop.ShopStatus = "Active";
                      //Send store URL to email and some movtivation message
                      string Mailtitle = "New Store Details";
                      string msgURL = "<center><a href='https://xamagos.com' title='Xamagos'> <img src='https://xamagos.com/Images/logosquare.png' style='width: 70px; height: 30px;background-color:#17A2B8' /></a></center><br>" + "<strong>" + " Hello " + textInfo.ToTitleCase(shop.User.CompanyName.ToLower()) + "</strong>, <br><br>";
                      msgURL += "<b>Congratulations!</b> Your store details has been verified successfuly.<br><br> Here is your store details<br>";
                      msgURL += "<b>Store:</b> " + shop.Name + " <br>";
                      msgURL += "<b>Store URL:</b> <a href='https://xamagos.com/" + shop.ShopURL + "'>https://xamagos.com/" + shop.ShopURL + "</a> <br>";
                      msgURL += "<b>Store Email:</b> " + shop.Email+" <br>";
                      msgURL += "<b>Phone Number:</b> " + shop.PhoneNumber + " <br><br>";
                      msgURL += "You can now proceed to add products to your store.";
                      msgURL += "<br><br>Follow the link below on how to add products to your online store: <br>";
                      msgURL += "<a href='https://tinyurl.com/mrx7zjne'>Adding Products to Your Store on Xamagos</a><br><br>";
                      msgURL += "<br><hr>";
                      msgURL += "<a href='https://xamagos.com'>www.xamagos.com</a> &nbsp;&nbsp;<a href='https://tinyurl.com/23a8r43k'>Get support</a>";
                      mydata.SendMail(shop.Email, Mailtitle, msgURL);
                    }
        }
                db.SaveChanges();
                //==================SEND MAIL============================//
              string title = "Email Verified";
              string msg = "<center><a href='https://xamagos.com' title='Xamagos'> <img src='https://xamagos.com/Images/logosquare.png' style='width: 70px; height: 30px;background-color:#17A2B8' /></a></center><br>" + "<strong>" + " Hello " + textInfo.ToTitleCase(shop.User.CompanyName.ToLower()) + "</strong>, <br><br>";
              msg += "Your " + shop.Name + " store email has been verified.";
              msg += "<br><hr>";
              msg += "<a href='https://xamagos.com'>www.xamagos.com</a> &nbsp;&nbsp;<a href='https://tinyurl.com/23a8r43k'>Get support</a>";
              mydata.SendMail(shop.Email, title, msg);
              Session["OTP_ShopEmailVerifyOTP"] = null;
                var result = from p in db.Shops
                             where p.ID == ShopID
                             select new { ID = p.ID, OTPExist = "true", PhoneVerifyStatus= PhoneVerify };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var result = from p in db.Shops
                             where p.ID == ShopID
                             select new { ID = p.ID, OTPExist = "false" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult Resend_VerifyEmailOTP(int ShopID)
        {
            int RegisterOTP_Numeric = mydata.Generate_OTP_Numeric();
            Session["OTP_ShopEmailVerifyOTP"] = RegisterOTP_Numeric;
            var shop = db.Set<Shop>().Find(ShopID);
            //shop.PhoneVerify = Convert.ToString(RegisterOTP_Numeric);
            //db.SaveChanges();
            //==================SEND MAIL============================//
            string title = "Xamagos - New Store Email Verification Code";
            string msg = "<center><a href='https://xamagos.com' title='Xamagos'> <img src='https://xamagos.com/Images/logosquare.png' style='width: 70px; height: 30px;background-color:#17A2B8' /></a></center><br>" + "<strong>" + " Hello " + textInfo.ToTitleCase(shop.User.CompanyName.ToLower()) + "</strong>, <br><br>";
            msg += "Your verification code is: <b>" + RegisterOTP_Numeric + "</b> <br>Please enter this code to verify your XAMAGOS store email (" + shop.Name + "). ";
            msg += "<br>This code expires in 10 minutes. <br> Do not share this code with anyone. Thank you!.";
            msg += "<br><hr>";
            msg += "<a href='https://xamagos.com'>www.xamagos.com</a> &nbsp;&nbsp;<a href='https://tinyurl.com/23a8r43k'>Get support</a>";
            mydata.SendMail(shop.Email, title, msg);
            var result = from p in db.Shops
                         where p.ID == ShopID
                         select new { ID = p.ID, OTPSent = "Yes" };
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        //code for phone verification
        public JsonResult Send_Phone_Verify_OTP(int ShopID)
        {
            Random random = new Random();
            int num = random.Next(99, 999999);
            Session["OTP_ShopPhoneVerifyOTP"] = num;
            string Phone = db.Shops.Where(s => s.ID == ShopID).Select(s => s.PhoneNumber).FirstOrDefault();
            // ================== SEND SMS =========================//
            //string sms ="Phone Number Verification. Please enter the code " + num + " to verify your store phone number on Market Square247.";
            string sms = "Xamagos welcomes you. Your account has been created successfully. Enjoy the world of Xamagos";
            mydata.Send_SMS_KudiSMS(Phone, sms, "XAMAGOS", DateTime.Now);
           // mydata.Send_SMS_KudiSMS(user.PhoneNumber, sms, "MARKTSQR247", DateTime.Now);

            //==================END SEND SMS=========================//

            var result = from p in db.Shops
                         where p.ID == ShopID
                         select new { ID = p.ID, OTPSent = "Yes" };
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public JsonResult PhoneOTP_Shop(int ShopID)
        {
            int RegisterOTP_Numeric = mydata.Generate_OTP_Numeric();
            Session["OTP_ShopPhoneVerifyOTP"] = RegisterOTP_Numeric;
            //==================SEND SMS=========================//
            string Phone = db.Shops.Where(s => s.ID == ShopID).Select(s => s.PhoneNumber).FirstOrDefault();
      //string sms = RegisterOTP_Numeric + " use this OTP to verify your Xamagos store phone number. Enjoy the world of Xamagos. www.xamagos.com";
      string sms = "Your OTP is " + RegisterOTP_Numeric + " www.xamagos.com";
      mydata.Send_SMS_KudiSMS(Phone, sms, "XAMAGOS", DateTime.Now);
            //==================END SEND SMS=========================//
            var result = from p in db.Shops
                            where p.ID == ShopID
                            select new { ID = p.ID, OTPExist = "true" };
            return Json(result, JsonRequestBehavior.AllowGet);
            }
           

        public JsonResult Verify_PhoneOTP_Shop(int ShopID, string phoneotp_shop)
        {
            if (Convert.ToString(Session["OTP_ShopPhoneVerifyOTP"]) == phoneotp_shop)
            {
                int Payment_Closed = (int)db.Settings.Select(s => s.Payment_Account_Activation).FirstOrDefault();
                var shop = db.Set<Shop>().Find(ShopID);
                string EmailVerify = db.Set<Shop>().Find(ShopID).EmailVerify;
                shop.PhoneVerify = "Yes";
                if (shop.EmailVerify == "Yes")
                {
                    if (Payment_Closed == 0)
                    {
                        shop.ShopStatus = "Active";
                      //Send store URL to email and some movtivation message
                      string Mailtitle = "New Store Details";
                      string msgURL = "<center><a href='https://xamagos.com' title='Xamagos'> <img src='https://xamagos.com/Images/logosquare.png' style='width: 70px; height: 30px;background-color:#17A2B8' /></a></center><br>" + "<strong>" + " Hello " + textInfo.ToTitleCase(shop.User.CompanyName.ToLower()) + "</strong>, <br><br>";
                      msgURL += "<b>Congratulations!</b> Your store details has been verified successfuly.<br><br> Here is your store details<br>";
                      msgURL += "<b>Store:</b> " + shop.Name + " <br>";
                      msgURL += "<b>Store URL:</b> <a href='https://xamagos.com/" + shop.ShopURL + "'>https://xamagos.com/" + shop.ShopURL + "</a> <br>";
                      msgURL += "<b>Store Email:</b> " + shop.Email + " <br>";
                      msgURL += "<b>Phone Number:</b> " + shop.PhoneNumber + " <br><br>";
                      msgURL += "You can now proceed to add products to your store.";
                      msgURL += "<br><br>Follow the link below on how to add products to your online store: <br>";
                      msgURL += "<a href='https://tinyurl.com/mrx7zjne'>Adding Products to Your Store on Xamagos</a><br><br>";
                      msgURL += "<br><hr>";
                      msgURL += "<a href='https://xamagos.com'>www.xamagos.com</a> &nbsp;&nbsp;<a href='https://tinyurl.com/23a8r43k'>Get support</a>";
                      mydata.SendMail(shop.Email, Mailtitle, msgURL);
                    }
                }
                db.SaveChanges();
                Session["OTP_ShopPhoneVerifyOTP"] = null;
                //==================SEND SMS=========================//
                //string sms = "Hello " + shop.Name + ", your phone number has been verified. Enjoy the world of Xamagos. www.xamagos.com";
                //mydata.Send_SMS_KudiSMS(shop.PhoneNumber, sms, "XAMAGOS", DateTime.Now);
                //==================END SEND SMS=========================//
          var result = from p in db.Shops
                             where p.ID == ShopID
                             select new { ID = p.ID, OTPExist = "true", EmailVerifyStatus= EmailVerify };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var result = from p in db.Shops
                             where p.ID == ShopID
                             select new { ID = p.ID, OTPExist = "false" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult Resend_Verify_Phone_OTP(int ShopID)
        {
            int RegisterOTP_Numeric = mydata.Generate_OTP_Numeric();
            Session["OTP_ShopPhoneVerifyOTP"] = RegisterOTP_Numeric;
            var shop = db.Set<Shop>().Find(ShopID);
      //shop.EmailVerify = Convert.ToString(RegisterOTP_Numeric);
      //db.SaveChanges();
      //==================SEND SMS=========================//
      string sms = "Your OTP is " + RegisterOTP_Numeric + " www.xamagos.com";
      //sms += "The code expires in 10 minutes, do not share it with others.";
      mydata.Send_SMS_KudiSMS(shop.PhoneNumber, sms, "XAMAGOS", DateTime.Now);
            //==================END SEND SMS=========================//
            var result = from p in db.Shops
                         where p.ID == ShopID
                         select new { ID = p.ID, OTPSent = "Yes" };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //====================

        
       public JsonResult Get_EmailPhoneVerifyStatus(int shopid)
        {
            var result = from p in db.Shops
                         where p.ID == shopid
                         select new { EmailVerify = p.EmailVerify, PhoneVerify =p.PhoneVerify };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    //====================

    public ActionResult BusinessCard(int? id)
    {
      if (id == null)
      {
        return Redirect("~/Home/AccessDenied");
      }
      Shop shop = db.Shops.Find(id);
      if (shop == null)
      {
        return Redirect("~/Home/AccessDenied");
      }

        return View(shop);
    }

       

    }
}


