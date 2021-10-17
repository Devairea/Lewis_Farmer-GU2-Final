using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Lewis_Farmer_GU2.Models;
using Microsoft.AspNet.Identity;

namespace Lewis_Farmer_GU2.Controllers
{
    [Authorize(Roles = "Customer")]
    public class VehiclesController : Controller
    {
        private ApplicationDBContext db = new ApplicationDBContext();

        // GET: Vehicles
        public ActionResult Index()
        {
            var vehicles = db.Vehicles.Include(v => v.User);
            return View(vehicles.ToList());
        }

        // GET: Vehicles/Details/5
        
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicle vehicle = db.Vehicles.Find(id);
            if (vehicle == null)
            {
                return HttpNotFound();
            }
            return View(vehicle);
        }

        // GET: Vehicles/Create
        
        public ActionResult Create()
        {
            string UserId = User.Identity.GetUserId();
            List<Booking> bookings = db.Bookings.Where(b => b.CustomerId.Equals(UserId)).Where(b => !b.BookingStatus.Equals("Complete") && !b.BookingStatus.Equals("Suspended")).ToList();
            if (bookings.Count > 0)
            {
                return RedirectToAction("Details", "Bookings", new { id = bookings.First().BookingId});
            }
            return View();
        }

        // POST: Vehicles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Customer")]
        public ActionResult Create([Bind(Include = "RegistrationNo,Make,Model,Year,EngineSize,Mileage,Id")] Vehicle vehicle)
        {
            vehicle.Id = User.Identity.GetUserId();
            vehicle.RegistrationNo = vehicle.RegistrationNo.ToUpper();
            if (ModelState.IsValid)
            {
                //Adds the new vehicle to the database
                db.Vehicles.Add(vehicle);
                //Updates the Customers current vehicle
                Customer customer = (Customer)db.Users.Find(vehicle.Id);
                customer.CurrentVehicle = vehicle;
                customer.ListOfVehicles.Add(vehicle);
                db.Entry(customer).State = EntityState.Modified;
                //Saves the changes to the Customer and the Vehicle
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Id = new SelectList(db.Users, "Id", "FirstName", vehicle.Id);
            return View(vehicle);
        }

        // GET: Vehicles/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicle vehicle = db.Vehicles.Find(id);
            if (vehicle == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = new SelectList(db.Users, "Id", "FirstName", vehicle.Id);
            return View(vehicle);
        }

        // POST: Vehicles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RegistrationNo,Make,Model,Year,EngineSize,Mileage,Id")] Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                //Updates the Vehicle's Information
                db.Entry(vehicle).State = EntityState.Modified;
                //Updates the customer's current vehicle infromation
                Customer customer = (Customer)db.Users.Find(vehicle.Id);
                customer.CurrentVehicle = vehicle;
                db.Entry(customer).State = EntityState.Modified;
                //Saves the changes to the Customer and the Vehicle                
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id = new SelectList(db.Users, "Id", "FirstName", vehicle.Id);
            return View(vehicle);
        }

        // GET: Vehicles/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicle vehicle = db.Vehicles.Find(id);
            if (vehicle == null)
            {
                return HttpNotFound();
            }
            return View(vehicle);
        }

        // POST: Vehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Vehicle vehicle = db.Vehicles.Find(id);
            db.Vehicles.Remove(vehicle);
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
