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
    public class PlanFeaturesController : Controller
    {
        private MyvshoponlineEntities db = new MyvshoponlineEntities();

        // GET: PlanFeatures
        public ActionResult Index()
        {
            var planFeatures = db.PlanFeatures.Include(p => p.PricingPlan);
            return View(planFeatures.ToList());
        }

        // GET: PlanFeatures/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            PlanFeature planFeature = db.PlanFeatures.Find(id);
            if (planFeature == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            return View(planFeature);
        }

        // GET: PlanFeatures/Create
        public ActionResult Create()
        {
            ViewBag.PricingPlanID = new SelectList(db.PricingPlans, "ID", "PlanName");
            return View();
        }

        // POST: PlanFeatures/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,PricingPlanID,Feature")] PlanFeature planFeature)
        {
            if (ModelState.IsValid)
            {
                db.PlanFeatures.Add(planFeature);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PricingPlanID = new SelectList(db.PricingPlans, "ID", "PlanName", planFeature.PricingPlanID);
            return View(planFeature);
        }

        // GET: PlanFeatures/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            PlanFeature planFeature = db.PlanFeatures.Find(id);
            if (planFeature == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            ViewBag.PricingPlanID = new SelectList(db.PricingPlans, "ID", "PlanName", planFeature.PricingPlanID);
            return View(planFeature);
        }

        // POST: PlanFeatures/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,PricingPlanID,Feature")] PlanFeature planFeature)
        {
            if (ModelState.IsValid)
            {
                db.Entry(planFeature).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PricingPlanID = new SelectList(db.PricingPlans, "ID", "PlanName", planFeature.PricingPlanID);
            return View(planFeature);
        }

        // GET: PlanFeatures/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            PlanFeature planFeature = db.PlanFeatures.Find(id);
            if (planFeature == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            return View(planFeature);
        }

        // POST: PlanFeatures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PlanFeature planFeature = db.PlanFeatures.Find(id);
            db.PlanFeatures.Remove(planFeature);
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
