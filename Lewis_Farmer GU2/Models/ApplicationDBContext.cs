using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lewis_Farmer_GU2.Models
{
    public class ApplicationDBContext : IdentityDbContext<User>
    {
        public ApplicationDBContext() 
            : base ("DefaultConnection", throwIfV1Schema : false)
        {
            Database.SetInitializer(new DatabaseInitialiser());
        }

        public static ApplicationDBContext Create()
        {
            return new ApplicationDBContext();
        }

        public System.Data.Entity.DbSet<Lewis_Farmer_GU2.Models.Supplier> Suppliers { get; set; }

        public System.Data.Entity.DbSet<Lewis_Farmer_GU2.Models.Part> Parts { get; set; }

        public System.Data.Entity.DbSet<Lewis_Farmer_GU2.Models.Job> Jobs { get; set; }

        public System.Data.Entity.DbSet<Lewis_Farmer_GU2.Models.Booking> Bookings { get; set; }

        public System.Data.Entity.DbSet<Lewis_Farmer_GU2.Models.Vehicle> Vehicles { get; set; }
    }
}