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
    public class customer_card_numberController : Controller
    {
        private rdthEntities db = new rdthEntities();

        // GET: customer_card_number
        public ActionResult Index()
        {
            return View(db.customer_card_number.ToList());
        }

        // GET: customer_card_number/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            customer_card_number customer_card_number = db.customer_card_number.Find(id);
            if (customer_card_number == null)
            {
                return HttpNotFound();
            }
            return View(customer_card_number);
        }

        // GET: customer_card_number/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: customer_card_number/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ccn_id,ccn_numbers,givenQuantity")] customer_card_number customer_card_number)
        {
            if (ModelState.IsValid)
            {
                int gq = customer_card_number.givenQuantity;
                for(int i=1; i<=gq; i++)
                {
                    Random r = new Random();
                    doItAgain:
                    var rno = r.Next();
                    var ccn = db.customer_card_number.FirstOrDefault(u => u.ccn_numbers == rno);
                    if(ccn!=null)
                    {
                        goto doItAgain;
                    }
                    else
                    {
                        customer_card_number.ccn_numbers = rno;
                        db.customer_card_number.Add(customer_card_number);
                        db.SaveChanges();
                    }
                }
                return RedirectToAction("Index");
            }

            return View(customer_card_number);
        }

        // GET: customer_card_number/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            customer_card_number customer_card_number = db.customer_card_number.Find(id);
            if (customer_card_number == null)
            {
                return HttpNotFound();
            }
            return View(customer_card_number);
        }

        // POST: customer_card_number/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ccn_id,ccn_numbers")] customer_card_number customer_card_number)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer_card_number).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer_card_number);
        }

        // GET: customer_card_number/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            customer_card_number customer_card_number = db.customer_card_number.Find(id);
            if (customer_card_number == null)
            {
                return HttpNotFound();
            }
            return View(customer_card_number);
        }

        // POST: customer_card_number/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            customer_card_number customer_card_number = db.customer_card_number.Find(id);
            db.customer_card_number.Remove(customer_card_number);
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
