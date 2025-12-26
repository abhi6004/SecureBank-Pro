using SecureBank_Pro.BankEntities;
using SecureBank_Pro.Data;
using static System.Net.WebRequestMethods;

namespace SecureBank_Pro.Services
{
    public class Reports
    {
        public async static Task<List<Transaction>> GenerateTransactionReport(int pageSize, int pageNumber, BankDbContext context , IHttpContextAccessor _http)
        {
            try
            {
                List<Transaction> _transaction = context.Transactions
                     .Skip((pageNumber - 1) * pageSize)
                     .Take(pageSize)
                     .ToList();

                int totalRecords = context.Transactions.Count();
                int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
                _http.HttpContext.Session.SetInt32("TotalPages", totalPages);
                return _transaction;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
