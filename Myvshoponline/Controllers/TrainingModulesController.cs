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
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using PagedList;
using PagedList.Mvc;



namespace SysmaxTrainingCenter.Controllers
{
    public class TrainingModulesController : Controller
    {
        private MyvshoponlineEntities db = new MyvshoponlineEntities();

        // GET: TrainingModules
        
        public ActionResult Index(int? px, int? sx,string search)
        {
            string random = Guid.NewGuid().ToString();
            var trainingModules = db.TrainingModules.Where(t=>t.ProjectID == px && t.Serial == sx);   

            ViewBag.Px = px;
            ViewBag.Sx = sx;
            ViewBag.Random = random;
            ViewBag.CountTotproject = db.TrainingModules.Where(r => r.ProjectID == px).ToList();
            ViewBag.CountTotprojectCount = db.TrainingModules.Where(r => r.ProjectID == px).Count();

            ViewBag.CountTotSearchproject = db.TrainingModules.Where(r => r.ProjectTitle.Contains(search) || r.Description.Contains(search)).ToList();
            ViewBag.CountTotprojectSearchCount = db.TrainingModules.Where(r => r.ProjectTitle.Contains(search) || r.Description.Contains(search)).Count();


            //return Content(" " + random);

            return View(trainingModules);            
        }


        //public ActionResult Training(int? projectid, int? serial)
        //{
        //    //  var trainingModules = db.TrainingModules.Include(t => t.Project);
        //    //  return View(trainingModules.Where(s => s.ProjectID == projectid && s.Serial == serial).ToList());
           

        //    if (projectid == null)
        //    {
        //        return Redirect("~/Home/AccessDenied");
        //    }
        //    TrainingModule trainingModule = db.TrainingModules.Where(s=>s.ProjectID==projectid && s.Serial==serial).FirstOrDefault();

        //    if (trainingModule == null)
        //    {
        //        return Redirect("~/Home/AccessDenied");
        //    }
        //    ViewBag.PreviousTopic=db.TrainingModules.Where(p=>p.ProjectID==projectid && p.Serial==serial-1).Select(p=>p.ProjectTitle).FirstOrDefault();
        //    ViewBag.NextTopic = db.TrainingModules.Where(p => p.ProjectID == projectid && p.Serial == serial + 1).Select(p => p.ProjectTitle).FirstOrDefault();

        //    ViewBag.CountTotproject = db.TrainingModules.Where(r => r.ProjectID == projectid).ToList();
        //    ViewBag.CountTotprojectCount = db.TrainingModules.Where(r => r.ProjectID == projectid).Count();

        //    return View(trainingModule);
        //}



        // GET: TrainingModules/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            TrainingModule trainingModule = db.TrainingModules.Find(id);
            if (trainingModule == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            return View(trainingModule);
        }

        // GET: TrainingModules/Create
        public ActionResult Create(int? ProjectID)
        {
            
            if (ProjectID != null)
            {
                ViewBag.PID = ProjectID;
                ViewBag.ProjectName=db.Projects.Find(ProjectID).ProjectName;

                ViewBag.Serial = db.TrainingModules.Where(p=>p.ProjectID==ProjectID).Select(p=>p.Serial).Max();
                ViewBag.SubjectImages = db.ProjectImages.Where(s => s.ProjectID == ProjectID).ToList();
                ViewBag.SubjectImagesCount = db.ProjectImages.Where(s => s.ProjectID == ProjectID).Count();
                ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "ProjectName");
                ViewBag.Images = db.ProjectImages.Where(p => p.ProjectID == ProjectID).ToList();
                ViewBag.ImagesCount = db.ProjectImages.Where(p => p.ProjectID == ProjectID).Count();

                //return Content(ViewBag.PID +" " + ViewBag.ProjectName);
                return View();
            }

            else
            {
               
                ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "ProjectName");
                
