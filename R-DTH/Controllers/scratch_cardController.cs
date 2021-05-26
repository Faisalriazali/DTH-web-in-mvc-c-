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
    public class scratch_cardController : Controller
    {
        private rdthEntities db = new rdthEntities();

        // GET: scratch_card
        public ActionResult Index()
        {
            var scratch_card = db.scratch_card.Include(s => s.package);
            return View(scratch_card.ToList());
        }

        // GET: scratch_card/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            scratch_card scratch_card = db.scratch_card.Find(id);
            if (scratch_card == null)
            {
                return HttpNotFound();
            }
            return View(scratch_card);
        }

        // GET: scratch_card/Create
        public ActionResult Create()
        {
            ViewBag.card_package = new SelectList(db.packages, "pack_id", "pack_name");
            return View();
        }

        // POST: scratch_card/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "card_number,card_package,card_charges,card_status")] scratch_card scratch_card)
        {
            if (ModelState.IsValid)
            {
                var lis = db.packages.Where(c => c.pack_id == scratch_card.card_package).FirstOrDefault();
                scratch_card.card_charges = lis.pack_charges;
                scratch_card.card_status = "Valid";
                db.scratch_card.Add(scratch_card);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.card_package = new SelectList(db.packages, "pack_id", "pack_name", scratch_card.card_package);
            return View(scratch_card);
        }

        public ActionResult RenewAccount()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RenewAccount([Bind(Include = "card_number,card_check,customerId,card_charges,card_status")] scratch_card scratch_card)
        {
            if (ModelState.IsValid)
            {
                var lis = db.scratch_card.Where(c => c.card_number == scratch_card.card_check && c.card_status == "Valid").FirstOrDefault();
                if (lis != null)
                {
                    customer customer = db.customers.Find(scratch_card.customerId);
                    scratch_card = db.scratch_card.Find(scratch_card.card_check);
                    customer.cus_payment_left = customer.cus_payment_left - lis.card_charges;
                    customer.cus_totalypaid = customer.cus_totalypaid + Convert.ToInt64( lis.card_charges);
                    db.Entry(customer).State = EntityState.Modified;
                    scratch_card.card_status = "Invalid";
                    db.Entry(scratch_card).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("RenewAccount");
                }
                else
                {
                    scratch_card.LoginErrorMessage = "Card Invalid Or Expired";
                    return View(scratch_card);
                }
            }

            ViewBag.card_package = new SelectList(db.packages, "pack_id", "pack_name", scratch_card.card_package);
            return View(scratch_card);
        }

        // GET: scratch_card/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            scratch_card scratch_card = db.scratch_card.Find(id);
            if (scratch_card == null)
            {
                return HttpNotFound();
            }
            ViewBag.card_package = new SelectList(db.packages, "pack_id", "pack_name", scratch_card.card_package);
            return View(scratch_card);
        }

        // POST: scratch_card/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "card_number,card_package,card_charges,card_status")] scratch_card scratch_card)
        {
            if (ModelState.IsValid)
            {
                db.Entry(scratch_card).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.card_package = new SelectList(db.packages, "pack_id", "pack_name", scratch_card.card_package);
            return View(scratch_card);
        }

        // GET: scratch_card/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            scratch_card scratch_card = db.scratch_card.Find(id);
            if (scratch_card == null)
            {
                return HttpNotFound();
            }
            return View(scratch_card);
        }

        // POST: scratch_card/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            scratch_card scratch_card = db.scratch_card.Find(id);
            db.scratch_card.Remove(scratch_card);
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
