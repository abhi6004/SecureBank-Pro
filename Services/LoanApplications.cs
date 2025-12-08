using Microsoft.EntityFrameworkCore;
using SecureBank_Pro.BankEntities;
using SecureBank_Pro.Data;
using SecureBank_Pro.Models;

namespace SecureBank_Pro.Services
{
    public class LoanApplications
    {
        public static async Task<List<NewApplications>> GetNewApplications(BankDbContext _context)
        {
            try
            {
                List<NewApplications> newApplicationsList = new List<NewApplications>();

                List<Applications> applications = await _context.Applications.ToListAsync();
                List<Balance> Balances = await _context.Balances.ToListAsync();
                List<Transaction> Transactions = await _context.Transactions.ToListAsync();
                List<Offers> Offers = await _context.Offers.ToListAsync();
                List<Users> Users = await _context.Users.ToListAsync();

                var _Applications = applications.ToDictionary(a => a.ApplicationId, a => a);
                var _Balances = Balances.ToDictionary(b => b.UserId, b => b);
                var _Transactions = Transactions.GroupBy(t => t.UserId)
                                                .ToDictionary(g => g.Key, g => g.ToList());
                var _Offers = Offers.ToDictionary(o => o.OfferId, o => o);
                var _Users = Users.ToDictionary(u => u.id, u => u);

                foreach (var application in applications)
                {
                    _Offers.TryGetValue(application.OfferId, out var _offer);
                    _Users.TryGetValue(application.CustomerId, out var _user);
                    _Balances.TryGetValue(application.CustomerId, out var _balance);
                    _Transactions.TryGetValue(application.CustomerId, out var _transactions);

                    var newApplicationsListItem = new NewApplications
                    {
                        ApplicationId = application.ApplicationId,
                        ApplicationData = application.ApplicationDate,
                        Nots = application.Notes ?? "",
                        Offers = _offer,
                        User = _user,
                        balance = _balance,
                        Transactions = _transactions
                    };

                    newApplicationsList.Add(newApplicationsListItem);
                }

                return newApplicationsList.Count > 0 ? newApplicationsList : null;
            }
            catch (Exception ex) {
                throw new Exception("Error in GetNewApplications: " + ex.Message);
            }
        }
    }
}
