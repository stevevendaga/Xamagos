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
using System.Drawing;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Data.OleDb;
using System.Xml;

namespace Myvshoponline.Controllers
{
    public class ProductsController : Controller
    {
        private MyvshoponlineEntities db = new MyvshoponlineEntities();
        Getdata mydata = new Getdata();
        // GET: Products
        public ActionResult Index()
        {
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                var products = db.Products.Include(p => p.ProductCategory).Include(p => p.ProductStatu).Include(p => p.Shop);
            return View(products.ToList());
        }
            return Redirect("~/Home/AccessDenied");
        }

        public ActionResult ProductCategoryDetails(int? id, int? sid,string search)
        {
            if(db.ProductCategories.Where(s=>s.ID==id).Count()==0 || db.Shops.Where(s=>s.ID == sid).Count() == 0 )
            {
                return Redirect("~/Home/AccessDenied");
            }
            if (id != null && sid!=null)
            {
                ViewBag.Products = db.ShopProductCategories.Where(s => s.ShopID == sid).ToList();
                ViewBag.RelatedProducts = db.Products.Where(s => s.ProductCategoryID == id && s.ShopID!=sid).ToList().Take(10);
                var products = db.Products.Include(p => p.ProductCategory).Include(p => p.ProductStatu).Include(p => p.Shop);
                ViewBag.ShopID = sid;
                ViewBag.UserID = db.Shops.Find(sid).UserID;
                ViewBag.CategoryID = id;
                ViewBag.CategoryName = db.ProductCategories.Find(id).Name;
                ViewBag.Icon = db.ProductCategories.Find(id).Icon;
                ViewBag.Shop = db.Shops.Where(s => s.ID == sid).ToList();
                ViewBag.CompanyName = db.Shops.Where(s => s.ID == sid).Select(s => s.User.CompanyName).FirstOrDefault().ToUpper();
                ViewBag.ShopName = db.Shops.Where(s => s.ID == sid).Select(s => s.Name).FirstOrDefault().ToUpper();
                if (search != null && id != null && sid != null)
                {
                 ViewBag.search = search;
                 return View(products.Where(p => p.ProductCategoryID == id && p.ShopID == sid && p.ProductStatu.Status == "Available" && p.Shop.ShopStatus == "Active" && p.Name.Contains(search)).ToList());
                }
                return View(products.Where(p => p.ProductCategoryID == id && p.ShopID==sid && p.ProductStatu.Status=="Available" && p.Shop.ShopStatus=="Active").OrderByDescending(p=>p.DateCreated).ToList());
              }
            return Redirect("~/Home/AccessDenied");
        }

        public ActionResult ProductCategoryDetailsUnavailable(int? sid, string search)
        {
            //if (db.ProductCategories.Where(s => s.ID == id).Count() == 0 || db.Shops.Where(s => s.ID == sid).Count() == 0)
            //{
            //    return Redirect("~/Home/AccessDenied");
            //}
            if (sid != null)
            {
                ViewBag.Products = db.ShopProductCategories.Where(s => s.ShopID == sid).ToList();
                var products = db.Products.Include(p => p.ProductCategory).Include(p => p.ProductStatu).Include(p => p.Shop);
                ViewBag.ShopID = sid;
                //ViewBag.CategoryID = id;
                ViewBag.UserID = db.Shops.Find(sid).UserID;
              //  ViewBag.CategoryName = db.ProductCategories.Find(id).Name;
                ViewBag.Shop = db.Shops.Where(s => s.ID == sid).ToList();
                ViewBag.CompanyName = db.Shops.Where(s => s.ID == sid).Select(s => s.User.CompanyName).FirstOrDefault().ToUpper();
                ViewBag.ShopName = db.Shops.Where(s => s.ID == sid).Select(s => s.Name).FirstOrDefault().ToUpper();
                if (search != null &&  sid != null)
                {
                    ViewBag.search = search;
                    return View(products.Where(p => p.ShopID == sid && p.ProductStatu.Status == "Unavailable" && p.Name.Contains(search)).ToList());
                }
                return View(products.Where(p => p.ShopID == sid && p.ProductStatu.Status == "Unavailable").OrderByDescending(p => p.DateCreated).ToList());
            }
            return Redirect("~/Home/AccessDenied");
        }
        public ActionResult ProductDetails(int? id, int? sid)
        {
           
            if (db.Products.Where(s => s.ID == id).Count() == 0 || db.Shops.Where(s => s.ID == sid).Count() == 0)
            {
                return Redirect("~/Home/AccessDenied");
            }
           // string ShopStatusCount = db.Shops.Where(s => s.ID == sid).Select(s=>s.ShopStatus).FirstOrDefault();
            if (id != null)
            {
                ProductVisit prod = new ProductVisit();
                prod.Date = DateTime.Now;
                prod.ProductID = id;
                prod.VisitorIP = mydata.GetIp();
                db.ProductVisits.Add(prod);
                db.SaveChanges();

                var state = from p in db.DeliveryLocations
                            where p.ID != 38 && p.ShopID==sid
                            select new { ID = p.LocationID, item = p.State.Name };

                //var state1 = from p in db.States
                //                where p.ID != 38 && p.ID == db.DeliveryLocations.Where(s => s.ShopID == sid).Select(s => s.LocationID).FirstOrDefault()
                //                select new { ID = p.ID, item = p.Name };
                ViewBag.StateID = new SelectList(state, "ID", "item");
                  
                ViewBag.ProductCategoryID = new SelectList(db.ProductCategories, "ID", "Name", selectedValue: 4);
                ViewBag.LocationID = new SelectList(db.States, "ID", "Name", selectedValue: 38);
                ViewBag.Comments = db.ProductComments.Where(s => s.ProductID == id).OrderByDescending(s=>s.Date).ToList();
                ViewBag.CommentsTotal = db.ProductComments.Where(s => s.ProductID == id).Count();
                ViewBag.ProductName = db.Products.Find(id).Name;
                string name = db.Products.Find(id).Name;

                int CategoryID =(int) db.Products.Where(s => s.ID == id && s.ShopID == sid).Select(s => s.ProductCategoryID).FirstOrDefault();
                ViewBag.CategoryID = db.Products.Where(s => s.ID == id && s.ShopID == sid).Select(s => s.ProductCategoryID).FirstOrDefault();
                ViewBag.Products = db.ShopProductCategories.Where(s => s.ShopID == sid).ToList();
                ViewBag.SimilarProducts = db.Products.Where(s => s.ProductCategoryID == CategoryID && s.Shop.ShopStatus == "Active" && s.ProductStatu.Status == "Available" && s.ID != id).ToList().Take(36);
                ViewBag.RelatedProducts = db.Products.Where(s => s.ProductCategoryID!=CategoryID && s.Name.StartsWith(name.Substring(0,3)) && s.Shop.ShopStatus == "Active" && s.ProductStatu.Status == "Available" && s.ID!=id).ToList().Take(36);
                ViewBag.ProductsDetail = db.Products.Where(s => s.ShopID == sid && s.ID == id && s.Shop.ShopStatus == "Active").ToList();
                var products = db.Products.Include(p => p.ProductCategory).Include(p => p.ProductStatu).Include(p => p.Shop);
                ViewBag.ShopID = sid;
                ViewBag.UserID = db.Shops.Find(sid).UserID;
               
                ViewBag.CategoryName = db.Products.Where(s=>s.ID== id && s.ShopID == sid).Select(s=>s.ProductCategory.Name).FirstOrDefault();
                ViewBag.Shop = db.Shops.Where(s => s.ID == sid).ToList();
                ViewBag.CompanyName = db.Shops.Where(s => s.ID == sid).Select(s => s.User.CompanyName).FirstOrDefault().ToUpper();
                ViewBag.ShopName = db.Shops.Where(s => s.ID == sid).Select(s => s.Name).FirstOrDefault().ToUpper();
                return View(products.Where(p => p.ID == id).ToList());
            }
            return Redirect("~/Home/AccessDenied");
        }


