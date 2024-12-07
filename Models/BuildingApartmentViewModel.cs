using System.Collections.Generic;

namespace FinalProjectJunyi.Models
{
    public class BuildingApartmentViewModel
    {
        public int ApartmentId { get; set; }
        public string UnitNumber { get; set; }
        public string CurrentStatus { get; set; }
        public List<string> StatusOptions { get; set; }
    }
}
