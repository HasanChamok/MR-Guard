using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Final_mrGuard.Models;

namespace Final_mrGuard.Controllers
{
    public class ProductsController : Controller
    {
        private Final_mrGuardEntities db = new Final_mrGuardEntities();

        // GET: Products
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Category);
            return View(products.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.Category_ID = new SelectList(db.Categories, "Category_ID", "Category_Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Product_ID,Category_ID,Product_Name,Price,Quantity,imagePath,ProductImageFile")] Product product)
        {
            if (ModelState.IsValid)
            {

                string fileName = Path.GetFileNameWithoutExtension(product.ProductImageFile.FileName);
                string extention = Path.GetExtension(product.ProductImageFile.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extention;
                product.imagePath = "~/Image/" + fileName;
                fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
                product.ProductImageFile.SaveAs(fileName);
                using (Final_mrGuardEntities db = new Final_mrGuardEntities())
                {
                    db.Products.Add(product);
                    db.SaveChanges();
                }
                //db.Products.Add(product);
                //db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Category_ID = new SelectList(db.Categories, "Category_ID", "Category_Name", product.Category_ID);
            return View(product);
        }
        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.Category_ID = new SelectList(db.Categories, "Category_ID", "Category_Name", product.Category_ID);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Product_ID,Category_ID,Product_Name,Price,Quantity,imagePath")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Category_ID = new SelectList(db.Categories, "Category_ID", "Category_Name", product.Category_ID);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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

        [HttpGet]
        public ActionResult View(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product p = db.Products.Find(id);
            if (p == null)
            {
                return HttpNotFound();
            }
            return View(p);
        }

        public ActionResult emptyStock()
        {
            var empty = db.Products.SqlQuery("Select * From Product");

            return View(empty);
        }


    }
}
