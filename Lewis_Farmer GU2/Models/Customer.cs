using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Lewis_Farmer_GU2.Models
{
    public class Customer : User
    {
        public virtual Vehicle CurrentVehicle { get; set; }

        public List<Vehicle> ListOfVehicles { get; set; }

        public Customer() : base ()
        {
            ListOfVehicles = new List<Vehicle>();
        }


    }
}