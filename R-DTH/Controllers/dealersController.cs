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
    public class dealersController : Controller
    {
        private rdthEntities db = new rdthEntities();

        // GET: dealers
        public ActionResult Index()
        {
            return View(db.dealers.ToList());
        }

        public ActionResult Dealers()
        {
            return View(db.dealers.ToList());
        }
        // GET: dealers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dealer dealer = db.dealers.Find(id);
            if (dealer == null)
            {
                return HttpNotFound();
            }
            return View(dealer);
        }

        // GET: dealers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: dealers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "dealer_id,dealer_name,dealer_contact_no,dealer_email,dealer_password,dealer_address,dealer_totalypaid,dealer_payment_left")] dealer dealer)
        {
            if (ModelState.IsValid)
            {
                db.dealers.Add(dealer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dealer);
        }

        public ActionResult DealerLogin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DealerLogin([Bind(Include = "dealer_id,dealer_name,dealer_contact_no,dealer_email,dealer_password,dealer_address,dealer_totalypaid,dealer_payment_left")] dealer dealer)
        {
            if (ModelState.IsValid)
            {
                var user = db.dealers.FirstOrDefault(u => u.dealer_email == dealer.dealer_email && u.dealer_password == dealer.dealer_password);
                if (user != null)
                {
                    Session["DealerId"] = user.dealer_id.ToString();
                    Session["DealerName"] = user.dealer_name.ToString();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    dealer.LoginErrorMessage = "Invalid Username & Password!";
                    return View(dealer);
                }

            }

            return View();
        }


        public ActionResult logOut()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }


        // GET: dealers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dealer dealer = db.dealers.Find(id);
            if (dealer == null)
            {
                return HttpNotFound();
            }
            return View(dealer);
        }

        // POST: dealers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "dealer_id,dealer_name,dealer_contact_no,dealer_email,dealer_password,dealer_address,dealer_totalypaid,dealer_payment_left")] dealer dealer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dealer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dealer);
        }

        // GET: dealers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dealer dealer = db.dealers.Find(id);
            if (dealer == null)
            {
                return HttpNotFound();
            }
            return View(dealer);
        }

        // POST: dealers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            dealer dealer = db.dealers.Find(id);
            db.dealers.Remove(dealer);
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
