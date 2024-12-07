using FinalProjectJunyi.Data;
using FinalProjectJunyi.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace FinalProjectJunyi.Controllers
{
    public class MessagesController : Controller
    {
        private readonly ProjectDbContext db = new ProjectDbContext();

        public ActionResult Index()
        {
            return RedirectToAction("ChatList");
        }


        // GET: Messages/ChatList
        public ActionResult ChatList()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            int currentUserId = (int)Session["UserId"];

            // Fetch all users the current user has messaged or received messages from
            var chats = db.Messages
                .Where(m => m.SenderId == currentUserId || m.ReceiverId == currentUserId)
                .Select(m => m.SenderId == currentUserId ? m.Receiver : m.Sender)
                .Distinct()
                .Where(u => u != null) // Exclude null references for deleted users
                .ToList();

            return View(chats);
        }

        // GET: Messages/Chat/{userId}
        public ActionResult Chat(int userId)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            int currentUserId = (int)Session["UserId"];

            // Fetch messages exchanged between the current user and the selected user
            var messages = db.Messages
                .Where(m => (m.SenderId == currentUserId && m.ReceiverId == userId) ||
                            (m.ReceiverId == currentUserId && m.SenderId == userId))
                .OrderBy(m => m.SentAt)
                .Select(m => new MessageViewModel
                {
                    MessageId = m.MessageId,
                    Content = m.Content,
                    SentAt = m.SentAt,
                    Read = m.Read,
                    SenderId = m.SenderId,
                    ReceiverId = m.ReceiverId,
                    SenderName = m.SenderId != null ? m.Sender.FullName : "Deleted User",
                    ReceiverName = m.ReceiverId != null ? m.Receiver.FullName : "Deleted User"
                })
                .ToList();

            ViewBag.ChatWith = db.Users.Find(userId)?.FullName ?? "Unknown User";
            ViewBag.ChatWithId = userId;

            return View(messages);
        }

        [HttpPost]
        public ActionResult SendMessage(int receiverId, string content)
        {
            if (Session["UserId"] == null || string.IsNullOrEmpty(content))
            {
                return new HttpStatusCodeResult(400, "Invalid request.");
            }

            int senderId = (int)Session["UserId"];

            var message = new Message
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Content = content,
                Read = false,
                SentAt = DateTime.Now
            };

            db.Messages.Add(message);
            db.SaveChanges();

            return RedirectToAction("Chat", new { userId = receiverId });
        }
    }
}
