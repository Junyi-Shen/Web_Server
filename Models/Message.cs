using FinalProjectJunyi.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProjectJunyi.Models
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; } 

        public int? SenderId { get; set; } 

        public int? ReceiverId { get; set; } 

        [Required]
        public string Content { get; set; } 

        [Required]
        public bool Read { get; set; }

        [Required]
        public DateTime SentAt { get; set; } = DateTime.Now; 

        // Navigation Properties
        [ForeignKey("SenderId")]
        public virtual User Sender { get; set; } 

        [ForeignKey("ReceiverId")]
        public virtual User Receiver { get; set; } 
    }
}
