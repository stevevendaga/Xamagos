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
    public class SocialMediaAdsStatisticsController : Controller
    {
        private MyvshoponlineEntities db = new MyvshoponlineEntities();
        Getdata mydata = new Getdata();
        // GET: SocialMediaAdsStatistics
        public ActionResult Index()
        {
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                var socialMediaAdsStatistics = db.SocialMediaAdsStatistics.Include(s => s.SocialMediaAd);
                return View(socialMediaAdsStatistics.ToList());
            }
            else
            {
                return Redirect("~/Home/AccessDenied");
            }

        }

        // GET: SocialMediaAdsStatistics/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            SocialMediaAdsStatistic socialMediaAdsStatistic = db.SocialMediaAdsStatistics.Find(id);
            if (socialMediaAdsStatistic == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            return View(socialMediaAdsStatistic);
        }

        // GET: SocialMediaAdsStatistics/Create
        public ActionResult Create(int?aid)
        {
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                ViewBag.SocialMediaAdsID = new SelectList(db.SocialMediaAds, "ID", "Country");
                return View();
            }
            else
            {
                return Redirect("~/Home/AccessDenied");
            }
        }

        // POST: SocialMediaAdsStatistics/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,SocialMediaAdsID,Date,Views,Clicks")] SocialMediaAdsStatistic socialMediaAdsStatistic)
        {
            if (ModelState.IsValid)
            {
                db.SocialMediaAdsStatistics.Add(socialMediaAdsStatistic);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SocialMediaAdsID = new SelectList(db.SocialMediaAds, "ID", "Country", socialMediaAdsStatistic.SocialMediaAdsID);
            return View(socialMediaAdsStatistic);
        }

        // GET: SocialMediaAdsStatistics/Edit/5
        public ActionResult Edit(int? id)
        {

            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            SocialMediaAdsStatistic socialMediaAdsStatistic = db.SocialMediaAdsStatistics.Find(id);
            if (socialMediaAdsStatistic == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            ViewBag.SocialMediaAdsID = new SelectList(db.SocialMediaAds, "ID", "Country", socialMediaAdsStatistic.SocialMediaAdsID);
            return View(socialMediaAdsStatistic);
} else
{
return Redirect("~/Home/AccessDenied");
}
}

        // POST: SocialMediaAdsStatistics/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,SocialMediaAdsID,Date,Views,Clicks")] SocialMediaAdsStatistic socialMediaAdsStatistic)
        {
            if (ModelState.IsValid)
            {
                db.Entry(socialMediaAdsStatistic).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SocialMediaAdsID = new SelectList(db.SocialMediaAds, "ID", "Country", socialMediaAdsStatistic.SocialMediaAdsID);
            return View(socialMediaAdsStatistic);
        }

        // GET: SocialMediaAdsStatistics/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            SocialMediaAdsStatistic socialMediaAdsStatistic = db.SocialMediaAdsStatistics.Find(id);
            if (socialMediaAdsStatistic == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            return View(socialMediaAdsStatistic);
        }

        // POST: SocialMediaAdsStatistics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SocialMediaAdsStatistic socialMediaAdsStatistic = db.SocialMediaAdsStatistics.Find(id);
            db.SocialMediaAdsStatistics.Remove(socialMediaAdsStatistic);
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
