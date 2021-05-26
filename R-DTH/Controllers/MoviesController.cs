using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using R_DTH.Models;
using System.IO;

namespace R_DTH.Controllers
{
    public class MoviesController : Controller
    {
        private rdthEntities db = new rdthEntities();

        // GET: Movies
        public ActionResult Index()
        {
            return View(db.Movies.ToList());
        }

        public ActionResult Movie()
        {
            return View(db.Movies.ToList());
        }

        // GET: Movies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movy movy = db.Movies.Find(id);
            if (movy == null)
            {
                return HttpNotFound();
            }
            return View(movy);
        }

        // GET: Movies/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "movie_id,movie_name,movie_image,MovieImage,movie_language,movie_availablity,movie_channelNo,movie_duration,movie_timing1,movie_timing2,movie_charges")] Movy movy)
        {
            if (ModelState.IsValid)
            {
                string fileName = Path.GetFileName(movy.MovieImage.FileName);
                movy.movie_image = fileName;
                fileName = Path.Combine(Server.MapPath("~/Content/AdminStuff/dist/img/"), fileName);
                movy.MovieImage.SaveAs(fileName);
                db.Movies.Add(movy);
                db.SaveChanges();
                ModelState.Clear();
                return RedirectToAction("Index");
            }

            return View(movy);
        }

        // GET: Movies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movy movy = db.Movies.Find(id);
            if (movy == null)
            {
                return HttpNotFound();
            }
            return View(movy);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "movie_id,movie_name,movie_image,MovieImage,movie_language,movie_availablity,movie_channelNo,movie_duration,movie_timing1,movie_timing2,movie_charges")] Movy movy)
        {
            if (ModelState.IsValid)
            {
                string fileName = Path.GetFileName(movy.MovieImage.FileName);
                movy.movie_image = fileName;
                fileName = Path.Combine(Server.MapPath("~/Content/AdminStuff/dist/img/"), fileName);
                movy.MovieImage.SaveAs(fileName);
                db.Entry(movy).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(movy);
        }

        // GET: Movies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movy movy = db.Movies.Find(id);
            if (movy == null)
            {
                return HttpNotFound();
            }
            return View(movy);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Movy movy = db.Movies.Find(id);
            db.Movies.Remove(movy);
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
