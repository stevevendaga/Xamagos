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
    public class DeliveryLocationsController : Controller
    {
        private MyvshoponlineEntities db = new MyvshoponlineEntities();

        // GET: DeliveryLocations
        public ActionResult Index(int?sid)
        {
            var deliveryLocations = db.DeliveryLocations.Include(d => d.Shop).Include(d => d.State);
            ViewBag.LocationID = new SelectList(db.States, "ID", "Name");
            ViewBag.ShopID = sid;
            ViewBag.ShopName = db.Shops.Find(sid).Name;
            int UserID =(int) Session["UserID"];
            ViewBag.UserID = UserID;
            ViewBag.CompanyName = db.Shops.Where(s => s.ID == sid).Select(s => s.User.CompanyName).FirstOrDefault();
            return View(deliveryLocations.Where(s=>s.ShopID==sid).ToList());
        }

        // GET: DeliveryLocations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeliveryLocation deliveryLocation = db.DeliveryLocations.Find(id);
            if (deliveryLocation == null)
            {
                return HttpNotFound();
            }
            return View(deliveryLocation);
        }

        // GET: DeliveryLocations/Create
        public ActionResult Create()
        {
            ViewBag.ShopID = new SelectList(db.Shops, "ID", "Name");
            ViewBag.LocationID = new SelectList(db.States, "ID", "Name");
            return View();
        }

        // POST: DeliveryLocations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,LocationID,ShopID,DateCreated")] DeliveryLocation deliveryLocation)
        {
            if (ModelState.IsValid)
            {
                db.DeliveryLocations.Add(deliveryLocation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ShopID = new SelectList(db.Shops, "ID", "Name", deliveryLocation.ShopID);
            ViewBag.LocationID = new SelectList(db.States, "ID", "Name", deliveryLocation.LocationID);
            return View(deliveryLocation);
        }

        // GET: DeliveryLocations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeliveryLocation deliveryLocation = db.DeliveryLocations.Find(id);
            if (deliveryLocation == null)
            {
                return HttpNotFound();
            }
            ViewBag.ShopID = new SelectList(db.Shops, "ID", "Name", deliveryLocation.ShopID);
            ViewBag.LocationID = new SelectList(db.States, "ID", "Name", deliveryLocation.LocationID);
            return View(deliveryLocation);
        }

        // POST: DeliveryLocations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,LocationID,ShopID,DateCreated")] DeliveryLocation deliveryLocation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(deliveryLocation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ShopID = new SelectList(db.Shops, "ID", "Name", deliveryLocation.ShopID);
            ViewBag.LocationID = new SelectList(db.States, "ID", "Name", deliveryLocation.LocationID);
            return View(deliveryLocation);
        }

        // GET: DeliveryLocations/Delete/5
        public ActionResult Delete(int? id,int?shopid)
        { 
            DeliveryLocation deliveryLocation = db.DeliveryLocations.Find(id);
            db.DeliveryLocations.Remove(deliveryLocation);
            db.SaveChanges();
            return Redirect("~/DeliveryLocations/Index/?sid=" + shopid);
        }

        public JsonResult GetLocations(int sid)
        {
            var totalLocations = db.DeliveryLocations.Where(s => s.ShopID == sid).Count();
            var result = (from r in db.DeliveryLocations.Where(s => s.ShopID == sid)
                          select new { Location = r.State.Name, Date=r.DateCreated, ID=r.ID,ShopID=r.ShopID,Total=totalLocations}).Distinct();
            return Json(result,JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveDeliveryLocation(int shopid, int locationid)
        {

            
            DeliveryLocation deliv = new DeliveryLocation();
            if (db.DeliveryLocations.Where(s => s.ShopID == shopid && s.LocationID == locationid).Count() < 1)
            {
                deliv.ShopID = shopid;
                deliv.LocationID = locationid;
                deliv.DateCreated = DateTime.Now;
                db.DeliveryLocations.Add(deliv);
                db.SaveChanges();
                var result = (from r in db.DeliveryLocations.Where(s => s.ShopID == shopid)
                              select new { ProductName = r.State.Name }).ToList();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var AlreadyExist = (from r in db.DeliveryLocations.Where(s => s.ShopID == shopid && s.LocationID==locationid)
                                    select new { ProductalreadyExist = r.State.Name }).Distinct();
                return Json(AlreadyExist, JsonRequestBehavior.AllowGet);
            }
               
               
            
        }

        // POST: DeliveryLocations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DeliveryLocation deliveryLocation = db.DeliveryLocations.Find(id);
            db.DeliveryLocations.Remove(deliveryLocation);
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
