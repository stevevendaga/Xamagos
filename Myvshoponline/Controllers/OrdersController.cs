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
    public class OrdersController : Controller
    {
        private MyvshoponlineEntities db = new MyvshoponlineEntities();
        Getdata mydata = new Getdata();
        // GET: Orders
        public ActionResult Index()
        {
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                var orders = db.Orders.Include(o => o.PaymentStatu).Include(o => o.Product).Include(o => o.ShopCustomer);
                return View(orders.ToList());
            }
            return Redirect("~/Home/AccessDenied");
        }


        public ActionResult MyOrders(int? id)
        {
            if (Session["UserID"] == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            if (id != null)
            {
                ViewBag.CompanyID = id;
                ViewBag.Orders = db.Orders.Where(o => o.UserID == id && o.OnCart == "No").Count();
                ViewBag.PendingPayment = db.Orders.Where(o => o.UserID == id && o.PaymentStatusID == 0 && o.OnCart == "No").Count();
                ViewBag.PendingDelivery = db.Orders.Where(o => o.UserID == id && o.PaymentStatusID == 1 && o.DeliveryStatus == null && o.OnCart == "No").Count();
                ViewBag.Delivered = db.Orders.Where(o => o.UserID == id && o.PaymentStatusID == 1 && o.DeliveryStatus == "Delivered" && o.OnCart == "No").Count();
                string Name = (string)Session["Name"];
                ViewBag.CompanyName = db.Users.Where(u => u.ID == id).Select(u => u.CompanyName).FirstOrDefault();
                ViewBag.Shops = db.CustomerShops.Where(s => s.UserID == id).Select(s => s.Shop).ToList();
                var orders = db.Orders.Include(o => o.PaymentStatu).Include(o => o.Product).Include(o => o.ShopCustomer);
                return View(orders.Where(o => o.UserID == id && o.OnCart == "No").OrderByDescending(o => o.DateOrdered).ToList());
            }
            return Redirect("~/Home/AccessDenied");
        }


        public ActionResult ShopOrders(int? id, int? sid)
        {
            if (id != null && Session["UserID"] != null && sid != null)
            {
                int? UserID = (int)Session["UserID"];
                ViewBag.ShopID = sid;
                ViewBag.Shopname = db.Shops.Find(sid).Name;
                ViewBag.CompanyID = sid;
                ViewBag.Orders = db.Orders.Where(o => o.Product.Shop.UserID == UserID && o.Product.ShopID == sid && o.OnCart == "No").Count();
                ViewBag.PendingPayment = db.Orders.Where(o => o.Product.Shop.UserID == UserID && o.Product.ShopID == sid && o.PaymentStatusID == 0 && o.OnCart == "No").Count();
                ViewBag.PendingDelivery = db.Orders.Where(o => o.Product.Shop.UserID == UserID && o.Product.ShopID == sid && o.PaymentStatusID == 1 && o.DeliveryStatus == null && o.OnCart == "No").Count();
                ViewBag.Delivered = db.Orders.Where(o => o.Product.Shop.UserID == UserID && o.Product.ShopID == sid && o.PaymentStatusID == 1 && o.DeliveryStatus == "Delivered" && o.OnCart == "No").Count();
                ViewBag.Negotiations = db.Orders.Where(o => o.Product.Shop.UserID == UserID && o.Product.ShopID == sid && (o.Negotiated == "Negotiating" || o.Negotiated == "Yes")).Count();
                ViewBag.CompanyName = db.Users.Where(u => u.ID == UserID).Select(u => u.CompanyName).FirstOrDefault();
                var orders = db.Orders.Include(o => o.PaymentStatu).Include(o => o.Product).Include(o => o.ShopCustomer);
                return View(orders.Where(o => o.Product.Shop.UserID == UserID && o.Product.ShopID == sid && o.OnCart == "No").OrderByDescending(o => o.DateOrdered).ToList());
            }
            return Redirect("~/Home/AccessDenied");
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["UserID"] == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            int UserID = (int)order.UserID;
            ViewBag.CompanyID = UserID;
            ViewBag.Orders = db.Orders.Where(o => o.UserID == UserID && o.OnCart == "No").Count();
            ViewBag.PendingPayment = db.Orders.Where(o => o.UserID == UserID && o.PaymentStatusID == 0 && o.OnCart == "No").Count();
            ViewBag.PendingDelivery = db.Orders.Where(o => o.UserID == UserID && o.PaymentStatusID == 1 && o.DeliveryStatus == null && o.OnCart == "No").Count();
            ViewBag.Delivered = db.Orders.Where(o => o.UserID == UserID && o.PaymentStatusID == 1 && o.DeliveryStatus == "Delivered" && o.OnCart == "No").Count();
            ViewBag.CompanyName = db.Users.Where(u => u.ID == UserID).Select(u => u.CompanyName).FirstOrDefault();
            return View(order);
        }

        public ActionResult OrderDetails(int? id)
        {
            if (id == null && Session["UserID"] == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            if (id != null && Session["UserID"] == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            int ShopID = order.Product.ShopID;
            ViewBag.CompanyID = ShopID;
            ViewBag.Orders = db.Orders.Where(o => o.Product.ShopID == ShopID && o.OnCart == "No").Count();
            ViewBag.PendingPayment = db.Orders.Where(o => o.Product.ShopID == ShopID && o.PaymentStatusID == 0 && o.OnCart == "No").Count();
            ViewBag.PendingDelivery = db.Orders.Where(o => o.Product.ShopID == ShopID && o.PaymentStatusID == 1 && o.DeliveryStatus == null && o.OnCart == "No").Count();
            ViewBag.Delivered = db.Orders.Where(o => o.Product.ShopID == ShopID && o.PaymentStatusID == 1 && o.DeliveryStatus == "Delivered" && o.OnCart == "No").Count();
            ViewBag.Negotiations = db.Orders.Where(o => o.Product.ShopID == ShopID && (o.Negotiated == "Negotiating" || o.Negotiated == "Yes")).Count();
            ViewBag.ShopName = db.Shops.Where(u => u.ID == ShopID).Select(u => u.Name).FirstOrDefault();
            return View(order);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                ViewBag.PaymentStatusID = new SelectList(db.PaymentStatus, "ID", "Status");
                ViewBag.ProductID = new SelectList(db.Products, "ID", "Name");
                ViewBag.ShopCustomerID = new SelectList(db.ShopCustomers, "ID", "Name");
                return View();
            }
            return Redirect("~/Home/AccessDenied");
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ShopCustomerID,ProductID,Quantity,Rate,Amount,DateOrdered,PaymentStatusID,PaymentRef,PaymentChannel,DatePaid,DeliveryStatus,DateDelivered")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PaymentStatusID = new SelectList(db.PaymentStatus, "ID", "Status", order.PaymentStatusID);
            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name", order.ProductID);
            ViewBag.ShopCustomerID = new SelectList(db.ShopCustomers, "ID", "Name", order.ShopCustomerID);
            return View(order);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            ViewBag.PaymentStatusID = new SelectList(db.PaymentStatus, "ID", "Status", order.PaymentStatusID);
            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name", order.ProductID);
            ViewBag.ShopCustomerID = new SelectList(db.ShopCustomers, "ID", "Name", order.ShopCustomerID);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ShopCustomerID,ProductID,Quantity,Rate,Amount,DateOrdered,PaymentStatusID,PaymentRef,PaymentChannel,DatePaid,DeliveryStatus,DateDelivered")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PaymentStatusID = new SelectList(db.PaymentStatus, "ID", "Status", order.PaymentStatusID);
            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name", order.ProductID);
            ViewBag.ShopCustomerID = new SelectList(db.ShopCustomers, "ID", "Name", order.ShopCustomerID);
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id, int? CustomerID)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            db.Orders.Remove(order);
            db.SaveChanges();
            return Redirect("~/Products/OrderDetailsCheckOut/?id=" + CustomerID);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
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

        public ActionResult Get_MyOrders(string option, int userid)
        {
            if (option == "orders")
            {

                var result = (from r in db.Orders
                              where r.UserID == userid && r.OnCart == "No"
                              select new { op = option, ProductName = r.ProductName, ID = r.ID, Quantity = r.Quantity, Rate = r.Rate, Amount = r.Amount, DateOrdered = r.DateOrdered, SubProductID = r.SubProductID, SubProductName = r.SubProductName }).OrderByDescending(r => r.DateOrdered).ToList();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else if (option == "ppayment")
            {
                //var result = (from r in db.TempPendingPayments
                //              where r.ShopCustomerID == shopcustomerid && r.Status==0
                //              select new { op = option, ProductName = r.ProductName, ID = r.ID, Quantity = r.Quantity, Rate = r.Rate, Amount = r.Amount, DateOrdered = r.DateOrdered, SubProductID = r.SubProductID, SubProductName = r.SubProductName }).OrderByDescending(r => r.DateOrdered).ToList();
                var result = (from r in db.Orders
                              where r.UserID == userid && r.PaymentStatusID == 0 && r.OnCart == "No"
                              select new { op = option, ProductName = r.ProductName, ID = r.ID, Quantity = r.Quantity, Rate = r.Rate, Amount = r.Amount, DateOrdered = r.DateOrdered, SubProductID = r.SubProductID, SubProductName = r.SubProductName }).OrderByDescending(r => r.DateOrdered).ToList();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else if (option == "pdelivery")
            {
                var result = (from r in db.Orders
                              where r.UserID == userid && r.PaymentStatusID == 1 && r.DeliveryStatus == null && r.OnCart == "No"
                              select new { op = option, ProductName = r.ProductName, ID = r.ID, Quantity = r.Quantity, Rate = r.Rate, Amount = r.Amount, DateOrdered = r.DateOrdered, SubProductID = r.SubProductID, SubProductName = r.SubProductName }).OrderByDescending(r => r.DateOrdered).ToList();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var result = (from r in db.Orders
                              where r.UserID == userid && r.PaymentStatusID == 1 && r.DeliveryStatus == "Delivered" && r.OnCart == "No"

                              select new { op = option, ProductName = r.ProductName, ID = r.ID, Quantity = r.Quantity, Rate = r.Rate, Amount = r.Amount, DateOrdered = r.DateOrdered, SubProductID = r.SubProductID, SubProductName = r.SubProductName }).OrderByDescending(r => r.DateOrdered).ToList();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Get_ShopOrders(string option, int shopid)
        {
            int CommissionPercent = Convert.ToInt32(db.Settings.Select(s => s.CommissionPercent).FirstOrDefault());
            decimal c_percent = (decimal)(CommissionPercent / 100.00);
            if (option == "orders")
            {

                var result = (from r in db.Orders
                              where r.Product.ShopID == shopid && r.OnCart == "No"
                              select new { op = option, option = option, ProductName = r.Product.Name, ID = r.ID, Quantity = r.Quantity, Rate = r.Rate, Amount = r.Amount,Commission=r.Commission,c_percent=c_percent, originalAmount=(r.SubProductID==0?r.Product.Amount:r.ProductSubProduct.Amount), DateOrdered = r.DateOrdered, SubProductID = r.SubProductID, SubProductName = r.ProductSubProduct.Name }).OrderByDescending(r => r.DateOrdered).ToList();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else if (option == "ppayment")
            {
                var result = (from r in db.Orders
                              where r.Product.ShopID == shopid && r.PaymentStatusID == 0 && r.OnCart == "No"
                              select new { op = option, option = option, ProductName = r.Product.Name, ID = r.ID, Quantity = r.Quantity, Rate = r.Rate, Amount = r.Amount, Commission = r.Commission, c_percent = c_percent, originalAmount = (r.SubProductID == 0 ? r.Product.Amount : r.ProductSubProduct.Amount), DateOrdered = r.DateOrdered, SubProductID = r.SubProductID, SubProductName = r.ProductSubProduct.Name }).OrderByDescending(r => r.DateOrdered).ToList();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else if (option == "pdelivery")
            {
                var result = (from r in db.Orders
                              where r.Product.ShopID == shopid && r.PaymentStatusID == 1 && r.DeliveryStatus == null && r.OnCart == "No"
                              select new { op = option, option = option, ProductName = r.Product.Name, ID = r.ID, Quantity = r.Quantity, Rate = r.Rate, Amount = r.Amount, Commission = r.Commission, c_percent = c_percent, originalAmount = (r.SubProductID == 0 ? r.Product.Amount : r.ProductSubProduct.Amount), DateOrdered = r.DateOrdered, SubProductID = r.SubProductID, SubProductName = r.ProductSubProduct.Name }).OrderByDescending(r => r.DateOrdered).ToList();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else if (option == "negotiation")
            {
                var result = (from r in db.Orders
                              where r.Product.ShopID == shopid && (r.Negotiated == "Negotiating" || r.Negotiated == "Yes")
                              select new { op = option, option = option, ProductName = r.Product.Name, ID = r.ID, Quantity = r.Quantity, Rate = r.Rate, Amount = r.Amount, Commission = r.Commission, c_percent = c_percent, originalAmount = (r.SubProductID == 0 ? r.Product.Amount : r.ProductSubProduct.Amount), DateOrdered = r.DateOrdered, SubProductID = r.SubProductID, SubProductName = r.ProductSubProduct.Name }).OrderByDescending(r => r.DateOrdered).ToList();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var result = (from r in db.Orders
                              where r.Product.ShopID == shopid && r.PaymentStatusID == 1 && r.DeliveryStatus == "Delivered"

                              select new { op = option, ProductName = r.Product.Name, ID = r.ID, Quantity = r.Quantity, Rate = r.Rate, Amount = r.Amount, Commission = r.Commission, c_percent = c_percent, originalAmount = (r.SubProductID == 0 ? r.Product.Amount : r.ProductSubProduct.Amount), DateOrdered = r.DateOrdered, SubProductID = r.SubProductID, SubProductName = r.ProductSubProduct.Name }).OrderByDescending(r => r.DateOrdered).ToList();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult OrderCheckOut(int? id, string src, int? sid)
        {
            if (Session["UserID"] == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            // return Content("" + id);
            if (src != null)
            {
                ViewBag.src = src;
            }
            if (id != null)
            {
                ViewBag.ShopID = sid;
                ViewBag.OrdersTotalAmount = db.Orders.Where(s => s.UserID == id && (s.Product.ShopID == sid || s.ProductSubProduct.Product.ShopID == sid) && s.PaymentStatusID == 0 && s.OnCart == "Yes").Select(s => s.Amount).Sum();
                ViewBag.Orders = db.Orders.Where(s => s.UserID == id && (s.Product.ShopID == sid || s.ProductSubProduct.Product.ShopID == sid) && s.PaymentStatusID == 0 && s.OnCart == "Yes").OrderByDescending(s => s.DateOrdered).ToList();
                ViewBag.OrdersCount = db.Orders.Where(s => s.UserID == id && (s.Product.ShopID == sid || s.ProductSubProduct.Product.ShopID == sid) && s.PaymentStatusID == 0 && s.OnCart == "Yes").OrderByDescending(s => s.DateOrdered).Count();
                ViewBag.SumTotalDeliveryCost = db.Orders.Where(s => s.UserID == id && (s.Product.ShopID == sid || s.ProductSubProduct.Product.ShopID == sid) && s.PaymentStatusID == 0 && s.OnCart == "Yes").Select(s => s.DeliveryCost).Sum();
                ViewBag.SingleDeliveryCost = db.Orders.Where(s => s.UserID == id && (s.Product.ShopID == sid || s.ProductSubProduct.Product.ShopID == sid) && s.PaymentStatusID == 0 && s.OnCart == "Yes").Select(s => s.DeliveryCost).FirstOrDefault();
                ViewBag.currency = db.Orders.Where(s => s.UserID == id && (s.Product.ShopID == sid || s.ProductSubProduct.Product.ShopID == sid) && s.PaymentStatusID == 0 && s.OnCart == "Yes").Select(s => s.Product.Currency).FirstOrDefault();
                ViewBag.OrderID = db.Orders.Where(s => s.UserID == id && (s.Product.ShopID == sid || s.ProductSubProduct.Product.ShopID == sid) && s.PaymentStatusID == 0 && s.OnCart == "Yes").Select(s => s.ID).FirstOrDefault();
                var order = new List<Order>();
                //for (var i = 0; i < 2; i++)
                for (var i = 0; i < ViewBag.OrdersCount; i++)
                {
                    order.Add(new Order());
                }
                return View(order);
            }
            return Redirect("~/Home/AccessDenied"); ;
        }

        public ActionResult Update_Payment_Product(string refno, string OrderGroupID)
        {
            int customerid = Convert.ToInt32(Session["CustomerID"]);
            var orders = db.Orders.Where(s => s.OrderGroupID == OrderGroupID).ToList();
            foreach (var iten in orders)
            {
                Update_orderStatus_after_payment(Convert.ToInt32(iten.ID), refno);
            }
            mydata.Delete_TempPayment_OrderGroupID(Convert.ToInt32(OrderGroupID));

            return Redirect("~/Orders/Details/" + customerid);
        }

        public void Update_orderStatus_after_payment(int orderid, string refno)
        {
            var orda = db.Set<Order>().Find(orderid);
            orda.PaymentRef = refno;
            orda.PaymentChannel = "Online Channel";
            orda.PaymentStatusID = 1;
            orda.DatePaid = DateTime.Now;
            orda.OnCart = "No";
            db.SaveChanges();
        }
        public ActionResult Update_Payment_Product_PayLater(string refno, string OrderGroupID)
        {
            int customerid = Convert.ToInt32(Session["CustomerID"]);
            var orders = db.Orders.Where(s => s.OrderGroupID == OrderGroupID).ToList();
            foreach (var iten in orders)
            {
                Update_orderStatus_after_payment(Convert.ToInt32(iten.ID), refno);
            }
            mydata.Delete_TempPayment_OrderGroupID(Convert.ToInt32(OrderGroupID));
       
            return Redirect("~/Orders/Details/" + customerid);
        }

        //public void Update_orderStatus_after_payment_OrderGroupID(int OrderGroupID, string refno)
        //{
        //    string channel = "Online Channel";
        //    DateTime d = DateTime.Now;
        //mydata.Update_orderStatus_after_payment_OrderGroupID(OrderGroupID,refno,channel,d);
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOrderShippingInfo(List<Order> order)
        {
            string apikey = Request.Form["apikey"];
            string Address = Request.Form["Address"];
            //int StateID =Convert.ToInt32(Request.Form["StateID"]);
            //string City = Request.Form["City"];
            //string country = Request.Form["Country"];
            string ShopID= Request.Form["ShopID"];
            string notes = Request.Form["Notes"];
            decimal Amount =Convert.ToDecimal(Request.Form["amount"]);
            int CustomerID =Convert.ToInt32(Request.Form["CustomerID"]);
            //Update Orders
            //  int CountryID = db.CountryRegions.Where(s => s.Country == country).Select(s => s.ID).FirstOrDefault();
            var OrderGroupID = Convert.ToString(mydata.Generate_OTP_Numeric());
            Session["OrderGroupID"] = OrderGroupID;
            mydata.Update_TempPayment_Status_to_zero(CustomerID);
            int UserID = CustomerID;
            foreach (var orda in order)
            {
                
                mydata.Update_Shipping_Information(orda.ID, Address, notes, DateTime.Now);
                UpdateOncart_to_No(orda.ID);
            }
           
                TempPendingPayment temp_pay = new TempPendingPayment();
                temp_pay.UserID = UserID;
                temp_pay.Amount = Amount;
                temp_pay.OrderGroupID = Convert.ToInt32(OrderGroupID);
                temp_pay.DateCreated = DateTime.Now;
                temp_pay.Status = 1;
                db.TempPendingPayments.Add(temp_pay);
                db.SaveChanges();
         
            Session["OrderGroupID"] = null;
            return Redirect("~/Orders/OrderPayment");
        }

        //public void insert_shipping_info(int orderid, string address, string notes)
        //{
        //    mydata.Update_Shipping_Information(orderid, address, notes, DateTime.Now);
        //}
        public void UpdateOncart_to_No(int orderid)
        {
            
            var ord = db.Set<Order>().Find(orderid);
            ord.OrderGroupID =(string)Session["OrderGroupID"];
            ord.OnCart = "No";
            db.SaveChanges();
        }
        public void SendCustomerOrderEmail(int orderid)
        {
            string productname = "";

            var ord = db.Set<Order>().Find(orderid);
            if (ord.SubProductID == 0)
            {
                productname = ord.Product.Name;
            }
            else
            {
                productname = ord.ProductSubProduct.Name;
            }
            var am = string.Format("{0:#,0}", ord.Amount);
            string title = "Your order details on " + ord.Product.Shop.Name;
            string msg = "<a href='https://xamagos.com' title='Xamagos'><img src='https://xamagos.com/Images/logosquare.png' style='width: 120px; height: 60px;background-color:#17A2B8' /></a>" + "<hr>" + " Hello " + ord.ShopCustomer.Name + "<br><br>" +
            "Your order details on " + ord.Product.Shop.Name + " Shop" + "<hr>" +
            "Product Name: " + productname + "<br>" +
            "Price: " + am + "<br>" +
            "Quantity: " + ord.Quantity + "<hr/>" +
            "<br>Enjoy the world of Xamagos." +
            "From:  Xamagos" + "<br/>" +
            "Email: support@xamagos.com";
            mydata.SendMail(ord.ShopCustomer.Email, title, msg);

            //==================SEND SMS=========================//
            string sms = "Order Received by " + ord.Product.Shop.Name + " Product:" + productname + " Qty:" + ord.Quantity + " Enjoy the world of Xamagos";
            mydata.Send_SMS_KudiSMS(ord.ShopCustomer.PhoneNumber, sms, "XAMAGOS", DateTime.Now);
            //==================END SEND SMS=========================//
        }

        public void SendSellerOrderEmail(int orderid)
        {
            string productname = "";

            var ord = db.Set<Order>().Find(orderid);
            if (ord.SubProductID == 0)
            {
                productname = ord.Product.Name;
            }
            else
            {
                productname = ord.ProductSubProduct.Name;
            }
            var am = string.Format("{0:#,0}", ord.Amount);
            string title = "An order has been placed on your store " + ord.Product.Shop.Name;
            string msg = "<a href='https://xamagos.com' title='Xamagos'><img src='https://xamagos.com/Images/logosquare.png' style='width: 120px; height: 60px;background-color:#17A2B8' /></a>" + "<hr>" + " Hello " + ord.Product.Shop.Name + "<br><br>" +
            "Order details on " + ord.Product.Shop.Name + "<hr>" +
            "Product Name: " + productname + "<br>" +
            "Price: " + am + "<br>" +
            "Quantity: " + ord.Quantity + "<hr/>" +
            "Customer Name/Phone: " + ord.ShopCustomer.Name + "/" + ord.ShopCustomer.PhoneNumber + "<hr/>" +
            "<br>Enjoy the world of Xamagos." +
            "From:  Xamagos" + "<br/>" +
            "Email: support@xamagos.com";
            mydata.SendMail(ord.Product.Shop.Email, title, msg);

            //==================SEND SMS=========================//
            string sms = "An order has been placed on your store " + ord.Product.Shop.Name + " Product:" + productname + " Qty:" + ord.Quantity + " Enjoy the world of Xamagos";
            mydata.Send_SMS_KudiSMS(ord.Product.Shop.PhoneNumber, sms, "XAMAGOS", DateTime.Now);
            //==================END SEND SMS=========================//

            //==================SEND SMS TO DELIVERER =========================//
            mydata.Send_SMS_KudiSMS("08081133134", sms, "XAMAGOS", DateTime.Now);
            //==================END SEND SMS TO DELIVERER=========================//
        }

        public JsonResult InsertOrder_Shipping_Info(int orderid, string address, string notes)
        {
            int customerid = (int)db.Orders.Where(s => s.ID == orderid).Select(s => s.ShopCustomerID).FirstOrDefault();
            int shopid = (int)db.Orders.Where(s => s.ID == orderid).Select(s => s.Product.ShopID).FirstOrDefault();
            var orders = db.Orders.Where(s => s.ShopCustomerID == customerid && s.Product.ShopID == shopid && s.OnCart == "Yes").ToList();
            foreach (var iten in orders)
            {
                mydata.Update_Shipping_Information(iten.ID, address, notes, DateTime.Now);
            }
            var result = from p in db.ShippingInformations
                         where p.OrderID == orderid
                         select new { ID = p.ID };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public void insert_shipping_for_all_Order(int orderid, string address, string notes)
        {
            ShippingInformation shipping = new ShippingInformation();
            shipping.Address = address;
            //shipping.City = city;
            //shipping.StateID = StateID;
            //shipping.CountryID = countryID;
            shipping.DeliveryNotes = notes;
            shipping.OrderID = orderid;
            shipping.DateCreated = DateTime.Now;
            db.ShippingInformations.Add(shipping);
            db.SaveChanges();
        }

        public ActionResult UpdateDeliveryStatus(int? id)
        {
            var ord = db.Set<Order>().Find(id);
            ord.DeliveryStatus = "Sent to Agent for Delivery";
            // ord.PaymentStatusID = 1;
            // ord.PaymentChannel = "Cash";
            ord.DateSentforDelivery = DateTime.Now;
            // ord.DatePaid = DateTime.Now;
          
            db.SaveChanges();
            //Update Stock
            int SubProductID = (int)ord.SubProductID;
            int ProductID = (int)ord.ProductID;
            if (SubProductID == 0)
            {
                int OrderQty = (int)ord.Quantity;
                int QtyInStock = (int)db.Products.Find(ProductID).QuantityinStock;
                int FinalQty = QtyInStock - OrderQty;
                var product = db.Set<Product>().Find(ProductID);
                product.QuantityinStock = FinalQty;
                db.SaveChanges();
            }
            else
            {

                int OrderQty = (int)ord.Quantity;
                int QtyInStock = (int)db.ProductSubProducts.Find(SubProductID).QuantityinStock;
                int FinalQty = QtyInStock - OrderQty;
                var product = db.Set<ProductSubProduct>().Find(SubProductID);
                product.QuantityinStock = FinalQty;
                db.SaveChanges();
            }
            return Redirect("~/Orders/OrderDetails/" + id);
        }


        public ActionResult UpdateDeliveryStatus_By_Agent(int? id)
        {
            var ord = db.Set<Order>().Find(id);
            ord.DeliveryStatus = "Delivered";
            // ord.PaymentStatusID = 1;
            // ord.PaymentChannel = "Cash";
            ord.DateDelivered = DateTime.Now;
            //GET AGENT ALLOCATION ID
            int UserID = (int)Session["UserID"];
            //Get Seller's State
            int StateID = Convert.ToInt32(ord.Product.Shop.StateID);
            int AllocationID = db.DeliveryAgentAllocations.Where(s => s.UserID == UserID && s.StateID == StateID).Select(s => s.ID).FirstOrDefault();
            ord.DeliveredByAgentAllocationID = AllocationID;
            // ord.DatePaid = DateTime.Now;
            //==========CALCULATE COMMISSION========================//
            //if (ord.Commission == null)
            //{
            //    int commission_percent = (int)db.Settings.Select(s => s.CommissionPercent).FirstOrDefault();
            //    decimal c_percent = (decimal)(commission_percent / 100.00);
            //    decimal TotalAmount = (decimal)ord.Amount;
            //    decimal totalCommission = TotalAmount * c_percent;
            //    decimal SellersMoney = TotalAmount - totalCommission;
            //    ord.SellersMoney = Convert.ToDecimal(SellersMoney);
            //    ord.Commission = Convert.ToDecimal(totalCommission);
            //}
            //==============================================//
            db.SaveChanges();
            //Update Stock
            int SubProductID = (int)ord.SubProductID;
            int ProductID = (int)ord.ProductID;
            if (SubProductID == 0)
            {
                int OrderQty = (int)ord.Quantity;
                int QtyInStock = (int)db.Products.Find(ProductID).QuantityinStock;
                int FinalQty = QtyInStock - OrderQty;
                var product = db.Set<Product>().Find(ProductID);
                product.QuantityinStock = FinalQty;
                db.SaveChanges();
            }
            else
            {

                int OrderQty = (int)ord.Quantity;
                int QtyInStock = (int)db.ProductSubProducts.Find(SubProductID).QuantityinStock;
                int FinalQty = QtyInStock - OrderQty;
                var product = db.Set<ProductSubProduct>().Find(SubProductID);
                product.QuantityinStock = FinalQty;
                db.SaveChanges();
            }
            return Redirect("~/Orders/UpdateOrder/" + id);
        }

        public ActionResult Accounts(int? id, int? sid)
        {
            if (Session["UserID"] != null && sid != null)
            {
                int? UserID = (int)Session["UserID"];
                ViewBag.ShopID = sid;
                ViewBag.Shopname = db.Shops.Find(sid).Name;
                ViewBag.CompanyID = sid;
                ViewBag.Orders = db.Orders.Where(o => o.Product.Shop.UserID == UserID && o.Product.ShopID == sid && o.OnCart == "No").Count();
                ViewBag.PendingPayment = db.Orders.Where(o => o.Product.Shop.UserID == UserID && o.Product.ShopID == sid && o.PaymentStatusID == 0).Count();
                ViewBag.PendingDelivery = db.Orders.Where(o => o.Product.Shop.UserID == UserID && o.Product.ShopID == sid && o.PaymentStatusID == 1 && o.DeliveryStatus == null).Count();
                ViewBag.Delivered = db.Orders.Where(o => o.Product.Shop.UserID == UserID && o.Product.ShopID == sid && o.PaymentStatusID == 1 && o.DeliveryStatus == "Delivered").Count();

                ViewBag.CompanyName = db.Users.Where(u => u.ID == UserID).Select(u => u.CompanyName).FirstOrDefault();
                var orders = db.Orders.Include(o => o.PaymentStatu).Include(o => o.Product).Include(o => o.ShopCustomer);
                return View(orders.Where(o => o.Product.Shop.UserID == UserID && o.Product.ShopID == sid).OrderByDescending(o => o.DateOrdered).ToList());
            }
            return Redirect("~/Home/AccessDenied");
        }


        public ActionResult Makesales(int? id, int? sid)
        {
            if (Session["UserID"] != null && sid != null)
            {
                int? UserID = (int)Session["UserID"];
                ViewBag.ShopID = sid;
                ViewBag.Shopname = db.Shops.Find(sid).Name;
                ViewBag.CompanyID = sid;
                ViewBag.Orders = db.Orders.Where(o => o.Product.Shop.UserID == UserID && o.Product.ShopID == sid && o.OnCart == "No").Count();
                ViewBag.PendingPayment = db.Orders.Where(o => o.Product.Shop.UserID == UserID && o.Product.ShopID == sid && o.PaymentStatusID == 0).Count();
                ViewBag.PendingDelivery = db.Orders.Where(o => o.Product.Shop.UserID == UserID && o.Product.ShopID == sid && o.PaymentStatusID == 1 && o.DeliveryStatus == null).Count();
                ViewBag.Delivered = db.Orders.Where(o => o.Product.Shop.UserID == UserID && o.Product.ShopID == sid && o.PaymentStatusID == 1 && o.DeliveryStatus == "Delivered").Count();

                ViewBag.CompanyName = db.Users.Where(u => u.ID == UserID).Select(u => u.CompanyName).FirstOrDefault();
                var orders = db.Orders.Include(o => o.PaymentStatu).Include(o => o.Product).Include(o => o.ShopCustomer);
                ViewBag.ShopProduct = (from p in db.Products.Where(s => s.ShopID == sid)
                                       select new { item = p.Name + " - N" + p.Amount, ID = p.ID });

                ViewBag.Products = new SelectList(ViewBag.ShopProduct, "ID", "item");
                return View(orders.Where(o => o.Product.Shop.UserID == UserID && o.Product.ShopID == sid).OrderByDescending(o => o.DateOrdered).ToList());
            }
            return Redirect("~/Home/AccessDenied");
        }

        public ActionResult MakesalesNewCustomer(int? id, int? sid)
        {
            if (Session["UserID"] != null && sid != null)
            {
                int? UserID = (int)Session["UserID"];
                ViewBag.ShopID = sid;
                ViewBag.Shopname = db.Shops.Find(sid).Name;
                ViewBag.CompanyID = sid;
                ViewBag.Orders = db.Orders.Where(o => o.Product.Shop.UserID == UserID && o.Product.ShopID == sid && o.OnCart == "No").Count();
                ViewBag.PendingPayment = db.Orders.Where(o => o.Product.Shop.UserID == UserID && o.Product.ShopID == sid && o.PaymentStatusID == 0).Count();
                ViewBag.PendingDelivery = db.Orders.Where(o => o.Product.Shop.UserID == UserID && o.Product.ShopID == sid && o.PaymentStatusID == 1 && o.DeliveryStatus == null).Count();
                ViewBag.Delivered = db.Orders.Where(o => o.Product.Shop.UserID == UserID && o.Product.ShopID == sid && o.PaymentStatusID == 1 && o.DeliveryStatus == "Delivered").Count();

                ViewBag.CompanyName = db.Users.Where(u => u.ID == UserID).Select(u => u.CompanyName).FirstOrDefault();
                var orders = db.Orders.Include(o => o.PaymentStatu).Include(o => o.Product).Include(o => o.ShopCustomer);
                ViewBag.ShopProduct = (from p in db.Products.Where(s => s.ShopID == sid)
                                       select new { item = p.Name + " - N" + p.Amount, ID = p.ID });

                ViewBag.Products = new SelectList(ViewBag.ShopProduct, "ID", "item");
                return View(orders.Where(o => o.Product.Shop.UserID == UserID && o.Product.ShopID == sid).OrderByDescending(o => o.DateOrdered).ToList());
            }
            return Redirect("~/Home/AccessDenied");
        }


        public ActionResult DailySalesReport(int? id, int? sid)
        {
            if (Session["UserID"] != null && sid != null)
            {
                int? UserID = (int)Session["UserID"];
                ViewBag.ShopID = sid;
                ViewBag.Shopname = db.Shops.Find(sid).Name;
                ViewBag.CompanyID = sid;
                ViewBag.Orders = db.Orders.Where(o => o.Product.Shop.UserID == UserID && o.Product.ShopID == sid && o.OnCart == "No").Count();
                ViewBag.PendingPayment = db.Orders.Where(o => o.Product.Shop.UserID == UserID && o.Product.ShopID == sid && o.PaymentStatusID == 0).Count();
                ViewBag.PendingDelivery = db.Orders.Where(o => o.Product.Shop.UserID == UserID && o.Product.ShopID == sid && o.PaymentStatusID == 1 && o.DeliveryStatus == null).Count();
                ViewBag.Delivered = db.Orders.Where(o => o.Product.Shop.UserID == UserID && o.Product.ShopID == sid && o.PaymentStatusID == 1 && o.DeliveryStatus == "Delivered").Count();

                ViewBag.CompanyName = db.Users.Where(u => u.ID == UserID).Select(u => u.CompanyName).FirstOrDefault();
                var orders = db.Orders.Include(o => o.PaymentStatu).Include(o => o.Product).Include(o => o.ShopCustomer);
                ViewBag.ShopProduct = (from p in db.Products.Where(s => s.ShopID == sid)
                                       select new { item = p.Name + " - N" + p.Amount, ID = p.ID });

                ViewBag.Products = new SelectList(ViewBag.ShopProduct, "ID", "item");
                return View(orders.Where(o => o.Product.Shop.UserID == UserID && o.Product.ShopID == sid).OrderByDescending(o => o.DateOrdered).ToList());
            }
            return Redirect("~/Home/AccessDenied");
        }
        public JsonResult AutoCompleteProducts(string option, int shopid)
        {
            var result = (from p in db.Products.Where(s => s.ShopID == shopid && (s.Name.Contains(option)))
                          select new { PName = p.Name, Amount = p.Amount, ID = p.ID }).Distinct();
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public JsonResult Get_Subitems(int option, int shopid)
        {

            var result = (from p in db.ProductSubProducts.Where(s => s.Product.ShopID == shopid && (s.ProductID == option))
                          select new { PName = p.Name, Amount = p.Amount, ID = p.ID }).Distinct();
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public JsonResult Get_SubitemsAmount(int option)
        {

            var result = (from p in db.ProductSubProducts.Where(s => s.ID == option)
                          select new { PName = p.Name, Amount = p.Amount, ID = p.ID }).Distinct();
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public JsonResult AutoCompletecustomer(string option, int shopid)
        {
            var result = (from p in db.CustomerShops.Where(s => s.ShopID == shopid && (s.User.CompanyName.Contains(option)))
                          select new { PName = p.User.CompanyName, Email = p.User.Email, ID = p.UserID }).Distinct();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SellNow(int shopid, int productid, int customerid, int qty, decimal amt, int? productsubproductid)
        {

            //  decimal rate =(decimal) db.Products.Where(r => r.ID == productid).Select(r => r.Amount).FirstOrDefault();
            //int SubProductID
            Order orda = new Order();
            orda.UserID = customerid;
            orda.ProductID = productid;
            if (productsubproductid == null)
            {
                orda.SubProductID = 0;
            }
            else
            {
                orda.SubProductID = productsubproductid;
            }
            
            orda.Quantity = qty;
            orda.Rate =(decimal) amt;
            orda.Amount = (decimal)(qty * amt);
            orda.DateOrdered = DateTime.Now;
            orda.DatePaid = DateTime.Now;
            orda.PaymentChannel = "Cash";
            orda.PaymentStatusID = 1;
            orda.OnCart = "No";
            db.Orders.Add(orda);
            db.SaveChanges();
            var result = (from p in db.Products.Where(s => s.ShopID == shopid)
                          select new { PName = p.Name, Amount = p.Amount, ID = p.ID }).FirstOrDefault();
            return Json(result, JsonRequestBehavior.AllowGet);

        }


        public JsonResult SellNowNewCustomer(int shopid, int productid, string customer, int qty, string phone, string email, string address, decimal amt, int? productsubproductid)
        {
            var result = (from p in db.CustomerShops.Where(s => s.ShopID == shopid)
                          select new { PName = p.ShopCustomer.Name, Email = p.ShopCustomer.Email, ID = p.ID }).Distinct();
            // decimal rate = (decimal)db.Products.Where(r => r.ID == productid).Select(r => r.Amount).FirstOrDefault();
            //int SubProductID
            //INSERT CUSTOMER TO SHOPCUSTOMERS
            ShopCustomer shopc = new ShopCustomer();
            shopc.Name = customer;
            shopc.PhoneNumber = phone;
            shopc.Email = email;
            shopc.Address = address;
            db.ShopCustomers.Add(shopc);
            db.SaveChanges();

            //=================================
            //INSERT SHOPID INTO CUSTOMERSSHOP
            CustomerShop custc = new CustomerShop();
            custc.ShopID = shopid;
            custc.UserID = shopc.ID;
            db.CustomerShops.Add(custc);
            db.SaveChanges();
            //=================
            Order orda = new Order();
            orda.UserID = shopc.ID;
            orda.ProductID = productid;
            if (productsubproductid == null)
            {
                orda.SubProductID = 0;
            }
            else
            {
                orda.SubProductID = productsubproductid;
            }
            orda.Quantity = qty;
            orda.Rate = amt;
            orda.Amount = qty * amt;
            orda.DateOrdered = DateTime.Now;
            orda.DatePaid = DateTime.Now;
            orda.PaymentChannel = "Cash";
            orda.PaymentStatusID = 1;
            orda.OnCart = "No";
            db.Orders.Add(orda);
            db.SaveChanges();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewMyNegotiations(int? id)
        {
            if (Session["UserID"] == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            if (id != null)
            {
                ViewBag.CompanyID = id;
                ViewBag.Orders = db.Orders.Where(o => o.UserID == id && o.OnCart == "No").Count();
                ViewBag.PendingPayment = db.Orders.Where(o => o.UserID == id && o.PaymentStatusID == 0 && o.OnCart == "No").Count();
                ViewBag.PendingDelivery = db.Orders.Where(o => o.UserID == id && o.PaymentStatusID == 1 && o.DeliveryStatus == null && o.OnCart == "No").Count();
                ViewBag.Delivered = db.Orders.Where(o => o.UserID == id && o.PaymentStatusID == 1 && o.DeliveryStatus == "Delivered" && o.OnCart == "No").Count();
                string Name = (string)Session["Name"];
                ViewBag.CompanyName = db.Users.Where(u => u.ID == id).Select(u => u.CompanyName).FirstOrDefault();
                ViewBag.Shops = db.CustomerShops.Where(s => s.UserID == id).Select(s => s.Shop).ToList();
                var orders = db.Orders.Include(o => o.PaymentStatu).Include(o => o.Product).Include(o => o.ShopCustomer);
                return View(orders.Where(o => o.UserID == id && (o.Negotiated == "Negotiating" || o.Negotiated == "Yes")).OrderByDescending(o => o.DateOrdered).ToList());
            }
            return Redirect("~/Home/AccessDenied");
        }

        public ActionResult UpdateOrder(int? id)
        {
            if (id == null && Session["UserID"] == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            if (id != null && Session["UserID"] == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            int ShopID = order.Product.ShopID;
            ViewBag.CompanyID = ShopID;
            ViewBag.Orders = db.Orders.Where(o => o.Product.ShopID == ShopID && o.OnCart == "No").Count();
            ViewBag.PendingPayment = db.Orders.Where(o => o.Product.ShopID == ShopID && o.PaymentStatusID == 0 && o.OnCart == "No").Count();
            ViewBag.PendingDelivery = db.Orders.Where(o => o.Product.ShopID == ShopID && o.PaymentStatusID == 1 && o.DeliveryStatus == null && o.OnCart == "No").Count();
            ViewBag.Delivered = db.Orders.Where(o => o.Product.ShopID == ShopID && o.PaymentStatusID == 1 && o.DeliveryStatus == "Delivered" && o.OnCart == "No").Count();
            ViewBag.Negotiations = db.Orders.Where(o => o.Product.ShopID == ShopID && (o.Negotiated == "Negotiating" || o.Negotiated == "Yes")).Count();
            ViewBag.ShopName = db.Shops.Where(u => u.ID == ShopID).Select(u => u.Name).FirstOrDefault();
            return View(order);
        }

        [HttpPost]
        public ActionResult UpdateDelivery_Rejected()
        {
            int OrderID = Convert.ToInt32(Request.Form["OrderID"]);
            string reason = Request.Form["reason"];
            var update = db.Set<Order>().Find(OrderID);
            update.RejectReason = reason;
            update.DateRejected = DateTime.Now;
            update.DeliveryStatus = "Rejected";
            //GET AGENT ALLOCATION ID
            int UserID = (int)Session["UserID"];
            //Get Seller's State
            int StateID = Convert.ToInt32(update.Product.Shop.StateID);
            int AllocationID = db.DeliveryAgentAllocations.Where(s => s.UserID == UserID && s.StateID == StateID).Select(s => s.ID).FirstOrDefault();
            update.DeliveredByAgentAllocationID = AllocationID;
            db.SaveChanges();
            return Redirect("~/Orders/UpdateOrder?id=" + OrderID);
        }

        [HttpPost]
        public ActionResult IncomingOrders()
        {
            if (!string.IsNullOrEmpty((string)Session["username"]))
            {
                int UserID = Convert.ToInt32(Session["UserID"]);
                var details = from p in db.DeliveryAgentAllocations
                              where p.UserID == UserID
                              select new { ID = p.StateID, item = p.State.Name };
                ViewBag.StateID = new SelectList(details, "ID", "item");
                string stateid = Request.Form["StateID"];
                ViewBag.Post = "true";
                ViewBag.State = stateid;
                int ssid = Convert.ToInt32(stateid);
                ViewBag.Statename = db.States.Where(s => s.ID == ssid).Select(s => s.Name).FirstOrDefault();
                var orders = db.Orders;
                return View(orders.Where(s => s.Product.Shop.StateID == stateid  && s.OnCart == "No" && (s.DeliveryStatus == null || s.DeliveryStatus == "Sent to Agent for Delivery")).OrderBy(s => s.ID).ToList());
            }
            return Redirect("~/Home/AccessDenied");
        }

        public ActionResult IncomingOrders(int? sid)
        {
            if (!string.IsNullOrEmpty((string)Session["username"]))
            {
                int UserID = Convert.ToInt32(Session["UserID"]);
                var details = from p in db.DeliveryAgentAllocations
                              where p.UserID == UserID
                              select new { ID = p.StateID, item = p.State.Name };
                ViewBag.StateID = new SelectList(details, "ID", "item");
                return View();
            }
            return Redirect("~/Home/AccessDenied");
        }
        public ActionResult IncomingOrdersAdmin(int? sid)
        {
            if (!string.IsNullOrEmpty((string)Session["username"]))
            {
                int UserID = Convert.ToInt32(Session["UserID"]);
                ViewBag.StateID = new SelectList(db.States, "ID", "Name");
                return View();
            }
            return Redirect("~/Home/AccessDenied");
        }
        [HttpPost]
        public ActionResult IncomingOrdersAdmin()
        {
            if (!string.IsNullOrEmpty((string)Session["username"]))
            {
                int UserID = Convert.ToInt32(Session["UserID"]);
                ViewBag.StateID = new SelectList(db.States, "ID", "Name");
                string stateid = Request.Form["StateID"];
                ViewBag.Post = "true";
                ViewBag.State = stateid;
                int ssid = Convert.ToInt32(stateid);
                ViewBag.Statename = db.States.Where(s => s.ID == ssid).Select(s => s.Name).FirstOrDefault();
                var orders = db.Orders;
                return View(orders.Where(s => s.Product.Shop.StateID == stateid && s.OnCart == "No" && (s.DeliveryStatus == null || s.DeliveryStatus == "Sent to Agent for Delivery")).OrderBy(s => s.ID).ToList());
            }
            return Redirect("~/Home/AccessDenied");
        }
  

        public ActionResult ViewOrder(int? id)
        {
            if (id == null && Session["UserID"] == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            if (id != null && Session["UserID"] == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            return View(order);
        }

        public ActionResult Get_IncomingOrders(string StateID)
        {
            int count = db.Orders.Where(s => s.Product.Shop.StateID == StateID && s.OnCart == "No" && s.DeliveryStatus == null || s.DeliveryStatus == "Sent to Agent for Delivery").Count();
            var result = (from r in db.Orders
                       where r.Product.Shop.StateID == StateID && r.OnCart=="No" && r.DeliveryStatus == null || r.DeliveryStatus == "Sent to Agent for Delivery"
                          select new { count = count, ID = r.ID,DateOrdered=r.DateOrdered}).OrderByDescending(r => r.DateOrdered).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Update_orderpayment_manual(int id)
        {
            var orda = db.Set<Order>().Find(id);
            orda.PaymentRef = Convert.ToString(mydata.Generate_OTP_Numeric());
            orda.PaymentChannel = "Paid on Delivery";
            orda.PaymentStatusID = 1;
            orda.DatePaid = DateTime.Now;
            db.SaveChanges();
            return Redirect("~/Orders/UpdatePaymentAfterDelivery?id="+id);
        }

        public ActionResult UpdatePaymentAfterDelivery(int? id)
        {
            if (id == null && Session["UserID"] != null && (string)Session["UserRole"] == "Super Admin")
            {
                return View();
            }
            else
            {
                Order order = db.Orders.Find(id);
                if (order == null)
                {
                    return Redirect("~/Home/AccessDenied");
                }
                return View(order);
            }
        }
        
        public ActionResult ViewSellersSales(int? id, int? sid)
        {
            if (Session["UserID"] != null && sid != null && (string)Session["UserRole"]=="Super Admin")
            {
                int? UserID = (int)Session["UserID"];
                ViewBag.ShopID = sid;
                ViewBag.Shopname = db.Shops.Find(sid).Name;
                ViewBag.CompanyID = sid;
                ViewBag.Orders = db.Orders.Where(o => o.Product.Shop.UserID == UserID && o.Product.ShopID == sid && o.OnCart == "No").Count();
                ViewBag.PendingPayment = db.Orders.Where(o => o.Product.Shop.UserID == UserID && o.Product.ShopID == sid && o.PaymentStatusID == 0).Count();
                ViewBag.PendingDelivery = db.Orders.Where(o => o.Product.Shop.UserID == UserID && o.Product.ShopID == sid && o.PaymentStatusID == 1 && o.DeliveryStatus == null).Count();
                ViewBag.Delivered = db.Orders.Where(o => o.Product.Shop.UserID == UserID && o.Product.ShopID == sid && o.PaymentStatusID == 1 && o.DeliveryStatus == "Delivered").Count();

                ViewBag.CompanyName = db.Users.Where(u => u.ID == UserID).Select(u => u.CompanyName).FirstOrDefault();
                var orders = db.Orders.Include(o => o.PaymentStatu).Include(o => o.Product).Include(o => o.ShopCustomer);
                return View(orders.Where(o => o.Product.Shop.UserID == UserID && o.Product.ShopID == sid).OrderByDescending(o => o.DateOrdered).ToList());
            }
            return Redirect("~/Home/AccessDenied");
        }

        public ActionResult OrderPayment()
        {
            if (Session["UserID"] == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            return View();
        }

        public ActionResult PendingPayment(int? id)
        {
            if (Session["UserID"] == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            if (id != null)
            {
                ViewBag.CompanyID = id;
                ViewBag.Orders = db.Orders.Where(o => o.UserID == id && o.OnCart == "No").Count();
                ViewBag.PendingPayment = db.Orders.Where(o => o.UserID == id && o.PaymentStatusID == 0 && o.OnCart == "No").Count();
                string Name = (string)Session["Name"];
                return View(db.TempPendingPayments.Where(o => o.UserID == id).OrderByDescending(o => o.DateCreated).ToList());
            }
            return Redirect("~/Home/AccessDenied");
        }

        public ActionResult OrdersPayment(string id)
        {
            if (Session["UserID"] == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            else
            {
                ViewBag.Orders = db.Orders.Where(s => s.OrderGroupID == id ).OrderByDescending(s => s.DateOrdered).ToList();
                ViewBag.OrdersCount = db.Orders.Where(s => s.OrderGroupID == id).OrderByDescending(s => s.DateOrdered).Count();

                return View();
            }
        }
    }
}