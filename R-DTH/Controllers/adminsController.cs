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
    public class adminsController : Controller
    {
        private rdthEntities db = new rdthEntities();

        // GET: admins
        public ActionResult Index()
        {
            return View(db.admins.ToList());
        }

        // GET: admins/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            admin admin = db.admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // GET: admins/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: admins/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "admin_id,admin_name,admin_contact_no,admin_email,admin_password,admin_address")] admin admin)
        {
            var user = db.admins.FirstOrDefault(u=>u.admin_email == admin.admin_email && u.admin_password == admin.admin_password);
            if(user != null)
            {
                Session["Id"] = user.admin_id.ToString();
                Session["Name"] = user.admin_name.ToString();
                return RedirectToAction("Index");
            }
            else
            {
                admin.LoginErrorMessage = "Invalid Username & Password!";
                return View(admin);
            }            
        }

        public ActionResult logOut()
        {
            Session.Abandon();
            return RedirectToAction("Create","admins");
        }

        //// GET: admins/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: admins/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "admin_id,admin_name,admin_contact_no,admin_email,admin_password,admin_address")] admin admin)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.admins.Add(admin);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(admin);
        //}

        // GET: admins/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            admin admin = db.admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // POST: admins/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "admin_id,admin_name,admin_contact_no,admin_email,admin_password,admin_address")] admin admin)
        {
            if (ModelState.IsValid)
            {
                db.Entry(admin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(admin);
        }

        // GET: admins/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            admin admin = db.admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // POST: admins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            admin admin = db.admins.Find(id);
            db.admins.Remove(admin);
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
