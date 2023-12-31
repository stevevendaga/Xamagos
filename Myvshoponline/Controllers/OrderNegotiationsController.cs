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
    public class OrderNegotiationsController : Controller
    {
        private MyvshoponlineEntities db = new MyvshoponlineEntities();
        Getdata mydata = new Getdata();

        // GET: OrderNegotiations
        public ActionResult Index()
        {
            var orderNegotiations = db.OrderNegotiations.Include(o => o.Order).Include(o => o.Shop).Include(o => o.ShopCustomer);
            return View(orderNegotiations.ToList());
        }

        // GET: OrderNegotiations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            OrderNegotiation orderNegotiation = db.OrderNegotiations.Find(id);
            if (orderNegotiation == null)
            {
                 return Redirect("~/Home/AccessDenied");
            }
            return View(orderNegotiation);
        }

        // GET: OrderNegotiations/Create
        public ActionResult Create()
        {
            ViewBag.OrderID = new SelectList(db.Orders, "ID", "PaymentRef");
            ViewBag.ShopID = new SelectList(db.Shops, "ID", "Name");
            ViewBag.ShopCustomerID = new SelectList(db.ShopCustomers, "ID", "Name");
            return View();
        }

        

        // POST: OrderNegotiations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,OrderID,ShopCustomerID,ShopID,Comment,CustomerOffer,SellerOffer,CustomerAccept,SellerAccept,Closed")] OrderNegotiation orderNegotiation)
        {
            if (ModelState.IsValid)
            {
                db.OrderNegotiations.Add(orderNegotiation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrderID = new SelectList(db.Orders, "ID", "PaymentRef", orderNegotiation.OrderID);
            ViewBag.ShopID = new SelectList(db.Shops, "ID", "Name", orderNegotiation.ShopID);
            ViewBag.ShopCustomerID = new SelectList(db.ShopCustomers, "ID", "Name", orderNegotiation.ShopCustomerID);
            return View(orderNegotiation);
        }

        // GET: OrderNegotiations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            OrderNegotiation orderNegotiation = db.OrderNegotiations.Find(id);
            if (orderNegotiation == null)
            {
                 return Redirect("~/Home/AccessDenied");
            }
            ViewBag.OrderID = new SelectList(db.Orders, "ID", "PaymentRef", orderNegotiation.OrderID);
            ViewBag.ShopID = new SelectList(db.Shops, "ID", "Name", orderNegotiation.ShopID);
            ViewBag.ShopCustomerID = new SelectList(db.ShopCustomers, "ID", "Name", orderNegotiation.ShopCustomerID);
            return View(orderNegotiation);
        }

        // POST: OrderNegotiations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,OrderID,ShopCustomerID,ShopID,Comment,CustomerOffer,SellerOffer,CustomerAccept,SellerAccept,Closed")] OrderNegotiation orderNegotiation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderNegotiation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrderID = new SelectList(db.Orders, "ID", "PaymentRef", orderNegotiation.OrderID);
            ViewBag.ShopID = new SelectList(db.Shops, "ID", "Name", orderNegotiation.ShopID);
            ViewBag.ShopCustomerID = new SelectList(db.ShopCustomers, "ID", "Name", orderNegotiation.ShopCustomerID);
            return View(orderNegotiation);
        }

        // GET: OrderNegotiations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            OrderNegotiation orderNegotiation = db.OrderNegotiations.Find(id);
            if (orderNegotiation == null)
            {
                 return Redirect("~/Home/AccessDenied");
            }
            return View(orderNegotiation);
        }

        // POST: OrderNegotiations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrderNegotiation orderNegotiation = db.OrderNegotiations.Find(id);
            db.OrderNegotiations.Remove(orderNegotiation);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult OrderNegos()
        {
            int id = Convert.ToInt32(Session["OrderID"]);
            if (string.IsNullOrEmpty((string)Session["Name"]) && string.IsNullOrEmpty((string)Session["username"]))
            {
                return Redirect("~/Home/AccessDenied");
            }
            if (Session["UserID"] == null)
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


        public void SaveNegotiations(string comment, int? orderid, int? customerid,int? shopid,string buyersession)
        {
            OrderNegotiation orda = new OrderNegotiation();
            orda.OrderID = orderid;
            //orda.ShopCustomerID = customerid;
            orda.UserID = customerid;
            orda.ShopID = shopid;
            orda.Comment = comment;
            //PERFORM CONDITION FOR BUYER AND SELLER
            if (!string.IsNullOrEmpty((string)Session["username"]) && buyersession=="NIL")
            {
                orda.Creator = "seller";
                //orda.SellerOffer = offer;
            }
            else
            {
                orda.Creator = "buyer";
                //orda.CustomerOffer = offer;
            }
            //orda.Quantity = qty;
            orda.DateCreated = DateTime.Now;
            db.OrderNegotiations.Add(orda);
            db.SaveChanges();
            var update = db.Set<Order>().Find(orderid);
            update.Negotiated = "Negotiating";
            db.SaveChanges();

            //if (!string.IsNullOrEmpty((string)Session["username"]))
            //{
            //    //================== SEND MAIL ============================//
            //     string title = "Xamagos: Order Negotiation";
            //    string msg = "<a href='https://xamagos.com' title='Xamagos'> <img src='https://xamagos.com/Images/logosquare.png' style='width: 160px; height: 50px;background-color:#17A2B8' /></a>" + "<hr>";
            //    msg += orda.ShopCustomer.Name + " has initiated a negotiation for " + update.Product.Name + " on <strong>" + update.Product.Shop.Name + " </strong ><br >";
            //    msg += "<br>Kindly visit your Mobile App to negotiate.";
            //    msg += "<br>Enjoy the world of Xamagos.";
            //    msg += "Best Regards:  Xamagos" + "<br>" + "Email: support@xamagos.com";
            //    mydata.SendMail(update.Product.Shop.Email, title, msg);
            //  //  ================== SEND SMS =========================//
            //     string sms = "Xamagos: " + orda.ShopCustomer.Name + " has initiated a negotiation for " + update.Product.Name + " on your store. Kindly visit your Mobile App to negotiate. Enjoy the world of Xamagos";
            //    mydata.Send_SMS_KudiSMS(update.Product.Shop.PhoneNumber, sms, "XAMAGOS", DateTime.Now);
            //    //================== END SEND SMS=========================//
            //}
            //else
            //{
            //==================SEND MAIL============================//
            if (db.OrderNegotiations.Where(s => s.OrderID == orderid).Count() == 1)
            {
                //string title = "Xamagos: Order Negotiation";
                //string msg = "<a href='https://xamagos.com' title='Xamagos'> <img src='https://xamagos.com/Images/logosquare.png' style='width: 160px; height: 50px;background-color:#17A2B8' /></a><br> Hello " + update.ShopCustomer.Name + "<hr>";
                //msg += update.Product.Shop.Name + " received your order negotiation for  " + update.Product.Name;
                //msg += "<br>Kindly visit the Mobile App to continue negotiation.";
                //msg += "<br>Enjoy the world of Xamagos.";
                //msg += "Best Regards:  Xamagos" + "<br>" + "Email: support@xamagos.com";
                //mydata.SendMail(update.ShopCustomer.Email, title, msg);
                //==================SEND SMS=========================//
                // string sms =update.User.CompanyName + " has initiated a negotiation for " + update.Product.Name + " on XAMAGOS. Kindly visit the Mobile App to negotiate.";
                //mydata.Send_SMS_KudiSMS(update.Product.Shop.PhoneNumber, sms, "XAMAGOS", DateTime.Now);
            }
            //==================END SEND SMS=========================//
            // }
            //var result = (from p in db.OrderNegotiations
            //              where p.ShopID == shopid && p.OrderID==orderid && p.ShopCustomerID==customerid
            //              select new { Comment = p.Comment, CustomerOffer = p.CustomerOffer, ID = p.ID, SellerOffer=p.SellerOffer,Quantity=p.Quantity,DateCreated=p.DateCreated,Creator=p.Creator, CustomerName=p.ShopCustomer.Name,Closed=p.Closed}).OrderBy(p=>p.DateCreated).ToList();
            // return Json(result, JsonRequestBehavior.AllowGet);

        }

        public JsonResult AcceptNegotiation(string Creator, int? NegoID)
        {
            int OrderID =(int) db.Set<OrderNegotiation>().Find(NegoID).OrderID;
            var update = db.Set<OrderNegotiation>().Find(NegoID);
            //SET ALL TO NO
            //mydata.Update_Negotiation_to_No_On_Accept_Negotiation((int)OrderID);
            if (Creator=="buyer")
                {
                update.SellerAccept = "Yes";
                }
            else
            {
                update.CustomerAccept = "Yes";
            }
            //update.Closed = "Yes";
            db.SaveChanges();
            //GET ORDER ID
            decimal SellerOffer =(db.Set<OrderNegotiation>().Find(NegoID).SellerOffer==null? 0:(decimal)db.Set<OrderNegotiation>().Find(NegoID).SellerOffer);
            decimal BuyerOffer = (db.Set<OrderNegotiation>().Find(NegoID).CustomerOffer == null ? 0 : (decimal)db.Set<OrderNegotiation>().Find(NegoID).CustomerOffer);
            int Quantity = (int)db.Set<OrderNegotiation>().Find(NegoID).Quantity;

            var updateorder = db.Set<Order>().Find(OrderID);
            updateorder.Negotiated = "Yes";
            updateorder.Rate = SellerOffer;
      //Update final negotiated price in order
      if (Creator == "seller")
            {
                updateorder.Amount =SellerOffer * Quantity;
                var result = (from p in db.OrderNegotiations
                         select new {Acceptor="buyer" }).Distinct();
            }
            else
            {
                updateorder.Amount = BuyerOffer * Quantity;
                var result = (from p in db.OrderNegotiations
                              select new { Acceptor = "seller" }).Distinct();
            }
            updateorder.Quantity =Quantity;
            db.SaveChanges();
            if (Creator == "seller")
            {
                var result = (from p in db.OrderNegotiations
                              select new { Acceptor = "buyer" }).Distinct();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //==================SEND MAIL============================//
                //string title = "Xamagos: Offer Accepted";
                //string msg = "<a href='https://xamagos.com' title='Xamagos'> <img src='https://xamagos.com/Images/logosquare.png' style='width: 120px; height: 60px;background-color:#17A2B8' /></a> Hello" + updateorder.ShopCustomer.Name + "<hr>";
                //msg += updateorder.Product.Shop.Name + " accepted your offer for " + updateorder.Product.Name + "<br>";
                //msg += "<br>Kindly view your shopping cart to place your order.";
                //msg += "<br>Enjoy the world of Xamagos.";
                //msg += "Best Regards:  Xamagos" + "<br>" + "Email: support@xamagos.com";
                //mydata.SendMail(updateorder.ShopCustomer.Email, title, msg);
                //==================SEND SMS=========================//
                //string sms = "XAMAGOS: " + update.Order.Product.Shop.Name + "accepted your offer for " + update.Order.Product.Name + " Kindly view your shopping cart to place your order. Enjoy the world of XAMAGOS";
                //mydata.Send_SMS_KudiSMS(update.ShopCustomer.PhoneNumber, sms, "XAMAGOS", DateTime.Now);
                //==================END SEND SMS=========================//
                var result = (from p in db.OrderNegotiations
                              select new { Acceptor = "seller" }).Distinct();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
           
        }

        public JsonResult get_negotiations(int? orderid, int? customerid, int? shopid)
        {
            var result = (from p in db.OrderNegotiations
                          where p.ShopID == shopid && p.OrderID == orderid && p.UserID == customerid
                          select new { Comment = p.Comment, CustomerOffer = p.CustomerOffer, ID = p.ID, SellerOffer = p.SellerOffer, Quantity = p.Quantity, DateCreated = p.DateCreated, Creator = p.Creator,SellerAccept=p.SellerAccept,BuyerAccept=p.CustomerAccept,Closed=p.Closed, ShopCustomerID =p.UserID, CustomerName=p.User.CompanyName, SellerShop=p.Shop.Name}).OrderBy(p => p.DateCreated).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveAgreedPrice(int qty,int price,int orderid,int shopid,int sellerid)
        {
            OrderNegotiation save = new OrderNegotiation();
            save.OrderID = orderid;
            save.UserID = sellerid;
            save.ShopID = shopid;
            save.Comment = "FINAL PRICE";
            save.SellerOffer = price;
            save.Quantity = qty;
            save.DateCreated = DateTime.Now;
            save.Creator="seller";
            db.OrderNegotiations.Add(save);
            db.SaveChanges();
                var result = from p in db.OrderNegotiations
                             select new { PriceSaved = "true" };
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

