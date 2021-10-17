using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Lewis_Farmer_GU2.Models
{
    // You can add profile data for the user by adding more properties to your User class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    /// <summary>
    /// <para>This Abstract Class functions as a bluepoint for the two types of users having all shared fields created here</para>
    /// <para>Inherits from the IdentityUser Class as a part of Microsoft.AspNet.Identity comes with a large suite</para>
    /// <para>of methods and properties that perform large aspects of the login sign up and roles system, and allows for many Compatibilities</para>
    /// </summary>
    public abstract class User : IdentityUser
    {
        /// <summary>
        /// A variable that stores an account suspended status, if they are fired banned or otherwise made inactive
        /// <para>This will be checked on login and any </para>
        /// </summary>
        [Required]
        public virtual bool? IsSuspended { get; set; }

        /// <summary>
        /// A Variable that stores the date a user was regsitered
        /// </summary>
        [Required]
        [Display(Name = "Registration Date")]
        public virtual DateTime RegisterDate { get; set; }

        /// <summary>
        /// A variable to store the user's first name
        /// </summary>
        [MaxLength(100)]
        [MinLength(1)]
        public virtual string FirstName { get; set; }

        /// <summary>
        /// A variable to store the user's last name
        /// </summary>
        [MaxLength(100)]
        [MinLength(1)]
        [Display(Name = "Last Name(s)")]
        public virtual string LastName { get; set; }

        /// <summary>
        /// A Variable that stores the firstline of a users address (Refers to their street and number)
        /// </summary>
        [MaxLength(100)]
        [MinLength(1)]
        public virtual string Street { get; set; }

        /// <summary>
        /// A variable that stores the second line of a user's address (Refers to the city or town they live in)
        /// </summary>
        [MaxLength(100)]
        [MinLength(1)]
        public virtual string Town { get; set; }

        /// <summary>
        /// A variable that stores the Postcode of a user's address
        /// </summary>
        [MaxLength(9)]
        [MinLength(6)]
        public virtual string PostCode { get; set; }

        /// <summary>
        /// The ApplicationUserManager allows for some methods interacting with the user in the database
        /// <para>For the functions of this class it it exclusively used to get the Users role</para>
        /// </summary>
        [NotMapped]
        private ApplicationUserManager userManager;


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        /// <summary>
        /// A Method for returning the current role of the user
        /// </summary>
        [NotMapped]
        public string CurrentRole
        {
            get
            {
                if (userManager == null)
                {
                    userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
                }
                return userManager.GetRoles(Id).Single();
            }
        }
    }    
}