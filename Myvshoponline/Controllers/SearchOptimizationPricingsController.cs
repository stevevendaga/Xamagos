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
    public class SearchOptimizationPricingsController : Controller
    {
        private MyvshoponlineEntities db = new MyvshoponlineEntities();
        Getdata mydata = new Getdata();
        // GET: SearchOptimizationPricings
        public ActionResult Index()
        {
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                return View(db.SearchOptimizationPricings.ToList());
            }
            else
            {
                return Redirect("~/Home/AccessDenied");
            }
        }

        // GET: SearchOptimizationPricings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            SearchOptimizationPricing searchOptimizationPricing = db.SearchOptimizationPricings.Find(id);
            if (searchOptimizationPricing == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            return View(searchOptimizationPricing);
        }

        // GET: SearchOptimizationPricings/Create
        public ActionResult Create()
        {

            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                return View();
            }
            else
            {
                return Redirect("~/Home/AccessDenied");
            }
        }

        // POST: SearchOptimizationPricings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Day,Amount")] SearchOptimizationPricing searchOptimizationPricing)
        {
            if (ModelState.IsValid)
            {
                db.SearchOptimizationPricings.Add(searchOptimizationPricing);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(searchOptimizationPricing);
        }

        // GET: SearchOptimizationPricings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            SearchOptimizationPricing searchOptimizationPricing = db.SearchOptimizationPricings.Find(id);
            if (searchOptimizationPricing == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            return View(searchOptimizationPricing);
        }
            else
            {
                return Redirect("~/Home/AccessDenied");
    }
}

        // POST: SearchOptimizationPricings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Day,Amount")] SearchOptimizationPricing searchOptimizationPricing)
        {
            if (ModelState.IsValid)
            {
                db.Entry(searchOptimizationPricing).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(searchOptimizationPricing);
        }

        // GET: SearchOptimizationPricings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            SearchOptimizationPricing searchOptimizationPricing = db.SearchOptimizationPricings.Find(id);
            if (searchOptimizationPricing == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            return View(searchOptimizationPricing);
        }

        // POST: SearchOptimizationPricings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SearchOptimizationPricing searchOptimizationPricing = db.SearchOptimizationPricings.Find(id);
            db.SearchOptimizationPricings.Remove(searchOptimizationPricing);
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
