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
    public class SettingsController : Controller
    {
        private MyvshoponlineEntities db = new MyvshoponlineEntities();

        // GET: Settings
        public ActionResult Index()
        {
            return View(db.Settings.ToList());
        }

        // GET: Settings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            Setting setting = db.Settings.Find(id);
            if (setting == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            return View(setting);
        }

        // GET: Settings/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Settings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,AboutSection1,AboutSection2,AboutSection3,AboutSection4,OpenandCloseCBTTest,ApplicationClosed,DiscountPercentage,BonusPercentage")] Setting setting)
        {
            if (ModelState.IsValid)
            {
                db.Settings.Add(setting);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(setting);
        }

        // GET: Settings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            Setting setting = db.Settings.Find(id);
            if (setting == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            return View(setting);
        }

        // POST: Settings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,AboutSection1,AboutSection2,AboutSection3,AboutSection4,OpenandCloseCBTTest,ApplicationClosed,DiscountPercentage,BonusPercentage")] Setting setting)
        {
            if (ModelState.IsValid)
            {
                db.Entry(setting).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(setting);
        }

        // GET: Settings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            Setting setting = db.Settings.Find(id);
            if (setting == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            return View(setting);
        }

        // POST: Settings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Setting setting = db.Settings.Find(id);
            db.Settings.Remove(setting);
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
