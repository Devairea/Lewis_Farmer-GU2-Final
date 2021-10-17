using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Lewis_Farmer_GU2.Models;
using Lewis_Farmer_GU2.ViewModels;
using Microsoft.AspNet.Identity;

namespace Lewis_Farmer_GU2.Controllers
{
    public class UsersController : AccountController
    {
        private ApplicationDBContext db = new ApplicationDBContext();

        // GET: Users
        public ActionResult Index(string RoleCriteria)
        {
            List<User> users = db.Users.ToList();
            if (RoleCriteria == null)
            {
                return View(db.Users.ToList());
            }
            if (RoleCriteria.Equals("Staff"))
            {
                List<User> staffMembers = new List<User>();
                foreach (var item in users)
                {
                    try
                    {
                        staffMembers.Add((Staff)item);
                    }
                    catch
                    {

                    }

                }
                return View(staffMembers);
            }
            if (RoleCriteria.Equals("Customer"))
            {
                List<User> customers = new List<User>();
                foreach (var item in users)
                {
                    try
                    {
                        customers.Add((Customer)item);
                    }
                    catch
                    {

                    }

                }
                return View(customers);
            }
            return View(db.Users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,IsSuspended,RegisterDate,FirstName,LastName,Street,Town,PostCode,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IsSuspended,RegisterDate,FirstName,LastName,Street,Town,PostCode,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        //public ActionResult Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    User user = db.Users.Find(id);
        //    if (user == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(user);
        //}

        //// POST: Users/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(string id)
        //{
        //    User user = db.Users.Find(id);
        //    db.Users.Remove(user);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        public ActionResult ChangeRole(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Can't change your own role
            if (id == User.Identity.GetUserId())
            {
                return RedirectToAction("Index", "users");
            }

            //The user that has been selected
            User user = db.Users.Find(id);
            //the current role of the selected user
            string oldRole = (UserManager.GetRoles(id)).Single();

            //Gets a list of all valid roles to be stored in the list of roles in ChangeViewModel
            try
            {

                List<SelectListItem> Roles = new List<SelectListItem>();
                Roles.Add(new SelectListItem
                {
                    Text = "Suspended",
                    Value = "Suspended",
                    Selected = oldRole.Equals("Suspended")
                });
                Roles.Add(new SelectListItem
                {
                    Text = "Customer",
                    Value = "Customer",
                    Selected = oldRole.Equals("Customer")
                });
                Customer customer = (Customer)user;
                return View(new ChangeRoleViewModel
                {
                    UserName = user.UserName,
                    Roles = Roles,
                    OldRole = oldRole
                });
            }
            catch
            {
                List<SelectListItem> Roles = new List<SelectListItem>();
                Roles.Add(new SelectListItem
                {
                    Text = "Mechanic",
                    Value = "Mechanic",
                    Selected = oldRole.Equals("Mechanic")
                });
                Roles.Add(new SelectListItem
                {
                    Text = "Manager",
                    Value = "Manager",
                    Selected = oldRole.Equals("Manager")
                });
                Roles.Add(new SelectListItem
                {
                    Text = "Admin",
                    Value = "Admin",
                    Selected = oldRole.Equals("Admin")
                });
                return View(new ChangeRoleViewModel
                {
                    UserName = user.UserName,
                    Roles = Roles,
                    OldRole = oldRole
                });
            }
        }

        //Post : Users/ChangeRole/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("ChangeRole")]
        public ActionResult ChangeRoleConfimation(string id, [Bind(Include = "Role")] ChangeRoleViewModel model)
        {
            //Cant change your own role
            if (id == User.Identity.GetUserId())
            {
                return RedirectToAction("Index", "users");
            }

            if (ModelState.IsValid)
            {
                // The user that has been selected
                User user = UserManager.FindById(id);
                //the current role of the selected user
                string oldRole = (UserManager.GetRoles(id)).Single();

                if (oldRole == model.Role)
                {
                    return RedirectToAction("Index", "Users");
                }

                // remove old role and add new one
                 UserManager.RemoveFromRole(id, oldRole);
                UserManager.AddToRole(id, model.Role);

                return RedirectToAction("Index", "Users");
            }
            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
