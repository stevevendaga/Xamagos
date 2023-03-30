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
    public class ShippingInformationsController : Controller
    {
        private MyvshoponlineEntities db = new MyvshoponlineEntities();
        Getdata mydata = new Getdata();
        // GET: ShippingInformations
        public ActionResult Index()
        {
            if (mydata.Is_ShopAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                var shippingInformations = db.ShippingInformations.Include(s => s.Order);
                return View(shippingInformations.ToList());
            }
            return Redirect("~/Home/AccessDenied");
        }

        // GET: ShippingInformations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            ShippingInformation shippingInformation = db.ShippingInformations.Find(id);
            if (shippingInformation == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            if (mydata.Is_ShopAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                return View(shippingInformation);
            }
            return Redirect("~/Home/AccessDenied");
        }

        // GET: ShippingInformations/Create
        public ActionResult Create()
        {
            if (mydata.Is_ShopAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                ViewBag.OrderID = new SelectList(db.Orders, "ID", "PaymentRef");
                return View();
            }
            return Redirect("~/Home/AccessDenied");
        }

        // POST: ShippingInformations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,OrderID,Address,State,City,DateCreated")] ShippingInformation shippingInformation)
        {
            if (ModelState.IsValid)
            {
                db.ShippingInformations.Add(shippingInformation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrderID = new SelectList(db.Orders, "ID", "PaymentRef", shippingInformation.OrderID);
            return View(shippingInformation);
        }

        // GET: ShippingInformations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            ShippingInformation shippingInformation = db.ShippingInformations.Find(id);
            if (shippingInformation == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            if (mydata.Is_ShopAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                ViewBag.OrderID = new SelectList(db.Orders, "ID", "PaymentRef", shippingInformation.OrderID);
                return View(shippingInformation);
            }
            return Redirect("~/Home/AccessDenied");
        }

        // POST: ShippingInformations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,OrderID,Address,State,City,DateCreated")] ShippingInformation shippingInformation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shippingInformation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrderID = new SelectList(db.Orders, "ID", "PaymentRef", shippingInformation.OrderID);
            return View(shippingInformation);
        }

        // GET: ShippingInformations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            ShippingInformation shippingInformation = db.ShippingInformations.Find(id);
            if (shippingInformation == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            if (mydata.Is_ShopAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                return View(shippingInformation);
            }
            return Redirect("~/Home/AccessDenied");
        }

        // POST: ShippingInformations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ShippingInformation shippingInformation = db.ShippingInformations.Find(id);
            db.ShippingInformations.Remove(shippingInformation);
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
