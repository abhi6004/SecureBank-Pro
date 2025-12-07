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

                foreach (var application in applications)
                {
                    var _user = Users.FirstOrDefault(b => b.id == application.OfferId);
                    var _offer = Offers.FirstOrDefault(o => o.OfferId == application.OfferId);

                    var _balance = Balances.FirstOrDefault(b => b.UserId == _user.id);
                    var _transactions = Transactions.Where(t => t.UserId == _user.id).ToList();

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
                }

                return newApplicationsList.Count > 0 ? newApplicationsList : null;
            }
            catch (Exception ex) {
                throw new Exception("Error in GetNewApplications: " + ex.Message);
            }
        }
    }
}
