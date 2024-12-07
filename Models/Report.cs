using FinalProjectJunyi.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProjectJunyi.Models
{
    public class Report
    {
        [Key]
        public int ReportId { get; set; } 

        [Required]
        public string EventDescription { get; set; } 

        [Required]
        public int PropertyManagerId { get; set; } 

        [Required]
        public int PropertyOwnerId { get; set; } 

        [Required]
        public DateTime ReportedAt { get; set; } = DateTime.Now; 

        // Navigation Properties
        [ForeignKey("PropertyManagerId")]
        public virtual User PropertyManager { get; set; } 

        [ForeignKey("PropertyOwnerId")]
        public virtual User PropertyOwner { get; set; } 
    }
}
