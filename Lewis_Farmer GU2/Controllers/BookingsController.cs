using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Lewis_Farmer_GU2.Models;
using Lewis_Farmer_GU2.ViewModels;
using Microsoft.AspNet.Identity;
using Stripe;

namespace Lewis_Farmer_GU2.Controllers
{
    [Authorize(Roles = "Customer, Admin, Mechanic, Manager")]
    public class BookingsController : Controller
    {
        private ApplicationDBContext db = new ApplicationDBContext();

        // GET: Bookings
        public ActionResult Index(string Criteria)
        {
            var bookings = new List<Booking>();
            if (User.IsInRole("Customer"))
            {
                string UserId = User.Identity.GetUserId();
                bookings = db.Bookings.Include(b => b.Customer).Include(b => b.Vehicle).Where(b => b.CustomerId.Equals(UserId)).ToList();
                return View(bookings);
            }
            else
            {
                //switch (Criteria)
                //{

                //    case "In Proggress":
                //        bookings = db.Bookings.Include(b => b.Customer).Include(b => b.Vehicle).Where(b => b.BookingStatus.Equals("In Proggress")).ToList();
                //        break;
                //    case "Awaiting Payment":
                //        bookings = db.Bookings.Include(b => b.Customer).Include(b => b.Vehicle).Where(b => b.BookingStatus.Equals("Awaiting Payment")).ToList();
                //        break;
                //    case "Suspended":
                //        bookings = db.Bookings.Include(b => b.Customer).Include(b => b.Vehicle).Where(b => b.BookingStatus.Equals("Suspended Booking")).ToList();
                //        break;
                //    case "Complete":
                //        bookings = db.Bookings.Include(b => b.Customer).Include(b => b.Vehicle).Where(b => b.BookingStatus.Equals("Complete")).ToList();
                //        break;
                //    case "None":
                //        bookings = db.Bookings.Include(b => b.Customer).Include(b => b.Vehicle).ToList();
                //        break;
                //    case null:
                //        bookings = db.Bookings.Include(b => b.Customer).Include(b => b.Vehicle).ToList();
                //        break;
                //    default:
                //        bookings = db.Bookings.Include(b => b.Customer).Include(b => b.Vehicle).ToList();
                //        break;
                //}
                bookings = db.Bookings.Include(b => b.Customer).Include(b => b.Vehicle).ToList();
            }
            
            return View(bookings);
        }

