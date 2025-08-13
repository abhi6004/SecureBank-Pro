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

        public static async Task<UserProfile> GetUserProfile(string email , BankDbContext context)
        {
            Users user = await context.Users.FirstOrDefaultAsync(c => c.email == email);
            List<Transaction> transactions = await context.Transactions.Where(c => c.UserId == user.id).ToListAsync();
            Balance userBalance = await context.Balances.FirstOrDefaultAsync(c => c.UserId == user.id);

            if (user != null)
            {
                UserProfile userProfile = new UserProfile();
                userProfile.Transactions = new List<Transaction>();
                userProfile.Users = user;
                userProfile.Balance = userBalance;
                userProfile.Transactions = transactions;

                return userProfile;
            }
            else
            {
                return null; // or throw an exception, depending on your error handling strategy
            }
        }
    }
}
