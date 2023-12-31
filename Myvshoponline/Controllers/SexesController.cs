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
    public class SexesController : Controller
    {
        private MyvshoponlineEntities db = new MyvshoponlineEntities();

        // GET: Sexes
        public ActionResult Index()
        {
            return View(db.Sexes.ToList());
        }

        // GET: Sexes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            Sex sex = db.Sexes.Find(id);
            if (sex == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            return View(sex);
        }

        // GET: Sexes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sexes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Sex1")] Sex sex)
        {
            if (ModelState.IsValid)
            {
                db.Sexes.Add(sex);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sex);
        }

        // GET: Sexes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            Sex sex = db.Sexes.Find(id);
            if (sex == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            return View(sex);
        }

        // POST: Sexes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Sex1")] Sex sex)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sex).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sex);
        }

        // GET: Sexes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            Sex sex = db.Sexes.Find(id);
            if (sex == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            return View(sex);
        }

        // POST: Sexes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sex sex = db.Sexes.Find(id);
            db.Sexes.Remove(sex);
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