        // GET: Bookings/Details/5
        [Authorize(Roles = "Customer, Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            booking.Customer = (Models.Customer)db.Users.Find(booking.CustomerId);
            booking.Vehicle = db.Vehicles.Find(booking.RegistrationNo);
            booking.ListOfJobs = db.Jobs.Include(j => j.AssignedStaff).Include(j => j.Part).Include(j => j.Booking).Where(j => j.BookingId == booking.BookingId).ToList();
            if (booking == null)
            {
                return HttpNotFound();
            }
            if (booking.CustomerId != User.Identity.GetUserId())
            {
                return HttpNotFound();
            }
            var stripePublishKey = ConfigurationManager.AppSettings["StripeApiKey"];
            ViewBag.StripePublishKey = stripePublishKey;
            return View(booking);
        }

        [Authorize(Roles = "Customer")]
        public ActionResult CustomerBooking()
        {
            var bookings = new List<Booking>();
            string UserId = User.Identity.GetUserId();
            bookings = db.Bookings.Include(b => b.Customer).Include(b => b.Vehicle).Where(b => b.CustomerId.Equals(UserId)).Where(b => !b.BookingStatus.Equals("Complete") && !b.BookingStatus.Equals("Suspended")).ToList();
            if (bookings.Count == 0)
            {
                return RedirectToAction("Create", "Bookings");
            }
            else
            {
                Booking booking = bookings.First();
                return RedirectToAction("Details", new { id = booking.BookingId });
            }
        }


        // GET: Bookings/Create
        [Authorize(Roles = "Customer")]
        public ActionResult Create()
        {
            Models.Customer tmp = (Models.Customer)db.Users.Find(User.Identity.GetUserId());
            if (tmp.CurrentVehicle == null)
            {
                RedirectToAction("Create", "Vehicle");
            }
            string UserId = User.Identity.GetUserId();
            if (CheckCustomerActiveBookings(UserId))
            {
                return RedirectToAction("Index");
            }
            //ViewBag.CustomerId = new SelectList(db.Users, "Id", "FirstName");
            //ViewBag.RegistrationNo = new SelectList(db.Vehicles, "RegistrationNo", "Make");
            List<Job> jobs = db.Jobs.Where(p => p.JobName.Equals("Opening Meeting")).Where(p => p.DueDate >= DateTime.Now).ToList();
            ViewBag.OccupiedDates = jobs;
            return View();
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Customer")]
        public ActionResult Create([Bind(Include = "ReasonForBooking,StartDate")] CreateBookingViewModel model)
        {
            if (CheckCustomerActiveBookings(User.Identity.GetUserId()))
            {
                return RedirectToAction("Index");
            }
            Booking booking = new Booking();
            if (ModelState.IsValid)
            {                
                booking.ReasonForBooking = model.ReasonForBooking;
                booking.StartDate = model.StartDate;
                booking.CustomerId = User.Identity.GetUserId();
                booking.Customer = (Models.Customer)db.Users.Find(booking.CustomerId);
                booking.RegistrationNo = booking.Customer.CurrentVehicle.RegistrationNo;
                booking.Vehicle = booking.Customer.CurrentVehicle;
                booking.BookingStatus = "In Proggress";
                booking.PaymentMethod = "DBD";
                booking.PaymentComplete = false;
                db.Bookings.Add(booking);
                db.SaveChanges();
                List<Booking> listOfCustomerActiveBookings = db.Bookings.Include(b => b.Customer).Include(b => b.Vehicle).Where(b => b.CustomerId.Equals(booking.CustomerId)).Where(b => b.BookingStatus.Equals("In Proggress")).ToList();
                Booking completeBooking = listOfCustomerActiveBookings.First();
                Staff staff = (Staff)db.Users.Find("Taz.Loyal@DMotors.com");
                db.Jobs.Add(new Job(completeBooking, staff));
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.CustomerId = new SelectList(db.Users, "Id", "FirstName", booking.CustomerId);
            //ViewBag.RegistrationNo = new SelectList(db.Vehicles, "RegistrationNo", "Make", booking.RegistrationNo);
            List<Job> jobs = db.Jobs.Where(p => p.JobName.Equals("Opening Meeting")).Where(p => p.DueDate >= DateTime.Now).ToList();
            ViewBag.OccupiedDates = jobs;
            return View(model);
        }

        // GET: Bookings/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Booking booking = db.Bookings.Find(id);
            EditBookingViewModel bookingView = new EditBookingViewModel
            {
                Booking = booking,
                Customer = (Models.Customer)db.Users.Find(booking.CustomerId),
                Vehicle = db.Vehicles.Find(booking.RegistrationNo),
                ListOfJobs = GetBookingsJobs(id),
                JobExample = new Job()
            };

            if (bookingView.Booking == null)
                return HttpNotFound();
            return View(bookingView);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit([Bind(Include = "Booking, Customer, Vehicle")] EditBookingViewModel bookingView)
        {
            if (ModelState.IsValid)
            {
                bool JobsStatusValid = true;
                Booking booking = bookingView.Booking;
                bookingView.ListOfJobs = GetBookingsJobs(booking.BookingId);
                foreach (var item in bookingView.ListOfJobs)
                {
                    if (!item.JobStatus.Equals("Complete"))
                    {
                        JobsStatusValid = false;
                    }
                }
                if (!bookingView.Booking.BookingStatus.Equals("Complete"))
                {
                    db.Entry(booking).State = EntityState.Modified;
                    EmailService ES = new EmailService();
                    await ES.SendAsync(new IdentityMessage { Destination = booking.CustomerId, Subject = "Booking Status", Body = "Your Booking has now been Set to " +booking.BookingStatus+ "If you have any questions or need to take further action please check your bookings account online or contact us at Vineet.Dhillion@DMotors.com" });

                    db.SaveChanges();
                    return View(bookingView);
                }
                if (bookingView.Booking.BookingStatus.Equals("Complete") && JobsStatusValid)
                {
                    EmailService ES = new EmailService();
                    await ES.SendAsync(new IdentityMessage { Destination = booking.CustomerId, Subject = "Booking Status", Body = "Your Booking has now been Set to Complete Thanks for booking with us"});
                    booking.CompletionDate = DateTime.Now.Date;
                    db.Entry(booking).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else if (bookingView.Booking.BookingStatus.Equals("Complete") && !JobsStatusValid)
                {
                    return RedirectToAction("Edit", new { id = booking.BookingId});
                }
                
                
                bookingView.ListOfJobs = GetBookingsJobs(booking.BookingId);
                return View(bookingView);
            }
            
            return View(bookingView);
        }

        /// <summary>  
        /// Print employee details  
        /// </summary>  
        /// <param name="id"></param>  
        /// <returns></returns>  
        public ActionResult PrintBookingPDF(int id)
        {
            var report = new Rotativa.ActionAsPdf("Index", "Home");
            return report;
        }

        // GET: Bookings/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Booking booking = db.Bookings.Find(id);
            booking.BookingStatus = "Suspended";
            db.Entry(booking).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Complete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            foreach(var item in booking.ListOfJobs)
            {
                if (!item.JobStatus.Equals("Complete"))
                {
                    return RedirectToAction("BookingCompletionFailure", new { Dispute = "All Jobs Must Be Completed Before Confirming A Bookings Completion" });
                }
            }
            if (booking.PaymentComplete == false)
            {
                return RedirectToAction("BookingCompletionFailure", new { Dispute = "Payment Must Be Complete Before Confirming A Bookings Completion " });
            }
            booking.CompletionDate = DateTime.Now;
            booking.BookingStatus = "Complete";
            return RedirectToAction("Details", new { id });
        }

        [Authorize(Roles = "Admin")]
        public ActionResult BookingCompletionFailure(string Dispute)
        {
            return View(Dispute);
        }

            /// <summary>
            /// Method to get a list of jobs relating to a booking
            /// </summary>
            /// <param name="bookingId">The Booking Id to check against the list of jobs</param>
            /// <returns>A List of Jobs, This list is usually a list of jobs matching the bookingId.
            /// <para>If the Id is null returns a list of all jobs, If there are no jobs returns an empty list of Jobs</para></returns>
            private List<Job> GetBookingsJobs(int? bookingId)
        {
            if (bookingId == null)
            {
                return db.Jobs.Include(c => c.AssignedStaff).Include(p => p.Part).ToList();
            }
            List<Job> jobs = db.Jobs.Include(c => c.AssignedStaff).Include(p => p.Part).Where(c => c.BookingId == bookingId).ToList();
            if (jobs == null)
            {
                return new List<Job>();
            }
            else
            {
                return jobs;
            }
        }

        [Authorize(Roles = "Customer")]
        public ActionResult AllPlacesBooked()
        {
            return View();
        }

        public ActionResult GetReasonPartial(string value)
        {
            // do calculations whatever you need to do
            // instantiate Model object
            BookingReasonPartialViewModel model = new BookingReasonPartialViewModel(value);
            return PartialView("_BookingReasonPartial", model);
        }

        public bool CheckCustomerActiveBookings(string UserId)
        {
            List<Booking> listOfCustomerActiveBookings = db.Bookings.Include(b => b.Customer).Include(b => b.Vehicle).Where(b => b.CustomerId.Equals(UserId)).Where(b => b.BookingStatus.Equals("In Proggress")).ToList();
            if (listOfCustomerActiveBookings.Count != 0)
                return true;
            else
                return false;
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
