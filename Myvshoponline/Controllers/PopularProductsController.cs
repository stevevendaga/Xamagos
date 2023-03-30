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
    public class PopularProductsController : Controller
    {
        private MyvshoponlineEntities db = new MyvshoponlineEntities();
        Getdata mydata = new Getdata();
        // GET: PopularProducts
        public ActionResult Index()
        {
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                var popularProducts = db.PopularProducts.Include(p => p.PopularStoreStatu).Include(p => p.Product).Include(p => p.ProductSubProduct);
                return View(popularProducts.ToList());
            }
            else
            {
                return Redirect("~/Home/AccessDenied");
            }
        }

        public ActionResult PopularProducts(int? sid)
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

        // GET: PopularProducts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PopularProduct popularProduct = db.PopularProducts.Find(id);
            if (popularProduct == null)
            {
                return HttpNotFound();
            }
            return View(popularProduct);
        }

        // GET: PopularProducts/Create
        public ActionResult Create()
        {
            ViewBag.PopularProductStatusID = new SelectList(db.PopularStoreStatus, "ID", "Status");
            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name");
            ViewBag.SubProductID = new SelectList(db.ProductSubProducts, "ID", "Name");
            return View();
        }

        // POST: PopularProducts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ProductID,SubProductID,PopularProductStatusID,PaymentStatus,RefNo,Amount,DatePaid,DaysActive,TotalDaysPaid,LaunchYear,LaunchMonth,LaunchDay,PreviousYear,PreviousMonth,PreviousDay,EndDate")] PopularProduct popularProduct)
        {
            if (ModelState.IsValid)
            {
                db.PopularProducts.Add(popularProduct);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PopularProductStatusID = new SelectList(db.PopularStoreStatus, "ID", "Status", popularProduct.PopularProductStatusID);
            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name", popularProduct.ProductID);
            ViewBag.SubProductID = new SelectList(db.ProductSubProducts, "ID", "Name", popularProduct.SubProductID);
            return View(popularProduct);
        }

        // GET: PopularProducts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PopularProduct popularProduct = db.PopularProducts.Find(id);
            if (popularProduct == null)
            {
                return HttpNotFound();
            }
            ViewBag.PopularProductStatusID = new SelectList(db.PopularStoreStatus, "ID", "Status", popularProduct.PopularProductStatusID);
            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name", popularProduct.ProductID);
            ViewBag.SubProductID = new SelectList(db.ProductSubProducts, "ID", "Name", popularProduct.SubProductID);
            return View(popularProduct);
        }

        // POST: PopularProducts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ProductID,SubProductID,PopularProductStatusID,PaymentStatus,RefNo,Amount,DatePaid,DaysActive,TotalDaysPaid,LaunchYear,LaunchMonth,LaunchDay,PreviousYear,PreviousMonth,PreviousDay,EndDate")] PopularProduct popularProduct)
        {
            if (ModelState.IsValid)
            {
                db.Entry(popularProduct).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PopularProductStatusID = new SelectList(db.PopularStoreStatus, "ID", "Status", popularProduct.PopularProductStatusID);
            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name", popularProduct.ProductID);
            ViewBag.SubProductID = new SelectList(db.ProductSubProducts, "ID", "Name", popularProduct.SubProductID);
            return View(popularProduct);
        }

        // GET: PopularProducts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PopularProduct popularProduct = db.PopularProducts.Find(id);
            if (popularProduct == null)
            {
                return HttpNotFound();
            }
            return View(popularProduct);
        }

        // POST: PopularProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PopularProduct popularProduct = db.PopularProducts.Find(id);
            db.PopularProducts.Remove(popularProduct);
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

        public void Update_Payment_Promote_PopularProducts(string refno, decimal amount, int ShopID, int noofdays, string enddate)
        {
            //UPDATE PopularProducts
            int StatusID = db.PopularStoreStatus.Where(s => s.Status == "Active").Select(s => s.ID).FirstOrDefault();
            mydata.Update_Payment_Promote_PopularProduct_Sql(refno, amount, ShopID, noofdays, enddate, StatusID);
        }

        public JsonResult SaveSelectionsTemp(int shopid, int productid)
        {


            PopularProduct seo = new PopularProduct();
            if (db.PopularProducts.Where(s => s.ShopID == shopid && s.ProductID == productid && s.PaymentStatus == 0).Count() < 1)
            {
                int StatusID = db.PopularStoreStatus.Where(s => s.Status == "Pending").Select(s => s.ID).FirstOrDefault();
                seo.ShopID = shopid;
                seo.ProductID = productid;
                seo.PaymentStatus = 0;
                seo.PopularProductStatusID = StatusID;
                db.PopularProducts.Add(seo);
                db.SaveChanges();
                var result = (from r in db.PopularProducts.Where(s => s.ShopID == shopid)
                              select new { ProductName = r.Product.Name }).ToList();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var AlreadyExist = (from r in db.PopularProducts.Where(s => s.ShopID == shopid && s.ProductID == productid)
                                    select new { ProductalreadyExist = r.Product.Name, ProductID = r.ProductID, ID = r.ID }).Distinct();
                return Json(AlreadyExist, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetProductSelections(int shopid)
        {
            var totalLocations = db.PopularProducts.Where(s => s.ShopID == shopid && s.PaymentStatus==0).Count();
            var result = (from r in db.PopularProducts.Where(s => s.ShopID == shopid && s.PaymentStatus == 0)
                          select new { ProductName = r.Product.Name, ID = r.ID, Total = totalLocations, ProductID = r.ProductID }).Distinct();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public void DeleteProduct(int id)
        {
            PopularProduct popular = db.PopularProducts.Find(id);
            db.PopularProducts.Remove(popular);
            db.SaveChanges();
        }


    }
}
