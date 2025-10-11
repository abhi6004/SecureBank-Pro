using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureBank_Pro.BankEntities;
using SecureBank_Pro.Data;
using SecureBank_Pro.Models;
using System.Text.Json.Nodes;

namespace SecureBank_Pro.Services
{
    public class Account
    {
        public static async Task<bool> MoneyTransaction(BankDbContext context, string Email, string Type ,string Description , decimal amount)
        {
            try
            {
                Users user = await SecureBank_Pro.Services.GetUsers.GetUserById(Email, context);
                var Account = user.Balance;

                if(Type == "withdraw")
                {
                    Account.Amount = Account.Amount - amount;
                }
                else
                {
                    Account.Amount = Account.Amount + amount;
                }

                await SecureBank_Pro.Services.Account.TransectionEntry(context, Description, amount, Type, user.id, Account.Amount);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
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
                throw;
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
                throw;
            }
        }

        public static async Task<decimal> Amountconversion(HttpClient _httpClient ,decimal amount , string CurrencyCode)
        {
            try
            {
                string URL = "https://open.er-api.com/v6/latest/" + CurrencyCode.ToString();
                var response = await _httpClient.GetAsync(URL);

                if (response != null)
                {
                    JsonObject result = await response.Content.ReadFromJsonAsync<JsonObject>();

                    if (result["rates"] != null)
                    {
                        if (result["rates"][CurrencyCode] != null)
                        {
                            decimal rate = result["rates"]["INR"].GetValue<decimal>();
                            return amount * rate;
                            
                        }
                    }
                }
                return 0;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

    }
}
