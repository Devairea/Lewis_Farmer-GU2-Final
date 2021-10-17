using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using static Lewis_Farmer_GU2.Models.CustomDataAnnotations;

namespace Lewis_Farmer_GU2.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        [Display(Name = "Reason For Booking")]
        [MaxLength(200, ErrorMessage = "Your Reason For making this booking must be less than 200 Characters")]
        public string ReasonForBooking { get; set; }

        [Display(Name = "Date Started")]
        [DataType(DataType.Date)]
        //[CurrentDate(ErrorMessage = "Your Booking must be made after the current date or later")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Date Completed")]
        [DataType(DataType.Date)]
        //[CurrentDate(ErrorMessage = "Your Booking must be completed after the current date or later")]
        public DateTime? CompletionDate { get; set; }

        [Display(Name = "Status")]
        public string BookingStatus { get; set; }

        [Display(Name = "Payment Method")]
        public string PaymentMethod { get; set; }

        [Display(Name = "Payment Complete?")]
        public bool PaymentComplete { get; set; }

        [ForeignKey("Customer")]
        public string CustomerId { get; set; }
        public Customer Customer { get; set; }

        [ForeignKey("Vehicle")]
        public string RegistrationNo { get; set; }
        public Vehicle Vehicle { get; set; }

        public List<Job> ListOfJobs { get; set; }

        public Booking()
        {
            ListOfJobs = new List<Job>();
        }

        public List<string> StatusOptions()
        {
            List<string> tmp = new List<string>();
            tmp.Add("In Proggress");
            tmp.Add("Awaiting Payment");
            tmp.Add("Suspended Booking");
            tmp.Add("Complete");
            return tmp;
        }

        public double CalculateCostDisplay()
        {
            double cost = 0.00;
            if (ListOfJobs == null || ListOfJobs == new List<Job>())
            {
                return cost;
            }
            else
            {
                foreach(Job item in ListOfJobs)
                {
                    cost = cost + item.JobCost;
                }
                return cost;
            }            
        }

        public int CalculateCostUse()
        {
            int intCost = 0;
            double doubleCost = 0.00;
            if (ListOfJobs == null || ListOfJobs == new List<Job>())
            {
                return intCost;
            }
            else
            {
                foreach (Job item in ListOfJobs)
                {
                    doubleCost = doubleCost + item.JobCost;
                }
                doubleCost = doubleCost * 100.00;
                intCost = int.Parse(doubleCost.ToString());
                return intCost;
            }
        }
    }
}