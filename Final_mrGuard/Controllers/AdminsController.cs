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
    public class AdminsController : Controller
    {
        private Final_mrGuardEntities db = new Final_mrGuardEntities();

        // GET: Admins
        /* public ActionResult Index()
          {
              return View(db.Admins.ToList());
          }

          // GET: Admins/Details/5
          public ActionResult Details(int? id)
          {
              if (id == null)
              {
                  return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
              }
              Admin admin = db.Admins.Find(id);
              if (admin == null)
              {
                  return HttpNotFound();
              }
              return View(admin);
          }

          // GET: Admins/Create
          public ActionResult Create()
          {
              return View();
          }

          // POST: Admins/Create
          // To protect from overposting attacks, enable the specific properties you want to bind to, for 
          // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
          [HttpPost]
          [ValidateAntiForgeryToken]
          public ActionResult Create([Bind(Include = "AdminID,AdminEmail,AdminPassword,AdminName")] Admin admin)
          {
              if (ModelState.IsValid)
              {
                  db.Admins.Add(admin);
                  db.SaveChanges();
                  return RedirectToAction("Index");
              }

              return View(admin);
          }

          // GET: Admins/Edit/5
          public ActionResult Edit(int? id)
          {
              if (id == null)
              {
                  return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
              }
              Admin admin = db.Admins.Find(id);
              if (admin == null)
              {
                  return HttpNotFound();
              }
              return View(admin);
          }

          // POST: Admins/Edit/5
          // To protect from overposting attacks, enable the specific properties you want to bind to, for 
          // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
          [HttpPost]
          [ValidateAntiForgeryToken]
          public ActionResult Edit([Bind(Include = "AdminID,AdminEmail,AdminPassword,AdminName")] Admin admin)
          {
              if (ModelState.IsValid)
              {
                  db.Entry(admin).State = EntityState.Modified;
                  db.SaveChanges();
                  return RedirectToAction("Index");
              }
              return View(admin);
          }

          // GET: Admins/Delete/5
          public ActionResult Delete(int? id)
          {
              if (id == null)
              {
                  return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
              }
              Admin admin = db.Admins.Find(id);
              if (admin == null)
              {
                  return HttpNotFound();
              }
              return View(admin);
          }

          // POST: Admins/Delete/5
          [HttpPost, ActionName("Delete")]
          [ValidateAntiForgeryToken]
          public ActionResult DeleteConfirmed(int id)
          {
              Admin admin = db.Admins.Find(id);
              db.Admins.Remove(admin);
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
          }*/
        [HttpGet]
        public ActionResult AdminLogin()
        {
            if (Session["admin_email"] != null)
                return RedirectToAction("Index", "Home");
            return View();

        }

        [HttpPost]
        public ActionResult AdminLogin(TempAdmin tempAdmin)
        {

            HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddYears(-1));
            HttpContext.Response.Cache.SetValidUntilExpires(false);
            HttpContext.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Response.Cache.SetNoStore();
            HttpContext.Response.ExpiresAbsolute = DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0, 0));
            HttpContext.Response.Expires = 0;
            HttpContext.Response.Cache.AppendCacheExtension("no-store, no-cache, must-revalidate, proxy-revalidate, post-check=0, pre-check=0");

            if (ModelState.IsValid)
            {
                var Admin = db.Admins.Where(u => u.AdminEmail.Equals(tempAdmin.AdminEmail) && u.AdminPassword.Equals(tempAdmin.AdminPassword)).FirstOrDefault();

                if (Admin != null)
                {

                    Session["admin_email"] = Admin.AdminEmail;
                    Session["admin_name"] = Admin.AdminName;

                    //return Content("Login Successfull");
                    return RedirectToAction("AdminDashBoard");
                }
                else
                {

                    // return Content("Login Failed");
                    ViewBag.LoginFailed = "Admin not found or password missmachted";

                }

            }

            return View();
        }

        public ActionResult AdminDashBoard()
        {
            HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddYears(-1));
            HttpContext.Response.Cache.SetValidUntilExpires(false);
            HttpContext.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Response.Cache.SetNoStore();
            HttpContext.Response.ExpiresAbsolute = DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0, 0));
            HttpContext.Response.Expires = 0;
            HttpContext.Response.Cache.AppendCacheExtension("no-store, no-cache, must-revalidate, proxy-revalidate, post-check=0, pre-check=0");

            String email = Convert.ToString(Session["admin_email"]);


            var admin = db.Admins.Where(u => u.AdminEmail.Equals(email)).FirstOrDefault();
            return View(admin);
        }

        public ActionResult AdminSignout()
        {
            HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddYears(-1));
            HttpContext.Response.Cache.SetValidUntilExpires(false);
            HttpContext.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Response.Cache.SetNoStore();
            HttpContext.Response.ExpiresAbsolute = DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0, 0));
            HttpContext.Response.Expires = 0;
            HttpContext.Response.Cache.AppendCacheExtension("no-store, no-cache, must-revalidate, proxy-revalidate, post-check=0, pre-check=0");
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
