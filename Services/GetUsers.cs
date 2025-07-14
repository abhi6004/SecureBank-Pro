using Microsoft.EntityFrameworkCore;
using SecureBank_Pro.BankEntities;
using SecureBank_Pro.Data;
using SecureBank_Pro.Models;

namespace SecureBank_Pro.Services
{
    public class GetUsers
    {
        public static async Task<List<Users>> FetchUsers(string role, BankDbContext context)
        {
            // Use LINQ to filter users by role
            var listOfUsers = await context.Users.Where(c => c.role == role).ToListAsync();
            return listOfUsers;
        }
    }
}
