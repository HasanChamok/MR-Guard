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
    public class ProductsCustomerController : Controller
    {
        private Final_mrGuardEntities db = new Final_mrGuardEntities();

        // GET: ProductsCustomer
       [HttpGet]
        public ActionResult Index()
        {

            using (db)
            {

                var p = db.Products.SqlQuery("SELECT * FROM Product").ToList<Product>();

                ViewData["pr"] = p;
                return View();
            }

           /*var products = db.Products.Include(p => p.Category);
            return View(products.ToList());*/
        }


        [HttpPost]

        public ActionResult Index(TempProducts pro)
        {
            if (ModelState.IsValid)
            {
               

                var p = db.Products.SqlQuery("Select *from Product where Product_Name like'%"
                    + (pro.Product_Name ?? "%") + "%'").ToList<Product>();



                ViewData["pr"] = p;
                return View();


            }

            return View();
        }


        public ActionResult Checkout()
        {
            return View();
        }


        public ActionResult CheckoutDetails()
        {
           


            List<Item> cart = (List<Item>)Session["cart"];
            for (int i = 0; i < cart.Count; i++)
            {
                Order index = new Order()
                {
                    Product_ID = cart[i].Product.Product_ID,
                    C_ID = Convert.ToInt32(Session["customer_id"]),
                    Price = cart[i].Product.Price * cart[i].Quantity,
                    O_Address= " ",
                    Payment_Type= " ",
                    Order_Quantity = cart[i].Quantity,

                };
                db.Orders.Add(index);
                db.SaveChanges();
                var o = db.Orders.SqlQuery("Select * From Orders").ToList<Order>();
                foreach (var x in o)
                {
                    ViewData["o_id"] = x.Order_ID;
                }
                Product product = db.Products.Find(cart[i].Product.Product_ID);
                //product.Quantity = product.Quantity - cart[i].Quantity;
                //Product product = new Product();
                if (product.Product_ID == cart[i].Product.Product_ID)
                {

                    product.Quantity = product.Quantity - cart[i].Quantity;

                }
                db.SaveChanges();

            }

            return View();
        }


        // GET: ProductsCustomer/Details/5
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

        // GET: ProductsCustomer/Create
        public ActionResult Create()
        {
            ViewBag.Category_ID = new SelectList(db.Categories, "Category_ID", "Category_Name");
            return View();
        }

        // POST: ProductsCustomer/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Product_ID,Category_ID,Product_Name,Price,Quantity,imagePath")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Category_ID = new SelectList(db.Categories, "Category_ID", "Category_Name", product.Category_ID);
            return View(product);
        }

        // GET: ProductsCustomer/Edit/5
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

        // POST: ProductsCustomer/Edit/5
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

        // GET: ProductsCustomer/Delete/5
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

        // POST: ProductsCustomer/Delete/5
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

        public ActionResult AddToCart(int productID)
        {

            if (Session["cart"] == null)
            {
                List<Item> cart = new List<Item>();
                var product = db.Products.Find(productID);
                cart.Add(new Item()
                {
                    Product = product,
                    Quantity = 1


                });

                Session["cart"] = cart;


            }
            else
            {
                List<Item> cart = (List<Item>)Session["cart"];
                var product = db.Products.Find(productID);

                foreach (var item in cart.ToList())
                {
                    if (item.Product.Product_ID == productID)
                    {
                        int prevQty = item.Quantity;
                        cart.Remove(item);
                        cart.Add(new Item()
                        {
                            Product = product,
                            Quantity = prevQty + 1

                        });
                        break;
                    }
                    else
                    {
                        cart.Add(new Item()
                        {
                            Product = product,
                            Quantity = 1
                        });

                    }
                }

                Session["cart"] = cart;
            }

            return Redirect("Index");
        }




        public ActionResult RemoveFromCart(int productID)
        {

            List<Item> cart = new List<Item>();
            //var product = db.Products.Find(productID);
            foreach (var item in cart.ToList())
            {
                if (item.Product.Product_ID == productID)
                {
                    cart.Remove(item);
                    break;
                }
            }

            Session["cart"] = cart;
            return Redirect("Index");
        }




        [HttpGet]
        public ActionResult View(int? id)
        {
            //Product product = new Product();
            /*using (mr_guardEntities db = new mr_guardEntities())
            {
                var product = db.Products.Where(x => x.Product_ID == id).FirstOrDefault();
                return View(product);
            }*/
            //return View(product);

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

        public ActionResult SaveOrder()
        {

            List<Item> cart = (List<Item>)Session["cart"];
            for (int i = 0; i < cart.Count; i++)
            {
                Order index = new Order()
                {
                    Product_ID = cart[i].Product.Product_ID,
                    C_ID = 1,
                    Price = cart[i].Product.Price,
                    Order_Quantity = cart[i].Quantity,

                };
                db.Orders.Add(index);
                db.SaveChanges();
            }
            return View();
        }
    }
}
