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
    public class PaymentAccountActivationsController : Controller
    {
        private MyvshoponlineEntities db = new MyvshoponlineEntities();

        // GET: PaymentAccountActivations
        public ActionResult Index()
        {
            var paymentAccountActivations = db.PaymentAccountActivations.Include(p => p.State);
            return View(paymentAccountActivations.ToList());
        }

        // GET: PaymentAccountActivations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            PaymentAccountActivation paymentAccountActivation = db.PaymentAccountActivations.Find(id);
            if (paymentAccountActivation == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            return View(paymentAccountActivation);
        }

        // GET: PaymentAccountActivations/Create
        public ActionResult Create()
        {
            ViewBag.StateID = new SelectList(db.States, "ID", "Name");
            return View();
        }

        // POST: PaymentAccountActivations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,StateID,Amount")] PaymentAccountActivation paymentAccountActivation)
        {
            if (ModelState.IsValid)
            {
                db.PaymentAccountActivations.Add(paymentAccountActivation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StateID = new SelectList(db.States, "ID", "Name", paymentAccountActivation.StateID);
            return View(paymentAccountActivation);
        }

        // GET: PaymentAccountActivations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            PaymentAccountActivation paymentAccountActivation = db.PaymentAccountActivations.Find(id);
            if (paymentAccountActivation == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            ViewBag.StateID = new SelectList(db.States, "ID", "Name", paymentAccountActivation.StateID);
            return View(paymentAccountActivation);
        }

        // POST: PaymentAccountActivations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,StateID,Amount")] PaymentAccountActivation paymentAccountActivation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(paymentAccountActivation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StateID = new SelectList(db.States, "ID", "Name", paymentAccountActivation.StateID);
            return View(paymentAccountActivation);
        }

        // GET: PaymentAccountActivations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            PaymentAccountActivation paymentAccountActivation = db.PaymentAccountActivations.Find(id);
            if (paymentAccountActivation == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            return View(paymentAccountActivation);
        }

        // POST: PaymentAccountActivations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PaymentAccountActivation paymentAccountActivation = db.PaymentAccountActivations.Find(id);
            db.PaymentAccountActivations.Remove(paymentAccountActivation);
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
