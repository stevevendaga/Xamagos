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
    public class ProductCategoriesController : Controller
    {
        private MyvshoponlineEntities db = new MyvshoponlineEntities();
        Getdata mydata = new Getdata();
        // GET: ProductCategories
        public ActionResult Index()
        {
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                return View(db.ProductCategories.ToList());
            }
            return Redirect("~/Home/AccessDenied");
        }

        public ActionResult BrowseCategories()
        {
            int id=Convert.ToInt32(Session["ProductCategoryID"]);
            if(Session["ProductCategoryID"] != null)
            {
                var products = db.Products.Where(p => p.ProductCategoryID == id && p.Shop.ShopStatus == "Active" &&  p.ProductStatu.Status == "Available");
                
                    ViewBag.ProductCategoryName = db.ProductCategories.Find(id).Name;
                    ViewBag.totalProducts = db.Products.Where(p => p.ProductCategoryID == id && p.Shop.ShopStatus == "Active" && p.ProductStatu.Status=="Available").Count();
                    ViewBag.Icon = db.ProductCategories.Find(id).Icon;
                    return View(products.ToList());
            }
             else
            {
                return Redirect("~/Home/AccessDenied");
            }
}

        // GET: ProductCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                return Redirect("~/Home/AccessDenied");
            }
          
            if (id == null)
                {
                    return Redirect("~/Home/AccessDenied");
                }
                ProductCategory productCategory = db.ProductCategories.Find(id);
                if (productCategory == null)
                {
                    return Redirect("~/Home/AccessDenied");
                }
                return View(productCategory);
           
        }

        // GET: ProductCategories/Create
        public ActionResult Create()
        {
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                return View();
            }
            return Redirect("~/Home/AccessDenied");
        }

        // POST: ProductCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Details,Icon")] ProductCategory productCategory)
        {
            if (ModelState.IsValid)
            {
                db.ProductCategories.Add(productCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(productCategory);
        }

        // GET: ProductCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                if (id == null)
                {
                    return Redirect("~/Home/AccessDenied");
                }
                ProductCategory productCategory = db.ProductCategories.Find(id);
                if (productCategory == null)
                {
                    return Redirect("~/Home/AccessDenied");
                }
                return View(productCategory);
            }
            return Redirect("~/Home/AccessDenied");
        }

        // POST: ProductCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Details,Icon")] ProductCategory productCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(productCategory);
        }

        // GET: ProductCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                if (id == null)
                {
                    return Redirect("~/Home/AccessDenied");
                }
                ProductCategory productCategory = db.ProductCategories.Find(id);
                if (productCategory == null)
                {
                    return Redirect("~/Home/AccessDenied");
                }
                return View(productCategory);
            }
            return Redirect("~/Home/AccessDenied");
        }

        // POST: ProductCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductCategory productCategory = db.ProductCategories.Find(id);
            db.ProductCategories.Remove(productCategory);
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
