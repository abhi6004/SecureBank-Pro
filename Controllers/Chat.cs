using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SecureBank_Pro.BankEntities;
using SecureBank_Pro.Data;
using SecureBank_Pro.Models;
using SecureBank_Pro.Services;
using System.Text.Json;

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
            try
            {
                var userJson = HttpContext.Session.GetString("UserData");
                Users currentUser = JsonConvert.DeserializeObject<Users>(userJson);

                // Load all users excluding current user
                var users = await _context.Users
                                .Where(u => u.id != currentUser.id)
                                .ToListAsync();

                ViewBag.Current = "MainChat";
                ViewBag.UserRole = currentUser.role;

                return View(users); // now matches List<Users>
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }


        public async Task<string> UserChat(string Reciver, string Section)
        {
            List<ChatHistory> chatHistory = new List<ChatHistory>();
            try
            {
                var userJson = HttpContext.Session.GetString("UserData");
                Users currentUser = JsonConvert.DeserializeObject<Users>(userJson);

                chatHistory = await Chat.GetUserChat(Reciver, Section, _context, currentUser);
                if (chatHistory.Count == 0) return "<p>No Chat </p>";

                string chatResponse = string.Empty;

                foreach (var chat in chatHistory)
                {
                    string style = chat.SenderId == currentUser.id ? "text-align:right;" : "text-align:left;";
                    string senderName = chat.SenderName;
                    string department = chat.Department;

                    // *** MAIN CHANGE HERE ***
                    if (Section.Equals("general", StringComparison.OrdinalIgnoreCase))
                    {
                        // show section name + full name in text (no data-* attributes)
                        chatResponse +=
                        $"<p style='{style}'>" +
                        $"<strong>{senderName}</strong> ({department}) - {chat.SentAt.ToShortTimeString()} : {chat.MessageText}</p>\n";

                    }
                    else if(Section.Equals("room", StringComparison.OrdinalIgnoreCase))
                    {
                        chatResponse +=
                        $"<p style='{style}'>" +
                        $"<strong>{senderName}</strong> - {chat.SentAt.ToShortTimeString()} : {chat.MessageText}</p>\n";
                    }
                    else
                    {
                        // original behavior
                        chatResponse +=
                            $"<p style='{style}' data-sender='{senderName}' data-department='{department}'>" +
                            $"{chat.SentAt.ToShortTimeString()} - {chat.MessageText}</p>\n";
                    }
                }

                return chatResponse;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching chat: {ex.Message}");
                throw;
            }
        }

    }
}
