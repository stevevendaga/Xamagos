using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Myvshoponline;
using System.Text;
using System.IO;

namespace Myvshoponline.Controllers
{
    public class ApplicationsController : Controller
    {
        private MyvshoponlineEntities db = new MyvshoponlineEntities();
        Getdata mydata = new Getdata();
        // GET: Applications
        //public ActionResult Index(string search)
        //{
        //    if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
        //    {
        //        var applications = db.Applications.Include(a => a.Position).Include(a => a.Sex).Include(a => a.State).Include(a => a.State1);
        //        return View(applications.Where(s => s.Position.Position1.Contains(search) || s.Surname.Contains(search) || s.OtherNames.Contains(search) || s.PhoneNumber.Contains(search) || s.Email.Contains(search) || s.City.Contains(search) || s.Sex.Sex1.Contains(search) || s.State1.Name.Contains(search)));
        //    }
        //    else
        //    {
            
        //    return Redirect("~/Home/AccessDenied");
        //}
        //}

        public ActionResult Index()
        {
            if (mydata.Is_SupperAdmin((string)Session["username"], (string)Session["UserRole"]))
            {
                var applications = db.Applications.Include(a => a.Position).Include(a => a.Sex).Include(a => a.State).Include(a => a.State1);
                return View(applications.ToList());
            }
            else
            {

                return Redirect("~/Home/AccessDenied");
            }
        }



        // GET: Applications/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Application application = db.Applications.Find(id);
            if (application == null)
            {
                return HttpNotFound();
            }
            return View(application);
        }
        public ActionResult Recruitment()
        {
            var position = db.Positions.ToList();
            return View(position);
        }

