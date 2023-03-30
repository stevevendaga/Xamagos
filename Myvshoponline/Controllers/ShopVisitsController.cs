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
    public class ShopVisitsController : Controller
    {
        private MyvshoponlineEntities db = new MyvshoponlineEntities();

        // GET: ShopVisits
        public ActionResult Index()
        {
            var shopVisits = db.ShopVisits.Include(s => s.Shop);
            return View(shopVisits.ToList());
        }

        // GET: ShopVisits/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShopVisit shopVisit = db.ShopVisits.Find(id);
            if (shopVisit == null)
            {
                return HttpNotFound();
            }
            return View(shopVisit);
        }

        // GET: ShopVisits/Create
        public ActionResult Create()
        {
            ViewBag.ShopID = new SelectList(db.Shops, "ID", "Name");
            return View();
        }

        // POST: ShopVisits/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ShopID,VisitorIP,Date")] ShopVisit shopVisit)
        {
            if (ModelState.IsValid)
            {
                db.ShopVisits.Add(shopVisit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ShopID = new SelectList(db.Shops, "ID", "Name", shopVisit.ShopID);
            return View(shopVisit);
        }

        // GET: ShopVisits/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShopVisit shopVisit = db.ShopVisits.Find(id);
            if (shopVisit == null)
            {
                return HttpNotFound();
            }
            ViewBag.ShopID = new SelectList(db.Shops, "ID", "Name", shopVisit.ShopID);
            return View(shopVisit);
        }

        // POST: ShopVisits/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ShopID,VisitorIP,Date")] ShopVisit shopVisit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shopVisit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ShopID = new SelectList(db.Shops, "ID", "Name", shopVisit.ShopID);
            return View(shopVisit);
        }

        // GET: ShopVisits/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShopVisit shopVisit = db.ShopVisits.Find(id);
            if (shopVisit == null)
            {
                return HttpNotFound();
            }
            return View(shopVisit);
        }

        // POST: ShopVisits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ShopVisit shopVisit = db.ShopVisits.Find(id);
            db.ShopVisits.Remove(shopVisit);
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
