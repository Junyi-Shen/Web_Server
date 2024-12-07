using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalProjectJunyi.Models
{
    public class ManagerAppointmentViewModel
    {
        public int AppointmentId { get; set; }
        public string ApartmentUnitNumber { get; set; }
        public string BuildingName { get; set; }
        public string TenantName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Status { get; set; }
        public List<string> StatusOptions { get; set; }
    }

}