        public ActionResult ProductDetailsSubitems(int? id, int? sid)
        {
            if (db.ProductSubProducts.Where(s => s.ID == id).Count() == 0 || db.Shops.Where(s => s.ID == sid).Count() == 0)
            {
                return Redirect("~/Home/AccessDenied");
            }
            if (id != null)
            {

                ProductVisit prod = new ProductVisit();
                prod.Date = DateTime.Now;
                prod.ProductSubItemID =id;
                prod.VisitorIP = mydata.GetIp();
                db.ProductVisits.Add(prod);
                db.SaveChanges();

                ViewBag.ProductCategoryID = new SelectList(db.ProductCategories, "ID", "Name", selectedValue: 4);
                ViewBag.LocationID = new SelectList(db.States, "ID", "Name", selectedValue: 38);
                ViewBag.Comments = db.ProductComments.Where(s => s.ProductID == id).OrderByDescending(s => s.Date).ToList();
                ViewBag.CommentsTotal = db.ProductComments.Where(s => s.ProductID == id).Count();
                ViewBag.ProductName = db.ProductSubProducts.Find(id).Name;
                ViewBag.ProductNameMain = db.ProductSubProducts.Find(id).Product.Name;
                string name = db.ProductSubProducts.Find(id).Name;
                ViewBag.Products = db.ShopProductCategories.Where(s => s.ShopID == sid).ToList();
                ViewBag.SimilarProducts = db.Products.Where(s => s.Name.Contains(name) && s.ID != id).ToList();
                ViewBag.RelatedProducts = db.Products.Where(s => s.Name.Contains(name) && s.ShopID != sid).ToList();
                ViewBag.ProductsDetail = db.ProductSubProducts.Where(s => s.ID == id).ToList();
                var products = db.ProductSubProducts.Include(p => p.Product).Include(p => p.ProductStatu);
                ViewBag.ShopID = sid;
                ViewBag.UserID = db.Shops.Find(sid).UserID;
                ViewBag.ProductID = db.ProductSubProducts.Find(id).ProductID;
                ViewBag.Shop = db.Shops.Where(s => s.ID == sid).ToList();
                ViewBag.CompanyName = db.Shops.Where(s => s.ID == sid).Select(s => s.User.CompanyName).FirstOrDefault().ToUpper();
                ViewBag.ShopName = db.Shops.Where(s => s.ID == sid).Select(s => s.Name).FirstOrDefault().ToUpper();
                var state = from p in db.DeliveryLocations
                            where p.ID != 38 && p.ShopID == sid
                            select new { ID = p.LocationID, item = p.State.Name };

                ViewBag.StateID = new SelectList(state, "ID", "item");
                return View(products.Where(p => p.ID ==id && p.Product.Shop.ShopStatus == "Active").ToList());
            }
            return Redirect("~/Home/AccessDenied");
        }


