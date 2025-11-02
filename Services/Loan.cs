using SecureBank_Pro.BankEntities;
using SecureBank_Pro.Data;
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
    }
}
