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
    public class CustomerStatusController : Controller
    {
        private MyvshoponlineEntities db = new MyvshoponlineEntities();
        Getdata mydata = new Getdata();
        // GET: CustomerStatus
        public ActionResult Index()
        {
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                return View(db.CustomerStatus.ToList());
            }
            return Redirect("~/Home/AccessDenied");
        }

        // GET: CustomerStatus/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            CustomerStatu customerStatu = db.CustomerStatus.Find(id);
            if (customerStatu == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            return View(customerStatu);
        }

        // GET: CustomerStatus/Create
        public ActionResult Create()
        {
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                return View();
            }
            return Redirect("~/Home/AccessDenied");
        }

        // POST: CustomerStatus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Status")] CustomerStatu customerStatu)
        {
            if (ModelState.IsValid)
            {
                db.CustomerStatus.Add(customerStatu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customerStatu);
        }

        // GET: CustomerStatus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            CustomerStatu customerStatu = db.CustomerStatus.Find(id);
            if (customerStatu == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                return View(customerStatu);
            }
            return Redirect("~/Home/AccessDenied");
        }

        // POST: CustomerStatus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Status")] CustomerStatu customerStatu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customerStatu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customerStatu);
        }

        // GET: CustomerStatus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            CustomerStatu customerStatu = db.CustomerStatus.Find(id);
            if (customerStatu == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                return View(customerStatu);
            }
            return Redirect("~/Home/AccessDenied");
        }

        // POST: CustomerStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CustomerStatu customerStatu = db.CustomerStatus.Find(id);
            db.CustomerStatus.Remove(customerStatu);
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