        public ActionResult AddtoCart(int? pid, int? sid, int? qty, string src,string negotiate,int?dcity)
        {
            int CommissionPercent = Convert.ToInt32(db.Settings.Select(s => s.CommissionPercent).FirstOrDefault());
            decimal c_percent = (decimal)(CommissionPercent / 100.00);

            var products = db.Products.Include(p => p.ProductCategory).Include(p => p.ProductStatu).Include(p => p.Shop);
            if (pid != null && sid != null && qty != null)
            {
                decimal rate;
                string Item;
                if (src != null && src == "subitem")
                {
                    rate = (db.ProductSubProducts.Where(p => p.ID == pid).Select(p => p.Amount).FirstOrDefault()!=null?(decimal)db.ProductSubProducts.Where(p => p.ID == pid).Select(p => p.Amount).FirstOrDefault():0);
                    Item =db.ProductSubProducts.Where(p => p.ID == pid).Select(p => p.Name).FirstOrDefault();
                }
                else
                {
                    rate =(db.Products.Where(p => p.ID == pid).Select(p => p.Amount).FirstOrDefault()!=null?(decimal)db.Products.Where(p => p.ID == pid).Select(p => p.Amount).FirstOrDefault():0);
                    Item = db.Products.Where(p => p.ID == pid).Select(p => p.Name).FirstOrDefault();
                }
                ViewBag.Item = Item;
                ViewBag.Quantity = qty;
                ViewBag.Rate = rate;
                ViewBag.Amount = rate * qty;
                ViewBag.pid = pid;
                ViewBag.src = src;

                int ShopID;
                int MainProductID;
                int SubProductID;
                string MainProductName;
                string SubProductName;
                //int ShopCustomerID = Convert.ToInt32(Session["CustomerID"]);
                int UserID = Convert.ToInt32(Session["UserID"]);
                //ShopID = db.Products.Find(pid).ShopID;

                //ADD TO SHOP CUSTOMERS IF ID DOES NOT EXIST
                if (db.CustomerShops.Where(s => s.ShopID == sid && s.UserID == UserID).Count() < 1)
                {
                    //INSERT SHOPID INTO CUSTOMERS SHOP
                    CustomerShop custs = new CustomerShop();
                    custs.ShopID = sid;
                    //custs.ShopCustomerID = ShopCustomerID;
                    custs.UserID = UserID;
                    custs.Datecreated = DateTime.Now;
                    db.CustomerShops.Add(custs);
                    db.SaveChanges();
                }
                if (src != null && src == "subitem")
                {
                    SubProductID = (int)pid;
                    MainProductID = (db.ProductSubProducts.Where(s=>s.ID==SubProductID).Count()!=0?(int)db.ProductSubProducts.Find(pid).ProductID:0);
                    ShopID = (db.ProductSubProducts.Where(s => s.ID == SubProductID).Count() != 0 ?db.ProductSubProducts.Find(pid).Product.ShopID:0);
                    SubProductName = (db.ProductSubProducts.Where(s => s.ID == SubProductID).Count() != 0 ?db.ProductSubProducts.Find(pid).Name:null);
                    MainProductName = (db.ProductSubProducts.Where(s => s.ID == SubProductID).Count() != 0 ?db.ProductSubProducts.Find(pid).Product.Name:null);

                    //==========CALCULATE COMMISSION========================//
                    decimal Commission = Convert.ToDecimal(db.ProductSubProducts.Find(SubProductID).Amount * c_percent);
                    decimal totalRate = rate;
                    decimal TotalAmount = Convert.ToDecimal(totalRate * qty);
                    //==============================================//
                    Order orda = new Order();
                    //orda.ShopCustomerID = ShopCustomerID;
                    orda.UserID = UserID;
                    orda.ProductID = MainProductID;
                    orda.SubProductID = SubProductID;
                    orda.Quantity = qty;
                    orda.Rate = totalRate;
                    orda.Amount =TotalAmount;
                    orda.Commission = Convert.ToDecimal(Commission * qty);
                    orda.DateOrdered = DateTime.Now;
                    orda.PaymentStatusID = 0;
                    orda.OnCart = "Yes";
                    orda.ProductName = MainProductName;
                    orda.SubProductName = SubProductName;
                    orda.Negotiated = "No";

                   //orda.OrderGroupID =Convert.ToString(mydata.Generate_OTP_Numeric());
                    if (dcity != null)
                    {
                        decimal DeliveryCost = (db.DeliveryCities.Where(s=>s.ID==dcity).Count()!=0?(decimal)db.DeliveryCities.Find(dcity).Cost:0);
                        orda.DeliveryCost = DeliveryCost;
                    }
                    db.Orders.Add(orda);
                    db.SaveChanges();
                    //INSERT INTO SHIPPING INFORMATION
                    if (dcity != null)
                    {
                        ShippingInformation ship = new ShippingInformation();
                        int CountryID = (int)db.DeliveryCity_Only.Find(dcity).State.CountryID;
                        int StateID = (int)db.DeliveryCity_Only.Find(dcity).StateID;
                        string City = db.DeliveryCity_Only.Find(dcity).City;
                        ship.OrderID = orda.ID;
                        ship.CountryID = CountryID;
                        ship.StateID = StateID;
                        ship.City = City;
                        db.ShippingInformations.Add(ship);
                        db.SaveChanges();
                    }
                    if (!string.IsNullOrEmpty(negotiate))
                    {
                        return Redirect("~/OrderNegotiations/OrderNegos/" +orda.ID +"?custid="+UserID);
                    }
                    else
                    {
                        //return Redirect("~/Users/CustomerDashboard/" + UserID);
                        return Redirect("~/Products/OrderDetailsCheckOut/?id=" + UserID);
                    }
                }
                else
                {
                    //==========CALCULATE COMMISSION========================//
                    decimal Commission = Convert.ToDecimal(db.Products.Find(pid).Amount * c_percent);
                    decimal totalRate = rate;
                    decimal TotalAmount =Convert.ToDecimal(totalRate * qty);
                    //==============================================//

                    MainProductID = (int)pid;
                    SubProductID = 0;
                    MainProductName = db.Products.Find(pid).Name;
                    Order orda = new Order();
                    //orda.ShopCustomerID = ShopCustomerID;
                    orda.UserID = UserID;
                    orda.ProductID = MainProductID;
                    orda.SubProductID = SubProductID;
                    orda.Quantity = qty;
                    orda.Rate = totalRate;
                    orda.Amount =TotalAmount ;
                    orda.Commission = Convert.ToDecimal(Commission * qty);
                    orda.DateOrdered = DateTime.Now;
                    orda.PaymentStatusID = 0;
                    orda.OnCart = "Yes";
                    orda.ProductName = MainProductName;
                    orda.SubProductName = null;
                    orda.Negotiated = "No";
              
                    // orda.OrderGroupID = Convert.ToString(mydata.Generate_OTP_Numeric());
                    if (dcity != null)
                    {
                        decimal DeliveryCost = (db.DeliveryCities.Where(s => s.ID == dcity).Count() != 0 ? (decimal)db.DeliveryCities.Find(dcity).Cost : 0);
                        orda.DeliveryCost = DeliveryCost;
                    }
                    db.Orders.Add(orda);
                    db.SaveChanges();
                    //INSERT INTO SHIPPING INFORMATION
                    if (dcity != null)
                    {
                        ShippingInformation ship = new ShippingInformation();
                        int CountryID = (int)db.DeliveryCity_Only.Find(dcity).State.CountryID;
                        int StateID = (int)db.DeliveryCity_Only.Find(dcity).StateID;
                        string City = db.DeliveryCity_Only.Find(dcity).City;
                        ship.OrderID = orda.ID;
                        ship.CountryID = CountryID;
                        ship.StateID = StateID;
                        ship.City = City;
                        db.ShippingInformations.Add(ship);
                        db.SaveChanges();
                    }

                    if (!string.IsNullOrEmpty(negotiate))
                    {
                     return Redirect("~/OrderNegotiations/OrderNegos/" + orda.ID + "?custid=" + UserID);
                    }
                    else
                    {
                     //return Redirect("~/Users/CustomerDashboard/" + UserID);
                     return Redirect("~/Products/OrderDetailsCheckOut/?id=" + UserID);
                        
                    }
                }
                
            }
            //decimal rate = (decimal)db.Products.Find(pid).Amount;
            //ViewBag.Item = db.Products.Find(pid).Name;
            //ViewBag.Quantity = qty;
            //ViewBag.Amount = rate * qty;
            return Redirect("~/Home/AccessDenied");
        }


