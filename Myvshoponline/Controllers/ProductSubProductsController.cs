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

namespace Myvshoponline.Controllers
{
    public class ProductSubProductsController : Controller
    {
        private MyvshoponlineEntities db = new MyvshoponlineEntities();
        Getdata mydata = new Getdata();

        // GET: ProductSubProducts1
        public ActionResult Index()
        {
            if (!string.IsNullOrEmpty((string)Session["username"]) && ((string)Session["UserRole"] == "Shop Admin" || (string)Session["UserRole"] == "Super Admin"))
            {

                var productSubProducts = db.ProductSubProducts.Include(p => p.Product).Include(p => p.ProductStatu);
                return View(productSubProducts.ToList());
            }
            else
            {
                return Redirect("~/Home/AccessDenied");
            }
        }

        // GET: ProductSubProducts1/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            ProductSubProduct productSubProduct = db.ProductSubProducts.Find(id);
            if (productSubProduct == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            return View(productSubProduct);
        }

        // GET: ProductSubProducts1/Create
        public ActionResult Create(int?pid)
        {
            int UserID =(int) db.Products.Find(pid).Shop.UserID ;
            ViewBag.ProductStatusID = new SelectList(db.ProductStatus, "ID", "Status");
            if (!string.IsNullOrEmpty((string)Session["username"]) && ((string)Session["UserRole"] == "Shop Admin" || (string)Session["UserRole"] == "Super Admin") && (int)Session["UserID"] == UserID)
            {

                int ShopID = db.Products.Find(pid).ShopID;
                ViewBag.ProductID = pid;
                ViewBag.ProductName = db.Products.Find(pid).Name;
                ViewBag.ShopID = ShopID;
                ViewBag.ShopName = db.Shops.Find(ShopID).Name;
                var item = db.ProductSubProducts.Where(p => p.ProductID == pid);
                return View();
            }
            else
            {
                return Redirect("~/Home/AccessDenied");
            }
        }

        // POST: ProductSubProducts1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "ID,Name,Details,ProductID,ProductStatusID,Amount,QuantityinStock,DateCreated,PurchasePrice,Currency")] ProductSubProduct productSubProduct, HttpPostedFileBase[] files)
        {
            if (ModelState.IsValid)
            {
                productSubProduct.DateCreated = DateTime.Now;
                db.ProductSubProducts.Add(productSubProduct);
                db.SaveChanges();

                int UserID = (int)Session["UserID"];
                string ShopName = db.Products.Where(p=>p.ID==productSubProduct.ProductID).Select(p=>p.Shop.Name).FirstOrDefault();
                int ShopID = db.Products.Where(p => p.ID == productSubProduct.ProductID).Select(p => p.ShopID).FirstOrDefault();
                string businessname = db.Users.Find(UserID).CompanyName;
                string hardtoken = db.Users.Find(UserID).HardToken;
                string DocumentName =productSubProduct.ProductID+"-"+productSubProduct.ID + ".jpg";
                string efilepath;
                foreach (HttpPostedFileBase file in files)
                {
                    if (file != null)
                    {
                        efilepath = Server.MapPath("~/BusinessImages/" + businessname+hardtoken + "/" + ShopName + "/" + "\\" + DocumentName);
                        string ext = Path.GetExtension(efilepath);

                        //if file extension is supported, save file and update database
                        if (ext == ".jpg" || ext == ".jpeg" || ext == ".png" || ext == ".gif" || ext == ".ico")
                        {
                            string newName = System.IO.Path.GetFileNameWithoutExtension(efilepath);
                            newName = newName + ".jpg";
                            efilepath = Server.MapPath("~/BusinessImages/" + businessname+hardtoken + "/" + ShopName + "/" + "\\" + newName);
                            file.SaveAs(efilepath);
                          //  mydata.ResizePicture(efilepath);
                        }
                    }

                }
                //Save the picture ends

                return Redirect("~/ProductSubProducts/ProductSubitems/?id="+productSubProduct.ProductID+"&sid="+ShopID);
            }

            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name", productSubProduct.ProductID);
            ViewBag.ProductStatusID = new SelectList(db.ProductStatus, "ID", "Status", productSubProduct.ProductStatusID);
            return View(productSubProduct);
        }

