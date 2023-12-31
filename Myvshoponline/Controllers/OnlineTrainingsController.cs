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
    public class OnlineTrainingsController : Controller
    {
        private MyvshoponlineEntities db = new MyvshoponlineEntities();
        Getdata mydata = new Getdata();

        // GET: OnlineTrainings
        public ActionResult Index()
        {
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                var onlineTrainings = db.OnlineTrainings.Include(o => o.Sex).Include(o => o.State).Include(o => o.State1);
                return View(onlineTrainings.ToList());
            }
            return Redirect("~/Home/AccessDenied");
        }

        // GET: OnlineTrainings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            OnlineTraining onlineTraining = db.OnlineTrainings.Find(id);
            if (onlineTraining == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            return View(onlineTraining);
        }

        // GET: OnlineTrainings/Create
        public ActionResult Application()
        {
            ViewBag.SexID = new SelectList(db.Sexes, "ID", "Sex1");
            ViewBag.StateOriginID = new SelectList(db.States, "ID", "Name");
            ViewBag.StateResidenceID = new SelectList(db.States, "ID", "Name");
            return View();
        }

        // POST: OnlineTrainings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Application([Bind(Include = "ID,Surname,OtherNames,SexID,PhoneNumber,Email,ContactAddress,StateOriginID,LGAResidence,StateResidenceID,City,Password,DateSubmitted,PaymentStatus,ReferenceNo,Amount,DatePaid")] OnlineTraining onlineTraining)
        {
            if (ModelState.IsValid)
            {
                if (db.OnlineTrainings.Where(s => s.Email == onlineTraining.Email).Count() == 1)
                {
                    ViewBag.CredentialsUsed = "Email already exist.";
                    ViewBag.SexID = new SelectList(db.Sexes, "ID", "Sex1", onlineTraining.SexID);
                    ViewBag.StateOriginID = new SelectList(db.States, "ID", "Name", onlineTraining.StateOriginID);
                    ViewBag.StateResidenceID = new SelectList(db.States, "ID", "Name", onlineTraining.StateResidenceID);
                    return View(onlineTraining);
                }
                else
                {
                    db.OnlineTrainings.Add(onlineTraining);
                    onlineTraining.DateSubmitted = DateTime.Now;
                    db.SaveChanges();
                    return Redirect("~/OnlineTrainings/Dashboard?apx=" + onlineTraining.ID);
                }
            }

            ViewBag.SexID = new SelectList(db.Sexes, "ID", "Sex1", onlineTraining.SexID);
            ViewBag.StateOriginID = new SelectList(db.States, "ID", "Name", onlineTraining.StateOriginID);
            ViewBag.StateResidenceID = new SelectList(db.States, "ID", "Name", onlineTraining.StateResidenceID);
            return View(onlineTraining);
        }

        // GET: OnlineTrainings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            OnlineTraining onlineTraining = db.OnlineTrainings.Find(id);
            if (onlineTraining == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            ViewBag.SexID = new SelectList(db.Sexes, "ID", "Sex1", onlineTraining.SexID);
            ViewBag.StateOriginID = new SelectList(db.States, "ID", "Name", onlineTraining.StateOriginID);
            ViewBag.StateResidenceID = new SelectList(db.States, "ID", "Name", onlineTraining.StateResidenceID);
            return View(onlineTraining);
        }
            return Redirect("~/Home/AccessDenied");
        }

        // POST: OnlineTrainings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Surname,OtherNames,SexID,PhoneNumber,Email,ContactAddress,StateOriginID,LGAResidence,StateResidenceID,City,Password,DateSubmitted,PaymentStatus,ReferenceNo,Amount,DatePaid")] OnlineTraining onlineTraining)
        {
            if (ModelState.IsValid)
            {
                db.Entry(onlineTraining).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SexID = new SelectList(db.Sexes, "ID", "Sex1", onlineTraining.SexID);
            ViewBag.StateOriginID = new SelectList(db.States, "ID", "Name", onlineTraining.StateOriginID);
            ViewBag.StateResidenceID = new SelectList(db.States, "ID", "Name", onlineTraining.StateResidenceID);
            return View(onlineTraining);
        }

        // GET: OnlineTrainings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            OnlineTraining onlineTraining = db.OnlineTrainings.Find(id);
            if (onlineTraining == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            return View(onlineTraining);
        }

        // POST: OnlineTrainings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OnlineTraining onlineTraining = db.OnlineTrainings.Find(id);
            db.OnlineTrainings.Remove(onlineTraining);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Dashboard(int apx)
        {
            var list = db.OnlineTrainings.Find(apx);
            return View(list);
        }
        [HttpPost]
        public ActionResult Login()
        {
            string Username = Request.Form["email"];
            string Password = Request.Form["password"];

            int UserLogin = db.OnlineTrainings.Where(r => r.Email == Username && r.Password == Password).Count();
            if (UserLogin > 0)
            {
                int UserID = db.OnlineTrainings.Where(s => s.Email == Username && s.Password == Password).Select(s => s.ID).FirstOrDefault();
                Session["ApplicantID"] = UserID;
                return Redirect("~/OnlineTrainings/Dashboard/?apx=" + UserID);
            }
            else
            {
                ViewBag.UserNotExist = " Access denied, try again";
                return Redirect("~/OnlineTrainings/Application/?err=true");
            }
        }

        public ActionResult LogOff()
        {
            Session["ApplicantID"] = null;
            return Redirect("~/OnlineTrainings/Application");
        }

        public ActionResult Update_Payment(string refno, decimal amount,  string name, string myemail, int id)
        {
            //Inser onlinetraining
            var  update =db.Set<OnlineTraining>().Find(id);
            update.Amount = amount;
            update.DatePaid = DateTime.Now;
            update.ReferenceNo = refno;
            update.PaymentStatus = 1;
            db.SaveChanges();

            string msg = "<a href='http://marketsquare247.com' title='Market Square247'> <img src='http://marketsquare247.com/Images/logosquare.jpg' style='width: 65px; height: 35px' /></a><hr>Hello! " + name + "<br/>" +
                " Transaction details for online training <br/>" +
                " Reference Number: " + refno + "<br/>" +
                " Amount Paid: " + amount / 100 + "<br/>" +

                " Website: http://www.marketsquare247.com <br/>" +
                "From MarketSquare247";
            mydata.SendMail(myemail, "Market Square247 - Payment Made Successfully", msg);

            return Redirect("~/OnlineTrainings/Dashboard/?apx="+id);
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
