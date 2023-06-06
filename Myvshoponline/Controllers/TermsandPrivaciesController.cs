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
    public class TermsandPrivaciesController : Controller
    {
        private MyvshoponlineEntities db = new MyvshoponlineEntities();

        // GET: TermsandPrivacies
        public ActionResult Index()
        {
            return View(db.TermsandPrivacies.ToList());
        }

        // GET: TermsandPrivacies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TermsandPrivacy termsandPrivacy = db.TermsandPrivacies.Find(id);
            if (termsandPrivacy == null)
            {
                return HttpNotFound();
            }
            return View(termsandPrivacy);
        }

        // GET: TermsandPrivacies/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TermsandPrivacies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
       [HttpPost]
    //[ValidateAntiForgeryToken]
       [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "ID,Terms")] TermsandPrivacy termsandPrivacy)
        {
            if (ModelState.IsValid)
            {
        //db.TermsandPrivacies.Add(termsandPrivacy);
        //db.SaveChanges();
        db.Entry(termsandPrivacy).State = EntityState.Modified;
        db.SaveChanges();
        return Redirect("~/Home/TnC");
            }

            return View(termsandPrivacy);
        }

        // GET: TermsandPrivacies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TermsandPrivacy termsandPrivacy = db.TermsandPrivacies.Find(id);
            if (termsandPrivacy == null)
            {
                return HttpNotFound();
            }
            return View(termsandPrivacy);
        }

          // GET: TermsandPrivacies/Edit/5
          public ActionResult EditPrivacy(int? id)
          {
            if (id == null)
            {
              return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TermsandPrivacy termsandPrivacy = db.TermsandPrivacies.Find(id);
            if (termsandPrivacy == null)
            {
              return HttpNotFound();
            }
            return View(termsandPrivacy);
          }

    // POST: TermsandPrivacies/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to, for 
    // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    //[ValidateAntiForgeryToken]
    [ValidateInput(false)]
    public ActionResult Edit([Bind(Include = "ID,Terms")] TermsandPrivacy termsandPrivacy)
        {
            if (ModelState.IsValid)
            {
                db.Entry(termsandPrivacy).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(termsandPrivacy);
        }


    [HttpPost]
    //[ValidateAntiForgeryToken]
    [ValidateInput(false)]
    public ActionResult EditPrivacy([Bind(Include = "ID,Terms,PrivacyPolicy")] TermsandPrivacy termsandPrivacy)
    {
      if (ModelState.IsValid)
      {
        db.Entry(termsandPrivacy).State = EntityState.Modified;
        db.SaveChanges();
        return RedirectToAction("Index");
      }
      return View(termsandPrivacy);
    }

    // GET: TermsandPrivacies/Delete/5
    public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TermsandPrivacy termsandPrivacy = db.TermsandPrivacies.Find(id);
            if (termsandPrivacy == null)
            {
                return HttpNotFound();
            }
            return View(termsandPrivacy);
        }

        // POST: TermsandPrivacies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TermsandPrivacy termsandPrivacy = db.TermsandPrivacies.Find(id);
            db.TermsandPrivacies.Remove(termsandPrivacy);
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
