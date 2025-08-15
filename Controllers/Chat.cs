using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SecureBank_Pro.BankEntities;
using SecureBank_Pro.Data;
using SecureBank_Pro.Services;


namespace SecureBank_Pro.Controllers
{
    [Authorize(AuthenticationSchemes = "UserCookies")]
    public class ChatController : Controller
    {
        private readonly BankDbContext _context;
        public ChatController(BankDbContext context) => _context = context;

        [Authorize(Roles = "Employee , Manager , Auditor")]
        public async Task<IActionResult> ChatRoom(string section)
        {
            List<string> users = new List<string>();
            ViewBag.Current = "MainChat";
            var userJson = HttpContext.Session.GetString("UserData");
            Users currentUser = JsonConvert.DeserializeObject<Users>(userJson);

            if (section == "private-messages")
            {
                users = await Chat.GetAllUsers(section, _context , currentUser.full_name);
                ViewBag.Current = "private-messages";
            }
            else if (section == "rooms-section")
            {
                ViewBag.Current = "rooms-section";
            }

            return View(users);
        }

        [HttpGet]
        public async Task<string> UserChat(string Reciver, string Section)
        {
            List<ChatHistory> chatHistory = new List<ChatHistory>();
            try
            {
                var userJson = HttpContext.Session.GetString("UserData");
                Users currentUser = JsonConvert.DeserializeObject<Users>(userJson);

                chatHistory = await Chat.GetUserChat(Reciver, Section, _context , currentUser);
                string chatResponse = string.Empty;

                if(chatHistory.Count == 0)
                {
                    chatResponse = "<p>No Chat </p>";
                    return chatResponse;
                }

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
