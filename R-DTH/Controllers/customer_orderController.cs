using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using R_DTH.Models;

namespace R_DTH.Controllers
{
    public class customer_orderController : Controller
    {
        private rdthEntities db = new rdthEntities();

        // GET: customer_order
        public ActionResult Index()
        {
            var customer_order = db.customer_order.Include(c => c.customer).Include(c => c.product);
            return View(customer_order.ToList());
        }

        // GET: customer_order/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            customer_order customer_order = db.customer_order.Find(id);
            if (customer_order == null)
            {
                return HttpNotFound();
            }
            return View(customer_order);
        }

        // GET: customer_order/Create
        public ActionResult Create(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product product = db.products.Find(id);
            customer_order customer_Order = new customer_order();
            customer_Order.cusOrder_product = product.product_id;
            customer_Order.cusOrder_productName = product.product_name;
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(customer_Order);
        }

        // POST: customer_order/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "cusOrder_id,cusOrder_customer,cusOrder_product,cusOrder_quantity,cusOrder_amount,cusOrder_date")] customer_order customer_order)
        {
            if (ModelState.IsValid)
            {
                product product = new product();
                product = db.products.Find(customer_order.cusOrder_product);
                customer_order.cusOrder_amount = (customer_order.cusOrder_quantity * product.product_charges);
                DateTime dt = DateTime.Today;
                customer_order.cusOrder_date = dt;
                db.customer_order.Add(customer_order);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(customer_order);
        }

        // GET: customer_order/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            customer_order customer_order = db.customer_order.Find(id);
            if (customer_order == null)
            {
                return HttpNotFound();
            }
            ViewBag.cusOrder_customer = new SelectList(db.customers, "cus_id", "cus_name", customer_order.cusOrder_customer);
            ViewBag.cusOrder_product = new SelectList(db.products, "product_id", "product_name", customer_order.cusOrder_product);
            return View(customer_order);
        }

        // POST: customer_order/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "cusOrder_id,cusOrder_customer,cusOrder_product,cusOrder_quantity,cusOrder_amount,cusOrder_date")] customer_order customer_order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer_order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.cusOrder_customer = new SelectList(db.customers, "cus_id", "cus_name", customer_order.cusOrder_customer);
            ViewBag.cusOrder_product = new SelectList(db.products, "product_id", "product_name", customer_order.cusOrder_product);
            return View(customer_order);
        }

        // GET: customer_order/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            customer_order customer_order = db.customer_order.Find(id);
            if (customer_order == null)
            {
                return HttpNotFound();
            }
            return View(customer_order);
        }

        // POST: customer_order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            customer_order customer_order = db.customer_order.Find(id);
            db.customer_order.Remove(customer_order);
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
    }
}
