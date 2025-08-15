using Microsoft.EntityFrameworkCore;
using SecureBank_Pro.BankEntities;
using SecureBank_Pro.Data;
using SecureBank_Pro.Models;

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
}