        // GET: ProductSubProducts1/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            ProductSubProduct productSubProduct = db.ProductSubProducts.Find(id);
            if (productSubProduct == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            ViewBag.ProductID = id;
            ViewBag.ProductStatusID = new SelectList(db.ProductStatus, "ID", "Status");
            ViewBag.ProductName = db.ProductSubProducts.Find(id).Name;
            ViewBag.ShopID = db.ProductSubProducts.Find(id).Product.ShopID;
            ViewBag.ShopName = db.ProductSubProducts.Find(id).Product.Shop.Name;
            int ShopID = ViewBag.ShopID;
            ViewBag.CompanyName = db.Shops.Where(s => s.ID ==ShopID ).Select(s => s.User.CompanyName).FirstOrDefault();
            ViewBag.ProductStatusID = new SelectList(db.ProductStatus, "ID", "Status", productSubProduct.ProductStatusID);
            return View(productSubProduct);
        }

        // POST: ProductSubProducts1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "ID,Name,Details,ProductID,ProductStatusID,Amount,QuantityinStock,DateCreated,PurchasePrice,Currency")] ProductSubProduct productSubProduct,int?pid, int?sid)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productSubProduct).State = EntityState.Modified;
                db.SaveChanges();
                return Redirect("~/ProductSubProducts/ProductSubitems/?id=" +pid + "&sid=" + sid);
            }
            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name", productSubProduct.ProductID);
            ViewBag.ProductStatusID = new SelectList(db.ProductStatus, "ID", "Status", productSubProduct.ProductStatusID);
            return View(productSubProduct);
        }

        // GET: ProductSubProducts1/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            ProductSubProduct productSubProduct = db.ProductSubProducts.Find(id);
            if (productSubProduct == null)
            {
                return Redirect("~/Home/AccessDenied");
            }
            return View(productSubProduct);
        }

        // POST: ProductSubProducts1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductSubProduct productSubProduct = db.ProductSubProducts.Find(id);
            db.ProductSubProducts.Remove(productSubProduct);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ProductSubitems(int? id, int? sid, string search)
        {
            if (db.ProductSubProducts.Where(s => s.ProductID == id).Count() == 0 || db.Shops.Where(s => s.ID == sid).Count() == 0)
            {
                return Redirect("~/Home/AccessDenied");
            }
            if (id != null && sid != null)
            {
                ViewBag.Products = db.ProductSubProducts.Where(s => s.ProductID == id).ToList();
                var products = db.ProductSubProducts.Include(p => p.Product).Include(p => p.ProductStatu);
                ViewBag.ShopID = sid;
                ViewBag.UserID = db.Shops.Find(sid).UserID;
                ViewBag.ProductID =id;
                ViewBag.CategoryID = db.Products.Find(id).ProductCategoryID;
                ViewBag.CategoryName = db.Products.Find(id).ProductCategory.Name;
                ViewBag.ProductName = db.ProductSubProducts.Where(p=>p.ProductID==id).Select(p=>p.Product.Name).FirstOrDefault();
                ViewBag.Shop = db.Shops.Where(s => s.ID == sid).ToList();
                ViewBag.CompanyName = db.Shops.Where(s => s.ID == sid).Select(s => s.User.CompanyName).FirstOrDefault().ToUpper();
                ViewBag.ShopName = db.Shops.Where(s => s.ID == sid).Select(s => s.Name).FirstOrDefault().ToUpper();
                if (search != null && id != null && sid != null)
                {
                    ViewBag.search = search;
                    return View(products.Where(p => p.ProductID == id && p.ProductStatu.Status == "Available" && p.Product.Shop.ShopStatus == "Active" && p.Name.Contains(search)).OrderByDescending(p => p.DateCreated).ToList());
                }
                return View(products.Where(p => p.ProductID == id && p.ProductStatu.Status == "Available" && p.Product.Shop.ShopStatus == "Active").OrderByDescending(p => p.DateCreated).ToList());
            }
            else
            {
                return Redirect("~/Home/AccessDenied");
            }
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
