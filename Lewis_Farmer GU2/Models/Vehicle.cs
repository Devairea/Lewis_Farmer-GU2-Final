using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using static Lewis_Farmer_GU2.Models.CustomDataAnnotations;

namespace Lewis_Farmer_GU2.Models
{
    public class Vehicle
    {
        [Key]
        [RegistrationNumber]
        public virtual string RegistrationNo { get; set; }

        [Required]
        public virtual string Make { get; set; }

        [Required]
        public virtual string Model { get; set; }

        [StringLength(4)]
        [Required]
        public virtual string Year { get; set; }

        [Display(Name = "Engine Size")]
        [Required]
        public virtual string EngineSize { get; set; }

        [Required]
        public virtual int Mileage { get; set; }

        //Foreign Keys
        [ForeignKey("User")]
        public virtual string Id { get; set; }
        public virtual Customer User { get; set; }

        public List<Booking> ListOfBookings { get; set; }

        public Vehicle()
        {
            ListOfBookings = new List<Booking>();
        }
    }
}