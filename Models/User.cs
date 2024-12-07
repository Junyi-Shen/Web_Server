using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Services.Description;

namespace FinalProjectJunyi.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; } 

        [Required]
        [StringLength(50)]
        public string FullName { get; set; } 

        [Required]
        [StringLength(255)]
        public string Password { get; set; } 

        [Required]
        [StringLength(20)]
        public string Role { get; set; } 

        public string Preferences { get; set; } 

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; } 

        [StringLength(20)]
        [Phone]
        public string PhoneNumber { get; set; } 

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now; 

        // Navigation Properties
        public virtual ICollection<Building> ManagedBuildings { get; set; } 
        public virtual ICollection<Appointment> Appointments { get; set; } 
        public virtual ICollection<Message> SentMessages { get; set; } 
        public virtual ICollection<Message> ReceivedMessages { get; set; } 
        public virtual ICollection<Report> ManagedReports { get; set; } 
        public virtual ICollection<Report> OwnedReports { get; set; } 
    }
}
