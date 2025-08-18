using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SecureBank_Pro.BankEntities;
using SecureBank_Pro.Data;
using SecureBank_Pro.Models;
using System.Security.Claims;
using System.Text.Json;
using static System.Collections.Specialized.BitVector32;

namespace SecureBank_Pro.Services
{
    public class Chat
    {
        public static async Task<List<string>> GetAllUsers(string section, BankDbContext _context, string userName)
        {
            List<string> users = new List<string>();

            if (section == "private-messages")
            {
                users = await _context.Users.Where(u => u.full_name != userName).Select(u => u.full_name).ToListAsync();
                if (users.Count != 0)
                {
                    return users;
                }
            }

            return null;

        }

        public static async Task<List<ChatHistory>> GetUserChat(string Reciver, string Section, BankDbContext _context, Users user)
        {
            try
            {
                string SenderId = user.id.ToString();
                string ReciverId = string.Empty;

                if (Section == "private")
                {
                    ReciverId = await _context.Users.Where(u => u.full_name == Reciver).Select(u => u.id.ToString()).FirstOrDefaultAsync();
                }

                List<ChatHistory> chatHistory = new List<ChatHistory>();

                if (Section == "private")
                {
                    chatHistory = await _context.ChatHistory.Where(c => c.SenderId.ToString() == SenderId && c.ReceiverId.ToString() == ReciverId && c.Section == Section).OrderBy(c => c.SentAt).ToListAsync();
                }
                else
                {
                    chatHistory = await _context.ChatHistory.Where(c => c.Section == Section).OrderBy(c => c.SentAt).ToListAsync();
                }

                return chatHistory;
            }
            catch (Exception ex)
            {
                // Handle exceptions, log errors, etc.
                Console.WriteLine($"An error occurred while getting user chat: {ex.Message}");
                throw ex;
            }
        }

    }

    public class ChatBoxHub : Hub
    {
        private readonly BankDbContext _context;

        public ChatBoxHub(BankDbContext context)
        {
            _context = context;
        }

        public async Task SendChat(string message, string id, string section, string room)
        {
            try
            {
                // Get the current user's identity from the hub context
                string email = Context.User.FindFirst(ClaimTypes.Email)?.Value;

                var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.email == email);
                if (currentUser == null)
                {
                    // User not found in the database
                    return;
                }

                int receiverId = 0;
                if (section == "private")
                {
                    id = id.Replace("-chatbox", "");
                    var receiver = await _context.Users.FirstOrDefaultAsync(u => u.full_name == id);
                }

                ChatHistory newChat = new ChatHistory
                {
                    SenderId = currentUser.id,
                    ReceiverId = receiverId, // This will be 0 for rooms/general chats
                    MessageText = message,
                    SentAt = DateTime.Now,
                    Section = section,
                    Room = room
                };

                _context.ChatHistory.Add(newChat);
                await _context.SaveChangesAsync();

                if(newChat.Section == "private")
                {
                    newChat.Room = id;
                }
                // Send message to all connected clients
                await Clients.All.SendAsync("ReceiveMessage", newChat);

                // Or, to send a message back to the sender
                // await Clients.Caller.SendAsync("ReceiveMessage", newChat);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"An error occurred while sending chat: {ex.Message}");
            }
        }
    }
}
//    public class ChatBoxHub : Hub
//    {
//        private readonly BankDbContext _context;
//        private readonly IHttpContextAccessor _httpContextAccessor;
//        public ChatBoxHub(BankDbContext context, IHttpContextAccessor httpContextAccessor)
//        {
//            _context = context;
//            _httpContextAccessor = httpContextAccessor;
//        }



//        public async Task<object> SendChat(string message, string id, string section, string room)
//        {
//            try
//            {
//                var userJson = _httpContextAccessor.HttpContext.Session.GetString("UserData");
//                Users currentUser = JsonConvert.DeserializeObject<Users>(userJson);


//                //string message = obj.GetProperty("message").GetString();
//                //string id = obj.GetProperty("id").GetString();
//                //string section = obj.GetProperty("section").GetString();
//                //string room = obj.GetProperty("room").GetString();
//                int ReciverId = 0;

//                if (section == "private")
//                {
//                    ReciverId = await _context.Users.Where(u => u.full_name == id).Select(u => u.id).FirstOrDefaultAsync();
//                }

//                if (section == "private")
//                {
//                    id = id.Replace("-chatbox", "");
//                }

//                ChatHistory newChat = new ChatHistory
//                {
//                    SenderId = currentUser.id,
//                    ReceiverId = ReciverId,
//                    MessageText = message,
//                    SentAt = DateTime.Now,
//                    Section = section
//                };

//                _context.ChatHistory.Add(newChat);
//                await _context.SaveChangesAsync();

//                await Clients.All.SendAsync("ReceiveMessage", newChat);
//                return newChat;
//            }
//            catch (Exception ex)
//            {
//                // Handle exceptions, log errors, etc.
//                Console.WriteLine($"An error occurred while getting rooms: {ex.Message}");
//                return $"Error: {ex.Message}";
//            }
//        }
//    }
//}



