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
    public class RequestOnlineOrdersController : Controller
    {
        private MyvshoponlineEntities db = new MyvshoponlineEntities();
        Getdata mydata = new Getdata();
        // GET: RequestOnlineOrders
        public ActionResult Index()
        {
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                var requestOnlineOrders = db.RequestOnlineOrders.Include(r => r.Shop);
                return View(requestOnlineOrders.ToList());
            }
            return Redirect("~/Home/AccessDenied");
        }

        // GET: RequestOnlineOrders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            RequestOnlineOrder requestOnlineOrder = db.RequestOnlineOrders.Find(id);
            if (requestOnlineOrder == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                return View(requestOnlineOrder);
            }
            return Redirect("~/Home/AccessDenied");
        }

        // GET: RequestOnlineOrders/Create
        public ActionResult Create()
        {
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                ViewBag.ShopID = new SelectList(db.Shops, "ID", "Name");
                return View();
            }
            return Redirect("~/Home/AccessDenied");
        }

        // POST: RequestOnlineOrders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ShopID,DateCreated")] RequestOnlineOrder requestOnlineOrder)
        {
            if (ModelState.IsValid)
            {
                if (db.RequestOnlineOrders.Where(s => s.ShopID == requestOnlineOrder.ShopID).Count() < 1)
                {
                    requestOnlineOrder.DateCreated = DateTime.Now;
                    db.RequestOnlineOrders.Add(requestOnlineOrder);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }

            ViewBag.ShopID = new SelectList(db.Shops, "ID", "Name", requestOnlineOrder.ShopID);
            return View(requestOnlineOrder);
        }

        // GET: RequestOnlineOrders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestOnlineOrder requestOnlineOrder = db.RequestOnlineOrders.Find(id);
            if (requestOnlineOrder == null)
            {
                return HttpNotFound();
            }
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                ViewBag.ShopID = new SelectList(db.Shops, "ID", "Name", requestOnlineOrder.ShopID);
                return View(requestOnlineOrder);
            }
            return Redirect("~/Home/AccessDenied");
        }

        // POST: RequestOnlineOrders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ShopID,DateCreated")] RequestOnlineOrder requestOnlineOrder)
        {
            if (ModelState.IsValid)
            {
                db.Entry(requestOnlineOrder).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ShopID = new SelectList(db.Shops, "ID", "Name", requestOnlineOrder.ShopID);
            return View(requestOnlineOrder);
        }

        // GET: RequestOnlineOrders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            RequestOnlineOrder requestOnlineOrder = db.RequestOnlineOrders.Find(id);
            if (requestOnlineOrder == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                return View(requestOnlineOrder);
            }
            return Redirect("~/Home/AccessDenied");
        }

        // POST: RequestOnlineOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RequestOnlineOrder requestOnlineOrder = db.RequestOnlineOrders.Find(id);
            db.RequestOnlineOrders.Remove(requestOnlineOrder);
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
