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
    public class CustomerShopsController : Controller
    {
        private MyvshoponlineEntities db = new MyvshoponlineEntities();
        Getdata mydata = new Getdata();
        // GET: CustomerShops
        public ActionResult Index()
        {
            //if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            //{
                var customerShops = db.CustomerShops.Include(c => c.Shop).Include(c => c.ShopCustomer);
                return View(customerShops.ToList());
            //}
            //return Redirect("~/Home/AccessDenied");
        }

        // GET: CustomerShops/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerShop customerShop = db.CustomerShops.Find(id);
            if (customerShop == null)
            {
                return HttpNotFound();
            }
            return View(customerShop);
        }

        // GET: CustomerShops/Create
        public ActionResult Create()
        {
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                ViewBag.ShopID = new SelectList(db.Shops, "ID", "Name");
                ViewBag.ShopCustomerID = new SelectList(db.ShopCustomers, "ID", "Name");
                return View();
            }
            return Redirect("~/Home/AccessDenied");
        }

        // POST: CustomerShops/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ShopCustomerID,ShopID,Datecreated")] CustomerShop customerShop)
        {
            if (ModelState.IsValid)
            {
                db.CustomerShops.Add(customerShop);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ShopID = new SelectList(db.Shops, "ID", "Name", customerShop.ShopID);
            ViewBag.ShopCustomerID = new SelectList(db.ShopCustomers, "ID", "Name", customerShop.ShopCustomerID);
            return View(customerShop);
        }

        // GET: CustomerShops/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            CustomerShop customerShop = db.CustomerShops.Find(id);
            if (customerShop == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                ViewBag.ShopID = new SelectList(db.Shops, "ID", "Name", customerShop.ShopID);
                ViewBag.ShopCustomerID = new SelectList(db.ShopCustomers, "ID", "Name", customerShop.ShopCustomerID);
                return View(customerShop);
            }
            return Redirect("~/Home/AccessDenied");
        }

        // POST: CustomerShops/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ShopCustomerID,ShopID,Datecreated")] CustomerShop customerShop)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customerShop).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ShopID = new SelectList(db.Shops, "ID", "Name", customerShop.ShopID);
            ViewBag.ShopCustomerID = new SelectList(db.ShopCustomers, "ID", "Name", customerShop.ShopCustomerID);
            return View(customerShop);
        }

        // GET: CustomerShops/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            CustomerShop customerShop = db.CustomerShops.Find(id);
            if (customerShop == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                return View(customerShop);
            }
            return Redirect("~/Home/AccessDenied");
        }

        // POST: CustomerShops/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CustomerShop customerShop = db.CustomerShops.Find(id);
            db.CustomerShops.Remove(customerShop);
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
