using Microsoft.EntityFrameworkCore;
using SecureBank_Pro.BankEntities;
using SecureBank_Pro.Data;

namespace SecureBank_Pro.Services
{
    public class Chat
    {
        public static async Task<List<string>> GetAllUsers(string section, BankDbContext _context)
        {
            List<string> users = new List<string>();

            if (section == "private-messages")
            {
                users = await _context.Users.Select(u => u.full_name).ToListAsync();

                if (users.Count != 0)
                {
                    return users;
                }
            }

            return null;

        }

        public static async Task<List<ChatHistory>> GetUserChat(string SenderId, string ReciverId, string Section, BankDbContext _context)
        {
            try
            {
                List<ChatHistory> chatHistory = new List<ChatHistory>();

                if (Section == "private")
                {
                    chatHistory = await _context.ChatHistory.Where(c => c.SenderId.ToString() == SenderId && c.ReceiverId.ToString() == ReciverId && c.Section == Section).OrderBy(c => c.SentAt).ToListAsync();
                }
                else
                {
                    chatHistory = await _context.ChatHistory.Where(c=> c.Section == Section).OrderBy(c => c.SentAt).ToListAsync();
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
}
