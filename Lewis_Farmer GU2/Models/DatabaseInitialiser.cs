using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Reflection.Emit;

namespace Lewis_Farmer_GU2.Models
{
    public class DatabaseInitialiser : DropCreateDatabaseAlways<ApplicationDBContext>
    {
        protected override void Seed(ApplicationDBContext context)
        {
            base.Seed(context);

            if (!context.Users.Any())
            {
                //Initialises the role manager
                RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

                if (!roleManager.RoleExists("Admin")) { roleManager.Create(new IdentityRole("Admin")); }
                if (!roleManager.RoleExists("Mechanic")) { roleManager.Create(new IdentityRole("Mechanic")); }
                if (!roleManager.RoleExists("Manager")) { roleManager.Create(new IdentityRole("Manager")); }
                if (!roleManager.RoleExists("Customer")) { roleManager.Create(new IdentityRole("Customer")); }
                if (!roleManager.RoleExists("Suspended")) { roleManager.Create(new IdentityRole("Suspended")); }

                UserManager<User> userManager = new UserManager<User>(new UserStore<User>(context));
                userManager.PasswordValidator = new PasswordValidator
                {
                    RequireDigit = false,
                    RequiredLength = 1,
                    RequireLowercase = false,
                    RequireNonLetterOrDigit = false,
                    RequireUppercase = false,
                };

                //if (userManager.FindByEmail("Sam.Conoughn@DMotors.com") == null)
                //{
                //    var user = new Staff
                //    {
                //        UserName = "Sam.Conaghan@DMotors.com",
                //        Email = "Sam.Conaghan@DMotors.com",
                //        FirstName = "Sam",
                //        LastName = "Conaghan",
                //        RegisterDate = DateTime.Now,
                //        IsSuspended = false,
                //        Street = "4 Achnasheen",
                //        Town = "Airdrie",
                //        PostCode = "ML6 8NT"
                //    };
                //    userManager.Create(user, "ManManBan123");
                //    userManager.AddToRole(user.Id, "Manager");
                //}

                //context.SaveChanges();

                if (userManager.FindByEmail("Sam.Conoughn@DMotors.com") == null)
                {
                    var mechanic1 = new Staff
                    {
                        UserName = "Sam.Conaghan@DMotors.com",
                        Email = "Sam.Conaghan@DMotors.com",
                        Id = "Sam.Conaghan@DMotors.com",
                        RegisterDate = DateTime.Now,
                        DateOfBirth = DateTime.Parse("2000-11-12 00:00:00"),
                        EmailConfirmed = true,
                        FirstName = "Sam",
                        LastName = "Conaghan",
                        IsSuspended = false,
                        Street = "4 Achnasheen",
                        Town = "Airdrie",
                        PostCode = "ML6 8NT"
                    };
                    userManager.Create(mechanic1, "ManManBan123");
                    userManager.AddToRole(mechanic1.Id, "Mechanic");
                }

                if (!context.Suppliers.Any())
                {
                    var Michelin = new Supplier
                    {
                        SupplierId = "Michelin",
                        SupplierName = "Michelin Tyre LTD.",
                        SupplierEmail = "Web.Micheal@Michelin.co.uk",
                        SupplierTelephoneNo = "01236 755 275",
                        ListOfParts = new List<Part>()
                    };
                    context.Suppliers.Add(Michelin);
                    context.SaveChanges();

                    var OilFast = new Supplier
                    {
                        SupplierId = "OilFast",
                        SupplierName = "Oil Fast Fuel & Fleet Solutions Group",
                        SupplierEmail = "OilyOil@gmail.com",
                        SupplierTelephoneNo = "01698 537 152",
                        ListOfParts = new List<Part>()
                    };
                    context.Suppliers.Add(OilFast);
                    context.SaveChanges();

                    var GoodYear = new Supplier
                    {
                        SupplierId = "Good Year",
                        SupplierName = "Good Year LTD.",
                        SupplierEmail = "JohnCorn@GDYR.com",
                        SupplierTelephoneNo = "01236 755 275",
                        ListOfParts = new List<Part>()
                    };
                    context.Suppliers.Add(GoodYear);
                    context.SaveChanges();

                    var NONE = new Supplier
                    {
                        SupplierId = "No Part",
                        SupplierName = "No Part",
                        SupplierEmail = "No Part",
                        SupplierTelephoneNo = "No Part",
                        ListOfParts = new List<Part>()
                    };
                    context.Suppliers.Add(NONE);
                    context.SaveChanges();

                    var part1 = new Part
                    {
                        PartId = "Tyre, 15 Inch, Michelin",
                        PartName = "15 Inch Michelin Tyre",
                        PartType = "Tyre",
                        StockLevel = 15,
                        SupplierId = "Michelin",
                        PartCost = 65.50,
                        PartCompatibility = "15 Inch Rims"
                    };

                    context.Parts.Add(part1);
                    context.SaveChanges();

                    var part4 = new Part
                    {
                        PartId = "Tyre, 10 Inch, Michelin",
                        PartName = "10 Inch Michelin Tyre",
                        PartType = "Tyre",
                        StockLevel = 0,
                        SupplierId = "Michelin",
                        PartCost = 80.00,
                        PartCompatibility = "10 Inch Rims"
                    };

                    context.Parts.Add(part4);
                    context.SaveChanges();

                    var part2 = new Part
                    {
                        PartId = "Tyre, 20 Inch, Michelin",
                        PartName = "20 Inch Michelin Tyre",
                        PartType = "Tyre",
                        StockLevel = 12,
                        SupplierId = "Michelin",
                        PartCost = 65.50,
                        PartCompatibility = "15 Inch Rims"
                    };

                    context.Parts.Add(part2);
                    context.SaveChanges();

                    var part3 = new Part
                    {
                        PartId = "200ML Oil Can",
                        PartName = "200ML Oil Can, OilFast",
                        PartType = "OilCan",
                        StockLevel = 15,
                        SupplierId = "OilFast",
                        PartCost = 20.00,
                        PartCompatibility = "N/A"
                    };

                    context.Parts.Add(part3);
                    context.SaveChanges();

                    var part5 = new Part
                    {
                        PartId = "NONE",
                        PartName = "NONE",
                        PartType = "No Part",
                        StockLevel = int.MaxValue,
                        SupplierId = "No Part",
                        PartCost = 0.00,
                        PartCompatibility = "N/A"
                    };

                    context.Parts.Add(part5);
                    context.SaveChanges();
                }

                if (userManager.FindByEmail("Vineet.Dhillion@DMotors.com") == null)
                {
                    var Manager1 = new Staff
                    {
                        UserName = "Vineet.Dhillion@DMotors.com",
                        Email = "Vineet.Dhillion@DMotors.com",
                        Id = "Vineet.Dhillion@DMotors.com",
                        RegisterDate = DateTime.Now,
                        DateOfBirth = DateTime.Parse("1997-05-22 00:00:00"),
                        EmailConfirmed = true,
                        FirstName = "Vineet",
                        LastName = "Dhillon",
                        IsSuspended = false,
                        Street = "22 India Road",
                        Town = "London",
                        PostCode = "ML6 1ND"
                    };
                    userManager.Create(Manager1, "svrown123");
                    userManager.AddToRole(Manager1.Id, "Manager");
                }

                if (userManager.FindByEmail("Indi.Loyal@DMotors.com") == null)
                {
                    var Mechanic2 = new Staff
                    {
                        UserName = "Indi.Loyal@DMotors.com",
                        Email = "Indi.Loyal@DMotors.com",
                        Id = "Indi.Loyal@DMotors.com",
                        RegisterDate = DateTime.Now,
                        DateOfBirth = DateTime.Parse("1987-05-22 00:00:00"),
                        EmailConfirmed = true,
                        FirstName = "Indi",
                        LastName = "Loyal",
                        IsSuspended = false,
                        Street = "The Punjabi Palace, Room 43",
                        Town = "London",
                        PostCode = "ML6 3TH"
                    };
                    userManager.Create(Mechanic2, "yeBasc123");
                    userManager.AddToRole(Mechanic2.Id, "Mechanic");
                }

                if (userManager.FindByEmail("Taz.Loyal@DMotors.com") == null)
                {
                    var Admin1 = new Staff
                    {
                        UserName = "Taz.Loyal@DMotors.com",
                        Email = "Taz.Loyal@DMotors.com",
                        Id = "Taz.Loyal@DMotors.com",
                        RegisterDate = DateTime.Now,
                        DateOfBirth = DateTime.Parse("1987-05-22 00:00:00"),
                        EmailConfirmed = true,
                        FirstName = "Taz",
                        LastName = "Loyal",
                        IsSuspended = false,
                        Street = "The Punjabi Palace, Room 12",
                        Town = "London",
                        PostCode = "ML6 3TH"
                    };
                    userManager.Create(Admin1, "Taza321");
                    userManager.AddToRole(Admin1.Id, "Admin");
                }

                if (userManager.FindByEmail("lewisf9@gmail.com") == null)
                {
                    var Customer1 = new Customer
                    {
                        UserName = "lewisf9@gmail.com",
                        Email = "lewisf9@gmail.com",
                        Id = "lewisf9@gmail.com",
                        RegisterDate = DateTime.Now,
                        EmailConfirmed = true,
                        FirstName = "Lewis",
                        LastName = "Farmer",
                        IsSuspended = false,
                        Street = "15 Islay",
                        Town = "Airdrie",
                        PostCode = "ML6 8EA"
                    };
                    userManager.Create(Customer1, "Harvey689");
                    userManager.AddToRole(Customer1.Id, "Customer");

                    var Vehicle1 = new Vehicle
                    {
                        RegistrationNo = "F32 G22",
                        Make = "Jaguar",
                        Model = "I-PACE",
                        Year = "2019",
                        EngineSize = "3.4 litre",
                        Mileage = 20000,
                        Id = Customer1.Id
                    };

                    context.Vehicles.Add(Vehicle1);
                    Customer customer = (Customer)context.Users.Find(Vehicle1.Id);
                    customer.CurrentVehicle = Vehicle1;
                    customer.ListOfVehicles.Add(Vehicle1);
                    context.Entry(customer).State = EntityState.Modified;
                    context.SaveChanges();

                    var booking = new Booking
                    {
                        ReasonForBooking = "MOT",
                        StartDate = DateTime.Now,
                        BookingStatus = "In Proggress",
                        PaymentMethod = "TBD",
                        PaymentComplete = false,
                        CustomerId = Customer1.Id,
                        Customer = (Customer)context.Users.Find(Customer1.Id),
                        RegistrationNo = Vehicle1.RegistrationNo,
                        Vehicle = context.Vehicles.Find(Vehicle1.RegistrationNo),
                        ListOfJobs = new List<Job>()
                    };
                    context.Bookings.Add(booking);
                    context.SaveChanges();
                }

                if (userManager.FindByEmail("lewispam065@gmail.com") == null)
                {
                    var Customer1 = new Customer
                    {
                        UserName = "lewispam065@gmail.com",
                        Email = "lewispam065@gmail.com",
                        Id = "lewispam065@gmail.com",
                        RegisterDate = DateTime.Now,
                        EmailConfirmed = false,
                        FirstName = "Lewis",
                        LastName = "Schnee",
                        IsSuspended = false,
                        Street = "174 holetnest",
                        Town = "Airdrie",
                        PostCode = "ML6 8MB"
                    };
                    userManager.Create(Customer1, "Python42");
                    userManager.AddToRole(Customer1.Id, "Customer");

                    var Vehicle1 = new Vehicle
                    {
                        RegistrationNo = "F22 TSR",
                        Make = "Ford",
                        Model = "Focus",
                        Year = "2016",
                        EngineSize = "1.2 litre",
                        Mileage = 100000,
                        Id = Customer1.Id
                    };

                    context.Vehicles.Add(Vehicle1);
                    Customer customer = (Customer)context.Users.Find(Vehicle1.Id);
                    customer.CurrentVehicle = Vehicle1;
                    customer.ListOfVehicles.Add(Vehicle1);
                    context.Entry(customer).State = EntityState.Modified;
                    context.SaveChanges();

                }

                if (userManager.FindByEmail("Haku@gmail.com") == null)
                {
                    var Customer1 = new Customer
                    {
                        UserName = "Haku@gmail.com",
                        Email = "Haku@gmail.com",
                        Id = "Haku@gmail.com",
                        RegisterDate = DateTime.Now,
                        EmailConfirmed = false,
                        FirstName = "Haku",
                        LastName = "The Ultimate Ruler",
                        IsSuspended = false,
                        Street = "2 Deity Place",
                        Town = "Down",
                        PostCode = "ML0 0AA"
                    };
                    userManager.Create(Customer1, "Haku666");
                    userManager.AddToRole(Customer1.Id, "Customer");

                    var Vehicle1 = new Vehicle
                    {
                        RegistrationNo = "SMP HNR",
                        Make = "Range Rover",
                        Model = "XXXL Class",
                        Year = "0001",
                        EngineSize = "10 litre",
                        Mileage = 200000000,
                        Id = Customer1.Id
                    };

                    context.Vehicles.Add(Vehicle1);
                    Customer customer = (Customer)context.Users.Find(Vehicle1.Id);
                    customer.CurrentVehicle = Vehicle1;
                    customer.ListOfVehicles.Add(Vehicle1);
                    context.Entry(customer).State = EntityState.Modified;
                    context.SaveChanges();

                    var booking = new Booking
                    {
                        ReasonForBooking = "MOT",
                        StartDate = DateTime.Now.AddDays(2).Date,
                        BookingStatus = "In Proggress",
                        PaymentMethod = "TBD",
                        PaymentComplete = false,
                        CustomerId = Customer1.Id,
                        Customer = (Customer)context.Users.Find(Customer1.Id),
                        RegistrationNo = Vehicle1.RegistrationNo,
                        Vehicle = context.Vehicles.Find(Vehicle1.RegistrationNo),
                        ListOfJobs = new List<Job>()
                    };
                    context.Bookings.Add(booking);
                    context.SaveChanges();

                    List<Booking> listOfCustomerActiveBookings = context.Bookings.Include(b => b.Customer).Include(b => b.Vehicle).Where(b => b.CustomerId.Equals(booking.CustomerId)).Where(b => b.BookingStatus.Equals("In Proggress")).ToList();
                    Booking completeBooking = listOfCustomerActiveBookings.First();
                    Staff staff = (Staff)context.Users.Find("Taz.Loyal@DMotors.com");
                    context.Jobs.Add(new Job(completeBooking, staff));
                    context.SaveChanges();
                }

                if (userManager.FindByEmail("SOC@gmail.com") == null)
                {
                    var Customer1 = new Customer
                    {
                        UserName = "SOC@gmail.com",
                        Email = "SOC@gmail.com",
                        Id = "SOC@gmail.com",
                        RegisterDate = DateTime.Now,
                        EmailConfirmed = false,
                        FirstName = "Soul",
                        LastName = "Of Cinder",
                        IsSuspended = false,
                        Street = "The Unkindled Flame",
                        Town = "Red Sun",
                        PostCode = "ML5 8BH"
                    };
                    userManager.Create(Customer1, "Ashen");
                    userManager.AddToRole(Customer1.Id, "Customer");

                    var Vehicle1 = new Vehicle
                    {
                        RegistrationNo = "GNB 882",
                        Make = "Mini",
                        Model = "Cooper",
                        Year = "2017",
                        EngineSize = "2.1 litre",
                        Mileage = 20000,
                        Id = Customer1.Id
                    };

                    context.Vehicles.Add(Vehicle1);
                    Customer customer = (Customer)context.Users.Find(Vehicle1.Id);
                    customer.CurrentVehicle = Vehicle1;
                    customer.ListOfVehicles.Add(Vehicle1);
                    context.Entry(customer).State = EntityState.Modified;
                    context.SaveChanges();

                    var booking = new Booking
                    {
                        ReasonForBooking = "Repair",
                        StartDate = DateTime.Now.AddDays(3).Date,
                        BookingStatus = "In Proggress",
                        PaymentMethod = "TBD",
                        PaymentComplete = false,
                        CustomerId = Customer1.Id,
                        Customer = (Customer)context.Users.Find(Customer1.Id),
                        RegistrationNo = Vehicle1.RegistrationNo,
                        Vehicle = context.Vehicles.Find(Vehicle1.RegistrationNo),
                        ListOfJobs = new List<Job>()
                    };
                    context.Bookings.Add(booking);
                    context.SaveChanges();

                    List<Booking> listOfCustomerActiveBookings = context.Bookings.Include(b => b.Customer).Include(b => b.Vehicle).Where(b => b.CustomerId.Equals(booking.CustomerId)).Where(b => b.BookingStatus.Equals("In Proggress")).ToList();
                    Booking completeBooking = listOfCustomerActiveBookings.First();
                    Staff staff = (Staff)context.Users.Find("Taz.Loyal@DMotors.com");
                    context.Jobs.Add(new Job(completeBooking, staff));
                    context.SaveChanges();
                }

                if (userManager.FindByEmail("GEHRMAN@gmail.com") == null)
                {
                    var Customer1 = new Customer
                    {
                        UserName = "GEHRMAN@gmail.com",
                        Email = "GEHRMAN@gmail.com",
                        Id = "GEHRMAN@gmail.com",
                        RegisterDate = DateTime.Now,
                        EmailConfirmed = false,
                        FirstName = "Gehrman",
                        LastName = "Hunter",
                        IsSuspended = false,
                        Street = "The Dream",
                        Town = "Hunter Manor",
                        PostCode = "ML2 22B"
                    };
                    userManager.Create(Customer1, "scythe1");
                    userManager.AddToRole(Customer1.Id, "Customer");

                    var Vehicle1 = new Vehicle
                    {
                        RegistrationNo = "TDM KM5",
                        Make = "Mini",
                        Model = "Cooper",
                        Year = "2012",
                        EngineSize = "1.6 litre",
                        Mileage = 20000,
                        Id = Customer1.Id
                    };

                    context.Vehicles.Add(Vehicle1);
                    Customer customer = (Customer)context.Users.Find(Vehicle1.Id);
                    customer.CurrentVehicle = Vehicle1;
                    customer.ListOfVehicles.Add(Vehicle1);
                    context.Entry(customer).State = EntityState.Modified;
                    context.SaveChanges();

                    var booking = new Booking
                    {
                        ReasonForBooking = "Repair",
                        StartDate = DateTime.Now.AddDays(-2).Date,
                        BookingStatus = "Awaiting Payment",
                        PaymentMethod = "TBD",
                        PaymentComplete = false,
                        CustomerId = Customer1.Id,
                        Customer = (Customer)context.Users.Find(Customer1.Id),
                        RegistrationNo = Vehicle1.RegistrationNo,
                        Vehicle = context.Vehicles.Find(Vehicle1.RegistrationNo),
                        ListOfJobs = new List<Job>()
                    };
                    context.Bookings.Add(booking);
                    context.SaveChanges();

                    List<Booking> listOfCustomerActiveBookings = context.Bookings.Include(b => b.Customer).Include(b => b.Vehicle).Where(b => b.CustomerId.Equals(booking.CustomerId)).Where(b => b.BookingStatus.Equals("Awaiting Payment")).ToList();
                    Booking completeBooking = listOfCustomerActiveBookings.First();
                    Staff staff = (Staff)context.Users.Find("Taz.Loyal@DMotors.com");

                    Job job1 = new Job
                    {
                        JobId = "OpeningMeeting" + booking.BookingId.ToString(),
                        JobName = "Opening Meeting",
                        JobDescription = "Initial Client-Admin Meeting",
                        JobCost = 30.00,
                        JobStatus = "Complete",
                        DueDate = booking.StartDate,
                        DateCompleted = booking.StartDate,
                        UserId = staff.Id,
                        AssignedStaff = staff,
                        BookingId = booking.BookingId,
                        Booking = booking
                    };

                    context.Jobs.Add(job1);
                    context.SaveChanges();

                    staff = (Staff)context.Users.Find("Indi.Loyal@DMotors.com");
                    Job job2 = new Job
                    {
                        JobId = "OilChange" + booking.BookingId.ToString(),
                        JobName = "Oil Change",
                        JobDescription = "Change The Oil",
                        JobCost = 25,
                        JobStatus = "Complete",
                        DueDate = booking.StartDate.Date.AddDays(+1),
                        DateCompleted = booking.StartDate.Date.AddDays(+1),
                        UserId = staff.Id,
                        AssignedStaff = staff,
                        BookingId = booking.BookingId,
                        Booking = booking,
                        PartId = "200ML Oil Can",
                        Part = context.Parts.Find("200ML Oil Can")
                    };

                    context.Jobs.Add(job2);
                    context.SaveChanges();
                }

                if (userManager.FindByEmail("Ebrietas@gmail.com") == null)
                {
                    var Customer1 = new Customer
                    {
                        UserName = "Ebrietas@gmail.com",
                        Email = "Ebrietas@gmail.com",
                        Id = "Ebrietas@gmail.com",
                        RegisterDate = DateTime.Now,
                        EmailConfirmed = false,
                        FirstName = "Ebrietas",
                        LastName = "Daughter Of The Cosmos",
                        IsSuspended = false,
                        Street = "10 Upper",
                        Town = "Cathedral Ward",
                        PostCode = "ML3 53X"
                    };
                    userManager.Create(Customer1, "Daughter");
                    userManager.AddToRole(Customer1.Id, "Customer");

                    
                    var Vehicle1 = new Vehicle
                    {
                        RegistrationNo = "KM5 T55",
                        Make = "Volvo",
                        Model = "Estate",
                        Year = "1994",
                        EngineSize = "4 litre",
                        Mileage = 20000,
                        Id = Customer1.Id
                    };

                    context.Vehicles.Add(Vehicle1);
                    Customer customer = (Customer)context.Users.Find(Vehicle1.Id);
                    customer.CurrentVehicle = Vehicle1;
                    customer.ListOfVehicles.Add(Vehicle1);
                    context.Entry(customer).State = EntityState.Modified;
                    context.SaveChanges();

                    var booking = new Booking
                    {
                        ReasonForBooking = "MOT",
                        StartDate = DateTime.Now.AddDays(-3).Date,
                        BookingStatus = "In Proggress",
                        PaymentMethod = "TBD",
                        PaymentComplete = false,
                        CustomerId = Customer1.Id,
                        Customer = (Customer)context.Users.Find(Customer1.Id),
                        RegistrationNo = Vehicle1.RegistrationNo,
                        Vehicle = context.Vehicles.Find(Vehicle1.RegistrationNo),
                        ListOfJobs = new List<Job>()
                    };
                    context.Bookings.Add(booking);
                    context.SaveChanges();

                    List<Booking> listOfCustomerActiveBookings = context.Bookings.Include(b => b.Customer).Include(b => b.Vehicle).Where(b => b.CustomerId.Equals(booking.CustomerId)).Where(b => b.BookingStatus.Equals("In Proggress")).ToList();
                    Booking completeBooking = listOfCustomerActiveBookings.First();
                    Staff staff = (Staff)context.Users.Find("Taz.Loyal@DMotors.com");

                    Job job1 = new Job
                    {
                        JobId = "OpeningMeeting" + booking.BookingId.ToString(),
                        JobName = "Opening Meeting",
                        JobDescription = "Initial Client-Admin Meeting",
                        JobCost = 30.00,
                        JobStatus = "Complete",
                        DueDate = booking.StartDate,
                        DateCompleted = booking.StartDate,
                        UserId = staff.Id,
                        AssignedStaff = staff,
                        BookingId = booking.BookingId,
                        Booking = booking
                    };

                    context.Jobs.Add(job1);
                    context.SaveChanges();

                    Job job2 = new Job
                    {
                        JobId = "CleanExhaust" + booking.BookingId.ToString(),
                        JobName = "Clean Exhaust",
                        JobDescription = "Clean the blockage suspected to be in the exhaust",
                        JobCost = 10.00,
                        JobStatus = "Unassigned",
                        DueDate = booking.StartDate.AddDays(1).Date,
                        BookingId = booking.BookingId,
                        Booking = booking,
                        PartId = "NONE",
                        Part = context.Parts.Find("NONE")
                    };

                    context.Jobs.Add(job2);
                    context.SaveChanges();
                }

                context.SaveChanges();
            }
        }
    }
}