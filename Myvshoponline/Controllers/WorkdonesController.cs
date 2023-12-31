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
    public class WorkdonesController : Controller
    {
        private MyvshoponlineEntities db = new MyvshoponlineEntities();

        // GET: Workdones
        public ActionResult Index(int?id)
        {
            return View(db.Workdones.Where(s => s.UserID == id).OrderByDescending(s => s.Date).ToList());
        }

        // GET: Workdones/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            Workdone workdone = db.Workdones.Find(id);
            if (workdone == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            return View(workdone);
        }

        // GET: Workdones/Create
        public ActionResult Create(int?id)
        {
            return View();
        }

        // POST: Workdones/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Description,Date,Status")] Workdone workdone)
        {
            if (ModelState.IsValid)
            {
              workdone.UserID = Convert.ToInt32(Request.QueryString["id"]);
                db.Workdones.Add(workdone);
                db.SaveChanges();
                return Redirect("~/Workdones/Index/?id="+ Request.QueryString["id"]);
            }

            return View(workdone);
        }

        // GET: Workdones/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            Workdone workdone = db.Workdones.Find(id);
            if (workdone == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            return View(workdone);
        }

        // POST: Workdones/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Description,Date,Status")] Workdone workdone)
        {
            if (ModelState.IsValid)
            {
                workdone.UserID = Convert.ToInt32(Request.QueryString["uid"]);
                db.Entry(workdone).State = EntityState.Modified;
                db.SaveChanges();
                return Redirect("~/Workdones/Index/?id=" + Request.QueryString["uid"]);
            }
            return View(workdone);
        }

        // GET: Workdones/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }

            Workdone workdone = db.Workdones.Find(id);
            db.Workdones.Remove(workdone);
            db.SaveChanges();
            return Redirect("~/Workdones/Index/?id=" + Request.QueryString["uid"]);
        }

        // POST: Workdones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Workdone workdone = db.Workdones.Find(id);
            db.Workdones.Remove(workdone);
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
