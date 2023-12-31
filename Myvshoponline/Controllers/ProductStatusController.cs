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
    public class ProductStatusController : Controller
    {
        private MyvshoponlineEntities db = new MyvshoponlineEntities();
        Getdata mydata = new Getdata();
        // GET: ProductStatus
        public ActionResult Index()
        {
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                return View(db.ProductStatus.ToList());
            }
            return Redirect("~/Home/AccessDenied");
        }

        // GET: ProductStatus/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            ProductStatu productStatu = db.ProductStatus.Find(id);
            if (productStatu == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            return View(productStatu);
        }

        // GET: ProductStatus/Create
        public ActionResult Create()
        {
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                return View();
            }
            return Redirect("~/Home/AccessDenied");
        }

        // POST: ProductStatus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Status")] ProductStatu productStatu)
        {
            if (ModelState.IsValid)
            {
                db.ProductStatus.Add(productStatu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(productStatu);
        }

        // GET: ProductStatus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            ProductStatu productStatu = db.ProductStatus.Find(id);
            if (productStatu == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            return View(productStatu);
        }

        // POST: ProductStatus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Status")] ProductStatu productStatu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productStatu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(productStatu);
        }

        // GET: ProductStatus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            ProductStatu productStatu = db.ProductStatus.Find(id);
            if (productStatu == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            return View(productStatu);
        }

        // POST: ProductStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductStatu productStatu = db.ProductStatus.Find(id);
            db.ProductStatus.Remove(productStatu);
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
