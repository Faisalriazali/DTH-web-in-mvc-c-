
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
    public class dealer_orderController : Controller
    {
        private rdthEntities db = new rdthEntities();

        // GET: dealer_order
        public ActionResult Index()
        {
            var dealer_order = db.dealer_order.Include(d => d.dealer).Include(d => d.product);
            return View(dealer_order.ToList());
        }

        // GET: dealer_order/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dealer_order dealer_order = db.dealer_order.Find(id);
            if (dealer_order == null)
            {
                return HttpNotFound();
            }
            return View(dealer_order);
        }

        // GET: dealer_order/Create
        public ActionResult Create(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product product = db.products.Find(id);
            dealer_order dealer_Order = new dealer_order();
            dealer_Order.dealOrder_product = product.product_id;
            dealer_Order.dealOrder_Name = product.product_name;
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(dealer_Order);
        }

        // POST: dealer_order/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "dealOrder_id,dealOrder_dealer,dealOrder_product,dealOrder_quantity,dealOrder_amount,dealOrder_date")] dealer_order dealer_order)
        {
            if (ModelState.IsValid)
            {
                product product = new product();
                product = db.products.Find(dealer_order.dealOrder_product);
                dealer_order.dealOrder_amount = (dealer_order.dealOrder_quantity * product.product_charges);
                DateTime dt = DateTime.Today;
                dealer_order.dealOrder_date = dt;
                db.dealer_order.Add(dealer_order);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(dealer_order);
        }

        // GET: dealer_order/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dealer_order dealer_order = db.dealer_order.Find(id);
            if (dealer_order == null)
            {
                return HttpNotFound();
            }
            ViewBag.dealOrder_dealer = new SelectList(db.dealers, "dealer_id", "dealer_name", dealer_order.dealOrder_dealer);
            ViewBag.dealOrder_product = new SelectList(db.products, "product_id", "product_name", dealer_order.dealOrder_product);
            return View(dealer_order);
        }

        // POST: dealer_order/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "dealOrder_id,dealOrder_dealer,dealOrder_product,dealOrder_quantity,dealOrder_amount,dealOrder_date")] dealer_order dealer_order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dealer_order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.dealOrder_dealer = new SelectList(db.dealers, "dealer_id", "dealer_name", dealer_order.dealOrder_dealer);
            ViewBag.dealOrder_product = new SelectList(db.products, "product_id", "product_name", dealer_order.dealOrder_product);
            return View(dealer_order);
        }

        // GET: dealer_order/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dealer_order dealer_order = db.dealer_order.Find(id);
            if (dealer_order == null)
            {
                return HttpNotFound();
            }
            return View(dealer_order);
        }

        // POST: dealer_order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            dealer_order dealer_order = db.dealer_order.Find(id);
            db.dealer_order.Remove(dealer_order);
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
