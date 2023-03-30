using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Myvshoponline;
using System.Data.OleDb;
using System.Xml;
using System.Globalization;
using System.Threading;

namespace Myvshoponline.Controllers
{
    public class MailingListNGsController : Controller
    {
        private MyvshoponlineEntities db = new MyvshoponlineEntities();
        Getdata mydata = new Getdata();
        // GET: MailingListNGs
        public ActionResult Index()
        {
            if ((string)Session["UserRole"]!= "Site Admin" && (string)Session["UserRole"]!= "Super Admin")
            {
                return Redirect("~/Home/AccessDenied");
            }
            ViewBag.EmailMessageID = new SelectList(db.EmailMessages, "ID", "Subject");
            if(!String.IsNullOrEmpty(Request.QueryString["EmailMessageID"]))
            {
                GetEmailsNGMailingList(Convert.ToInt32(Request.QueryString["EmailMessageID"]), Convert.ToInt32(Request.QueryString["selecttop"]));
            }
            if(!String.IsNullOrEmpty(Request.QueryString["pending"]))
            {
                return View(db.MailingListNGs.Where(e=>e.Status==0).ToList());
            }
            else if(!String.IsNullOrEmpty(Request.QueryString["sent"]))
            {
                return View(db.MailingListNGs.Where(e => e.Status == 1).ToList());
            }
            return View(db.MailingListNGs.ToList());
        }

        // GET: MailingListNGs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MailingListNG mailingListNG = db.MailingListNGs.Find(id);
            if (mailingListNG == null)
            {
                return HttpNotFound();
            }
            return View(mailingListNG);
        }

        // GET: MailingListNGs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MailingListNGs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Email,Status,DateCreated")] MailingListNG mailingListNG)
        {
            if (ModelState.IsValid)
            {
                db.MailingListNGs.Add(mailingListNG);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mailingListNG);
        }

        // GET: MailingListNGs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MailingListNG mailingListNG = db.MailingListNGs.Find(id);
            if (mailingListNG == null)
            {
                return HttpNotFound();
            }
            return View(mailingListNG);
        }

        // POST: MailingListNGs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Email,Status,DateCreated")] MailingListNG mailingListNG)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mailingListNG).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mailingListNG);
        }

        // GET: MailingListNGs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MailingListNG mailingListNG = db.MailingListNGs.Find(id);
            if (mailingListNG == null)
            {
                return HttpNotFound();
            }
            return View(mailingListNG);
        }

        // POST: MailingListNGs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MailingListNG mailingListNG = db.MailingListNGs.Find(id);
            db.MailingListNGs.Remove(mailingListNG);
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

        public ActionResult UploadMailingList(HttpPostedFileBase files)
        {
            DataSet ds = new DataSet();
            //check file lenght
            if (Request.Files["file"].ContentLength > 0)
            {
                string fileExtension = System.IO.Path.GetExtension(Request.Files["file"].FileName);
                if (fileExtension == ".xls" || fileExtension == ".xlsx")
                {
                    string currenttime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    string fileLocation = Server.MapPath("~/MailingList/") +currenttime + Request.Files["file"].FileName;

                    if (System.IO.File.Exists(fileLocation))
                    {
                        System.IO.File.Delete(fileLocation);
                    }
                    Request.Files["file"].SaveAs(fileLocation);

                    string excelConnectionString = string.Empty;
                    excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                    fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    //connection String for xls file format.
                    if (fileExtension == ".xls")
                    {
                        excelConnectionString = "Provider=Microsoft.Jet.OLEDB.12.0;Data Source=" +
                        fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                    }
                    //connection String for xlsx file format.
                    else if (fileExtension == ".xlsx")
                    {
                        excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                        fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    }
                    //Create Connection to Excel work book and add oledb namespace
                    OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
                    excelConnection.Open();
                    DataTable dt = new DataTable();
                    dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    if (dt == null)
                    {
                        return null;
                    }

                    String[] excelSheets = new String[dt.Rows.Count];
                    int t = 0;
                    //excel data saves in temp file here.
                    foreach (DataRow row in dt.Rows)
                    {
                        excelSheets[t] = row["TABLE_NAME"].ToString();
                        t++;
                    }
                    OleDbConnection excelConnection1 = new OleDbConnection(excelConnectionString);


                    string query = string.Format("Select * from [{0}]", excelSheets[0]);
                    using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                    {
                        dataAdapter.Fill(ds);
                    }
                }
                if (fileExtension.ToString().ToLower().Equals(".xml"))
                {
                    string currenttime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    string fileLocation = Server.MapPath("~/MailingList/") + currenttime + Request.Files["file"].FileName;
                    if (System.IO.File.Exists(fileLocation))
                    {
                        System.IO.File.Delete(fileLocation);
                    }

                    Request.Files["file"].SaveAs(fileLocation);
                    XmlTextReader xmlreader = new XmlTextReader(fileLocation);
                    // DataSet ds = new DataSet();
                    ds.ReadXml(xmlreader);
                    xmlreader.Close();
                }
               // string Name;
                string Email;
               
                int totalrows = ds.Tables[0].Rows.Count;
                int totalcols = ds.Tables[0].Columns.Count;
                
                    for (int i = 0; i < totalrows; i++)
                    {
                        CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
                        TextInfo textInfo = cultureInfo.TextInfo;
                        //get records from excel dataset
                        // column 1 - questionnumber

                      //  Name = ds.Tables[0].Rows[i][0].ToString();
                        // column 2 - subject
                        Email = ds.Tables[0].Rows[i][0].ToString();
                    if (db.MailingListNGs.Where(e => e.Email == Email).Count() < 1)
                    {
                        MailingListNG list = new MailingListNG();
                       //list.Name = Name;
                        list.Email = Email;
                        list.Status = 0;
                        list.DateCreated = DateTime.Now;
                        db.MailingListNGs.Add(list);
                        db.SaveChanges();
                    }
                    }
                
                return Redirect("~/MailingListNGs/Index");
            }
            else
            {
                //no content uploaded
                return View();
            }
        }

         
        public ActionResult GetEmailsNGMailingList(int EmailMessageID,int selecttop)
        {
            // var mail = db.MailingListNGs.Where(s=>s.Status==0 && (s.Email.EndsWith("yahoo.com") || s.Email.EndsWith("yahoo.co.uk") || !s.Email.EndsWith("gmail.com"))).ToList().Take(selecttop);
            var mail = db.MailingListNGs.Where(s => s.Status == 0).ToList().Take(selecttop);
            foreach (var item in mail)
            {
                //string n = item.Name;
                
                SendMailNGMailingList(item.Email, EmailMessageID);
            }
            return Redirect("~/MailingListNGs/Index");
        }

        public void SendMailNGMailingList(string email,int MailID)
        {
            var MailSubject = db.EmailMessages.Where(i => i.ID == MailID).Select(i => i.Subject).FirstOrDefault();
            string subject = HttpUtility.HtmlDecode(MailSubject);
            var MailMessage = db.EmailMessages.Where(i => i.ID == MailID).Select(i => i.Message).FirstOrDefault();
            string msg = HttpUtility.HtmlDecode(MailMessage);
            mydata.SendMail(email.TrimEnd().TrimStart(), subject, msg);

            int MailingListID = db.MailingListNGs.Where(m => m.Email == email).Select(m => m.ID).FirstOrDefault();
            var update = db.Set<MailingListNG>().Find(MailingListID);
            update.Status = 1;
            db.SaveChanges();
        }

        public ActionResult ResetMailStatus()
        {
            mydata.ResetNGMailStatustoZero();
            return RedirectToAction("Index");
        }

    }
}
