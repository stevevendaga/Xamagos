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
    public class OurLeadsController : Controller
    {
        private MyvshoponlineEntities db = new MyvshoponlineEntities();

        // GET: OurLeads
        public ActionResult Index(int?id)
        {
            var ourLeads = db.OurLeads.Include(o => o.State);
            return View(ourLeads.Where(s=>s.UserID== id).OrderByDescending(s=>s.DateCreated).ToList());
        }

        // GET: OurLeads/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OurLead ourLead = db.OurLeads.Find(id);
            if (ourLead == null)
            {
                return HttpNotFound();
            }
            return View(ourLead);
        }

        // GET: OurLeads/Create
        public ActionResult Create()
        {
            ViewBag.state = db.States.Where(s => s.Name != "All Locations");
            ViewBag.StateID = new SelectList(ViewBag.state, "ID", "Name");
            return View();
        }

        // POST: OurLeads/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Phone,Address,StateID,DateCreated")] OurLead ourLead)
        {
            if (ModelState.IsValid)
            {
                ourLead.UserID =Convert.ToInt32(Request.QueryString["id"]);
                ourLead.DateCreated = DateTime.Now;
                db.OurLeads.Add(ourLead);
                db.SaveChanges();
                return Redirect("~/OurLeads/Index/?id=" + Request.QueryString["id"]);
            }

            ViewBag.state = db.States.Where(s => s.Name != "All Locations");
            ViewBag.StateID = new SelectList(ViewBag.state, "ID", "Name", ourLead.StateID);
            return View(ourLead);
        }

        // GET: OurLeads/Edit/5
        public ActionResult Edit(int? id,int?uid)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OurLead ourLead = db.OurLeads.Find(id);
            if (ourLead == null)
            {
                return HttpNotFound();
            }
            ViewBag.state = db.States.Where(s => s.Name != "All Locations");
            ViewBag.StateID = new SelectList(ViewBag.state, "ID", "Name", ourLead.StateID);
            return View(ourLead);
        }

        // POST: OurLeads/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Phone,Address,StateID")] OurLead ourLead)
        {
            if (ModelState.IsValid)
            {
                ourLead.UserID =Convert.ToInt32(Request.QueryString["uid"]);
                db.Entry(ourLead).State = EntityState.Modified;
                db.SaveChanges();
                return Redirect("~/OurLeads/Index/?id="+Request.QueryString["uid"]);
            }
            ViewBag.state = db.States.Where(s => s.Name != "All Locations");
            ViewBag.StateID = new SelectList(ViewBag.state, "ID", "Name", ourLead.StateID);
            return View(ourLead);
        }

        // GET: OurLeads/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OurLead ourLead = db.OurLeads.Find(id);
            db.OurLeads.Remove(ourLead);
            db.SaveChanges();
            return Redirect("~/OurLeads/Index/?id="+Request.QueryString["uid"]);
        }

        // POST: OurLeads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OurLead ourLead = db.OurLeads.Find(id);
            db.OurLeads.Remove(ourLead);
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
