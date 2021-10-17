using Lewis_Farmer_GU2.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lewis_Farmer_GU2.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Customer"))
                {
                    ApplicationDBContext db = new ApplicationDBContext();
                    string UserId = User.Identity.GetUserId();
                    Customer customer = (Customer)db.Users.Find(UserId);

                    if (customer.CurrentVehicle == null)
                    {
                        return RedirectToAction("Create", "Vehicles");
                    }
                }
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Customer"))
                {
                    ApplicationDBContext db = new ApplicationDBContext();
                    string UserId = User.Identity.GetUserId();
                    Customer customer = (Customer)db.Users.Find(UserId);

                    if (customer.CurrentVehicle == null)
                    {
                        return RedirectToAction("Create", "Vehicles");
                    }
                }
            }
            return View();
        }

        [AllowAnonymous]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Customer"))
                {
                    ApplicationDBContext db = new ApplicationDBContext();
                    string UserId = User.Identity.GetUserId();
                    Customer customer = (Customer)db.Users.Find(UserId);

                    if (customer.CurrentVehicle == null)
                    {
                        return RedirectToAction("Create", "Vehicles");
                    }
                }
            }
            return View();
        }
    }
}