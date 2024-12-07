using FinalProjectJunyi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProjectJunyi.Models
{
    public class Building
    {
        [Key]
        public int BuildingId { get; set; } 

        [Required]
        [StringLength(100)]
        public string Name { get; set; } 

        [Required]
        [StringLength(255)]
        public string Address { get; set; } 

        [Required]
        public int PropertyManagerId { get; set; } 

        public string ImageURL { get; set; } 

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now; 

        // Navigation Properties
        [ForeignKey("PropertyManagerId")]
        public virtual User PropertyManager { get; set; } 
        public virtual ICollection<Apartment> Apartments { get; set; } 
    }
}
