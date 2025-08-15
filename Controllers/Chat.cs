using Microsoft.AspNetCore.Mvc;
using SecureBank_Pro.BankEntities;
using SecureBank_Pro.Data;
using SecureBank_Pro.Services;

namespace SecureBank_Pro.Controllers
{
    public class ChatController : Controller
    {
        private readonly BankDbContext _context;
        public ChatController(BankDbContext context) => _context = context;

        public async Task<IActionResult> ChatRoom(string section)
        {
            List<string> users = new List<string>();
            ViewBag.Current = "MainChat";

            if (section == "private-messages")
            {
                users = await Chat.GetAllUsers(section, _context);
                ViewBag.Current = "private-messages";
            }
            else if (section == "rooms-section")
            {
                ViewBag.Current = "rooms-section";
            }

            return View(users);
        }

        public async Task<string> UserChat(string SenderId, string ReciverId, string Section)
        {
            List<ChatHistory> chatHistory = new List<ChatHistory>();
            try
            {
                chatHistory = await Chat.GetUserChat(SenderId, ReciverId, Section, _context);
                string chatResponse = string.Empty;

                foreach (var chat in chatHistory)
                {
                    chatResponse += "<p>";
                    chatResponse += $"{chat.SentAt.ToShortTimeString()} - {chat.MessageText}";
                    chatResponse += "</p>\n";
                }

                return chatResponse;
            }
            catch (Exception ex)
            {
                // Handle exceptions, log errors, etc.
                Console.WriteLine($"An error occurred while getting user chat: {ex.Message}");
                return $"Error: {ex.Message}";
            }
        }
    }
}
