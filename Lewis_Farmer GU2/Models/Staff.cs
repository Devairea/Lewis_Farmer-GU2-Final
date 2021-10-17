using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Lewis_Farmer_GU2.Models
{
    public class Staff : User
    {
        /// <summary>
        /// The Staff members Date of Birth 
        /// </summary>
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public virtual DateTime DateOfBirth { get; set; }

        //ForeignKeys
        public virtual Job CurrentJob { get; set; }

        /// <summary>
        /// List of all jobs this staff member has been assigned
        /// </summary>
        public virtual List<Job> ListOfJobs { get; set; }

        //Constructors

        public Staff() : base()
        {
            ListOfJobs = new List<Job>();
        }
    }
}