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
            try
            {
                List<Users> listOfUsers = await context.Users.Where(c => c.role == role).ToListAsync();
                return listOfUsers;
            }
            catch(Exception ex) { return null; }

        }

        public static async Task<Users> GetUserById(string email, BankDbContext context)
        {
            // Use LINQ to find a user by ID
            Users user = await context.Users.FirstOrDefaultAsync(c => c.email == email);
            return user;
        }

        public static async Task<UserProfile> GetUserProfile(string email, BankDbContext context)
        {
            try
            {
                Users user = await context.Users.FirstOrDefaultAsync(c => c.email == email);

                if (user != null)
                {
                    List<Transaction> transactions = await context.Transactions.Where(c => c.UserId == user.id).ToListAsync();
                    Balance userBalance = await context.Balances.FirstOrDefaultAsync(c => c.UserId == user.id);

                    UserProfile userProfile = new UserProfile();
                    userProfile.Transactions = new List<Transaction>();
                    userProfile.Users = user;
                    userProfile.Balance = userBalance;
                    userProfile.Transactions = transactions;

                    return userProfile;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
