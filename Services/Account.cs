using Microsoft.EntityFrameworkCore;
using SecureBank_Pro.Data;

namespace SecureBank_Pro.Services
{
    public class Account
    {
        public static async Task WithdrawMoney(BankDbContext context, decimal amount, string type, int id)
        {
            try
            {
                //var userId = await context.Users.Where(e => e.email == email).Select(e => e.id).FirstOrDefaultAsync();
                var Account = await context.Balances.Where(e => e.UserId == id).FirstOrDefaultAsync();

                Account.Amount = Account.Amount - amount;
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public static async Task AddBalance(BankDbContext context, decimal amount, string type, int id)
        {
            try
            {
                var Account = await context.Balances.Where(e => e.UserId == id).FirstOrDefaultAsync();

                Account.Amount = Account.Amount + amount;
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static async Task UserTransfer(BankDbContext context, decimal amount, string type, string email, string id)
        {
            try
            {
                var userId = await context.Users.Where(e => e.email == email).Select(e => e.id).FirstOrDefaultAsync();
                var ReciverId = await context.Users.Where(e => e.email == id).Select(e => e.id).FirstOrDefaultAsync();

                var Sender = await context.Balances.Where(e => e.UserId == userId).FirstOrDefaultAsync();
                var Reciver = await context.Balances.Where(e => e.UserId == userId).FirstOrDefaultAsync();

                Sender.Amount = Sender.Amount - amount;
                Reciver.Amount = Reciver.Amount + amount;

                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