                return View();
            }
        }

        // POST: TrainingModules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "ModuleID,ProjectID,ProjectTitle,Description,VideoUrl,DateUploaded,TimeInMins,Notes,Activity,Serial")] TrainingModule trainingModule)
        {
            if (ModelState.IsValid)
            {
                trainingModule.DateUploaded=DateTime.Now;
                int ProjectID=1;
                trainingModule.ProjectID=ProjectID;

                int serial = Convert.ToInt32(trainingModule.Serial);
                trainingModule.Serial = serial;

                db.TrainingModules.Add(trainingModule);
                db.SaveChanges();
                return Redirect("~/Projects/Details/"+trainingModule.ProjectID);
            }

            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "ProjectName", trainingModule.ProjectID);
            return View(trainingModule);
        }

        // GET: TrainingModules/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            TrainingModule trainingModule = db.TrainingModules.Find(id);
            if (trainingModule == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            ViewBag.Serial = db.TrainingModules.Where(p => p.ProjectID == id).Select(p => p.Serial).Max();
            ViewBag.Random = Guid.NewGuid().ToString();
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "ProjectName", trainingModule.ProjectID);
            return View(trainingModule);
        }

        // POST: TrainingModules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "ModuleID,ProjectID,ProjectTitle,Description,VideoUrl,DateUploaded,TimeInMins,Notes,Activity,Serial")] TrainingModule trainingModule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(trainingModule).State = EntityState.Modified;
                db.SaveChanges();
                return Redirect("~/Projects/Details/"+trainingModule.ProjectID);
            }
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "ProjectName", trainingModule.ProjectID);
            return View(trainingModule);
        }

        // GET: TrainingModules/Delete/5
        public ActionResult Delete(int? id, int? projectid)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            TrainingModule trainingModule = db.TrainingModules.Find(id);
            if (trainingModule == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            return View(trainingModule);
        }

        // POST: TrainingModules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int?id, int? projectid)
        {
            TrainingModule trainingModule = db.TrainingModules.Find(id);
            db.TrainingModules.Remove(trainingModule);
            db.SaveChanges();
            return Redirect("~/Projects/Details/"+projectid);
        }

        public ActionResult UploadProjectImage(HttpPostedFileBase[] filesmultiple)
        {
           
            Random random = new Random();
            //   Random generator = new Random();
            int rand1 = random.Next(9999, 9999999);
            int rand2 = random.Next(9999, 9999999);
            StringBuilder builder = new StringBuilder();
            int size = 550;
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                ViewBag.Random = builder.Append(ch);
            }
            int ProjectID = Convert.ToInt32(Request.Form["ProjectID"]);
            string efilepath;
            foreach (HttpPostedFileBase file in filesmultiple)
            {
                if (file != null)
                {
                    string ImageName = System.IO.Path.GetFileNameWithoutExtension(file.FileName);
                    //   MatricNo = MatricNo.Replace("-", "/");

                        string DocumentName = ImageName + ".jpg";
                        string DocumentNameDB = ImageName;
                        efilepath = Server.MapPath("~/ProjectImages/" + "\\" + DocumentName);
                        string ext = Path.GetExtension(efilepath);

                        //if file extension is supported, save file and update database
                        if (ext == ".jpg" || ext == ".jpeg" || ext == ".png" || ext == ".gif")
                        {
                            string newName = System.IO.Path.GetFileNameWithoutExtension(efilepath);
                            newName = newName + ".jpg";
                            //save image here
                            efilepath = Server.MapPath("~/ProjectImages/" + "\\" + DocumentName);
                            file.SaveAs(efilepath);
                            //SAVE TO QuestionImage Table
                            ProjectImage image = new ProjectImage();
                            image.ProjectID = ProjectID;
                            image.Image = DocumentNameDB;
                            db.ProjectImages.Add(image);
                            db.SaveChanges();
                        }

                    }

            }
            return Redirect("~/TrainingModules/Create/?projectid="+ProjectID);
        }

        [HttpPost]
        public ActionResult DeleteImage()
        {
            string efilepath = Request.Form["ImagePath"];
            int ID = Convert.ToInt32(Request.Form["ID"]);
            int ProjectID = Convert.ToInt32(Request.Form["ProjectID"]);
            DeleteImageFromDB(ID);
            efilepath = Server.MapPath("~/ProjectImages/" + "\\" + efilepath);
            System.IO.File.Delete(efilepath);
            return Redirect("~/TrainingModules/Create/?ProjectID=" + ProjectID);
        }

        public void DeleteImageFromDB(int id)
        {
            var constring = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
            SqlConnection conn = new SqlConnection(constring);
            conn.Open();
            string oString = "delete from ProjectImage where ID=@id";
            SqlCommand comm = new SqlCommand(oString, conn);
            comm.Parameters.AddWithValue("@id", id);
            comm.ExecuteScalar();
            conn.Close();
        }


        public ActionResult Comments(int? px, int? sx)
        {
            if (String.IsNullOrEmpty((string)Session["Name"])) return Redirect("/Account/Login");


            Comment comment = new Comment();
          
            string CommentMsg = Request.Form["CommentText"];
            
            int pp = Convert.ToInt32( Request.Form["project"]);
            int ss = Convert.ToInt32(Request.Form["serial"]);

           
                DateTime dateCommented = DateTime.Now;
                comment.DateCommented = (dateCommented);

                comment.PostedBy = Convert.ToString(Session["Name"]);
                string uname = Convert.ToString(Session["username"]);
                string pw = Convert.ToString(Session["password"]);
                //GET REGID FROM REGISTRATIONS
                int RegID = db.Users.Where(d => d.Email == uname && d.Password == pw).Select(d => d.ID).FirstOrDefault();

                comment.RegID = RegID;
                comment.Comments = CommentMsg;
                comment.ProjectID = pp;
                comment.Serial = ss;


                db.Comments.Add(comment);
                db.SaveChanges();


               

            return Redirect("~/TrainingModules/Index/?px="+pp+"&"+"sx="+ss);
            

            //ViewBag.Comment = db.Comments.Where(r => r.ProjectID == px && r.Serial == sx).Select(r => r.Comments).ToList();
            //ViewBag.commentCount = db.Comments.Where(r => r.ProjectID == px && r.Serial == sx).Count();
           
        }

        public ActionResult Get_Last_Serial(int projectid)
        {

            var result = (from r in db.TrainingModules
                          where r.ProjectID == projectid
                          select new { Text = r.Serial }).Max();
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

        //public ActionResult ChangePosition()
        //{
        //    int sigID = Convert.ToInt32(Request.Form["FileID"]);
        //    TrainingModule sigImage = new TrainingModule();
        //    int pos1 = Convert.ToInt32(Request.Form["position1"]);
        //    int pos2 = Convert.ToInt32(Request.Form["position2"]);
        //    int p1 = db.TrainingModules.Where(a => a.Serial == pos1).Count();
        //    int p2 = db.TrainingModules.Where(a => a.Serial == pos2).Count();
        //    if (p1 == 0 || p2 == 0)
        //    {
        //        ViewBag.Error = "There is no image at position " + p1 + " or " + p2;
        //        return RedirectToAction("Details/" + Request.Form["FileID"]);
        //    }
        //    else
        //    {
        //        var swap1 = db.Set<TrainingModule>().SingleOrDefault(o =>o.Serial == pos1);
        //        swap1.Serial = pos2;
        //        var swap2 = db.Set<TrainingModule>().SingleOrDefault(o => o.Serial ==pos2);
        //        swap2.Serial = pos1;

        //        db.SaveChanges();
        //        return Redirect("/SigmoidoscopyColonoscopies/Details/" + Request.Form["FileID"]);
        //    }
        //}

        public ActionResult Training1()
        {
            return View();
        }
    }
}
