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
    public class PaymentStatusController : Controller
    {
        private MyvshoponlineEntities db = new MyvshoponlineEntities();
        Getdata mydata = new Getdata();
        // GET: PaymentStatus
        public ActionResult Index()
        {
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                return View(db.PaymentStatus.ToList());
            }
            return Redirect("~/Home/AccessDenied");
        }

        // GET: PaymentStatus/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            PaymentStatu paymentStatu = db.PaymentStatus.Find(id);
            if (paymentStatu == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                return View(paymentStatu);
            }
            return Redirect("~/Home/AccessDenied");
        }

        // GET: PaymentStatus/Create
        public ActionResult Create()
        {
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                return View();
            }
            return Redirect("~/Home/AccessDenied");
           }

        // POST: PaymentStatus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Status")] PaymentStatu paymentStatu)
        {
            if (ModelState.IsValid)
            {
                db.PaymentStatus.Add(paymentStatu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(paymentStatu);
        }

        // GET: PaymentStatus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            PaymentStatu paymentStatu = db.PaymentStatus.Find(id);
            if (paymentStatu == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                return View(paymentStatu);
            }
            return Redirect("~/Home/AccessDenied");
        }

        // POST: PaymentStatus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Status")] PaymentStatu paymentStatu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(paymentStatu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(paymentStatu);
        }

        // GET: PaymentStatus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            PaymentStatu paymentStatu = db.PaymentStatus.Find(id);
            if (paymentStatu == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                return View(paymentStatu);
            }
            return Redirect("~/Home/AccessDenied");
        }

        // POST: PaymentStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PaymentStatu paymentStatu = db.PaymentStatus.Find(id);
            db.PaymentStatus.Remove(paymentStatu);
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
