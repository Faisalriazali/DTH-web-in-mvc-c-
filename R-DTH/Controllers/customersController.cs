using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using R_DTH.Models;
using System.Data.SqlClient;
using System.Data.Sql;

namespace R_DTH.Controllers
{
    public class customersController : Controller
    {
        private rdthEntities db = new rdthEntities();

        // GET: customers
        public ActionResult Index()
        {
            var customers = db.customers.Include(c => c.package);
            return View(customers.ToList());
        }

        // GET: customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            customer customer = db.customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: customers/Create
        public ActionResult Create()
        {
            ViewBag.cus_package = new SelectList(db.packages, "pack_id", "pack_name");
            return View();
        }

        // POST: customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "cus_id,cus_name,cus_card_no,cus_package,cus_contact_no,cus_email,cus_password,cus_address,cus_ssd,cus_sed,cus_totalypaid,cus_payment_left")] customer customer)
        {
            if (ModelState.IsValid)
            {
                var user = db.customers.FirstOrDefault(u => u.cus_card_no == customer.cus_card_no || u.cus_email == customer.cus_email);
                if(user!=null)
                {
                    customer.LoginErrorMessage = "Card No or Email already exsist!";
                    ViewBag.cus_package = new SelectList(db.packages, "pack_id", "pack_name", customer.cus_package);
                    return View(customer);
                }
                else
                {
                    var card = db.customer_card_number.FirstOrDefault(u => u.ccn_numbers == customer.cus_card_no);
                    if(card!=null)
                    {
                        var lis = db.packages.Where(c => c.pack_id == customer.cus_package).FirstOrDefault();
                        customer.cus_payment_left = lis.pack_charges;
                        DateTime dt = DateTime.Today;
                        customer.cus_ssd = dt;
                        customer.cus_sed = dt.AddDays(30);
                        db.customers.Add(customer);
                        db.SaveChanges();
                        return RedirectToAction("Index", "customers");
                    }
                    else
                    {
                        customer.LoginErrorMessage = "Card No not Found!";
                        ViewBag.cus_package = new SelectList(db.packages, "pack_id", "pack_name", customer.cus_package);
                        return View(customer);
                    }
                }
            }
            
            ViewBag.cus_package = new SelectList(db.packages, "pack_id", "pack_name", customer.cus_package);
            return View(customer);
        }


        public ActionResult userRegister()
        {
            ViewBag.cus_package = new SelectList(db.packages, "pack_id", "pack_name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult userRegister([Bind(Include = "cus_id,cus_name,cus_card_no,cus_package,cus_contact_no,cus_email,cus_password,cus_address,cus_ssd,cus_sed,cus_totalypaid,cus_payment_left")] customer customer)
        {
            if (ModelState.IsValid)
            {
                var user = db.customers.FirstOrDefault(u => u.cus_card_no == customer.cus_card_no || u.cus_email == customer.cus_email);
                if (user != null)
                {
                    customer.LoginErrorMessage = "Card No or Email already exsist!";
                    ViewBag.cus_package = new SelectList(db.packages, "pack_id", "pack_name", customer.cus_package);
                    return View(customer);
                }
                else
                {
                    var card = db.customer_card_number.FirstOrDefault(u => u.ccn_numbers == customer.cus_card_no);
                    if (card != null)
                    {
                        var lis = db.packages.Where(c => c.pack_id == customer.cus_package).FirstOrDefault();
                        customer.cus_payment_left = lis.pack_charges;
                        DateTime dt = DateTime.Today;
                        customer.cus_ssd = dt;
                        customer.cus_sed = dt.AddDays(30);
                        db.customers.Add(customer);
                        db.SaveChanges();
                        return RedirectToAction("CustomerLogin", "customers");
                    }
                    else
                    {
                        customer.LoginErrorMessage = "Card No not Found!";
                        ViewBag.cus_package = new SelectList(db.packages, "pack_id", "pack_name", customer.cus_package);
                        return View(customer);
                    }
                }
            }

            ViewBag.cus_package = new SelectList(db.packages, "pack_id", "pack_name", customer.cus_package);
            return View(customer);
        }

        public ActionResult CustomerLogin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CustomerLogin([Bind(Include = "cus_id,cus_name,cus_card_no,cus_package,cus_contact_no,cus_email,cus_password,cus_address,cus_ssd,cus_sed,cus_totalypaid,cus_payment_left")] customer customer)
        {
            if (ModelState.IsValid)
            {
                var user = db.customers.FirstOrDefault(u => u.cus_email == customer.cus_email && u.cus_password == customer.cus_password);
                if (user != null)
                {
                    Session["CustomerId"] = user.cus_id.ToString();
                    Session["CustomerName"] = user.cus_name.ToString();
                    Session["CustomerCardNo"] = user.cus_card_no.ToString();
                    return RedirectToAction("Index","Home");
                }
                else
                {
                    customer.LoginErrorMessage = "Invalid Username & Password!";
                    return View(customer);
                }

            }

            return View();
        }


        public ActionResult logOut()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetPackageCharges(package id)
        {
            Object charges;
            using (rdthEntities dc=new rdthEntities())
            {
               charges = dc.packages.Where(abc => abc.pack_id.Equals(id.pack_id));
            }
            return new JsonResult { Data = charges, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        // GET: customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            customer customer = db.customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            ViewBag.cus_package = new SelectList(db.packages, "pack_id", "pack_name", customer.cus_package);
            return View(customer);
        }

        // POST: customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "cus_id,cus_name,cus_card_no,cus_package,cus_contact_no,cus_email,cus_password,cus_address,cus_ssd,cus_sed,cus_totalypaid,cus_payment_left")] customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.cus_package = new SelectList(db.packages, "pack_id", "pack_name", customer.cus_package);
            return View(customer);
        }

        // GET: customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            customer customer = db.customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            customer customer = db.customers.Find(id);
            db.customers.Remove(customer);
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
