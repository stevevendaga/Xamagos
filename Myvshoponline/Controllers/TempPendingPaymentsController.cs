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
    public class TempPendingPaymentsController : Controller
    {
        private MyvshoponlineEntities db = new MyvshoponlineEntities();

        // GET: TempPendingPayments
        public ActionResult Index()
        {
            return View(db.TempPendingPayments.ToList());
        }

        // GET: TempPendingPayments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            TempPendingPayment tempPendingPayment = db.TempPendingPayments.Find(id);
            if (tempPendingPayment == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            return View(tempPendingPayment);
        }

        // GET: TempPendingPayments/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TempPendingPayments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,OrderID,ShopCustomerID,Amount,Status,DateCreated")] TempPendingPayment tempPendingPayment)
        {
            if (ModelState.IsValid)
            {
                db.TempPendingPayments.Add(tempPendingPayment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tempPendingPayment);
        }

        // GET: TempPendingPayments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            TempPendingPayment tempPendingPayment = db.TempPendingPayments.Find(id);
            if (tempPendingPayment == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            return View(tempPendingPayment);
        }

        // POST: TempPendingPayments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,OrderID,ShopCustomerID,Amount,Status,DateCreated")] TempPendingPayment tempPendingPayment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tempPendingPayment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tempPendingPayment);
        }

        // GET: TempPendingPayments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            TempPendingPayment tempPendingPayment = db.TempPendingPayments.Find(id);
            if (tempPendingPayment == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            return View(tempPendingPayment);
        }

        // POST: TempPendingPayments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TempPendingPayment tempPendingPayment = db.TempPendingPayments.Find(id);
            db.TempPendingPayments.Remove(tempPendingPayment);
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
