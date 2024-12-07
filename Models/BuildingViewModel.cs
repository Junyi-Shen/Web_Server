using FinalProjectJunyi.Models;
using System.Collections.Generic;

namespace FinalProjectJunyi.Models
{
    public class BuildingViewModel
    {
        public int BuildingId { get; set; }
        public string BuildingName { get; set; } // building name
        public List<BuildingApartmentViewModel> Apartments { get; set; } 
    }
}
