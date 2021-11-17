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
    public class CustomersController : Controller
    {
        private Final_mrGuardEntities db = new Final_mrGuardEntities();

        // GET: Customers
        public ActionResult Index()
        {
            HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddYears(-1));
            HttpContext.Response.Cache.SetValidUntilExpires(false);
            HttpContext.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Response.Cache.SetNoStore();
            HttpContext.Response.ExpiresAbsolute = DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0, 0));
            HttpContext.Response.Expires = 0;
            HttpContext.Response.Cache.AppendCacheExtension("no-store, no-cache, must-revalidate, proxy-revalidate, post-check=0, pre-check=0");
            return View(db.Customers.ToList());
        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "C_ID,C_Email,C_Password,C_Name,C_Phone,C_Address")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customer);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "C_ID,C_Email,C_Password,C_Name,C_Phone,C_Address")] Customer customer)
        {
           

            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("DashBoard");
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
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
        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(Customer customer)
        {

            if (ModelState.IsValid)
            {
                var checkUser = db.Customers.Where(c => c.C_Email.Equals(customer.C_Email)).FirstOrDefault();
                if (checkUser != null)
                {
                    ViewBag.errorMsg = "An account already exists with this email";
                }
                else
                {

                    db.Customers.Add(customer);
                    db.SaveChanges();
                    //  return Content("SignUp Successful");
                    return RedirectToAction("Login");
                }

            }
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddYears(-1));
            HttpContext.Response.Cache.SetValidUntilExpires(false);
            HttpContext.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Response.Cache.SetNoStore();
            HttpContext.Response.ExpiresAbsolute = DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0, 0));
            HttpContext.Response.Expires = 0;
            HttpContext.Response.Cache.AppendCacheExtension("no-store, no-cache, must-revalidate, proxy-revalidate, post-check=0, pre-check=0");
            if (Session["customer_email"] != null)
                return RedirectToAction("Index", "Home");
            return View();

        }

        [HttpPost]
        public ActionResult Login(TempUser tempUser)
        {


            if (ModelState.IsValid)
            {
                var Customer = db.Customers.Where(u => u.C_Email.Equals(tempUser.C_Email) && u.C_Password.Equals(tempUser.C_Password)).FirstOrDefault();

                if (Customer != null)
                {
                    Session["customer_email"] = Customer.C_Email;
                    Session["customer_id"] = Customer.C_ID;
                    Session["customer_name"] = Customer.C_Name;
                    // return Content("Login Successfull");
                    return RedirectToAction("DashBoard");
                }
                else
                {
                    //return Content("Login Failed");
                    ViewBag.LoginFailed = "User not found or password missmachted";
                }

            }

            return View();
        }

        public ActionResult DashBoard()
        {
            

            String email = Convert.ToString(Session["customer_email"]);
            var customer = db.Customers.Where(u => u.C_Email.Equals(email)).FirstOrDefault();
            return View(customer);
        }


        public ActionResult Signout()
        {


            Session.Clear();
            HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddYears(-1));
            HttpContext.Response.Cache.SetValidUntilExpires(false);
            HttpContext.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Response.Cache.SetNoStore();
            HttpContext.Response.ExpiresAbsolute = DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0, 0));
            HttpContext.Response.Expires = 0;
            HttpContext.Response.Cache.AppendCacheExtension("no-store, no-cache, must-revalidate, proxy-revalidate, post-check=0, pre-check=0");
            return RedirectToAction("Index", "Home");
        }
    }
}
