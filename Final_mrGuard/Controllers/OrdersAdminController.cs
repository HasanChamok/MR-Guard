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
    public class OrdersAdminController : Controller
    {
        private Final_mrGuardEntities db = new Final_mrGuardEntities();

        // GET: OrdersAdmin
        public ActionResult Index()
        {
            var orders = db.Orders.Include(o => o.Customer).Include(o => o.Product);
            return View(orders.ToList());
        }

        // GET: OrdersAdmin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: OrdersAdmin/Create
        public ActionResult Create()
        {
            ViewBag.C_ID = new SelectList(db.Customers, "C_ID", "C_Email");
            ViewBag.Product_ID = new SelectList(db.Products, "Product_ID", "Product_Name");
            return View();
        }

        // POST: OrdersAdmin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Order_ID,Product_ID,C_ID,Price,O_Address,Payment_Type,Order_Quantity")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.C_ID = new SelectList(db.Customers, "C_ID", "C_Email", order.C_ID);
            ViewBag.Product_ID = new SelectList(db.Products, "Product_ID", "Product_Name", order.Product_ID);
            return View(order);
        }

        // GET: OrdersAdmin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.C_ID = new SelectList(db.Customers, "C_ID", "C_Email", order.C_ID);
            ViewBag.Product_ID = new SelectList(db.Products, "Product_ID", "Product_Name", order.Product_ID);
            return View(order);
        }

        // POST: OrdersAdmin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Order_ID,Product_ID,C_ID,Price,O_Address,Payment_Type,Order_Quantity")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.C_ID = new SelectList(db.Customers, "C_ID", "C_Email", order.C_ID);
            ViewBag.Product_ID = new SelectList(db.Products, "Product_ID", "Product_Name", order.Product_ID);
            return View(order);
        }

        // GET: OrdersAdmin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: OrdersAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
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

        public ActionResult TopSoldProducts()
        {
            // e.g. List.TopX(3) would return..
            var orders = db.Orders.OrderByDescending(d => d.Order_Quantity).GroupBy(r => r.Product_ID).SelectMany(g => g).ToList();


            //cits.OrderBy(d => d.cityname).GroupBy(d => d.state).SelectMany(g => g).ToList();

            //var orders1 = db.Orders.OrderByDescending(d => d.Order_Quantity).ToList(); 
            /*var prodList = (from Safety in db.Orders
                             select new { Safety.Product_ID, Safety.Order_Quantity } into p
                             group p by p.Product_ID into g
                             select new
                             {
                                 Product_ID = g.Key,
                                 Order_Quantity = g.Sum(x => x.Order_Quantity)
                             }).OrderByDescending(y => y.Order_Quantity).Take(3).ToList();



             List<TopSoldProduct> topSoldProds = new List<TopSoldProduct>();

             for (int i = 0; i < 3; i++)
             {
                 topSoldProds.Add(new TopSoldProduct()
                 {
                     order = db.Orders.Find(prodList[i].Product_ID),
                     Order_Quantity = Convert.ToInt32(prodList[i].Order_Quantity)
                 });
             }*/


            return View(orders.ToList());
        }
    }
}
