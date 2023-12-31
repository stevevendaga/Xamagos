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

    public class UserRolesController : Controller
    {
        private MyvshoponlineEntities db = new MyvshoponlineEntities();
        Getdata mydata = new Getdata();
     
         
        

    // GET: UserRoles
    public ActionResult Index()
        {
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                return View(db.UserRoles.ToList());
            }
            return Redirect("~/Home/AccessDenied");
        }

        // GET: UserRoles/Details/5
        public ActionResult Details(int? id)
        {
            
                if (id == null)
                {
                    return Redirect("~/Home/AccessDenied");
                }
                UserRole userRole = db.UserRoles.Find(id);
                if (userRole == null)
                {
                    return Redirect("~/Home/AccessDenied");
                }
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                return View(userRole);
            }
            return Redirect("~/Home/AccessDenied");
        }

        // GET: UserRoles/Create
        public ActionResult Create()
        {
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                return Redirect("~/Home/AccessDenied");
            }
            return View();
        
        }

        // POST: UserRoles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Role")] UserRole userRole)
        {
            if (ModelState.IsValid)
            {
                db.UserRoles.Add(userRole);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(userRole);
        }

        // GET: UserRoles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                if (id == null)
                {
                    return Redirect("~/Home/AccessDenied");
                }
                UserRole userRole = db.UserRoles.Find(id);
                if (userRole == null)
                {
                    return Redirect("~/Home/AccessDenied");
                }
                return View(userRole);
            }
                return Redirect("~/Home/AccessDenied");
            }

        // POST: UserRoles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Role")] UserRole userRole)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userRole).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userRole);
        }

        // GET: UserRoles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                if (id == null)
                {
                    return Redirect("~/Home/AccessDenied");
                }
                UserRole userRole = db.UserRoles.Find(id);
                if (userRole == null)
                {
                    return Redirect("~/Home/AccessDenied");
                }
                return View(userRole);
            }
            return Redirect("~/Home/AccessDenied");
        }

        // POST: UserRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserRole userRole = db.UserRoles.Find(id);
            db.UserRoles.Remove(userRole);
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
