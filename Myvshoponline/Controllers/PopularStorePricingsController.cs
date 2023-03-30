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
    public class PopularStorePricingsController : Controller
    {
        private MyvshoponlineEntities db = new MyvshoponlineEntities();
        Getdata mydata = new Getdata();
        // GET: PopularStorePricings
        public ActionResult Index()
        {
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                return View(db.PopularStorePricings.ToList());
            }
            else
            {
                return Redirect("~/Home/AccessDenied");
            }
        }

        // GET: PopularStorePricings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PopularStorePricing popularStorePricing = db.PopularStorePricings.Find(id);
            if (popularStorePricing == null)
            {
                return HttpNotFound();
            }
            return View(popularStorePricing);
        }

        // GET: PopularStorePricings/Create
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

        // POST: PopularStorePricings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Day,Amount")] PopularStorePricing popularStorePricing)
        {
            if (ModelState.IsValid)
            {
                db.PopularStorePricings.Add(popularStorePricing);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(popularStorePricing);
        }

        // GET: PopularStorePricings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PopularStorePricing popularStorePricing = db.PopularStorePricings.Find(id);
            if (popularStorePricing == null)
            {
                return HttpNotFound();
            }
            return View(popularStorePricing);
        }

        // POST: PopularStorePricings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Day,Amount")] PopularStorePricing popularStorePricing)
        {
            if (ModelState.IsValid)
            {
                db.Entry(popularStorePricing).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(popularStorePricing);
        }

        // GET: PopularStorePricings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PopularStorePricing popularStorePricing = db.PopularStorePricings.Find(id);
            if (popularStorePricing == null)
            {
                return HttpNotFound();
            }
            return View(popularStorePricing);
        }

        // POST: PopularStorePricings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PopularStorePricing popularStorePricing = db.PopularStorePricings.Find(id);
            db.PopularStorePricings.Remove(popularStorePricing);
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