        public ActionResult JobDetails()
        {
            var position = db.Positions.ToList();
            return View(position);
        }
        // GET: Applications/Create
        public ActionResult ApplyNow(int?AppType)
        {
            int PositionID = db.Settings.Select(s => s.ID).First();
            int ApplicationClosed = (int)db.Settings.Find(PositionID).ApplicationClosed;
        if(ApplicationClosed==0)
            {
                return Redirect("~/Applications/Recruitment");
            }
            if(AppType==null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            Position pos = db.Positions.Find(AppType);
            if(pos==null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            ViewBag.PositionID = new SelectList(db.Positions, "ID", "Position1");
            ViewBag.SexID = new SelectList(db.Sexes, "ID", "Sex1");
            ViewBag.StateOriginID = new SelectList(db.States, "ID", "Name");
            ViewBag.StateResidenceID = new SelectList(db.States, "ID", "Name");
            return View();
        }

        // POST: Applications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApplyNow([Bind(Include = "ID,Surname,OtherNames,SexID,PhoneNumber,Email,ContactAddress,StateOriginID,LGAResidence,StateResidenceID,City,PositionID,Password,ApplicationStatus,DateSubmitted")] Application application)
        {

            ViewBag.PositionID = new SelectList(db.Positions, "ID", "Position1", application.PositionID);
            ViewBag.SexID = new SelectList(db.Sexes, "ID", "Sex1", application.SexID);
            ViewBag.StateOriginID = new SelectList(db.States, "ID", "Name", application.StateOriginID);
            ViewBag.StateResidenceID = new SelectList(db.States, "ID", "Name", application.StateResidenceID);
            if (ModelState.IsValid)
            {
                if (db.Applications.Where(s => s.Email == application.Email || s.PhoneNumber == application.PhoneNumber).Count() > 0)
                {
                    ViewBag.CredentialsUsed = "Email or Phone Number already used!";
                    return View(application);
                }
                else
                {
                    string random = Guid.NewGuid().ToString();
                    application.ID = random;
                    application.PositionID = Convert.ToInt32(Request.QueryString["AppType"]);
                    application.DateSubmitted = DateTime.Now;
                    application.ApplicationStatus = 0;
                    db.Applications.Add(application);
                    db.SaveChanges();
                    string msg ="Hello, " +application.Surname +  " Application submited successfull.";
                  //  mydata.Send_SMS(application.PhoneNumber, msg, "marketsquar", DateTime.Now);
                    Session["ApplicantID"] = application.ID;
                    return Redirect("~/Applications/UploadCredentials/?apx="+application.ID);
                }
            }

            return View(application);
        }

        // GET: Applications/Edit/5
        public ActionResult Edit(string id)
        {
            if(string.IsNullOrEmpty((string) Session["ApplicantID"]))
            {
                return Redirect("~/Applications/Recruitment");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Application application = db.Applications.Find(id);
            if (application == null)
            {
                return HttpNotFound();
            }
            ViewBag.PositionID = new SelectList(db.Positions, "ID", "Position1", application.PositionID);
            ViewBag.SexID = new SelectList(db.Sexes, "ID", "Sex1", application.SexID);
            ViewBag.StateOriginID = new SelectList(db.States, "ID", "Name", application.StateOriginID);
            ViewBag.StateResidenceID = new SelectList(db.States, "ID", "Name", application.StateResidenceID);
            return View(application);
        }

        // POST: Applications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Surname,OtherNames,SexID,PhoneNumber,Email,ContactAddress,StateOriginID,LGAResidence,StateResidenceID,City,PositionID,Password,ApplicationStatus,DateSubmitted")] Application application)
        {
            if (ModelState.IsValid)
            {
                db.Entry(application).State = EntityState.Modified;
                db.SaveChanges();
                return Redirect("~/Applications/UploadCredentials/?apx=" + application.ID);
            }
            ViewBag.PositionID = new SelectList(db.Positions, "ID", "Position1", application.PositionID);
            ViewBag.SexID = new SelectList(db.Sexes, "ID", "Sex1", application.SexID);
            ViewBag.StateOriginID = new SelectList(db.States, "ID", "Name", application.StateOriginID);
            ViewBag.StateResidenceID = new SelectList(db.States, "ID", "Name", application.StateResidenceID);
            return Redirect("~/Applications/UploadCredentials/?apx=" + application.ID);
        }

        // GET: Applications/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Application application = db.Applications.Find(id);
            if (application == null)
            {
                return HttpNotFound();
            }
            return View(application);
        }

        // POST: Applications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Application application = db.Applications.Find(id);
            db.Applications.Remove(application);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

         
        public ActionResult UploadCredentials(string apx)
        {
           
            if (string.IsNullOrEmpty((string) Session["ApplicantID"]))
            {
                return Redirect("~/Applications/Recruitment");
            }
            if (apx == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            Application app = db.Applications.Find(apx);
            if (app == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            ViewBag.SexID = new SelectList(db.Sexes, "ID", "Sex1",app.SexID);
            ViewBag.StateOriginID = new SelectList(db.States, "ID", "Name",app.StateOriginID);
            ViewBag.StateResidenceID = new SelectList(db.States, "ID", "Name",app.StateResidenceID);
            return View(app);
        }

        [HttpPost]
        public ActionResult Login()
        {
            string Username = Request.Form["email"];
            string Password = Request.Form["password"];

            int UserLogin = db.Applications.Where(r => r.Email == Username && r.Password == Password).Count();
            if (UserLogin > 0)
            {
                string UserID = db.Applications.Where(s => s.Email == Username && s.Password == Password).Select(s => s.ID).FirstOrDefault();
                Session["ApplicantID"] = UserID;
                return Redirect("~/Applications/UploadCredentials/?apx=" + UserID);
            }
            else
            {
                ViewBag.UserNotExist = " Access denied, try again";
                return Redirect("~/Applications/Recruitment/?err=true");
            }


        }


        public ActionResult UploadPassport(HttpPostedFileBase[] files)
        {
            Random random = new Random();
            //   Random generator = new Random();
            int rand1 = random.Next(99999, 99999999);
            int rand2 = random.Next(99999, 99999999);
            StringBuilder builder = new StringBuilder();
            int size = 550;
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                ViewBag.Random = builder.Append(ch);
            }

            string UserID = (string)Session["ApplicantID"];

            string DocumentName = UserID + ".jpg";
            string efilepath;
            foreach (HttpPostedFileBase file in files)
            {
                if (file != null)
                {
                    efilepath = Server.MapPath("~/ApplicantPassports/" + "\\" + DocumentName);
                    string ext = Path.GetExtension(efilepath);

                    //if file extension is supported, save file and update database
                    if (ext == ".jpg" || ext == ".jpeg" || ext == ".png" || ext == ".gif" || ext == ".ico")
                    {
                        string newName = System.IO.Path.GetFileNameWithoutExtension(efilepath);
                        newName = newName + ".jpg";

                        efilepath = Server.MapPath("~/ApplicantPassports/" + "\\" + newName);
                        file.SaveAs(efilepath);
                        mydata.ResizePicture(efilepath);


                    }
                }

            }
            return Redirect("~/Applications/UploadCredentials/?apx=" + UserID);
        }

        public ActionResult UploadCV(HttpPostedFileBase[] files)
        {
            Random random = new Random();
            //   Random generator = new Random();
            int rand1 = random.Next(99999, 99999999);
            int rand2 = random.Next(99999, 99999999);
            StringBuilder builder = new StringBuilder();
            int size = 550;
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                ViewBag.Random = builder.Append(ch);
            }

            string UserID = (string)Session["ApplicantID"];

            string DocumentName = UserID + ".pdf";
            string efilepath;
            foreach (HttpPostedFileBase file in files)
            {
                if (file != null)
                {
                    efilepath = Server.MapPath("~/ApplicantCV/" + "\\" + DocumentName);
                    string ext = Path.GetExtension(efilepath);

                    //if file extension is supported, save file and update database
                        string newName = System.IO.Path.GetFileNameWithoutExtension(efilepath);
                        newName = newName + ".pdf";

                        efilepath = Server.MapPath("~/ApplicantCV/" + "\\" + newName);
                        file.SaveAs(efilepath);
                       // mydata.ResizePicture(efilepath);
                }

            }
            var app = db.Set<Application>().Find(UserID);
            app.ApplicationStatus = 1;
            db.SaveChanges();
            var AptGuidelines = db.AptitudeTestGuidelines.Select(s => s.Description).FirstOrDefault();
            string title = "YOUR APPLICATION ON MARKETSQUARE247.COM HAS BEEN RECIEVED";
            string msg = "<a href='http://marketsquare247.com' title='Market Square'> <img src='http://marketsquare247.com/Images/logosquare.jpg' style='width: 65px; height: 35px' /><a><hr>"+
             "<span style='color: rgb(56,118, 29); height: 325px; '>CONGRATULATIONS!</span>&nbsp;" + app.Surname.ToUpper() + ' ' + app.OtherNames.ToUpper() + "<br>"+
            "You have successfully completed the application for " +HttpUtility.HtmlDecode(app.Position.Position1) + " on marketsquare247.com <br/>" +
            "<span style='height: 325px'>Selected applicants are expected to write a Computer Based Test (CBT) Examination on </span><span style='height: 325px;'><b style='height:325px;'>25th November, 2019 - 30th November, 2019</b>.</span><br><br>BELOW ARE THE APTITUDE TEST GUIDELINES: <br><br>"+
            HttpUtility.HtmlDecode(AptGuidelines) + " <hr>"+
             "From:  www.marketsquare247.com" + "<br>" +
             "Email: info@marketsqaure247.com";
            mydata.SendMail(app.Email, title, msg);
            return Content("<script>alert('Curriculum Vitae uploaded successfully');window.location='/Applications/UploadCredentials/?apx=" + UserID+"'</script>");
           // return Redirect("~/Applications/UploadCredentials/?apx=" + UserID);
        }
        public ActionResult LogOff()
        {
            Session["ApplicantID"] = null;
            return Redirect("~/Applications/Recruitment");
        }

        public ActionResult CloseApplication()
        {
            int SettingID = db.Settings.Select(s => s.ID).First();
            var close = db.Set<Setting>().Find(SettingID);
            close.ApplicationClosed = 0;
            db.SaveChanges();
            return Redirect("~/Applications/Index");
        }

        public ActionResult Enableapplication()
        {
            int ID = db.Settings.Select(s => s.ID).FirstOrDefault();
            var setting = db.Set<Setting>().Find(ID);
            setting.ApplicationClosed=1;
            db.SaveChanges();
            var result = 1;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Disableapplication()
        {
            int ID = db.Settings.Select(s => s.ID).FirstOrDefault();
            var setting = db.Set<Setting>().Find(ID);
            setting.ApplicationClosed = 0;
            db.SaveChanges();
            var result = 1;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult enablecbttest()
        {
            int ID = db.Settings.Select(s => s.ID).FirstOrDefault();
            var setting = db.Set<Setting>().Find(ID);
            setting.OpenandCloseCBTTest = 1;
            db.SaveChanges();
            var result = 1;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult disablecbttest() 
        {
            int ID = db.Settings.Select(s => s.ID).FirstOrDefault();
            var setting = db.Set<Setting>().Find(ID);
            setting.OpenandCloseCBTTest = 0;
            db.SaveChanges();
            var result = 1;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SendMailtoApplicants()
        {
            var applicant = db.Applications.ToList();
            foreach(var item in applicant)
            {
                string n = item.Surname + " " + item.OtherNames;
                GetEmails(item.Email,n);
            }
            return Redirect("~/Applications/Index/?id="+Request.QueryString["id"]);
        }

        public ActionResult SendMailtoApplicantsAptitude()
        {
            var applicant = db.Applications.ToList();
            foreach (var item in applicant)
            {
                string n = item.Surname + " " + item.OtherNames;
                SendMsgAptitude(item.Email, n);
            }
            return Redirect("~/Applications/Index/?id=" + Request.QueryString["id"]);
        }
        public ActionResult SendMailCBTLink()
        {
            var applicant = db.Applications.ToList();
            foreach (var item in applicant)
            {
                string n = item.Surname + " " + item.OtherNames;
                SendCBTLink(item.Email, n);
            }
            return Redirect("~/Applications/Index/?id=" + Request.QueryString["id"]);
        }

        public void GetEmails(string email,string name)
        {
            //string subject = "Selection for Aptitude Test";
            //string msg = "<a href='http://marketsquare247.com' title='Market Square'> <img src='http://marketsquare247.com/Images/logosquare.jpg' style='width: 65px; height: 35px' /><a>" + "<hr>" + " Dear " + name + "<br>" +
            //              "Congratulations! Sequel to your application on marketsquare247, you have been selected to participate in Aptitude Test scheduled to start from " + "<br/>" +
            //              "<br> Monday 25th November, 2019 to Saturday 30th November, 2019." + "<br/><br>" +
            //              "<br>The CBT Examination is going to be taken online." + "<br/><br>" +
            //              "<font color=red>Note:</font> You can choose any day from  25th November, 2019 to Saturday 30th November, 2019 to take your CBT Examination.<hr/>" +
            //              "A link will be forwared to your email to click and take the test or you can logon to http://marketsquare247.com/applications/recruitment once on your dashboard click on Take test." + "<br>" +
            //              "From:  Market Square" + "<br>" +
            //              "Email: info@marketsqaure247.com";

            

            string msg = "Welcome back to marketsquare247.com" +
            "Due to the Federal Government directives on total lockdown as a result of Covid - 19 pandemic, Market Square247 also suspended its activities. <br><br>" +
            "Since there is an ease on the lockdown, we have divised enticing strategies to enable our customers have easy and flexible" +
            "access to manage their shop online and showcase their products/ services while staying at home.<br><br>" +
            "Because of Covid - 19 pandemic Market Square247 is providing this services free of charge to both old and new customers" +
            "all over the globe.<br><br>" +
            "Visit our website www.marketsquare247.com to register your business for free and upload your products/services and start selling.";
            string subject = "Welcome back to marketsquare247.com";

            mydata.SendMail(email,subject,msg);
        }

        public void SendMsgAptitude(string email, string name)
        {
            string msg = " APTITUDE TEST RESULTS RELEASED!" +
            "Aptitude test results for marketsquare247.com 2019 Recruitment Exercise has been released." +
            "Applicants are advised to log onto www.marketsquare247.com to check their scores and application status." +
            "Best Regards.";
            string subject = "MARKET SQUARE247 2019 RECRUITMENT APTITUDE TEST RESULTS RELEASED!";
            mydata.SendMail(email, subject, msg);
        }
       

        public void SendCBTLink(string email, string name)
        {
            string subject = "Aptitude Test Link";
            string msg = "<a href='http://marketsquare247.com' title='Market Square'> <img src='http://marketsquare247.com/Images/logosquare.jpg' style='width: 65px; height: 35px' /><a>" + "<hr>" + " Dear " + name + "<br>" +
                          "Click on the link below to take Aptitude test" + "<br/>" +
                          "<br> http://cbt.marketsquare247.com" + "<br/><br>" +
                          "<br> Or logon to http://marketsquare247.com/applications/recruitment once on your dashboard click on Take test." + "<br/><br>" +
                          "<font color=red>Note:</font> You can choose any day from  25th November, 2019 to Saturday 30th November, 2019 to take your CBT Examination.<hr/>" +
                          "From:  Market Square" + "<br>" +
                          "Email: info@marketsqaure247.com";
            mydata.SendMail(email, subject, msg);
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
