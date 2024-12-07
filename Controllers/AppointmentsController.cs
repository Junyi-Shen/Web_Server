using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using FinalProjectJunyi.Data;
using FinalProjectJunyi.Models;

namespace FinalProjectJunyi.Controllers
{
    public class AppointmentsController : Controller
    {
        private ProjectDbContext db = new ProjectDbContext();

        // GET: Appointments/Create
        [HttpGet]
        public ActionResult Create(int apartmentId)
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Login", "Account");

            // Ensure apartmentId is valid
            var apartment = db.Apartments.Find(apartmentId);
            if (apartment == null)
                return HttpNotFound("Apartment not found.");

            var model = new Appointment
            {
                ApartmentId = apartmentId,
                TenantId = (int)Session["UserId"]
            };

            return View(model);
        }


        // POST: Appointments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                db.Appointments.Add(appointment);
                db.SaveChanges();
                return RedirectToAction("Details", "Apartments", new { id = appointment.ApartmentId });
            }

            return View(appointment);
        }

        // GET: Appointments/List
        public ActionResult List()
        {
            if (Session["UserId"] == null || Session["Role"]?.ToString() != "Potential Tenant")
                return RedirectToAction("Login", "Account");

            int tenantId = (int)Session["UserId"];

            var appointments = db.Appointments
                .Include(a => a.Apartment.Building)
                .Where(a => a.TenantId == tenantId)
                .Select(a => new TenantAppointmentViewModel
                {
                    AppointmentId = a.AppointmentId,
                    ApartmentUnitNumber = a.Apartment.UnitNumber,
                    BuildingName = a.Apartment.Building.Name,
                    AppointmentDate = a.AppointmentDate,
                    Status = a.Status
                })
                .ToList();

            return View(appointments);
        }

        [HttpGet]
        public ActionResult InitiateChat(int appointmentId)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Retrieve the appointment
            var appointment = db.Appointments.Include(a => a.Apartment.Building).FirstOrDefault(a => a.AppointmentId == appointmentId);
            if (appointment == null)
            {
                return HttpNotFound("Appointment not found.");
            }

            // Get the Property Manager associated with the apartment's building
            var propertyManagerId = appointment.Apartment.Building.PropertyManagerId;

            // Redirect to the Messages/Chat action
            return RedirectToAction("Chat", "Messages", new { userId = propertyManagerId });
        }





        public ActionResult ManagerList()
        {
            if (Session["UserId"] == null || Session["Role"]?.ToString() != "Property Manager")
                return RedirectToAction("Login", "Account");

            int managerId = (int)Session["UserId"];

            var appointments = db.Appointments
                .Include(a => a.Apartment.Building)
                .Where(a => a.Apartment.Building.PropertyManagerId == managerId)
                .Select(a => new ManagerAppointmentViewModel
                {
                    AppointmentId = a.AppointmentId,
                    ApartmentUnitNumber = a.Apartment.UnitNumber,
                    BuildingName = a.Apartment.Building.Name,
                    TenantName = a.Tenant.FullName,
                    AppointmentDate = a.AppointmentDate,
                    Status = a.Status,
                    StatusOptions = new List<string> { "Scheduled", "Completed", "Cancelled" }
                })
                .ToList();

            return View(appointments);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CancelAppointment(int id)
        {
            var appointment = db.Appointments.Find(id);

            if (appointment == null)
                return HttpNotFound();

            db.Appointments.Remove(appointment);
            db.SaveChanges();

            if (Session["Role"]?.ToString() == "Property Manager")
                return RedirectToAction("ManagerList");
            else if (Session["Role"]?.ToString() == "Potential Tenant")
                return RedirectToAction("List");

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateStatus(int id, string newStatus)
        {
            var appointment = db.Appointments.Find(id);

            if (appointment == null || Session["Role"]?.ToString() != "Property Manager")
                return HttpNotFound();

            if (new[] { "Scheduled", "Completed", "Cancelled" }.Contains(newStatus))
            {
                appointment.Status = newStatus;
                db.SaveChanges();
            }

            return RedirectToAction("ManagerList");
        }


    }
}