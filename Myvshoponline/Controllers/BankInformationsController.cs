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
    public class BankInformationsController : Controller
    {
        private MyvshoponlineEntities db = new MyvshoponlineEntities();

        // GET: BankInformations
        public ActionResult Index()
        {
            var bankInformations = db.BankInformations.Include(b => b.Bank);
            return View(bankInformations.ToList());
        }

        // GET: BankInformations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            BankInformation bankInformation = db.BankInformations.Find(id);
            if (bankInformation == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            return View(bankInformation);
        }

        // GET: BankInformations/Create
        public ActionResult Create()
        {
            ViewBag.BankID = new SelectList(db.Banks, "ID", "Name");
            return View();
        }

        // POST: BankInformations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,AccountHolder,AccountNumber,BankID,DateCreated")] BankInformation bankInformation)
        {
            if (ModelState.IsValid)
            {
                db.BankInformations.Add(bankInformation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BankID = new SelectList(db.Banks, "ID", "Name", bankInformation.BankID);
            return View(bankInformation);
        }

        // GET: BankInformations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            BankInformation bankInformation = db.BankInformations.Find(id);
            if (bankInformation == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            ViewBag.BankID = new SelectList(db.Banks, "ID", "Name", bankInformation.BankID);
            return View(bankInformation);
        }

        // POST: BankInformations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,AccountHolder,AccountNumber,BankID,DateCreated")] BankInformation bankInformation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bankInformation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BankID = new SelectList(db.Banks, "ID", "Name", bankInformation.BankID);
            return View(bankInformation);
        }

        // GET: BankInformations/Delete/5
        public JsonResult DeleteBank(int id)
        {
            BankInformation bankInformation = db.BankInformations.Find(id);
            db.BankInformations.Remove(bankInformation);
            db.SaveChanges();
            var r = "Deleted";
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        // POST: BankInformations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BankInformation bankInformation = db.BankInformations.Find(id);
            db.BankInformations.Remove(bankInformation);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public JsonResult AddBankDetails(int ShopID, string acctname, int bankid, string acctno)
        {
                BankInformation bank = new BankInformation();
                bank.ShopID = ShopID;
                bank.AccountHolder = acctname;
                bank.BankID = bankid;
                bank.AccountNumber = acctno;
                bank.DateCreated = DateTime.Now;
                db.BankInformations.Add(bank);
                db.SaveChanges();
            var result = from p in db.BankInformations
                         where p.ShopID == ShopID
                         select new { ShopID = p.ShopID, acctname =p.AccountHolder, bank = p.Bank.Name, acctno = p.AccountNumber};
            return Json(result, JsonRequestBehavior.AllowGet);
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
