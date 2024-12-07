using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FinalProjectJunyi.Data;
using FinalProjectJunyi.Models;

namespace FinalProjectJunyi.Controllers
{
    public class UsersController : Controller
    {
        private ProjectDbContext db = new ProjectDbContext();

        // GET: Users
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        public ActionResult ManageAccounts(string searchQuery = "")
        {
            if (Session["Role"] == null || !(Session["Role"].ToString() == "Administrator" || Session["Role"].ToString() == "Property Owner"))
            {
                return RedirectToAction("Login", "Account");
            }

            // Search for users based on the search query
            var users = db.Users
                .Where(u => u.Role == "Property Manager" || u.Role == "Potential Tenant")
                .Where(u => string.IsNullOrEmpty(searchQuery) || u.FullName.Contains(searchQuery) || u.Email.Contains(searchQuery))
                .ToList();

            ViewBag.SearchQuery = searchQuery;
            return View(users);
        }


        // GET: Users/Details/5
        public ActionResult Details(int? id)
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

        [HttpGet]
        public ActionResult Create(string role)
        {
            if (Session["Role"] == null || !(Session["Role"].ToString() == "Administrator" || Session["Role"].ToString() == "Property Owner"))
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.Role = role;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {
            if (Session["Role"] == null || !(Session["Role"].ToString() == "Administrator" || Session["Role"].ToString() == "Property Owner"))
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("ManageAccounts");
            }

            return View(user);
        }


        //// GET: Users/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Users/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "UserId,FullName,Password,Role,Preferences,Email,PhoneNumber,CreatedAt")] User user)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Users.Add(user);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(user);
        //}

        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (Session["Role"] == null || !(Session["Role"].ToString() == "Administrator" || Session["Role"].ToString() == "Property Owner"))
            {
                return RedirectToAction("Login", "Account");
            }

            // Retrieve the user by ID
            var user = db.Users.Find(id);

            if (user == null)
            {
                return HttpNotFound("User not found.");
            }

            return View(user); // Pass the user model to the view
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int UserId, string FullName, string Email)
        {
            if (Session["Role"] == null || !(Session["Role"].ToString() == "Administrator" || Session["Role"].ToString() == "Property Owner"))
            {
                return RedirectToAction("Login", "Account");
            }

            // Find the existing user in the database
            var user = db.Users.Find(UserId);

            if (user == null)
            {
                return HttpNotFound("User not found.");
            }

            // Update the fields
            user.FullName = FullName;
            user.Email = Email;

            // Validate email uniqueness
            if (db.Users.Any(u => u.Email == Email && u.UserId != UserId))
            {
                ModelState.AddModelError("Email", "Email is already in use by another user.");
                return View(user); // Return the view with validation errors
            }

            // Save changes to the database
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ManageAccounts"); // Redirect to the account management page
            }

            return View(user); // Return the view with errors if validation fails
        }




        //// GET: Users/Edit/5
        //public ActionResult Edit(int? id)
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

        //// POST: Users/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "UserId,FullName,Password,Role,Preferences,Email,PhoneNumber,CreatedAt")] User user)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(user).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(user);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            if (Session["Role"] == null || !(Session["Role"].ToString() == "Administrator" || Session["Role"].ToString() == "Property Owner"))
            {
                return RedirectToAction("Login", "Account");
            }

            var user = db.Users.Find(id);

            if (user != null)
            {
                db.Users.Remove(user);
                db.SaveChanges();
            }

            return RedirectToAction("ManageAccounts");
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
