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
    public class PopularStoresController : Controller
    {
        private MyvshoponlineEntities db = new MyvshoponlineEntities();
        Getdata mydata = new Getdata();
        // GET: PopularStores
        public ActionResult Index(int?sid)
        {
            if (mydata.Is_ShopAdmin((string)Session["username"], (string)Session["UserRole"]) && db.Shops.Find(sid).UserID == (int)Session["UserID"] || mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                var popularStores = db.PopularStores.Include(p => p.PopularStoreStatu).Include(p => p.Shop);
                return View(popularStores.Where(s => s.ShopID == sid).ToList().OrderByDescending(s => s.DatePaid).OrderByDescending(s => s.PopularStoreStatu.Status == "Active"));
            }
            else
            {
                return Redirect("~/Home/AccessDenied");
            }
        }

        public ActionResult PopularStoreAdmin()
        {
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                var popularStores = db.PopularStores.Include(p => p.PopularStoreStatu).Include(p => p.Shop);
                return View(popularStores.ToList().OrderBy(s => s.DatePaid).OrderByDescending(s => s.PopularStoreStatu.Status == "Active"));
            }
            else
            {
                return Redirect("~/Home/AccessDenied");
            }
        }

        // GET: PopularStores/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PopularStore popularStore = db.PopularStores.Find(id);
            if (popularStore == null)
            {
                return HttpNotFound();
            }
            return View(popularStore);
        }

        // GET: PopularStores/Create
        public ActionResult Create()
        {
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                ViewBag.PopularStoreStatusID = new SelectList(db.PopularStoreStatus, "ID", "Status");
                ViewBag.ShopID = new SelectList(db.Shops, "ID", "Name");
                return View();
            }
            else
            {
                return Redirect("~/Home/AccessDenied");
            }
        }

        // POST: PopularStores/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ShopID,PopularStoreStatusID,DateCreated,PaymentStatus,RefNo,Amount,DatePaid,DaysActive,TotalDaysPaid,LaunchYear,LaunchMonth,LaunchDay,PreviousYear,PreviousMonth,PreviousDay")] PopularStore popularStore)
        {
            if (ModelState.IsValid)
            {
                db.PopularStores.Add(popularStore);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PopularStoreStatusID = new SelectList(db.PopularStoreStatus, "ID", "Status", popularStore.PopularStoreStatusID);
            ViewBag.ShopID = new SelectList(db.Shops, "ID", "Name", popularStore.ShopID);
            return View(popularStore);
        }

        // GET: PopularStores/Edit/5
        public ActionResult Edit(int? id)
        {
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                PopularStore popularStore = db.PopularStores.Find(id);
                if (popularStore == null)
                {
                    return HttpNotFound();
                }
                ViewBag.PopularStoreStatusID = new SelectList(db.PopularStoreStatus, "ID", "Status", popularStore.PopularStoreStatusID);
                ViewBag.ShopID = new SelectList(db.Shops, "ID", "Name", popularStore.ShopID);
                return View(popularStore);
            }
            else
            {
                return Redirect("~/Home/AccessDenied");
            }
        }

        // POST: PopularStores/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ShopID,PopularStoreStatusID,DateCreated,PaymentStatus,RefNo,Amount,DatePaid,DaysActive,TotalDaysPaid,LaunchYear,LaunchMonth,LaunchDay,PreviousYear,PreviousMonth,PreviousDay")] PopularStore popularStore)
        {
            if (ModelState.IsValid)
            {
                db.Entry(popularStore).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PopularStoreStatusID = new SelectList(db.PopularStoreStatus, "ID", "Status", popularStore.PopularStoreStatusID);
            ViewBag.ShopID = new SelectList(db.Shops, "ID", "Name", popularStore.ShopID);
            return View(popularStore);
        }

        // GET: PopularStores/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PopularStore popularStore = db.PopularStores.Find(id);
            if (popularStore == null)
            {
                return HttpNotFound();
            }
            return View(popularStore);
        }

        // POST: PopularStores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PopularStore popularStore = db.PopularStores.Find(id);
            db.PopularStores.Remove(popularStore);
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
