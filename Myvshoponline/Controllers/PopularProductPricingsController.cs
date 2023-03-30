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
    public class PopularProductPricingsController : Controller
    {
        private MyvshoponlineEntities db = new MyvshoponlineEntities();
        Getdata mydata = new Getdata();
        // GET: PopularProductPricings
        public ActionResult Index()
        {
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                return View(db.PopularProductPricings.ToList());
            }
            else
            {
                return Redirect("~/Home/AccessDenied");
            }
        }

        // GET: PopularProductPricings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PopularProductPricing popularProductPricing = db.PopularProductPricings.Find(id);
            if (popularProductPricing == null)
            {
                return HttpNotFound();
            }
            return View(popularProductPricing);
        }

        // GET: PopularProductPricings/Create
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

        // POST: PopularProductPricings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Amount")] PopularProductPricing popularProductPricing)
        {
            if (ModelState.IsValid)
            {
                db.PopularProductPricings.Add(popularProductPricing);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(popularProductPricing);
        }

        // GET: PopularProductPricings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                PopularProductPricing popularProductPricing = db.PopularProductPricings.Find(id);
                if (popularProductPricing == null)
                {
                    return HttpNotFound();
                }
                return View(popularProductPricing);
            }
            else
            {
                return Redirect("~/Home/AccessDenied");
            }

        }

        // POST: PopularProductPricings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Amount")] PopularProductPricing popularProductPricing)
        {
            if (ModelState.IsValid)
            {
                db.Entry(popularProductPricing).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(popularProductPricing);
        }

        // GET: PopularProductPricings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PopularProductPricing popularProductPricing = db.PopularProductPricings.Find(id);
            if (popularProductPricing == null)
            {
                return HttpNotFound();
            }
            return View(popularProductPricing);
        }

        // POST: PopularProductPricings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PopularProductPricing popularProductPricing = db.PopularProductPricings.Find(id);
            db.PopularProductPricings.Remove(popularProductPricing);
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
