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
    public class DeliveryAgentAllocationsController : Controller
    {
        private MyvshoponlineEntities db = new MyvshoponlineEntities();
        Getdata mydata = new Getdata();

        // GET: DeliveryAgentAllocations
        public ActionResult Index()
        {
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            { 
                var deliveryAgentAllocations = db.DeliveryAgentAllocations.Include(d => d.State).Include(d => d.User);
            return View(deliveryAgentAllocations.ToList());
        }
            else
            {
                return Redirect("~/Home/AccessDenied");
            }
        }

        public ActionResult AllAgents()
        {
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                var deliveryAgentAllocations = db.DeliveryAgentAllocations.Include(d => d.State).Include(d => d.User);
                return View(deliveryAgentAllocations.ToList());
            }
            else
            {
                return Redirect("~/Home/AccessDenied");
            }
        }

        public ActionResult ItemDelivered()
        {
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                var deliveryAgentAllocations = db.DeliveryAgentAllocations.Include(d => d.State).Include(d => d.User);
                return View();
            }
            else
            {
                return Redirect("~/Home/AccessDenied");
            }
        }

        // GET: DeliveryAgentAllocations/Details/5
        public ActionResult Details(int? id)
        {
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                if (id == null)
                {
                    return Redirect("~/Home/AccessDenied");
                }
                DeliveryAgentAllocation deliveryAgentAllocation = db.DeliveryAgentAllocations.Find(id);
                if (deliveryAgentAllocation == null)
                {
                    return Redirect("~/Home/AccessDenied");
                }
                return View(deliveryAgentAllocation);
            }
            else
            {
                return Redirect("~/Home/AccessDenied");
            }

        }

        // GET: DeliveryAgentAllocations/Create
        public ActionResult Create()
        {
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                ViewBag.StateID = new SelectList(db.States, "ID", "Name");
                var details = from p in db.Users
                              select new { ID = p.ID, item = p.CompanyName + " - " + p.Email };
            ViewBag.UserID = new SelectList(details, "ID", "item");
            return View();
            }
            else
            {
                return Redirect("~/Home/AccessDenied");
            }
        }

        // POST: DeliveryAgentAllocations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,UserID,StateID,DateCreated,Status")] DeliveryAgentAllocation deliveryAgentAllocation)
        {
            if (ModelState.IsValid)
            {
                db.DeliveryAgentAllocations.Add(deliveryAgentAllocation);
                deliveryAgentAllocation.DateCreated = DateTime.Now;
                deliveryAgentAllocation.Status = "Active";
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StateID = new SelectList(db.States, "ID", "Name", deliveryAgentAllocation.StateID);
            var details = from p in db.Users
                          select new { ID = p.ID, item = p.CompanyName + " - " + p.Email };
            ViewBag.UserID = new SelectList(details, "ID", "item");
            return View(deliveryAgentAllocation);
        }

        // GET: DeliveryAgentAllocations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            DeliveryAgentAllocation deliveryAgentAllocation = db.DeliveryAgentAllocations.Find(id);
            if (deliveryAgentAllocation == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            ViewBag.StateID = new SelectList(db.States, "ID", "Name", deliveryAgentAllocation.StateID);
                var details = from p in db.Users
                              select new { ID = p.ID, item = p.CompanyName + " - " + p.Email };
                ViewBag.UserID = new SelectList(details, "ID", "item");
                return View(deliveryAgentAllocation);
        }
            else
            {
                return Redirect("~/Home/AccessDenied");
    }
}

        // POST: DeliveryAgentAllocations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,UserID,StateID,DateCreated,Status")] DeliveryAgentAllocation deliveryAgentAllocation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(deliveryAgentAllocation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StateID = new SelectList(db.States, "ID", "Name", deliveryAgentAllocation.StateID);
            var details = from p in db.Users
                          select new { ID = p.ID, item = p.CompanyName + " - " + p.Email };
            ViewBag.UserID = new SelectList(details, "ID", "item");
            return View(deliveryAgentAllocation);
        }

        // GET: DeliveryAgentAllocations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            DeliveryAgentAllocation deliveryAgentAllocation = db.DeliveryAgentAllocations.Find(id);
            if (deliveryAgentAllocation == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            return View(deliveryAgentAllocation);
            }
            else
            {
                return Redirect("~/Home/AccessDenied");
            }
        }

        // POST: DeliveryAgentAllocations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DeliveryAgentAllocation deliveryAgentAllocation = db.DeliveryAgentAllocations.Find(id);
            db.DeliveryAgentAllocations.Remove(deliveryAgentAllocation);
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
