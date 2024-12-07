using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Services.Description;

namespace FinalProjectJunyi.Models
{
    public class Apartment
    {
        [Key]
        public int ApartmentId { get; set; } 

        [Required]
        public int BuildingId { get; set; } 

        [Required]
        [StringLength(20)]
        public string UnitNumber { get; set; } 

        [Required]
        public int RentAmount { get; set; } 

        [Required]
        public int Bedrooms { get; set; } 

        [Required]
        public int Bathrooms { get; set; } 

        [Required]
        public int LivingRoom { get; set; } 

        [Required]
        public bool ElevatorAccess { get; set; } 

        [Required]
        public int SquareFootage { get; set; } 

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Available"; // 

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now; 

        // Navigation Properties
        [ForeignKey("BuildingId")]
        public virtual Building Building { get; set; } 
        public virtual ICollection<Appointment> Appointments { get; set; } 
    }
}
