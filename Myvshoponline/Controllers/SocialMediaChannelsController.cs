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
    public class SocialMediaChannelsController : Controller
    {
        private MyvshoponlineEntities db = new MyvshoponlineEntities();
        Getdata mydata = new Getdata();
        // GET: SocialMediaChannels
        public ActionResult Index()
        {

            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
            return View(db.SocialMediaChannels.ToList());
            }
            else
            {
            return Redirect("~/Home/AccessDenied");
           }

}

        // GET: SocialMediaChannels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SocialMediaChannel socialMediaChannel = db.SocialMediaChannels.Find(id);
            if (socialMediaChannel == null)
            {
                return HttpNotFound();
            }
            return View(socialMediaChannel);
        }

        // GET: SocialMediaChannels/Create
        public ActionResult Create()
        {
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                return View();
            }
            else
            {
                return Redirect("~/Home/AccessDenied");
            }
        }

        // POST: SocialMediaChannels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Channel,CostPerDay")] SocialMediaChannel socialMediaChannel)
        {
            if (ModelState.IsValid)
            {
                db.SocialMediaChannels.Add(socialMediaChannel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(socialMediaChannel);
        }

        // GET: SocialMediaChannels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SocialMediaChannel socialMediaChannel = db.SocialMediaChannels.Find(id);
            if (socialMediaChannel == null)
            {
                return HttpNotFound();
            }
            return View(socialMediaChannel);
            }
            else
            {
            return Redirect("~/Home/AccessDenied");
    }
}

        // POST: SocialMediaChannels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Channel,CostPerDay")] SocialMediaChannel socialMediaChannel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(socialMediaChannel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(socialMediaChannel);
        }

        // GET: SocialMediaChannels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SocialMediaChannel socialMediaChannel = db.SocialMediaChannels.Find(id);
            if (socialMediaChannel == null)
            {
                return HttpNotFound();
            }
            return View(socialMediaChannel);
        }

        // POST: SocialMediaChannels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SocialMediaChannel socialMediaChannel = db.SocialMediaChannels.Find(id);
            db.SocialMediaChannels.Remove(socialMediaChannel);
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
