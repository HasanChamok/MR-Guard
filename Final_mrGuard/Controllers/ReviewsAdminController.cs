using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Final_mrGuard.Models;

namespace Final_mrGuard.Controllers
{
    public class ReviewsAdminController : Controller
    {
        private Final_mrGuardEntities db = new Final_mrGuardEntities();

        // GET: ReviewsAdmin
        public ActionResult Index()
        {
            var reviews = db.Reviews.Include(r => r.Customer).Include(r => r.Product);
            return View(reviews.ToList());
        }

        // GET: ReviewsAdmin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        // GET: ReviewsAdmin/Create
        public ActionResult Create()
        {
            ViewBag.C_ID = new SelectList(db.Customers, "C_ID", "C_Email");
            ViewBag.Product_ID = new SelectList(db.Products, "Product_ID", "Product_Name");
            return View();
        }

        // POST: ReviewsAdmin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "R_ID,C_ID,Product_ID,Comment,Rating")] Review review)
        {
            if (ModelState.IsValid)
            {
                db.Reviews.Add(review);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.C_ID = new SelectList(db.Customers, "C_ID", "C_Email", review.C_ID);
            ViewBag.Product_ID = new SelectList(db.Products, "Product_ID", "Product_Name", review.Product_ID);
            return View(review);
        }

        // GET: ReviewsAdmin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            ViewBag.C_ID = new SelectList(db.Customers, "C_ID", "C_Email", review.C_ID);
            ViewBag.Product_ID = new SelectList(db.Products, "Product_ID", "Product_Name", review.Product_ID);
            return View(review);
        }

        // POST: ReviewsAdmin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "R_ID,C_ID,Product_ID,Comment,Rating")] Review review)
        {
            if (ModelState.IsValid)
            {
                db.Entry(review).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.C_ID = new SelectList(db.Customers, "C_ID", "C_Email", review.C_ID);
            ViewBag.Product_ID = new SelectList(db.Products, "Product_ID", "Product_Name", review.Product_ID);
            return View(review);
        }

        // GET: ReviewsAdmin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        // POST: ReviewsAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Review review = db.Reviews.Find(id);
            db.Reviews.Remove(review);
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

        public ActionResult productRanking()
        {

            var rating = db.Reviews.OrderByDescending(d => d.Rating).GroupBy(r => r.Product_ID).SelectMany(g => g).ToList();

            return View(rating.ToList());
        }



      }
}
