using Microsoft.EntityFrameworkCore;
using SecureBank_Pro.BankEntities;
using SecureBank_Pro.Data;
using SecureBank_Pro.Models;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SecureBank_Pro.Services
{
    public class Loan
    {
        public static async Task<LoanData> GetOffers(BankDbContext context , int id)
        {
            try
            {
                var loan = new LoanData
                {
                    Applications = await context.Applications.Where(a => a.CustomerId == id).ToListAsync(),
                    Offers = await context.Offers.ToListAsync(),
                };

                return loan;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }

        public static string CustomerappliedLoan(BankDbContext context , int customerId)
        {
            var customer = context.Users.Include(u => u.Applications).FirstOrDefault(u => u.id == customerId);

            if(customer.Applications.Count > 0)
            {
                string LoanString = "[Loan]";

                foreach(var applicaton in customer.Applications)
                {
                    if(applicaton.Status == "Pending")
                    {
                        LoanString = LoanString += applicaton.OfferId.ToString() + "-";
                    }
                    else if (applicaton.Status == "Approved")
                    {
                        string temp = applicaton.OfferId.ToString() + "-";
                        LoanString = temp += LoanString;
                    }
                }

                return LoanString;
            }
            else
            {
                return null;
            }
        }

        public static async Task<string> ApplyLoan(BankDbContext context, int customerId, int offerId)
        {
            try
            {
                var ApplyLoan =  await context.Offers.FirstOrDefaultAsync(o => o.OfferId == offerId);

                if(ApplyLoan == null)
                {
                    return "Invalid Offer.";
                }

                var Application = new Applications
                {
                    CustomerId = customerId,
                    OfferId = ApplyLoan.OfferId,
                    ApplicationDate = DateTime.Now,
                    Status = "Pending"
                };

                await context.Applications.AddAsync(Application);
                await context.SaveChangesAsync();

                return "Loan application submitted successfully.";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
