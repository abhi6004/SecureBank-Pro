using Microsoft.EntityFrameworkCore;
using SecureBank_Pro.Data;

namespace SecureBank_Pro.Services
{
    public class Account
    {
        public static async void WithdrawMoney(BankDbContext context, decimal amount, string type, string email)
        {
            var userId = await context.Users.Where(e => e.email == email).Select(e => e.id).FirstOrDefaultAsync();
            var Account = await context.Balances.Where(e => e.UserId == userId).FirstOrDefaultAsync();

            Account.Amount = Account.Amount - amount;
            await context.SaveChangesAsync();
        }

        public static async void AddBalance(BankDbContext context, decimal amount, string type, string email)
        {
            var userId = await context.Users.Where(e => e.email == email).Select(e => e.id).FirstOrDefaultAsync();
            var Account = await context.Balances.Where(e => e.UserId == userId).FirstOrDefaultAsync();

            Account.Amount = Account.Amount + amount;
            await context.SaveChangesAsync();
        }

        public static async void UserTransfer(BankDbContext context, decimal amount, string type, string email, string id)
        {
            var userId = await context.Users.Where(e => e.email == email).Select(e => e.id).FirstOrDefaultAsync();
            var ReciverId = await context.Users.Where(e => e.email == id).Select(e => e.id).FirstOrDefaultAsync();

            var Sender = await context.Balances.Where(e => e.UserId == userId).FirstOrDefaultAsync();
            var Reciver = await context.Balances.Where(e => e.UserId == userId).FirstOrDefaultAsync();

            Sender.Amount = Sender.Amount - amount;
            Reciver.Amount = Reciver.Amount + amount;

            await context.SaveChangesAsync();
        }
    }
}
