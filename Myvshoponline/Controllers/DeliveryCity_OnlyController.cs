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
    public class DeliveryCity_OnlyController : Controller
    {
        private MyvshoponlineEntities db = new MyvshoponlineEntities();

        // GET: DeliveryCity_Only
        public ActionResult Index()
        {
            var deliveryCity_Only = db.DeliveryCity_Only.Include(d => d.State);
            return View(deliveryCity_Only.ToList());
        }

        // GET: DeliveryCity_Only/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeliveryCity_Only deliveryCity_Only = db.DeliveryCity_Only.Find(id);
            if (deliveryCity_Only == null)
            {
                return HttpNotFound();
            }
            return View(deliveryCity_Only);
        }

        // GET: DeliveryCity_Only/Create
        public ActionResult Create()
        {
            ViewBag.StateID = new SelectList(db.States, "ID", "Name");
            return View();
        }

        // POST: DeliveryCity_Only/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,StateID,City")] DeliveryCity_Only deliveryCity_Only)
        {
            if (ModelState.IsValid)
            {
                db.DeliveryCity_Only.Add(deliveryCity_Only);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StateID = new SelectList(db.States, "ID", "Name", deliveryCity_Only.StateID);
            return View(deliveryCity_Only);
        }

        // GET: DeliveryCity_Only/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeliveryCity_Only deliveryCity_Only = db.DeliveryCity_Only.Find(id);
            if (deliveryCity_Only == null)
            {
                return HttpNotFound();
            }
            ViewBag.StateID = new SelectList(db.States, "ID", "Name", deliveryCity_Only.StateID);
            return View(deliveryCity_Only);
        }

        // POST: DeliveryCity_Only/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,StateID,City")] DeliveryCity_Only deliveryCity_Only)
        {
            if (ModelState.IsValid)
            {
                db.Entry(deliveryCity_Only).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StateID = new SelectList(db.States, "ID", "Name", deliveryCity_Only.StateID);
            return View(deliveryCity_Only);
        }

        // GET: DeliveryCity_Only/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeliveryCity_Only deliveryCity_Only = db.DeliveryCity_Only.Find(id);
            if (deliveryCity_Only == null)
            {
                return HttpNotFound();
            }
            return View(deliveryCity_Only);
        }

        // POST: DeliveryCity_Only/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DeliveryCity_Only deliveryCity_Only = db.DeliveryCity_Only.Find(id);
            db.DeliveryCity_Only.Remove(deliveryCity_Only);
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
