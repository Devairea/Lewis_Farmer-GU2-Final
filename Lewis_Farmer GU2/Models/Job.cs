using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Lewis_Farmer_GU2.Models
{
    public class Job
    {
        [Key]
        public virtual string JobId { get; set; }

        [Display(Name = "Job")]
        public virtual string JobName { get; set; }

        [Display(Name = "Description")]
        public virtual string JobDescription { get; set; }

        [Display(Name = "Cost")]
        [DataType(DataType.Currency)]
        public virtual double JobCost { get; set; }

        [Display(Name = "Status")]
        [MaxLength(15)]
        public virtual string JobStatus { get; set; }

        [MaxLength(1000)]
        public virtual string Memo { get; set; }

        [DataType(DataType.Date)]
        public virtual DateTime DueDate { get; set; }

        [DataType(DataType.Date)]
        public virtual DateTime? DateCompleted { get; set; }

        [ForeignKey("AssignedStaff")]
        public virtual string UserId { get; set; }
        [Display(Name = "Mechanic")]
        public virtual Staff AssignedStaff { get; set; }

        [ForeignKey("Booking")]
        public int BookingId { get; set; }
        public Booking Booking { get; set; }

        [ForeignKey("Part")]
        public string PartId { get; set; }
        public Part Part { get; set; }

        public Job()
        {

        }

        public Job(Booking booking, Staff staff)
        {
            JobId = "OpeningMeeting" + booking.BookingId.ToString();
            JobName = "Opening Meeting";
            JobDescription = "Initial Client-Admin Meeting";
            JobCost = GenerateDefaultCost(booking.ReasonForBooking);
            JobStatus = "In Proggress";
            DueDate = booking.StartDate;
            UserId = staff.Id;
            AssignedStaff = staff;
            BookingId = booking.BookingId;
            Booking = booking;
        }

        private double GenerateDefaultCost(string type)
        {
            if (type.Equals("MOT"))
            {
                return 50.00;
            }
            else if (type.Equals("Repair"))
            {
                return 30.00;
            }
            else if (type.Equals("Customisation"))
            {
                return 20.00;
            }
            else if (type.Equals("Consultation"))
            {
                return 20.00;
            }
            else if (type.Equals("Other"))
            {
                return 20.00;
            }
            return 0;
        }
    }
}