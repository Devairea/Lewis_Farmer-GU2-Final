using Lewis_Farmer_GU2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lewis_Farmer_GU2.ViewModels
{
    public class EditBookingViewModel
    {
        public Booking Booking { get; set; }

        public Customer Customer { get; set; }

        public Vehicle Vehicle { get; set; }

        public List<Job> ListOfJobs { get; set; }

        public Job JobExample { get; set; }

        //public string SelectedOptionIso3 { get; set; }
        //public List<SelectListItem> BookingStatusOptions { get; set; }


    }
}