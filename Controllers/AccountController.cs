using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FinalProjectJunyi.Data;
using FinalProjectJunyi.Models;

namespace FinalProjectJunyi.Controllers
{
    public class AccountController : Controller
    {
        private readonly ProjectDbContext db = new ProjectDbContext();

        // GET: Account/Register
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }


        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if email already exists
                if (db.Users.Any(u => u.Email == model.Email))
                {
                    ModelState.AddModelError("Email", "Email is already taken.");
                    return View(model);
                }

                // Create a new user with the role "Potential Tenant"
                var newUser = new User
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    Password = model.Password,
                    Role = "Potential Tenant", // Hardcoded role
                    PhoneNumber = model.PhoneNumber,
                    CreatedAt = System.DateTime.Now
                };

                db.Users.Add(newUser);
                db.SaveChanges();

                return RedirectToAction("Login", "Account");
            }

            return View(model);
        }


        // GET: Account/Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }


        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users
                    .FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);

                if (user != null)
                {
                    // Set user session details
                    Session["UserId"] = user.UserId;
                    Session["FullName"] = user.FullName;
                    Session["Role"] = user.Role;

                    if (user.Role == "Potential Tenant")
                    {
                        // For Potential tenants, go to the apartments list view
                        return RedirectToAction("List", "Apartments");
                    }
                    else if (user.Role == "Property Manager")
                    {
                        return RedirectToAction("Apartments", "Buildings");
                    }
                    else if (user.Role == "Property Owner" || user.Role == "Administrator")
                    {
                        return RedirectToAction("ManageAccounts", "Users");
                    }

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Invalid email or password.");
            }

            return View(model);
        }


        // GET: Account/Logout
        public ActionResult Logout()
        {
            Session.Clear(); // Clear all session variables
            return RedirectToAction("Login");
        }
    }
}
