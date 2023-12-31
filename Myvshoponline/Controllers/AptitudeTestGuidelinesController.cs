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
    public class AptitudeTestGuidelinesController : Controller
    {
        private MyvshoponlineEntities db = new MyvshoponlineEntities();
        Getdata mydata = new Getdata();
        // GET: AptitudeTestGuidelines
        public ActionResult Index()
        {
            if (!mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                return Redirect("~/Home/AccessDenied");

            }
            return View(db.AptitudeTestGuidelines.ToList());
        }

        // GET: AptitudeTestGuidelines/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            AptitudeTestGuideline aptitudeTestGuideline = db.AptitudeTestGuidelines.Find(id);
            if (aptitudeTestGuideline == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            return View(aptitudeTestGuideline);
        }

        // GET: AptitudeTestGuidelines/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AptitudeTestGuidelines/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "ID,Description")] AptitudeTestGuideline aptitudeTestGuideline)
        {
            if (ModelState.IsValid)
            {
                db.AptitudeTestGuidelines.Add(aptitudeTestGuideline);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(aptitudeTestGuideline);
        }

        // GET: AptitudeTestGuidelines/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            AptitudeTestGuideline aptitudeTestGuideline = db.AptitudeTestGuidelines.Find(id);
            if (aptitudeTestGuideline == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            return View(aptitudeTestGuideline);
        }

        // POST: AptitudeTestGuidelines/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "ID,Description")] AptitudeTestGuideline aptitudeTestGuideline)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aptitudeTestGuideline).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(aptitudeTestGuideline);
        }

        // GET: AptitudeTestGuidelines/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            AptitudeTestGuideline aptitudeTestGuideline = db.AptitudeTestGuidelines.Find(id);
            if (aptitudeTestGuideline == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            return View(aptitudeTestGuideline);
        }

        // POST: AptitudeTestGuidelines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AptitudeTestGuideline aptitudeTestGuideline = db.AptitudeTestGuidelines.Find(id);
            db.AptitudeTestGuidelines.Remove(aptitudeTestGuideline);
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