        public ActionResult OrderDetailsCheckOut(int? id)
        {
            if (Session["UserID"] == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            var products = db.Orders.Include(p => p.Product).Include(p => p.ProductSubProduct);
            if (id != null)
            {
                //decimal rate;
                //string Item;

                //if ()
                //{
                //    rate = (decimal)db.ProductSubProducts.Where(p => p.ID == pid).Select(p => p.Amount).FirstOrDefault();
                //    Item = db.ProductSubProducts.Where(p => p.ID == pid).Select(p => p.Name).FirstOrDefault();
                //}
                //else
                //{
                //    rate = (decimal)db.Products.Where(p => p.ID == pid).Select(p => p.Amount).FirstOrDefault();
                //    Item = db.Products.Where(p => p.ID == pid).Select(p => p.Name).FirstOrDefault();
                //}
                //ViewBag.Item = Item;
                //ViewBag.Quantity = qty;
                //ViewBag.Rate = rate;
                //ViewBag.Amount = rate * qty;
                //ViewBag.pid = pid;
                //ViewBag.src = src;
                ViewBag.customerID = id;
                return View();
            }
            return Redirect("~/Home/AccessDenied"); ;
        }

       
        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                if (id == null)
                {
                    return Redirect("~/Home/AccessDenied");
                }
                Product product = db.Products.Find(id);
                if (product == null)
                {
                    return Redirect("~/Home/AccessDenied");
                }
                return View(product);
            }
            return Redirect("~/Home/AccessDenied");
        }

        // GET: Products/Create
        public ActionResult Create(int?id)
        {
            if (db.Shops.Where(s => s.ID == id).Count() == 0)
            {
                return Redirect("~/Home/AccessDenied");
            }
            if(id==null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            int ShopID =(int) id;
            int UserID = db.Shops.Where(s => s.ID == ShopID).Select(s => s.User.ID).FirstOrDefault();
            if (!string.IsNullOrEmpty((string)Session["username"]) && ((string)Session["UserRole"] == "Shop Admin" || (string)Session["UserRole"] == "Super Admin") && (int)Session["UserID"] == UserID)
            {
                ViewBag.ProductCategoryID = new SelectList(db.ProductCategories, "ID", "Name");
                ViewBag.ProductStatusID = new SelectList(db.ProductStatus, "ID", "Status");
                ViewBag.ShopID = id;
                ViewBag.ShopName = db.Shops.Find(id).Name;
                ViewBag.UserID = UserID;
                ViewBag.CompanyName = db.Shops.Where(s => s.ID == id).Select(s => s.User.CompanyName).FirstOrDefault();
                return View();
            }
            else
            {
                return Redirect("~/Home/AccessDenied");
            }
        }



        public ActionResult MyProducts(int? id)
        {
            if (db.Shops.Where(s => s.ID == id).Count() == 0)
            {
                return Redirect("~/Home/AccessDenied");
            }
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            int ShopID = (int)id;
            int UserID = db.Shops.Where(s => s.ID == ShopID).Select(s => s.User.ID).FirstOrDefault();
            if (!string.IsNullOrEmpty((string)Session["username"]) && ((string)Session["UserRole"] == "Shop Admin" || (string)Session["UserRole"] == "Super Admin") && (int)Session["UserID"] == UserID)
            {
                ViewBag.Products = db.ShopProductCategories.Where(s => s.ShopID == id).ToList();
                ViewBag.ProductCategoryID = new SelectList(db.ProductCategories, "ID", "Name");
                ViewBag.ProductStatusID = new SelectList(db.ProductStatus, "ID", "Status");
                ViewBag.ShopID = id;
                ViewBag.ShopName = db.Shops.Find(id).Name;
                ViewBag.UserID = UserID;
                ViewBag.CompanyName = db.Shops.Where(s => s.ID == id).Select(s => s.User.CompanyName).FirstOrDefault();
                return View();
            }
            else
            {
                return Redirect("~/Home/AccessDenied");
            }
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "ID,ShopID,Name,Details,ProductCategoryID,ProductStatusID,Amount,QuantityinStock,DateCreated,PurchasePrice,Currency")] Product product, HttpPostedFileBase[] files)
        {
            if (ModelState.IsValid)
            {
                if (db.Products.Where(p => p.Name == product.Name && p.ShopID == product.ShopID && p.ProductCategoryID==product.ProductCategoryID).Count() > 0)
                {
                    ViewBag.ProductCategoryID = new SelectList(db.ProductCategories, "ID", "Name");
                    ViewBag.ProductStatusID = new SelectList(db.ProductStatus, "ID", "Status");
                    ViewBag.ShopID = product.ShopID;
                    ViewBag.ShopName = db.Shops.Find(product.ShopID).Name;
                    ViewBag.CompanyName = db.Shops.Where(s => s.ID == product.ShopID).Select(s => s.User.CompanyName).FirstOrDefault();
                    ViewBag.ProductExist = product.Name + " already exist!";
                    return View();
                }
                else
                {
                    string PricingPlan = db.Shops.Where(p => p.ID == product.ShopID).Select(p => p.User.PricingPlan.PlanName).FirstOrDefault();
                    int NumberofProducts = (int)db.PricingPlans.Where(p => p.PlanName == PricingPlan).Select(p => p.NumberofProducts).FirstOrDefault();
                    int TotalNumberofProducts = db.Products.Where(s => s.ShopID == product.ShopID).Count();
                    if (TotalNumberofProducts < NumberofProducts)
                    {
                        //string Details =Convert.ToString(Request.Form["Details"]);
                        //product.Details =Details;
                        product.DateCreated = DateTime.Now;
                        product.Published = 0;
                        product.Ready_for_Publishing = 1;
                        product.Negotiable = "No";
                        product.ProductStatusID = 2;
                        db.Products.Add(product);
                        db.SaveChanges();
                    }
                    else
                    {
                        ViewBag.ProductCategoryID = new SelectList(db.ProductCategories, "ID", "Name");
                        ViewBag.ProductStatusID = new SelectList(db.ProductStatus, "ID", "Status");
                        ViewBag.ShopID = product.ShopID;
                        ViewBag.ShopName = db.Shops.Find(product.ShopID).Name;
                        ViewBag.CompanyName = db.Shops.Where(s => s.ID == product.ShopID).Select(s => s.User.CompanyName).FirstOrDefault();
                        ViewBag.TotalNumberofProductsExceeded = " Maximun number of products reached for your current plan for this shop.";
                        return View();
                    }
                    //Save Picture
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
                    string ShopName = db.Shops.Find(product.ShopID).Name;
                    string businessname = db.Users.Find(UserID).CompanyName;
                    string DocumentName = product.ID + ".jpg";
                    string efilepath;
                    foreach (HttpPostedFileBase file in files)
                    {
                        if (file != null)
                        {
                            efilepath = Server.MapPath("~/BusinessImages/" + businessname + "/" + ShopName + "/" + "\\" + DocumentName);
                            string ext = Path.GetExtension(efilepath);

                            //if file extension is supported, save file and update database
                            if (ext == ".jpg" || ext == ".jpeg" || ext == ".png" || ext == ".gif" || ext == ".ico")
                            {
                                string newName = System.IO.Path.GetFileNameWithoutExtension(efilepath);
                                newName = newName + ".jpg";
                               
                                efilepath = Server.MapPath("~/BusinessImages/" + businessname + "/" + ShopName + "/" + "\\" + newName);
                                file.SaveAs(efilepath);

                                mydata.ResizePicture(efilepath);
                               
                            }
                        }

                    }
                    //Save the picture ends

                    if (db.ShopProductCategories.Where(s => s.ProductCategoryID == product.ProductCategoryID && s.ShopID == product.ShopID).Count() < 1)
                    {

                        ShopProductCategory shop = new ShopProductCategory();
                        shop.Date = DateTime.Now;
                        shop.ShopID = product.ShopID;
                        shop.ProductCategoryID = product.ProductCategoryID;
                        db.ShopProductCategories.Add(shop);
                        db.SaveChanges();
                    }
                }
                // return Redirect("~/Products/ProductCategoryDetails/?id="+product.ProductCategoryID+"&sid="+ product.ShopID);
                return Redirect("~/Products/ViewProducts?id=" +product.ShopID + "&un_pub=1");
            }

            ViewBag.ProductCategoryID = new SelectList(db.ProductCategories, "ID", "Name", product.ProductCategoryID);
            ViewBag.ProductStatusID = new SelectList(db.ProductStatus, "ID", "Status", product.ProductStatusID);
            ViewBag.ShopID = new SelectList(db.Shops, "ID", "Name", product.ShopID);
            return View(product);
        }

        [ValidateInput(false)]
        public void SaveproductAjax(int shopid,string name,string details,int categoryid,decimal sellingamount,int qty,decimal purchaseprice,string currency)
        {
                string PricingPlan = db.Shops.Where(p => p.ID == shopid).Select(p => p.User.PricingPlan.PlanName).FirstOrDefault();
                int NumberofProducts = (int)db.PricingPlans.Where(p => p.PlanName == PricingPlan).Select(p => p.NumberofProducts).FirstOrDefault();
                int TotalNumberofProducts = db.Products.Where(s => s.ShopID == shopid).Count();
                if (TotalNumberofProducts < NumberofProducts)
                {
                    Product product = new Product();
                    product.Name = name;  
                    product.Details =details;
                    product.ProductCategoryID = categoryid;
                    product.ShopID = shopid;
                    product.Amount = sellingamount;
                    product.QuantityinStock = qty;
                    product.PurchasePrice = purchaseprice;
                    product.Currency = currency;
                    product.DateCreated = DateTime.Now;
                    product.Published = 0;
                    product.Ready_for_Publishing = 1;
                    product.Negotiable = "No";
                    product.ProductStatusID = 2;
                    db.Products.Add(product);
                   db.SaveChanges();
                }
               
                if (db.ShopProductCategories.Where(s => s.ProductCategoryID == categoryid && s.ShopID == shopid).Count() < 1)
                {
                    ShopProductCategory shop = new ShopProductCategory();
                    shop.Date = DateTime.Now;
                    shop.ShopID = shopid;
                    shop.ProductCategoryID = categoryid;
                    db.ShopProductCategories.Add(shop);
                    db.SaveChanges();
                }
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            if (mydata.Is_ShopAdmin((string)Session["username"], (string)Session["UserRole"]) && db.Products.Find(id).Shop.UserID == (int)Session["UserID"] || mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                ViewBag.ProductCategoryID = new SelectList(db.ProductCategories, "ID", "Name", product.ProductCategoryID);
                ViewBag.ProductStatusID = new SelectList(db.ProductStatus, "ID", "Status", product.ProductStatusID);
                //  ViewBag.ShopID = new SelectList(db.Shops, "ID", "Name", product.ShopID);
                int UserID = (int)Session["UserID"];
                ViewBag.ShopID = db.Products.Find(id).ShopID;
                ViewBag.ShopName = db.Products.Find(id).Shop.Name;
                ViewBag.UserID = UserID;
                int Sid = ViewBag.ShopID;
                ViewBag.CompanyName = db.Shops.Where(s => s.ID ==Sid ).Select(s => s.User.CompanyName).FirstOrDefault();
                return View(product);
            }
            return Redirect("~/Home/AccessDenied");
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "ID,ShopID,Name,Details,ProductCategoryID,ProductStatusID,Amount,QuantityinStock,DateCreated,PurchasePrice,Currency,Published,Ready_for_Publishing,Negotiable")] Product product)
        {
            
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
            string up =Request.Form["un_pub"];
            if (up=="1")
            {
                return Redirect("~/Products/ViewProducts/?id=" + product.ShopID + "&un_pub=1");
            }
            else
            {
                return Redirect("~/Products/ViewProducts/?id=" + product.ShopID + "&un_pub=0");
            }

            //return Redirect("~/Products/ProductCategoryDetails/?id=" + product.ProductCategoryID + "&sid=" + product.ShopID);

            //ViewBag.ProductCategoryID = new SelectList(db.ProductCategories, "ID", "Name", product.ProductCategoryID);
            //ViewBag.ProductStatusID = new SelectList(db.ProductStatus, "ID", "Status", product.ProductStatusID);
            //ViewBag.ShopID = new SelectList(db.Shops, "ID", "Name", product.ShopID);
            // return View(product);
        }


        public ActionResult UploadProductPicture(HttpPostedFileBase[] files, int ShopID,int ProductID, int ProductCategoryID)
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
            string ShopName = db.Shops.Find(ShopID).Name;
            string businessname = db.Users.Find(UserID).CompanyName;
            string DocumentName = ProductID + ".jpg";
            string efilepath;
            foreach (HttpPostedFileBase file in files)
            {
                if (file != null)
                {
                    efilepath = Server.MapPath("~/BusinessImages/" + businessname + "/" + ShopName + "/" + "\\" + DocumentName);
                    string ext = Path.GetExtension(efilepath);

                    //if file extension is supported, save file and update database
                    if (ext == ".jpg" || ext == ".jpeg" || ext == ".png" || ext == ".gif" || ext == ".ico")
                    {
                        string newName = System.IO.Path.GetFileNameWithoutExtension(efilepath);
                        newName = newName + ".jpg";
                        efilepath = Server.MapPath("~/BusinessImages/" + businessname + "/" + ShopName + "/" + "\\" + newName);
                        file.SaveAs(efilepath);
                        mydata.ResizePicture(efilepath);

                    }
                }

            }
            return Redirect("~/Products/ProductDetails/"+ProductID + "?&sid=" +ShopID);
        }

        
       public ActionResult UploadProductPictureSubitem(HttpPostedFileBase[] files, int ShopID, int subitemID, int ProductID)
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
            string ShopName = db.Shops.Find(ShopID).Name;
            string businessname = db.Users.Find(UserID).CompanyName;
            string DocumentName =ProductID + "-"+ subitemID + ".jpg";
            string hardtoken = db.Users.Find(UserID).HardToken;
            string efilepath;
            foreach (HttpPostedFileBase file in files)
            {
                if (file != null)
                {
                    efilepath = Server.MapPath("~/BusinessImages/" + businessname+hardtoken + "/" + ShopName + "/" + "\\" + DocumentName);
                    string ext = Path.GetExtension(efilepath);

                    //if file extension is supported, save file and update database
                    if (ext == ".jpg" || ext == ".jpeg" || ext == ".png" || ext == ".gif" || ext == ".ico")
                    {
                        string newName = System.IO.Path.GetFileNameWithoutExtension(efilepath);
                        newName = newName + ".jpg";
                        efilepath = Server.MapPath("~/BusinessImages/" + businessname+hardtoken + "/" + ShopName + "/" + "\\" + newName);
                        file.SaveAs(efilepath);
                    }
                }

            }
            return Redirect("~/Products/ProductDetailsSubitems/" + subitemID + "?&sid=" + ShopID);
        }


        public ActionResult Upload_Product_Picture_from_ViewProducts(HttpPostedFileBase[] files, int ShopID, int ProductID)
        {
            int un_pub = Convert.ToInt32(Request.Form["un_pub"]);
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
            string ShopName = db.Shops.Find(ShopID).Name;
            string businessname = db.Users.Find(UserID).CompanyName;
            string hardtoken = db.Users.Find(UserID).HardToken;
            string DocumentName = ProductID + ".jpg";
            string efilepath;
            foreach (HttpPostedFileBase file in files)
            {
                if (file != null)
                {
                    efilepath = Server.MapPath("~/BusinessImages/" + businessname + hardtoken + "/" + ShopName + "/" + "\\" + DocumentName);
                    string ext = Path.GetExtension(efilepath);

                    //if file extension is supported, save file and update database
                    if (ext == ".jpg" || ext == ".jpeg" || ext == ".png" || ext == ".gif" || ext == ".ico")
                    {
                        string newName = System.IO.Path.GetFileNameWithoutExtension(efilepath);
                        newName = newName + ".jpg";
                        efilepath = Server.MapPath("~/BusinessImages/" + businessname + hardtoken + "/" + ShopName + "/" + "\\" + newName);
                        file.SaveAs(efilepath);
                        mydata.ResizePicture(efilepath);

                    }
                }

            }
            mydata.Update_ready_for_publishing_After_ImageUpload(ShopID, ProductID);
            if (un_pub==1)
            {
                return Redirect("~/Products/ViewProducts/?id=" + ShopID +"&un_pub=1");
            }
            else
            {
                return Redirect("~/Products/ViewProducts/?id=" + ShopID + "&un_pub=0");
            }
        }



        // GET: Products/Delete/5
        public ActionResult Delete(int? sid,int?id,int?un_pub)
        {
            
           if (id != null)
            {
                if (mydata.Is_ShopAdmin((string)Session["username"], (string)Session["UserRole"]) && db.Products.Find(id).Shop.UserID == (int)Session["UserID"] || mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
                {
                    Product product = db.Products.Find(id);
                    if (db.Orders.Where(o => o.ProductID == id).Count() < 1)
                    {
                        mydata.Delete_Product_Visits((int)id);
                        db.Products.Remove(product);
                        db.SaveChanges();
                        return Redirect("~/Products/ViewProducts/?id=" + sid +"&un_pub="+un_pub);
                    }
                    else
                    {
                        return Content("<script>alert('This product cannot be deleted. It has been ordered.');window.location='/products/ViewProducts/?id="+sid+"&un_pub="+un_pub+"';</script>");
                        //return Content("<script>alert('CBT subjects prepared for all candidates successfully');window.location='/Registrations/index';</script>");
                    }
                }
            }
           return Redirect("~/Home/AccessDenied");
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ImportProducts(int?id)
        {
            if (mydata.Is_ShopAdmin((string)Session["username"], (string)Session["UserRole"]) && db.Shops.Find(id).UserID == (int)Session["UserID"] || mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                int UserID = (int)Session["UserID"];
                ViewBag.ShopID = id;
                ViewBag.ShopName = db.Shops.Find(id).Name;
                ViewBag.UserID = UserID;
                ViewBag.CompanyName = db.Shops.Where(s => s.ID == id).Select(s => s.User.CompanyName).FirstOrDefault();
                var products = db.Products.Include(p => p.ProductCategory).Include(p => p.ProductStatu).Include(p => p.Shop);
                return View(products.Where(s=>s.ShopID==id && s.ProductStatusID==2).ToList());
            }
            return Redirect("~/Home/AccessDenied");
        }


        public ActionResult ViewProducts(int? id,int? un_pub)
        {
            if (mydata.Is_ShopAdmin((string)Session["username"], (string)Session["UserRole"]) && db.Shops.Find(id).UserID == (int)Session["UserID"] || mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                int UserID =(int)Session["UserID"];
                ViewBag.ShopID = id;
                ViewBag.ShopName = db.Shops.Find(id).Name;
                ViewBag.UserID = UserID;
                ViewBag.CompanyName = db.Shops.Where(s => s.ID == id).Select(s => s.User.CompanyName).FirstOrDefault();

                var products = db.Products.Include(p => p.ProductCategory).Include(p => p.ProductStatu).Include(p => p.Shop);
                if (un_pub==1 && un_pub!=null)
                {
                    return View(products.Where(s => s.ShopID == id && s.ProductStatusID == 2).OrderByDescending(s=>s.ID).ToList());
                }
                else
                {
                    return View(products.Where(s => s.ShopID == id && s.ProductStatusID == 1).OrderByDescending(s => s.ID).ToList());
                }
               
               
            }
            return Redirect("~/Home/AccessDenied");
        }

        public ActionResult Download_Products_template(int? id)
        {
            var grid = new GridView();
            grid.DataSource = from a in db.Shops.ToList()
                              where a.ID==Convert.ToInt32(id)
                              select new
                              {
                                  //id
                                  Shop_Number = mydata.EncodePasswordToBase64(Convert.ToString(a.ID)),
                                  Shop_Name = a.Name,
                                  Product_Name ="",
                                  Product_Details ="",
                                  Product_Category = "",
                                  Selling_Price = "",
                                  Quantity_in_Stock = "",
                                  Price_Bought = "",
                                  Currency = "NGN",
                              };
            grid.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Product_Upload_Template_Xamagos.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            grid.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            return View();
        }

        public ActionResult Download_Product_Categories()
        {
            var grid = new GridView();
            grid.DataSource = from a in db.ProductCategories.ToList()
                              select new
                              {
                                  Product_Number = mydata.EncodePasswordToBase64(Convert.ToString(a.ID)),
                                  Product_Category = a.Name,
                              };
            grid.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Product_Categories_Xamagos.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            grid.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            return View();
        }

        public ActionResult Upload_Products(HttpPostedFileBase filequestions,int MainShopID)
        {
            DataSet ds = new DataSet();
            //check file lenght 
            if (Request.Files["file"].ContentLength > 0)
            {
                string fileExtension = System.IO.Path.GetExtension(Request.Files["file"].FileName);
                if (fileExtension == ".xls" || fileExtension == ".xlsx")
                {
                    string currenttime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    string fileLocation = Server.MapPath("~/Product_Uploads/") + User.Identity.Name + currenttime + Request.Files["file"].FileName;

                    if (System.IO.File.Exists(fileLocation))
                    {
                        System.IO.File.Delete(fileLocation);
                    }
                    Request.Files["file"].SaveAs(fileLocation);

                    string excelConnectionString = string.Empty;
                    excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                    fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    //connection String for xls file format.
                    if (fileExtension == ".xls")
                    {
                        excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
                        fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                    }
                    //connection String for xlsx file format.
                    else if (fileExtension == ".xlsx")
                    {
                        excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                        fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    }
                    //Create Connection to Excel work book and add oledb namespace
                    OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
                    excelConnection.Open();
                    DataTable dt = new DataTable();

                    dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    if (dt == null)
                    {
                        return null;
                    }

                    String[] excelSheets = new String[dt.Rows.Count];
                    int t = 0;
                    //excel data saves in temp file here.
                    foreach (DataRow row in dt.Rows)
                    {
                        excelSheets[t] = row["TABLE_NAME"].ToString();
                        t++;
                    }
                    OleDbConnection excelConnection1 = new OleDbConnection(excelConnectionString);
                    string query = string.Format("Select * from [{0}]", excelSheets[0]);
                    using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                    {
                        dataAdapter.Fill(ds);
                    }
                }
                if (fileExtension.ToString().ToLower().Equals(".xml"))
                {
                    string fileLocation = Server.MapPath("~/Product_Uploads/") + Request.Files["file"].FileName;
                    if (System.IO.File.Exists(fileLocation))
                    {
                        System.IO.File.Delete(fileLocation);
                    }

                    Request.Files["file"].SaveAs(fileLocation);
                    XmlTextReader xmlreader = new XmlTextReader(fileLocation);
                    // DataSet ds = new DataSet();
                    ds.ReadXml(xmlreader);
                    xmlreader.Close();
                }
                string Product_Name;
                string Product_Details;
                string Product_Category;
                decimal Selling_Price;
                int Quantity_in_Stock;
                decimal Price_Bought;
                string Currency = "NGN";
                Random random = new Random();
                int totalrows = ds.Tables[0].Rows.Count;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                  string Shopi = mydata.DecodeFrom64(ds.Tables[0].Rows[i][0].ToString());
                  int  ShopID = Convert.ToInt32(Shopi);
                    Product_Name = ds.Tables[0].Rows[i][2].ToString();
                    Product_Details = ds.Tables[0].Rows[i][3].ToString();
                    Product_Category = ds.Tables[0].Rows[i][4].ToString();
                    int CategoryID = db.ProductCategories.Where(s => s.Name == Product_Category).Select(s => s.ID).FirstOrDefault();
                    Selling_Price =Convert.ToDecimal(ds.Tables[0].Rows[i][5].ToString());
                    Quantity_in_Stock =Convert.ToInt32(ds.Tables[0].Rows[i][6].ToString());
                    Price_Bought = Convert.ToDecimal(ds.Tables[0].Rows[i][7].ToString());

                    int rand1 = random.Next(99999, 99999999);
                    int rand2 = random.Next(99999, 99999999);
                    Product pro = new Product();
                    pro.ShopID = ShopID;
                    pro.Name = Product_Name;
                    pro.Details = Product_Details;
                    pro.ProductCategoryID = CategoryID;
                    pro.ProductStatusID = db.ProductStatus.Where(s => s.Status == "Unavailable").Select(s => s.ID).FirstOrDefault();
                    pro.Amount = Selling_Price;
                    pro.QuantityinStock = Quantity_in_Stock;
                    pro.DateCreated = DateTime.Now;
                    pro.PurchasePrice = Price_Bought;
                    pro.Currency = Currency;
                    pro.Published = 0;
                    pro.Ready_for_Publishing = 0;
                    pro.Negotiable = "No";
                    db.Products.Add(pro);
                    db.SaveChanges();
                }
                return Redirect("~/Products/ViewProducts?id="+MainShopID+"&un_pub=1");
            }
            else
            {

                //no content uploaded 
                return View();
            }
        }

        public void Select_product_for_publishing(int pid)
        {
            var p = db.Set<Product>().Find(pid);
            p.Ready_for_Publishing = 1;
            db.SaveChanges();
        }

        public void De_selectProduct_for_publishing(int pid)
        {
            var p = db.Set<Product>().Find(pid);
            p.Ready_for_Publishing = 0;
            db.SaveChanges();
        }


        public ActionResult Count_Selected(int shopid)
        {
            int CountTotal1 = db.Products.Where(s => s.ShopID == shopid && s.Ready_for_Publishing == 1).Count();
            var result = from a in db.Products
                         where a.ShopID == shopid && a.Ready_for_Publishing == 1
                         select new { CountTotal = CountTotal1 };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Publish_Products(int ShopID)
        {
            
            var prod = db.Products.Where(s => s.ShopID == ShopID).ToList();
            foreach (var item in prod)
            {
                string ImageName = item.ID + ".jpg";
                int UserID = (int)Session["UserID"];
                string ShopName = db.Shops.Find(item.ShopID).Name;
                string businessname = db.Users.Find(UserID).CompanyName;
                string hardtoken = db.Users.Find(UserID).HardToken;
                string filepath = Server.MapPath("~/BusinessImages/" + businessname+hardtoken + "/" + ShopName + "/" + ImageName);

                if (System.IO.File.Exists(filepath))
                {
                    mydata.Publish_Products(ShopID);
                    mydata.Update_ready_for_publishing_back(item.ID);
                }
            }
          
            return Redirect("~/Products/ViewProducts?id=" + ShopID + "&un_pub=0");
        }

        public ActionResult Un_Publish_Product(int id,int ShopID)
        {
            var unpub = db.Set<Product>().Find(id);
            unpub.Published = 0;
            unpub.ProductStatusID = 2;
            db.SaveChanges();
         return Redirect("~/Products/ViewProducts?id=" + ShopID + "&un_pub=0");
        }

        public void Update_Negotiable_Yes(int ProductID)
        {
            mydata.Update_Negotiable_Yes(ProductID);
        }
        public void Update_Negotiable_No(int ProductID)
        {
            mydata.Update_Negotiable_No(ProductID);
        }

        public JsonResult GetCities(int StateID)
        {

            var result = (from r in db.DeliveryCity_Only
                          where r.StateID==StateID
                          select new { ID = r.ID, CityName = r.City }).Distinct();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDeliveryInfo(int? StateID, int? city, int? FromStateID)
        {
            var result = (from r in db.DeliveryCities
                          where r.StateID == StateID && r.CityID == city && r.FromStateID == FromStateID
                          select new { ID = r.ID, shippingcost = r.Cost, deliverynote = r.DeliveryNote, Location = r.State.Name +"-"+ r.DeliveryCity_Only.City, FromLocation = r.State1.Name }).Distinct();
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
    }
}
