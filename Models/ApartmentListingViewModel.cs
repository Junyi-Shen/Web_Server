using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalProjectJunyi.Models
{
    public class ApartmentListingViewModel 
    {
        public int ApartmentId { get; set; }
        public string UnitNumber { get; set; }
        public string BuildingAddress { get; set; }
        public string RentAmount { get; set; }
        public string ImageURL { get; set; }
    }

}