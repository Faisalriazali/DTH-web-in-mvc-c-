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
    public class package_change_requestController : Controller
    {
        private rdthEntities db = new rdthEntities();

        // GET: package_change_request
        public ActionResult Index()
        {
            return View(db.package_change_request.ToList());
        }

        // GET: package_change_request/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            package_change_request package_change_request = db.package_change_request.Find(id);
            if (package_change_request == null)
            {
                return HttpNotFound();
            }
            return View(package_change_request);
        }

        // GET: package_change_request/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            package package = db.packages.Find(id);
            package_change_request package_Change_Request=new package_change_request();
            package_Change_Request.pcr_packId = package.pack_id;
            package_Change_Request.pcr_packName = package.pack_name;
            package_Change_Request.pcr_packCharges = package.pack_charges;
            if (package == null)
            {
                return HttpNotFound();
            }
            return View(package_Change_Request);
        }

        // POST: package_change_request/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "pcr_id,pcr_packId,pcr_packName,pcr_packCharges,pcr_cusId,pcr_cusName,pcr_cusCardNo")] package_change_request package_change_request)
        {
            if (ModelState.IsValid)
            {
                db.package_change_request.Add(package_change_request);
                db.SaveChanges();
                return RedirectToAction("Packages","packages");
            }

            return View(package_change_request);
        }

        // GET: package_change_request/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            package_change_request package_change_request = db.package_change_request.Find(id);
            if (package_change_request == null)
            {
                return HttpNotFound();
            }
            return View(package_change_request);
        }

        // POST: package_change_request/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "pcr_id,pcr_packId,pcr_packName,pcr_packCharges,pcr_cusId,pcr_cusName,pcr_cusCardNo")] package_change_request package_change_request)
        {
            if (ModelState.IsValid)
            {
                customer customer = db.customers.Find(package_change_request.pcr_cusId);
                customer.cus_payment_left = package_change_request.pcr_packCharges + customer.cus_payment_left;
                customer.cus_package = package_change_request.pcr_packId;
                db.Entry(customer).State = EntityState.Modified;
                package_change_request = db.package_change_request.Find(package_change_request.pcr_id);
                db.package_change_request.Remove(package_change_request);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(package_change_request);
        }

        public ActionResult changeCustomer(int id,[Bind(Include = "cus_id,cus_name,cus_card_no,cus_package,cus_contact_no,cus_email,cus_password,cus_address,cus_ssd,cus_sed,cus_totalypaid,cus_payment_left")] customer customer)
        {
            if (ModelState.IsValid)
            {
                package_change_request package_change_request = db.package_change_request.Find(id);
                customer.cus_payment_left = package_change_request.pcr_packCharges + customer.cus_payment_left;
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.cus_package = new SelectList(db.packages, "pack_id", "pack_name", customer.cus_package);
            return View(customer);
        }

        public ActionResult Change(int id)
        {
            package_change_request package_change_request = db.package_change_request.Find(id);
            customer customer = new customer();
            customer.cus_package = package_change_request.pcr_packId;
            var charges = db.customers.Find(package_change_request.pcr_cusId);
            customer.cus_payment_left = charges.cus_payment_left + package_change_request.pcr_packCharges;
            customer.cus_id = Convert.ToInt32(charges.cus_id);
            db.Entry(customer).State = EntityState.Modified;
            db.SaveChanges();
            db.package_change_request.Remove(package_change_request);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        // GET: package_change_request/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            package_change_request package_change_request = db.package_change_request.Find(id);
            if (package_change_request == null)
            {
                return HttpNotFound();
            }
            return View(package_change_request);
        }

        // POST: package_change_request/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            package_change_request package_change_request = db.package_change_request.Find(id);
            db.package_change_request.Remove(package_change_request);
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
