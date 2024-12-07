using FinalProjectJunyi.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProjectJunyi.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; } 

        [Required]
        public int TenantId { get; set; } 

        [Required]
        public int ApartmentId { get; set; } 

        [Required]
        public DateTime AppointmentDate { get; set; } 

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Scheduled"; 

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now; 

        // Navigation Properties
        [ForeignKey("TenantId")]
        public virtual User Tenant { get; set; } 

        [ForeignKey("ApartmentId")]
        public virtual Apartment Apartment { get; set; } 
    }
}
