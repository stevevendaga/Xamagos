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
    public class ProductCommentsController : Controller
    {
        private MyvshoponlineEntities db = new MyvshoponlineEntities();

        // GET: ProductComments
        public ActionResult Index()
        {
            return View(db.ProductComments.ToList());
        }

        // GET: ProductComments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            ProductComment productComment = db.ProductComments.Find(id);
            if (productComment == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            return View(productComment);
        }

        // GET: ProductComments/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductComments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ProductID,Comment,Name,Date,Email,Subject")] ProductComment productComment)
        {
          int id=Convert.ToInt32(Request.Form["id"]);
          int sid =Convert.ToInt32(Request.Form["shopid"]);
         if (ModelState.IsValid)
            {
                int UserID =(int) db.Shops.Find(sid).UserID;
                if (Session["UserID"]!=null)
                {
                    int UserIDSession = (int)Session["UserID"];
                
                if(UserID==UserIDSession)
                {
                    productComment.PostedBy = "Shop Admin";
                }
                else
                {
                    productComment.PostedBy = "Visitor";
                }
                }
                productComment.ProductID = id;
                productComment.Date = DateTime.Now;
                db.ProductComments.Add(productComment);
                db.SaveChanges();
                return Redirect("~/Products/ProductDetails");
            }

            return View(productComment);
        }

        // GET: ProductComments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            ProductComment productComment = db.ProductComments.Find(id);
            if (productComment == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            return View(productComment);
        }

        // POST: ProductComments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ProductID,Comment,Name,Date,Email,Subject")] ProductComment productComment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productComment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(productComment);
        }

        // GET: ProductComments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            ProductComment productComment = db.ProductComments.Find(id);
            if (productComment == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            return View(productComment);
        }

        // POST: ProductComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductComment productComment = db.ProductComments.Find(id);
            db.ProductComments.Remove(productComment);
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
