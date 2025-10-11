using Microsoft.EntityFrameworkCore;
using SecureBank_Pro.BankEntities;
using SecureBank_Pro.Data;
using SecureBank_Pro.Models;
using System.Data;
using System.Linq;

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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }

        public static async Task<Users> GetUserById(string email, BankDbContext context)
        {

            try
            {
                Users user = await context.Users.Include(u => u.Balance).Include(u => u.Transactions).FirstOrDefaultAsync(c => c.email == email);
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public static async Task<UserProfile> GetUserProfile(string email, BankDbContext context)
        {
            try
            {
                Users user = await context.Users.Include(u => u.Balance).Include(u => u.Transactions).FirstOrDefaultAsync(c => c.email == email);

                if (user != null)
                {
                    UserProfile userProfile = new UserProfile();
                    userProfile.Transactions = new List<Transaction>();
                    userProfile.Users = user;
                    userProfile.Balance = user.Balance;

                    if(user.Transactions != null)
                    {
                        userProfile.Transactions = user.Transactions.ToList();
                    }

                    return userProfile;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
