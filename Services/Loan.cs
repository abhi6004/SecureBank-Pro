using Microsoft.EntityFrameworkCore;
using SecureBank_Pro.BankEntities;
using SecureBank_Pro.Data;
using SecureBank_Pro.Models;
using System.Data;
using System.Linq;

namespace SecureBank_Pro.Services
{
    public class Loan
    {
        public static List<Offers> GetOffers(BankDbContext context)
        {
            try
            {
                var offers = context.Offers.ToList();
                return offers;
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
    }
}
