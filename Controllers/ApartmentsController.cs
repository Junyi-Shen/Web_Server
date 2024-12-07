using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FinalProjectJunyi.Models;
using FinalProjectJunyi.Data;

namespace FinalProjectJunyi.Controllers
{
    public class ApartmentsController : Controller
    {
        private ProjectDbContext db = new ProjectDbContext();

        private readonly ProjectDbContext _context;

        public ApartmentsController()
        {
            _context = new ProjectDbContext();
        }

        // GET: Apartments
        public ActionResult Index()
        {
            var apartments = db.Apartments.Include(a => a.Building);
            return View(apartments.ToList());
        }

        [HttpGet]
        public ActionResult List(ApartmentFilterViewModel filters)
        {
            var elevatorAccess = Request.QueryString["ElevatorAccess"] == "on";
            // Query apartments from the database
            var query = _context.Apartments.Where(a => a.Status == "Available").AsQueryable();

            // Apply filters
            if (filters.MinBedrooms.HasValue)
                query = query.Where(a => a.Bedrooms >= filters.MinBedrooms.Value);
            if (filters.MaxBedrooms.HasValue)
                query = query.Where(a => a.Bedrooms <= filters.MaxBedrooms.Value);
            if (filters.MinBathrooms.HasValue)
                query = query.Where(a => a.Bathrooms >= filters.MinBathrooms.Value);
            if (filters.MaxBathrooms.HasValue)
                query = query.Where(a => a.Bathrooms <= filters.MaxBathrooms.Value);
            if (filters.MinLivingRooms.HasValue)
                query = query.Where(a => a.LivingRoom >= filters.MinLivingRooms.Value);
            if (filters.MaxLivingRooms.HasValue)
                query = query.Where(a => a.LivingRoom <= filters.MaxLivingRooms.Value);
            if (filters.MinSquareFootage.HasValue)
                query = query.Where(a => a.SquareFootage >= filters.MinSquareFootage.Value);
            if (filters.MaxSquareFootage.HasValue)
                query = query.Where(a => a.SquareFootage <= filters.MaxSquareFootage.Value);
            if (filters.MinRent.HasValue)
                query = query.Where(a => a.RentAmount >= filters.MinRent.Value);
            if (filters.MaxRent.HasValue)
                query = query.Where(a => a.RentAmount <= filters.MaxRent.Value);

            // Apply elevator access filter if "on"
            if (elevatorAccess)
                query = query.Where(a => a.ElevatorAccess == true);

            // Execute query and load data into memory
            var apartments = query
                .Select(a => new
                {
                    a.ApartmentId,
                    a.UnitNumber,
                    BuildingAddress = a.Building.Address,
                    a.RentAmount,
                    ImageURL = a.Building.ImageURL
                })
                .ToList(); // Load data into memory

            // Format RentAmount and map to the view model
            var apartmentListings = apartments.Select(a => new ApartmentListingViewModel
            {
                ApartmentId = a.ApartmentId,
                UnitNumber = a.UnitNumber,
                BuildingAddress = a.BuildingAddress,
                RentAmount = $"${a.RentAmount}/month", // Format in memory
                ImageURL = a.ImageURL
            }).ToList();

            // Return the view with the filter criteria and results
            filters.Apartments = apartmentListings;
            return View(filters);
        }

        public ActionResult Create(int buildingId)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Check if the building exists
            var building = db.Buildings.FirstOrDefault(b => b.BuildingId == buildingId);
            if (building == null)
            {
                return HttpNotFound("Building not found.");
            }

            // Pass the building information to the view
            ViewBag.BuildingName = building.Name;
            ViewBag.BuildingId = building.BuildingId;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Apartment apartment)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                db.Apartments.Add(apartment);
                db.SaveChanges();
                return RedirectToAction("Apartments", "Buildings");
            }

            // Repopulate building information if validation fails
            var building = db.Buildings.FirstOrDefault(b => b.BuildingId == apartment.BuildingId);
            if (building != null)
            {
                ViewBag.BuildingName = building.Name;
                ViewBag.BuildingId = building.BuildingId;
            }

            return View(apartment);
        }

        // GET: Apartments/Details/{id}
        public ActionResult Details(int id)
        {
            var apartment = db.Apartments
                .Where(a => a.ApartmentId == id)
                .Select(a => new ApartmentDetailsViewModel
                {
                    UnitNumber = a.UnitNumber,
                    BuildingName = a.Building.Name,
                    Address = a.Building.Address,
                    RentAmount = a.RentAmount,
                    Bedrooms = a.Bedrooms,
                    Bathrooms = a.Bathrooms,
                    LivingRooms = a.LivingRoom,
                    ElevatorAccess = a.ElevatorAccess,
                    SquareFootage = a.SquareFootage,
                    ImageURL = string.IsNullOrEmpty(a.Building.ImageURL) ? "~/Content/Images/Buildings/placeholder.png" : a.Building.ImageURL
                })
                .FirstOrDefault();

            if (apartment == null)
            {
                return HttpNotFound("Apartment not found.");
            }

            return View(apartment);
        }

        

        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var apartment = db.Apartments.FirstOrDefault(a => a.ApartmentId == id);

            if (apartment == null)
            {
                return HttpNotFound("Apartment not found.");
            }

            return View(apartment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Apartment apartment)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                db.Entry(apartment).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Apartments", "Buildings");
            }

            return View(apartment);
        }


        // POST: Apartments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "ApartmentId,BuildingId,UnitNumber,RentAmount,Bedrooms,Bathrooms,SquareFootage,Status,CreatedAt")] Apartment apartment)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(apartment).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.BuildingId = new SelectList(db.Buildings, "BuildingId", "Name", apartment.BuildingId);
        //    return View(apartment);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteApartment(int apartmentId)
        {
            // Check if the user is logged in
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Find the apartment in the database
            var apartment = db.Apartments.FirstOrDefault(a => a.ApartmentId == apartmentId);

            if (apartment == null)
            {
                return HttpNotFound("Apartment not found.");
            }

            // Remove the apartment
            db.Apartments.Remove(apartment);
            db.SaveChanges();

            // Redirect to the managed buildings page
            return RedirectToAction("Apartments", "Buildings");
        }


        // GET: Apartments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Apartment apartment = db.Apartments.Find(id);
            if (apartment == null)
            {
                return HttpNotFound();
            }
            return View(apartment);
        }

        // POST: Apartments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Apartment apartment = db.Apartments.Find(id);
            db.Apartments.Remove(apartment);
            db.SaveChanges();
            return RedirectToAction("Index");
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
