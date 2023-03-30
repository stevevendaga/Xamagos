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
    public class SearchOptimizationsController : Controller
    {
        private MyvshoponlineEntities db = new MyvshoponlineEntities();
        Getdata mydata = new Getdata();

        // GET: SearchOptimizations
        public ActionResult Index()
        {
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                var searchOptimizations = db.SearchOptimizations.Include(s => s.PopularStoreStatu).Include(s => s.Product).Include(s => s.ProductSubProduct);
                return View(searchOptimizations.ToList());
            }
            else
            {
                return Redirect("~/Home/AccessDenied");
            }
        }

        // GET: SearchOptimizations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SearchOptimization searchOptimization = db.SearchOptimizations.Find(id);
            if (searchOptimization == null)
            {
                return HttpNotFound();
            }
            return View(searchOptimization);
        }

        public ActionResult SEO(int?sid)
        {
            if (mydata.Is_ShopAdmin((string)Session["username"], (string)Session["UserRole"]) && db.Shops.Find(sid).UserID == (int)Session["UserID"] || mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                ViewBag.ProductCategories = db.ShopProductCategories.Where(s => s.ShopID == sid).ToList();
                return View();
            }
            else
            {
                return Redirect("~/Home/AccessDenied");
            }
        }

        // GET: SearchOptimizations/Create
        public ActionResult Create()
        {
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                ViewBag.PopularProductStatusID = new SelectList(db.PopularStoreStatus, "ID", "Status");
                ViewBag.ProductID = new SelectList(db.Products, "ID", "Name");
                ViewBag.SubProductID = new SelectList(db.ProductSubProducts, "ID", "Name");
                return View();
            }
            else
            {
                return Redirect("~/Home/AccessDenied");
            }
        }

        // POST: SearchOptimizations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ProductID,SubProductID,PopularProductStatusID,PaymentStatus,RefNo,Amount,DatePaid,DaysActive,TotalDaysPaid,LaunchYear,LaunchMonth,LaunchDay,PreviousYear,PreviousMonth,PreviousDay,EndDate")] SearchOptimization searchOptimization)
        {
            if (ModelState.IsValid)
            {
                db.SearchOptimizations.Add(searchOptimization);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PopularProductStatusID = new SelectList(db.PopularStoreStatus, "ID", "Status", searchOptimization.PopularProductStatusID);
            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name", searchOptimization.ProductID);
            ViewBag.SubProductID = new SelectList(db.ProductSubProducts, "ID", "Name", searchOptimization.SubProductID);
            return View(searchOptimization);
        }

        // GET: SearchOptimizations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                SearchOptimization searchOptimization = db.SearchOptimizations.Find(id);
                if (searchOptimization == null)
                {
                    return HttpNotFound();
                }
                ViewBag.PopularProductStatusID = new SelectList(db.PopularStoreStatus, "ID", "Status", searchOptimization.PopularProductStatusID);
                ViewBag.ProductID = new SelectList(db.Products, "ID", "Name", searchOptimization.ProductID);
                ViewBag.SubProductID = new SelectList(db.ProductSubProducts, "ID", "Name", searchOptimization.SubProductID);
                return View(searchOptimization);
            }
            else
            {
                return Redirect("~/Home/AccessDenied");
            }
        }

        // POST: SearchOptimizations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ProductID,SubProductID,PopularProductStatusID,PaymentStatus,RefNo,Amount,DatePaid,DaysActive,TotalDaysPaid,LaunchYear,LaunchMonth,LaunchDay,PreviousYear,PreviousMonth,PreviousDay,EndDate")] SearchOptimization searchOptimization)
        {
            if (ModelState.IsValid)
            {
                db.Entry(searchOptimization).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PopularProductStatusID = new SelectList(db.PopularStoreStatus, "ID", "Status", searchOptimization.PopularProductStatusID);
            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name", searchOptimization.ProductID);
            ViewBag.SubProductID = new SelectList(db.ProductSubProducts, "ID", "Name", searchOptimization.SubProductID);
            return View(searchOptimization);
        }

        // GET: SearchOptimizations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SearchOptimization searchOptimization = db.SearchOptimizations.Find(id);
            if (searchOptimization == null)
            {
                return HttpNotFound();
            }
            return View(searchOptimization);
        }

        // POST: SearchOptimizations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SearchOptimization searchOptimization = db.SearchOptimizations.Find(id);
            db.SearchOptimizations.Remove(searchOptimization);
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

        public JsonResult GetProductSelections(int shopid)
        {
            var totalLocations = db.SearchOptimizations.Where(s => s.ShopID == shopid && s.PaymentStatus==0).Count();
            var result = (from r in db.SearchOptimizations.Where(s => s.ShopID == shopid && s.PaymentStatus == 0)
                          select new { ProductName = r.Product.Name, ID = r.ID,Total = totalLocations,ProductID=r.ProductID }).Distinct();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveSelectionsTemp(int shopid, int productid)
        {


            SearchOptimization seo = new SearchOptimization();
            if (db.SearchOptimizations.Where(s => s.ShopID == shopid && s.ProductID == productid && s.PaymentStatus==0).Count() < 1)
            {
                int StatusID= db.PopularStoreStatus.Where(s => s.Status == "Pending").Select(s => s.ID).FirstOrDefault();
                seo.ShopID = shopid;
                seo.ProductID = productid;
                seo.PaymentStatus = 0;
                seo.PopularProductStatusID = StatusID;
                db.SearchOptimizations.Add(seo);
                db.SaveChanges();
                var result = (from r in db.SearchOptimizations.Where(s => s.ShopID == shopid)
                              select new { ProductName = r.Product.Name }).ToList();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var AlreadyExist = (from r in db.SearchOptimizations.Where(s => s.ShopID == shopid && s.ProductID == productid)
                                    select new { ProductalreadyExist = r.Product.Name, ProductID = r.ProductID,ID=r.ID }).Distinct();
                return Json(AlreadyExist, JsonRequestBehavior.AllowGet);
            }

            

        }

        public void DeleteProduct(int id)
        {
            SearchOptimization searchOptimization = db.SearchOptimizations.Find(id);
            db.SearchOptimizations.Remove(searchOptimization);
            db.SaveChanges();
        }

        
        public void Update_Payment_Promote_SEO(string refno, decimal amount, int ShopID, int noofdays,string enddate)
        {
           //UPDATE SEOoptimization
            int StatusID = db.PopularStoreStatus.Where(s => s.Status == "Active").Select(s => s.ID).FirstOrDefault();
            mydata.Update_Payment_Promote_SEO_Sql(refno, amount, ShopID, noofdays,enddate, StatusID);
        }

    }
}
