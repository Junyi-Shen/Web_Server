using System.Collections.Generic;

namespace FinalProjectJunyi.Models
{
    public class ApartmentFilterViewModel
    {
        
        public int? MinBedrooms { get; set; }
        public int? MaxBedrooms { get; set; }
        public int? MinBathrooms { get; set; }
        public int? MaxBathrooms { get; set; }
        public int? MinLivingRooms { get; set; }
        public int? MaxLivingRooms { get; set; }
        public int? MinSquareFootage { get; set; }
        public int? MaxSquareFootage { get; set; }
        public int? MinRent { get; set; }
        public int? MaxRent { get; set; }
        public bool? ElevatorAccess { get; set; }

        // Results
        public List<ApartmentListingViewModel> Apartments { get; set; }
    }
}
