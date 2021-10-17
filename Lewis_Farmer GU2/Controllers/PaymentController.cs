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
    public class PaymentController : Controller
    {
        private ApplicationDBContext db = new ApplicationDBContext();

        [Authorize(Roles = "Customer")]
        public async Task<ActionResult> Charge(string stripeEmail, string stripeToken)
        {
            var customers = new CustomerService();
            var charges = new ChargeService();

            var customer = customers.Create(new CustomerCreateOptions
            {
                Email = stripeEmail,
                SourceToken = stripeToken
            });
            string UserId = User.Identity.GetUserId();
            List<Booking> techBookings = db.Bookings.Include(c => c.Customer).Include(b => b.Vehicle).Include(b => b.ListOfJobs).Where(b => b.CustomerId.Equals(UserId)).Where(b => b.BookingStatus.Equals("Awaiting Payment")).ToList();
            Booking booking = techBookings.First();

            var charge = charges.Create(new ChargeCreateOptions
            {
                Amount = Convert.ToInt64(booking.CalculateCostUse()),
                Description = "Booking Payment Charge",
                Currency = "GBP",
                CustomerId = customer.Id,
                ReceiptEmail = customer.Email,
            });

            EmailService ES = new EmailService();

            if (booking.CustomerId.Equals(stripeEmail))
            {
                await ES.SendAsync(new IdentityMessage { Destination = booking.CustomerId, Subject = "Booking Status", Body = "We have recieved confirmation of you paying for your booking, Thank you for booking with us" });
            }
            else
            {
                await ES.SendAsync(new IdentityMessage { Destination = stripeEmail, Subject = "Booking Status", Body = "We have recieved confirmation of you paying for your booking, Thank you for booking with us" });
                await ES.SendAsync(new IdentityMessage { Destination = booking.CustomerId, Subject = "Booking Status", Body = "We have recieved confirmation of you paying for your booking, Thank you for booking with us" });
            }

            booking.BookingStatus = "Complete";
            booking.PaymentComplete = true;
            booking.PaymentMethod = "Stripe";
            booking.CompletionDate = DateTime.Now;
            db.Entry(booking).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Details", "Bookings", new {id =booking.BookingId });
        }
    }
}