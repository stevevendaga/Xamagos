using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Myvshoponline;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using PagedList;
using PagedList.Mvc;





namespace SysmaxTrainingCenter.Controllers
{
    public class ProjectsController : Controller
    {
        private MyvshoponlineEntities db = new MyvshoponlineEntities();

        // GET: Projects
        public ActionResult Index()
        {        

            string random = Guid.NewGuid().ToString();
            //if (String.IsNullOrEmpty((string)Session["Name"])) return Redirect("/Account/Login");
            ViewBag.Random = random;
            if (((string)Session["UserRole"] == "Super Admin"))
            {
                return View(db.Projects.ToList());
            }
            else if(((string)Session["UserRole"] == "Site Admin"))
            {                
                return View(db.Projects.ToList());
            }
            else
            {
                return Redirect("~/Account/Login");
            }

                    
        }

        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (String.IsNullOrEmpty((string)Session["Name"])) return Redirect("/Account/Login");
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
                        
            Project project = db.Projects.Find(id);
          
            if (project == null)
            {
                return Redirect("~/Home/AccessDenied");
            } 
                       
            return View(project);

        }

        // GET: Projects/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProjectID,ProjectName,Description,ProjectLinks,LinkProtocol")] Project project)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string protocol1 = Request.Form["radio"];
                    project.LinkProtocol = protocol1;

                    db.Projects.Add(project);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {

                ViewBag.Try = ex.Message;
                return View(project);
            }
            
            return View(project);

        }

        // GET: Projects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (String.IsNullOrEmpty((string)Session["Name"])) return Redirect("/Account/Login");
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
                
            }

            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return Redirect("~/Home/AccessDenied");
                
            }
            return View(project);
            
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProjectID,ProjectName,Description,ProjectLinks")] Project project)
        {
            
            if (ModelState.IsValid)
            {
                string protocol = Request.Form["radio"];
                // project.LinkProtocol = protocol1;              

                project.LinkProtocol = protocol;                
                db.Projects.Add(project);

                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
                

            }
            return View(project);           

        }

        // GET: Projects/Delete/5
        public ActionResult Delete(int? id, int? projectid)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
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

        public ActionResult UploadLogo(int? id)
        {
            
                ViewBag.ProjectID = id;
                return View();
            
        }


        [HttpPost]
        public ActionResult UploadLogo(HttpPostedFileBase[] files)
        {
                           //Upload Picture Method
                int FName = Convert.ToInt32(Request.Form["ProjectID"]);
            string DocumentName = FName + ".jpg";
            string efilepath;
            foreach (HttpPostedFileBase file in files)
            {
                if (file != null)
                {
                    efilepath = Server.MapPath("~/Images/" + "\\" + DocumentName);
                    string ext = Path.GetExtension(efilepath);
                    //if file extension is supported, save file and update database
                    if (ext == ".jpg" || ext == ".jpeg" || ext == ".png" || ext == ".gif")
                    {
                        string newName = System.IO.Path.GetFileNameWithoutExtension(efilepath);
                        newName = newName + ".jpg";
                        efilepath = Server.MapPath("~/Images/" + "\\" + newName);
                        file.SaveAs(efilepath);
                    }
                }

            }

            return Redirect("~/Projects/Index");
        }



    }
}