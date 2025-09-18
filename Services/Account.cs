using Microsoft.EntityFrameworkCore;
using SecureBank_Pro.Data;
using SecureBank_Pro.BankEntities;

namespace SecureBank_Pro.Services
{
    public class Account
    {
        public static async Task<bool> WithdrawMoney(BankDbContext context, decimal amount, string type, int id)
        {
            try
            {
                //var userId = await context.Users.Where(e => e.email == email).Select(e => e.id).FirstOrDefaultAsync();
                var Account = await context.Balances.Where(e => e.UserId == id).FirstOrDefaultAsync();

                Account.Amount = Account.Amount - amount;
                await context.SaveChangesAsync();
                //await Logs.LogTransaction($"Withdraw of {amount} for User ID: {id}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

        }

        public static async Task<bool> AddBalance(BankDbContext context, decimal amount, string type, int id)
        {
            try
            {
                var Account = await context.Balances.Where(e => e.UserId == id).FirstOrDefaultAsync();

                Account.Amount = Account.Amount + amount;
                await context.SaveChangesAsync();
                //await Logs.LogTransaction($"Deposit of {amount} for User ID: {id}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public static async Task TransectionEntry(BankDbContext context, string Description, decimal amount, string type, int id , decimal balance)
        {
            try
            {
                var entry = new Transaction(new Transaction
                {
                    UserId = id,
                    TransactionType = type,
                    Amount = amount,
                    BalanceAfter = balance,
                    Description = Description,
                    CreatedAt = DateTime.UtcNow
                });

                await context.AddAsync(entry);
                await context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public static async Task<decimal> CheckBalance(BankDbContext context , int id)
        {
            try
            {
                decimal balance = await context.Balances.Where(e => e.UserId == id).Select(e => e.Amount).FirstOrDefaultAsync();
                return balance;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return 0;
        }
    }
}
