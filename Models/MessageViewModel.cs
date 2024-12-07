using System;

namespace FinalProjectJunyi.Models
{
    public class MessageViewModel
    {
        public int MessageId { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; }
        public bool Read { get; set; }
        public int? SenderId { get; set; }
        public int? ReceiverId { get; set; }
        public string SenderName { get; set; }
        public string ReceiverName { get; set; }
    }
}
