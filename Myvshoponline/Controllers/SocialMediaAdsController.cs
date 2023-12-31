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
    public class SocialMediaAdsController : Controller
    {
        private MyvshoponlineEntities db = new MyvshoponlineEntities();
        Getdata mydata = new Getdata();
        // GET: SocialMediaAds
        public ActionResult Index()
        {
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                var socialMediaAds = db.SocialMediaAds.Include(s => s.PopularStoreStatu).Include(s => s.Product).Include(s => s.Shop).Include(s => s.SocialMediaChannel);
                return View(socialMediaAds.ToList());
            }
            else
            {
                return Redirect("~/Home/AccessDenied");
            }
        }

        // GET: SocialMediaAds/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            SocialMediaAd socialMediaAd = db.SocialMediaAds.Find(id);
            if (socialMediaAd == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            return View(socialMediaAd);
        }

        // GET: SocialMediaAds/Create
        public ActionResult Create()
        {
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                ViewBag.AdsStatus = new SelectList(db.PopularStoreStatus, "ID", "Status");
                ViewBag.ProductID = new SelectList(db.Products, "ID", "Name");
                ViewBag.ShopID = new SelectList(db.Shops, "ID", "Name");
                ViewBag.SocialMediaChannelID = new SelectList(db.SocialMediaChannels, "ID", "Channel");
                return View();
            }
            else
            {
                return Redirect("~/Home/AccessDenied");
            }
        }

        // POST: SocialMediaAds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,SocialMediaChannelID,ProductID,ShopID,Country,Locations,TargetPeople,TotalDaysPaid,DatePaid,RefNo,PaymentStatus,AdsStatus,EndDate,AmountPaid,DaysActive,LaunchYear,LaunchMonth,LaunchDay,PreviousYear,PreviousMonth,PreviousDay")] SocialMediaAd socialMediaAd)
        {
            if (ModelState.IsValid)
            {
                db.SocialMediaAds.Add(socialMediaAd);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AdsStatus = new SelectList(db.PopularStoreStatus, "ID", "Status", socialMediaAd.AdsStatus);
            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name", socialMediaAd.ProductID);
            ViewBag.ShopID = new SelectList(db.Shops, "ID", "Name", socialMediaAd.ShopID);
            ViewBag.SocialMediaChannelID = new SelectList(db.SocialMediaChannels, "ID", "Channel", socialMediaAd.SocialMediaChannelID);
            return View(socialMediaAd);
        }

        // GET: SocialMediaAds/Edit/5
        public ActionResult Edit(int? id)
        {
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            SocialMediaAd socialMediaAd = db.SocialMediaAds.Find(id);
            if (socialMediaAd == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            ViewBag.AdsStatus = new SelectList(db.PopularStoreStatus, "ID", "Status", socialMediaAd.AdsStatus);
            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name", socialMediaAd.ProductID);
            ViewBag.ShopID = new SelectList(db.Shops, "ID", "Name", socialMediaAd.ShopID);
            ViewBag.SocialMediaChannelID = new SelectList(db.SocialMediaChannels, "ID", "Channel", socialMediaAd.SocialMediaChannelID);
            return View(socialMediaAd);
        }
             else
            {
                return Redirect("~/Home/AccessDenied");
            }
        }

        // POST: SocialMediaAds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,SocialMediaChannelID,ProductID,ShopID,Country,Locations,TargetPeople,TotalDaysPaid,DatePaid,RefNo,PaymentStatus,AdsStatus,EndDate,AmountPaid,DaysActive,LaunchYear,LaunchMonth,LaunchDay,PreviousYear,PreviousMonth,PreviousDay")] SocialMediaAd socialMediaAd)
        {
            if (ModelState.IsValid)
            {
                db.Entry(socialMediaAd).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AdsStatus = new SelectList(db.PopularStoreStatus, "ID", "Status", socialMediaAd.AdsStatus);
            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name", socialMediaAd.ProductID);
            ViewBag.ShopID = new SelectList(db.Shops, "ID", "Name", socialMediaAd.ShopID);
            ViewBag.SocialMediaChannelID = new SelectList(db.SocialMediaChannels, "ID", "Channel", socialMediaAd.SocialMediaChannelID);
            return View(socialMediaAd);
        }

        // GET: SocialMediaAds/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            SocialMediaAd socialMediaAd = db.SocialMediaAds.Find(id);
            if (socialMediaAd == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            return View(socialMediaAd);
        }

        // POST: SocialMediaAds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SocialMediaAd socialMediaAd = db.SocialMediaAds.Find(id);
            db.SocialMediaAds.Remove(socialMediaAd);
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

        public JsonResult Get_AdsStatistics(int id)
        {

            var result = (from r in db.SocialMediaAdsStatistics
                          where r.SocialMediaAdsID == id
                          select new { Views = r.Views,Clicks=r.Clicks }).Distinct();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
