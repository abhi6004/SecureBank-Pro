using Microsoft.EntityFrameworkCore;
using SecureBank_Pro.Data;
using SecureBank_Pro.BankEntities;
using System.Text.Json.Nodes;

namespace SecureBank_Pro.Services
{
    public class Account
    {
        public static async Task<bool> MoneyTransaction(BankDbContext context, string email, string type, string desc, decimal amount)
        {
            var user = await context.Users.Include(x => x.Balance)
                                          .FirstOrDefaultAsync(x => x.email == email);

            if (user == null) throw new Exception("User not found");

            if (type == "withdraw")
                user.Balance.Amount -= amount;
            else
                user.Balance.Amount += amount;

            await TransectionEntry(context, desc, amount, type, user.id, user.Balance.Amount);

            await context.SaveChangesAsync();

            return true;
        }

        public static async Task TransectionEntry(BankDbContext context, string desc, decimal amount, string type, int userId, decimal balance)
        {
            var entry = new Transaction
            {
                UserId = userId,
                TransactionType = type,
                Amount = amount,
                BalanceAfter = balance,
                Description = desc,
                CreatedAt = DateTime.UtcNow
            };

            await context.AddAsync(entry);
        }

        public static async Task<decimal> Amountconversion(HttpClient http, decimal amount, string currency)
        {
            string url = $"https://open.er-api.com/v6/latest/{currency}";

            var response = await http.GetAsync(url);
            var obj = await response.Content.ReadFromJsonAsync<JsonObject>();

            decimal rate = obj["rates"]["INR"].GetValue<decimal>();

            return amount * rate;
        }
    }
}
