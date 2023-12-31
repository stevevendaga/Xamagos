using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Myvshoponline;
using System.Globalization;
using System.Threading;
using System.Xml;
using System.Data.OleDb;

namespace Myvshoponline.Controllers
{
    public class MailingListNonNGsController : Controller
    {
        private MyvshoponlineEntities db = new MyvshoponlineEntities();
        Getdata mydata = new Getdata();
        // GET: MailingListNonNGs
        public ActionResult Index()
        {
            if((string)Session["UserRole"] != "Site Admin" && (string)Session["UserRole"] != "Super Admin")
            {
                return Redirect("~/Home/AccessDenied");
            }
            ViewBag.EmailMessageID = new SelectList(db.EmailMessages, "ID", "Subject");
            if (!String.IsNullOrEmpty(Request.QueryString["EmailMessageID"]))
            {
                GetEmailsNonNGMailingList(Convert.ToInt32(Request.QueryString["EmailMessageID"]),Convert.ToInt32(Request.QueryString["selecttop"]));
            }
            return View(db.MailingListNonNGs.ToList());
        }

        // GET: MailingListNonNGs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            MailingListNonNG mailingListNonNG = db.MailingListNonNGs.Find(id);
            if (mailingListNonNG == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            return View(mailingListNonNG);
        }

        // GET: MailingListNonNGs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MailingListNonNGs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Email,Status,DateCreated")] MailingListNonNG mailingListNonNG)
        {
            if (ModelState.IsValid)
            {
                db.MailingListNonNGs.Add(mailingListNonNG);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mailingListNonNG);
        }

        // GET: MailingListNonNGs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            MailingListNonNG mailingListNonNG = db.MailingListNonNGs.Find(id);
            if (mailingListNonNG == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            return View(mailingListNonNG);
        }

        // POST: MailingListNonNGs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Email,Status,DateCreated")] MailingListNonNG mailingListNonNG)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mailingListNonNG).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mailingListNonNG);
        }

        // GET: MailingListNonNGs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            MailingListNonNG mailingListNonNG = db.MailingListNonNGs.Find(id);
            if (mailingListNonNG == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            return View(mailingListNonNG);
        }

        // POST: MailingListNonNGs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MailingListNonNG mailingListNonNG = db.MailingListNonNGs.Find(id);
            db.MailingListNonNGs.Remove(mailingListNonNG);
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
                    string fileLocation = Server.MapPath("~/MailingList/") + currenttime + Request.Files["file"].FileName;

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
                    string fileLocation = Server.MapPath("~/MailingList/") + currenttime +  Request.Files["file"].FileName;
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
               //string Name;
                string Email;

                int totalrows = ds.Tables[0].Rows.Count;
                int totalcols = ds.Tables[0].Columns.Count;

                for (int i = 0; i < totalrows; i++)
                {
                    CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
                    TextInfo textInfo = cultureInfo.TextInfo;
                    //get records from excel dataset
                    // column 1 - questionnumber

                 //   Name = ds.Tables[0].Rows[i][0].ToString();
                    // column 2 - subject
                    Email = ds.Tables[0].Rows[i][0].ToString();
                    if (db.MailingListNonNGs.Where(e => e.Email == Email).Count() < 1)
                    {
                        MailingListNonNG listng = new MailingListNonNG();
                       // listng.Name = Name;
                        listng.Email = Email;
                        listng.Status = 0;
                        listng.DateCreated = DateTime.Now;
                        db.MailingListNonNGs.Add(listng);
                        db.SaveChanges();
                    }
                }
return Redirect("~/MailingListNonNGs/Index");
            }
            else
            {
                //no content uploaded
                return View();
            }
        }

        public ActionResult GetEmailsNonNGMailingList(int EmailMessageID,int selecttop)
        {
            var mail = db.MailingListNonNGs.Where(s => s.Status == 0 && (s.Email.EndsWith("yahoo.com") || s.Email.EndsWith("yahoo.co.uk") || !s.Email.EndsWith("gmail.com"))).ToList().Take(selecttop);
            foreach (var item in mail)
            {
                //string n = item.Name;
                SendMailNonNGMailingList(item.Email, EmailMessageID);
            }
            return Redirect("~/MailingListNonNGs/Index");
        }

        public void SendMailNonNGMailingList(string email, int? MailID)
        {
            var MailSubject = db.EmailMessages.Where(i => i.ID == MailID).Select(i => i.Subject).FirstOrDefault();
            string subject = HttpUtility.HtmlDecode(MailSubject);
            var MailMessage = db.EmailMessages.Where(i => i.ID == MailID).Select(i => i.Message).FirstOrDefault();
            string msg = HttpUtility.HtmlDecode(MailMessage);
            mydata.SendMail(email.TrimEnd().TrimStart(), subject, msg);
            int MailingListID = db.MailingListNonNGs.Where(m => m.Email == email).Select(m => m.ID).FirstOrDefault();
            var update = db.Set<MailingListNonNG>().Find(MailingListID);
            update.Status = 1;
            db.SaveChanges();
        }
        public ActionResult ResetMailStatus()
        {
            mydata.ResetNonNGMailStatustoZero();
            return RedirectToAction("Index");
        }
    }
}
