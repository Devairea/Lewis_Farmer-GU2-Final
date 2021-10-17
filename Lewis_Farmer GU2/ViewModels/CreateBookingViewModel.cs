using Lewis_Farmer_GU2.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using static Lewis_Farmer_GU2.Models.CustomDataAnnotations;

namespace Lewis_Farmer_GU2.ViewModels
{
    public class CreateBookingViewModel
    {
        [Display(Name = "Reason For Booking")]
        [MaxLength(200)]
        public string ReasonForBooking { get; set; }

        [Display(Name = "Date Started")]
        [DataType(DataType.Date)]
        [CurrentDate(ErrorMessage = "You Must Enter a Date In The Future")]
        [UnoccupiedDate]
        public DateTime StartDate { get; set; }
    }
}