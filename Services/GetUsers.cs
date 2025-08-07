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
            List<Users> listOfUsers = await context.Users.Where(c => c.role == role).ToListAsync();
            return listOfUsers;
        }

        public static async Task<Users> GetUserById(string email, BankDbContext context)
        {
            // Use LINQ to find a user by ID
            Users user = await context.Users.FirstOrDefaultAsync(c => c.email == email);
            return user;
        }
    }
}
