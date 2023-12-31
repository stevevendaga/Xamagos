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
    public class EmailMessagesController : Controller
    {
        private MyvshoponlineEntities db = new MyvshoponlineEntities();

        // GET: EmailMessages
        public ActionResult Index()
        {
            if ((string)Session["UserRole"] != "Site Admin" && (string)Session["UserRole"] != "Super Admin")
            {
                return Redirect("~/Home/AccessDenied");
            }
            return View(db.EmailMessages.OrderByDescending(s=>s.ID).ToList());
        }

        // GET: EmailMessages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            EmailMessage emailMessage = db.EmailMessages.Find(id);
            if (emailMessage == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            return View(emailMessage);
        }

        // GET: EmailMessages/Create
        public ActionResult Create()
        {
            if ((string)Session["UserRole"] != "Site Admin" && (string)Session["UserRole"] != "Super Admin")
            {
                return Redirect("~/Home/AccessDenied");
            }
            return View();
        }

        // POST: EmailMessages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "ID,Subject,Message,DateCreated,msgKey")] EmailMessage emailMessage)
        {
            if (ModelState.IsValid)
            {
                emailMessage.DateCreated = DateTime.Now;
                db.EmailMessages.Add(emailMessage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(emailMessage);
        }

        // GET: EmailMessages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            EmailMessage emailMessage = db.EmailMessages.Find(id);
            if (emailMessage == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            return View(emailMessage);
        }

        // POST: EmailMessages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "ID,Subject,Message,DateCreated,msgKey")] EmailMessage emailMessage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(emailMessage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(emailMessage);
        }

        // GET: EmailMessages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            EmailMessage emailMessage = db.EmailMessages.Find(id);
            if (emailMessage == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            return View(emailMessage);
        }

        // POST: EmailMessages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EmailMessage emailMessage = db.EmailMessages.Find(id);
            db.EmailMessages.Remove(emailMessage);
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
