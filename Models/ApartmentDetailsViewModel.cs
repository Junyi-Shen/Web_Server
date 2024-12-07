using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalProjectJunyi.Models
{
    public class ApartmentDetailsViewModel
    {
        public int ApartmentId { get; set; }
        public string UnitNumber { get; set; }
        public string BuildingName { get; set; }
        public string Address { get; set; }
        public int RentAmount { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public int LivingRooms { get; set; }
        public bool ElevatorAccess { get; set; }
        public int SquareFootage { get; set; }
        public string ImageURL { get; set; }
    }
}