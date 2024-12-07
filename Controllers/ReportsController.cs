using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FinalProjectJunyi.Data;
using FinalProjectJunyi.Models;

namespace FinalProjectJunyi.Controllers
{
    public class ReportsController : Controller
    {
        private ProjectDbContext db = new ProjectDbContext();

        // GET: Reports
        public ActionResult Index()
        {
            var reports = db.Reports.Include(r => r.PropertyManager).Include(r => r.PropertyOwner);
            return View(reports.ToList());
        }

        // GET: Reports/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Report report = db.Reports.Find(id);
            if (report == null)
            {
                return HttpNotFound();
            }
            return View(report);
        }

        // GET: Reports/Create
        public ActionResult Create()
        {
            ViewBag.PropertyManagerId = new SelectList(db.Users, "UserId", "FullName");
            ViewBag.PropertyOwnerId = new SelectList(db.Users, "UserId", "FullName");
            return View();
        }

        // POST: Reports/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ReportId,EventDescription,PropertyManagerId,PropertyOwnerId,ReportedAt")] Report report)
        {
            if (ModelState.IsValid)
            {
                db.Reports.Add(report);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PropertyManagerId = new SelectList(db.Users, "UserId", "FullName", report.PropertyManagerId);
            ViewBag.PropertyOwnerId = new SelectList(db.Users, "UserId", "FullName", report.PropertyOwnerId);
            return View(report);
        }

        // GET: Reports/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Report report = db.Reports.Find(id);
            if (report == null)
            {
                return HttpNotFound();
            }
            ViewBag.PropertyManagerId = new SelectList(db.Users, "UserId", "FullName", report.PropertyManagerId);
            ViewBag.PropertyOwnerId = new SelectList(db.Users, "UserId", "FullName", report.PropertyOwnerId);
            return View(report);
        }

        // POST: Reports/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ReportId,EventDescription,PropertyManagerId,PropertyOwnerId,ReportedAt")] Report report)
        {
            if (ModelState.IsValid)
            {
                db.Entry(report).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PropertyManagerId = new SelectList(db.Users, "UserId", "FullName", report.PropertyManagerId);
            ViewBag.PropertyOwnerId = new SelectList(db.Users, "UserId", "FullName", report.PropertyOwnerId);
            return View(report);
        }

        // GET: Reports/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Report report = db.Reports.Find(id);
            if (report == null)
            {
                return HttpNotFound();
            }
            return View(report);
        }

        // POST: Reports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Report report = db.Reports.Find(id);
            db.Reports.Remove(report);
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
