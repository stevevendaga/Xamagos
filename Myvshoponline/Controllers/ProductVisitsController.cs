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
    public class ProductVisitsController : Controller
    {
        private MyvshoponlineEntities db = new MyvshoponlineEntities();

        // GET: ProductVisits
        public ActionResult Index()
        {
            var productVisits = db.ProductVisits.Include(p => p.Product).Include(p => p.ProductSubProduct);
            return View(productVisits.ToList());
        }

        // GET: ProductVisits/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            ProductVisit productVisit = db.ProductVisits.Find(id);
            if (productVisit == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            return View(productVisit);
        }

        // GET: ProductVisits/Create
        public ActionResult Create()
        {
            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name");
            ViewBag.ProductSubItemID = new SelectList(db.ProductSubProducts, "ID", "Name");
            return View();
        }

        // POST: ProductVisits/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ProductID,ProductSubItemID,VisitorIP,Date")] ProductVisit productVisit)
        {
            if (ModelState.IsValid)
            {
                db.ProductVisits.Add(productVisit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name", productVisit.ProductID);
            ViewBag.ProductSubItemID = new SelectList(db.ProductSubProducts, "ID", "Name", productVisit.ProductSubItemID);
            return View(productVisit);
        }

        // GET: ProductVisits/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            ProductVisit productVisit = db.ProductVisits.Find(id);
            if (productVisit == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name", productVisit.ProductID);
            ViewBag.ProductSubItemID = new SelectList(db.ProductSubProducts, "ID", "Name", productVisit.ProductSubItemID);
            return View(productVisit);
        }

        // POST: ProductVisits/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ProductID,ProductSubItemID,VisitorIP,Date")] ProductVisit productVisit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productVisit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name", productVisit.ProductID);
            ViewBag.ProductSubItemID = new SelectList(db.ProductSubProducts, "ID", "Name", productVisit.ProductSubItemID);
            return View(productVisit);
        }

        // GET: ProductVisits/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            ProductVisit productVisit = db.ProductVisits.Find(id);
            if (productVisit == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            return View(productVisit);
        }

        // POST: ProductVisits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductVisit productVisit = db.ProductVisits.Find(id);
            db.ProductVisits.Remove(productVisit);
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
