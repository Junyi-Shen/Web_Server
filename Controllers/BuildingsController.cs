using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FinalProjectJunyi.Data;
using FinalProjectJunyi.Models;

namespace FinalProjectJunyi.Controllers
{
    public class BuildingsController : Controller
    {
        private ProjectDbContext db = new ProjectDbContext();

        // GET: Buildings/Apartments
        public ActionResult Apartments()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            int loggedInUserId = (int)Session["UserId"];
            var buildings = db.Buildings
                .Where(b => b.PropertyManagerId == loggedInUserId)
                .Select(b => new BuildingViewModel
                {
                    BuildingName = b.Name,
                    BuildingId = b.BuildingId,
                    Apartments = db.Apartments
                        .Where(a => a.BuildingId == b.BuildingId)
                        .Select(a => new BuildingApartmentViewModel
                        {
                            ApartmentId = a.ApartmentId,
                            UnitNumber = a.UnitNumber,
                            CurrentStatus = a.Status,
                            StatusOptions = new List<string> { "Available", "Occupied", "Maintenance" }
                        })
                        .ToList()
                })
                .ToList();

            return View(buildings);
        }



        [HttpPost]
        public JsonResult UpdateApartmentStatus(int apartmentId, string newStatus)
        {
            if (Session["UserId"] == null)
            {
                return Json(new { success = false, message = "Unauthorized" });
            }

            // Find the apartment
            var apartment = db.Apartments.FirstOrDefault(a => a.ApartmentId == apartmentId);
            if (apartment == null)
            {
                return Json(new { success = false, message = "Apartment not found" });
            }

            // Update the status
            apartment.Status = newStatus;
            db.SaveChanges();

            return Json(new { success = true, message = "Status updated successfully" });
        }

        // POST: Buildings/AddBuilding
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddBuilding(string buildingName, string address)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            int loggedInUserId = (int)Session["UserId"];
            var newBuilding = new Building
            {
                Name = buildingName,
                Address = address,
                PropertyManagerId = loggedInUserId,
                CreatedAt = System.DateTime.Now
            };

            db.Buildings.Add(newBuilding);
            db.SaveChanges();

            return RedirectToAction("Apartments");
        }

        // POST: Buildings/DeleteBuilding
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteBuilding(int buildingId)
        {
            var building = db.Buildings.Find(buildingId);
            if (building == null)
            {
                return HttpNotFound();
            }

            db.Buildings.Remove(building);
            db.SaveChanges();

            return RedirectToAction("Apartments");
        }


        // GET: Buildings
        public ActionResult Index()
        {
            var buildings = db.Buildings.Include(b => b.PropertyManager);
            return View(buildings.ToList());
        }

        // GET: Buildings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Building building = db.Buildings.Find(id);
            if (building == null)
            {
                return HttpNotFound();
            }
            return View(building);
        }

        // GET: Buildings/Create
        [HttpGet]
        public ActionResult Create()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        // POST: Buildings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Building building, HttpPostedFileBase imageFile)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                // Handle the image upload
                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    string fileName = Path.GetFileNameWithoutExtension(imageFile.FileName);
                    string extension = Path.GetExtension(imageFile.FileName);
                    fileName = fileName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    string filePath = Path.Combine(Server.MapPath("~/Content/Images/Buildings"), fileName);

                    // Save the file
                    imageFile.SaveAs(filePath);

                    // Store the relative path in the database
                    building.ImageURL = "/Content/Images/Buildings/" + fileName;
                }

                // Assign the Property Manager ID from the session
                building.PropertyManagerId = (int)Session["UserId"];
                building.CreatedAt = DateTime.Now;

                db.Buildings.Add(building);
                db.SaveChanges();

                return RedirectToAction("Apartments");
            }

            return View(building);
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Retrieve the building from the database
            var building = db.Buildings.FirstOrDefault(b => b.BuildingId == id);

            if (building == null)
            {
                return HttpNotFound("Building not found.");
            }

            return View(building);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Building building, HttpPostedFileBase imageFile)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                var existingBuilding = db.Buildings.FirstOrDefault(b => b.BuildingId == building.BuildingId);

                if (existingBuilding != null)
                {
                    existingBuilding.Name = building.Name;
                    existingBuilding.Address = building.Address;

                    // Handle image upload
                    if (imageFile != null && imageFile.ContentLength > 0)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(imageFile.FileName);
                        string extension = Path.GetExtension(imageFile.FileName);
                        fileName = fileName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                        string filePath = Path.Combine(Server.MapPath("~/Content/Images/Buildings"), fileName);

                        imageFile.SaveAs(filePath);
                        existingBuilding.ImageURL = "/Content/Images/Buildings/" + fileName;
                    }

                    db.SaveChanges();
                    return RedirectToAction("Apartments", "Buildings");
                }
            }

            return View(building);
        }



        // GET: Buildings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Building building = db.Buildings.Find(id);
            if (building == null)
            {
                return HttpNotFound();
            }
            return View(building);
        }

        // POST: Buildings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Building building = db.Buildings.Find(id);
            db.Buildings.Remove(building);
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
