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
    public class DeliveryCitiesController : Controller
    {
        private MyvshoponlineEntities db = new MyvshoponlineEntities();

        // GET: DeliveryCities
        public ActionResult Index()
        {
            var deliveryCities = db.DeliveryCities.Include(d => d.DeliveryCity_Only).Include(d => d.State).Include(d => d.State1);
            return View(deliveryCities.ToList());
        }

        // GET: DeliveryCities/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeliveryCity deliveryCity = db.DeliveryCities.Find(id);
            if (deliveryCity == null)
            {
                return HttpNotFound();
            }
            return View(deliveryCity);
        }

        // GET: DeliveryCities/Create
        public ActionResult Create()
        {
            ViewBag.CityID = new SelectList(db.DeliveryCity_Only, "ID", "City");
            ViewBag.StateID = new SelectList(db.States, "ID", "Name");
            ViewBag.FromStateID = new SelectList(db.States, "ID", "Name");
            return View();
        }

        // POST: DeliveryCities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CityID,StateID,Cost,FromStateID,DeliveryNote")] DeliveryCity deliveryCity)
        {
            if (ModelState.IsValid)
            {
                db.DeliveryCities.Add(deliveryCity);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CityID = new SelectList(db.DeliveryCity_Only, "ID", "City", deliveryCity.CityID);
            ViewBag.StateID = new SelectList(db.States, "ID", "Name", deliveryCity.StateID);
            ViewBag.FromStateID = new SelectList(db.States, "ID", "Name", deliveryCity.FromStateID);
            return View(deliveryCity);
        }

        // GET: DeliveryCities/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeliveryCity deliveryCity = db.DeliveryCities.Find(id);
            if (deliveryCity == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityID = new SelectList(db.DeliveryCity_Only, "ID", "City", deliveryCity.CityID);
            ViewBag.StateID = new SelectList(db.States, "ID", "Name", deliveryCity.StateID);
            ViewBag.FromStateID = new SelectList(db.States, "ID", "Name", deliveryCity.FromStateID);
            return View(deliveryCity);
        }

        // POST: DeliveryCities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CityID,StateID,Cost,FromStateID,DeliveryNote")] DeliveryCity deliveryCity)
        {
            if (ModelState.IsValid)
            {
                db.Entry(deliveryCity).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CityID = new SelectList(db.DeliveryCity_Only, "ID", "City", deliveryCity.CityID);
            ViewBag.StateID = new SelectList(db.States, "ID", "Name", deliveryCity.StateID);
            ViewBag.FromStateID = new SelectList(db.States, "ID", "Name", deliveryCity.FromStateID);
            return View(deliveryCity);
        }

        // GET: DeliveryCities/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeliveryCity deliveryCity = db.DeliveryCities.Find(id);
            if (deliveryCity == null)
            {
                return HttpNotFound();
            }
            return View(deliveryCity);
        }

        // POST: DeliveryCities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DeliveryCity deliveryCity = db.DeliveryCities.Find(id);
            db.DeliveryCities.Remove(deliveryCity);
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
