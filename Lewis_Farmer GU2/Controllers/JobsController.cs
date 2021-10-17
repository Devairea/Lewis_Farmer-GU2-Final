using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Lewis_Farmer_GU2.Models
{
    [Authorize(Roles = "Admin, Manager, Mechanic")]
    public class JobsController : Controller
    {
        private ApplicationDBContext db = new ApplicationDBContext();

        // GET: Jobs
        [Authorize(Roles = "Manager")]
        public ActionResult Index()
        {
            var jobs = db.Jobs.Include(j => j.AssignedStaff).Include(j => j.Booking).Include(j => j.Part).Where(j => j.JobStatus == "Unassigned");
            return View(jobs.ToList());
        }

        // GET: Jobs/Details/5
        public ActionResult Details(string id)
        {
            if (User.IsInRole("Mechanic"))
            {
                string UserId = User.Identity.GetUserId();
                Staff Mechanic = (Staff)db.Users.Find(UserId);
                if (Mechanic.CurrentJob != null)
                {
                    Job assignedJob = Mechanic.CurrentJob;
                    assignedJob = db.Jobs.Find(assignedJob.JobId);
                    return View(assignedJob);
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = db.Jobs.Find(id);
            job.Booking = db.Bookings.Find(job.BookingId);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // GET: Jobs/Create
        // GET: Jobs/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<Booking> JobBooking = new List<Booking>();
            JobBooking.Add(db.Bookings.Find(id));
            List<Part> parts = new List<Part>();
            parts = db.Parts.Where(p => p.StockLevel > 0).ToList();
            ViewBag.BookingId = new SelectList(JobBooking, "BookingId", "Customer.FirstName");
            ViewBag.PartId = new SelectList(parts, "PartId", "PartName");
            ViewBag.ListOfParts = db.Parts.Include(p => p.PartSupplier);
            return View();
        }

        // POST: Jobs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "JobId,JobName,JobDescription,JobCost,JobStatus,Memo,DueDate,DateCompleted,UserId,BookingId,PartId")] Job job)
        {
            job.JobId = job.JobName + job.BookingId.ToString();
            job.JobStatus = job.JobStatus = "Unassigned";
            if (ModelState.IsValid)
            {
                db.Jobs.Add(job);
                db.SaveChanges();
                return RedirectToAction("Details", "Jobs", new { id =  job.JobId});
            }

            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", job.UserId);
            ViewBag.BookingId = new SelectList(db.Bookings, "BookingId", "ReasonForBooking", job.BookingId);
            ViewBag.PartId = new SelectList(db.Parts, "PartId", "PartName", job.PartId);
            ViewBag.ListOfParts = db.Parts.Include(p => p.PartSupplier);
            return View(job);
        }

        // GET: Jobs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = db.Jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            List<User> users = db.Users.ToList();
            List<Staff> staffMembers = new List<Staff>();
            List<Staff> availableStaff = new List<Staff>();
            foreach (var item in users)
            {
                try
                {
                    staffMembers.Add((Staff)item);
                }
                catch
                {

                }
            }
            foreach (var item in staffMembers)
            {
                if (item.CurrentJob == null)
                {
                    availableStaff.Add(item);
                }
            }
            ViewBag.UserId = new SelectList(availableStaff, "Id", "FirstName", job.UserId);
            ViewBag.BookingId = new SelectList(db.Bookings, "BookingId", "ReasonForBooking", job.BookingId);
            ViewBag.PartId = new SelectList(db.Parts, "PartId", "PartName", job.PartId);
            ViewBag.ListOfParts = db.Parts.Include(p => p.PartSupplier);
            return View(job);
        }

        // POST: Jobs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "JobId,JobName,JobDescription,JobCost,JobStatus,Memo,DueDate,DateCompleted,UserId,BookingId,PartId")] Job job)
        {           
            if (ModelState.IsValid)
            {
                if (job.UserId != null)
                {
                    if (job.JobStatus.Equals("Unassigned"))
                    {
                        job.JobStatus = "In Proggress";
                    }
                    Staff staff = (Staff)db.Users.Find(job.UserId);
                    
                    staff.CurrentJob = job;
                    //staff.ListOfJobs.Add(job);
                    db.Entry(job).State = EntityState.Modified;
                    db.SaveChanges();
                    db.Entry(staff).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Details", new {id = job.JobId });
                }
                else
                {
                    db.Entry(job).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                
            }
            List<User> users = db.Users.ToList();
            List<Staff> staffMembers = new List<Staff>();
            List<Staff> availableStaff = new List<Staff>();
            foreach (var item in users)
            {
                try
                {
                    staffMembers.Add((Staff)item);
                }
                catch
                {

                }
            }
            foreach (var item in staffMembers)
            {
                if (item.CurrentJob == null)
                {
                    availableStaff.Add(item);
                }
            }
            ViewBag.UserId = new SelectList(availableStaff, "Id", "FirstName", job.UserId);
            ViewBag.BookingId = new SelectList(db.Bookings, "BookingId", "ReasonForBooking", job.BookingId);
            ViewBag.PartId = new SelectList(db.Parts, "PartId", "PartName", job.PartId);
            ViewBag.ListOfParts = db.Parts.Include(p => p.PartSupplier);
            return View(job);
        }

        // GET: Jobs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = db.Jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // POST: Jobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Job job = db.Jobs.Find(id);
            db.Jobs.Remove(job);
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
