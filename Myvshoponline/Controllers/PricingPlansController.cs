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
    public class PricingPlansController : Controller
    {
        private MyvshoponlineEntities db = new MyvshoponlineEntities();

        // GET: PricingPlans
        public ActionResult Index()
        {
            return View(db.PricingPlans.ToList());
        }

        // GET: PricingPlans/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            PricingPlan pricingPlan = db.PricingPlans.Find(id);
            if (pricingPlan == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            return View(pricingPlan);
        }

        // GET: PricingPlans/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PricingPlans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,PlanName,Amount")] PricingPlan pricingPlan)
        {
            if (ModelState.IsValid)
            {
                db.PricingPlans.Add(pricingPlan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(pricingPlan);
        }

        // GET: PricingPlans/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            PricingPlan pricingPlan = db.PricingPlans.Find(id);
            if (pricingPlan == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            return View(pricingPlan);
        }

        // POST: PricingPlans/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,PlanName,Amount")] PricingPlan pricingPlan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pricingPlan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pricingPlan);
        }

        // GET: PricingPlans/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            PricingPlan pricingPlan = db.PricingPlans.Find(id);
            if (pricingPlan == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            return View(pricingPlan);
        }

        // POST: PricingPlans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PricingPlan pricingPlan = db.PricingPlans.Find(id);
            db.PricingPlans.Remove(pricingPlan);
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
