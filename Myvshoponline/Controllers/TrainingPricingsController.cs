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
    public class TrainingPricingsController : Controller
    {
        private MyvshoponlineEntities db = new MyvshoponlineEntities();

        // GET: TrainingPricings
        public ActionResult Index()
        {
            return View(db.TrainingPricings.ToList());
        }

        // GET: TrainingPricings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            TrainingPricing trainingPricing = db.TrainingPricings.Find(id);
            if (trainingPricing == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            return View(trainingPricing);
        }

        // GET: TrainingPricings/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TrainingPricings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,TrainingAmount,EnablePayment")] TrainingPricing trainingPricing)
        {
            if (ModelState.IsValid)
            {
                db.TrainingPricings.Add(trainingPricing);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(trainingPricing);
        }

        // GET: TrainingPricings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            TrainingPricing trainingPricing = db.TrainingPricings.Find(id);
            if (trainingPricing == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            return View(trainingPricing);
        }

        // POST: TrainingPricings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,TrainingAmount,EnablePayment")] TrainingPricing trainingPricing)
        {
            if (ModelState.IsValid)
            {
                db.Entry(trainingPricing).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(trainingPricing);
        }

        // GET: TrainingPricings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            TrainingPricing trainingPricing = db.TrainingPricings.Find(id);
            if (trainingPricing == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            return View(trainingPricing);
        }

        // POST: TrainingPricings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TrainingPricing trainingPricing = db.TrainingPricings.Find(id);
            db.TrainingPricings.Remove(trainingPricing);
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
