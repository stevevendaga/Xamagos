﻿using System;
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
    public class DonationsController : Controller
    {
        private MyvshoponlineEntities db = new MyvshoponlineEntities();
        Getdata mydata = new Getdata();

        // GET: Donations
        public ActionResult Index()
        {
            return View(db.Donations.ToList());
        }

        // GET: Donations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            Donation donation = db.Donations.Find(id);
            if (donation == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            return View(donation);
        }

        // GET: Donations/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Donations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Email,Phone,Address,Amount,DateCreated")] Donation donation)
        {
            if (ModelState.IsValid)
            {
                db.Donations.Add(donation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(donation);
        }

        // GET: Donations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            Donation donation = db.Donations.Find(id);
            if (donation == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            return View(donation);
        }

        // POST: Donations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Email,Phone,Address,Amount,DateCreated")] Donation donation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(donation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(donation);
        }

        // GET: Donations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            Donation donation = db.Donations.Find(id);
            if (donation == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            return View(donation);
        }

        // POST: Donations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Donation donation = db.Donations.Find(id);
            db.Donations.Remove(donation);
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

        public ActionResult Update_Donation(string refno, decimal amount,string name,string phone,string address, string myemail)

        {
                       //Inser donation
            Donation donate = new Donation();
            donate.Name = name;
            donate.Phone = phone;
            donate.Email = myemail;
            donate.Address = address;
            donate.Amount = amount;
            donate.DateCreated = DateTime.Now;
            donate.RefNo = refno;
            db.Donations.Add(donate);
            db.SaveChanges();
          
            string msg = "<a href='http://marketsquare247.com' title='Market Square'> <img src='http://marketsquare247.com/Images/logosquare.jpg' style='width: 65px; height: 35px' /></a><hr>Hello! " + name + "<br/>" +
                " Thank you for supporting Market Square247. Your transaction details <br/>" +
                " Reference Number: " + refno + "<br/>" +
                " Amount Paid: " + amount / 100 + "<br/>" +
              
                " Website: http://www.marketsquare247.com <br/>" +
                "From Market Square";
            mydata.SendMail(myemail, "Market Square - Payment Made Successfully", msg);

            return Redirect("/Hone/Index/");
        }
    }
}
