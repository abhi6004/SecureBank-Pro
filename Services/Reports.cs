using Microsoft.EntityFrameworkCore;
using SecureBank_Pro.BankEntities;
using SecureBank_Pro.Data;
using static System.Net.WebRequestMethods;

namespace SecureBank_Pro.Services
{
    public class Reports
    {
        public async static Task<List<Transaction>> GenerateTransactionReport(
    int pageSize,
    int pageNumber,
    string Category,
    int UserId,
    string fromDate,
    string toDate,
    string searchTransactionId,
    string searchUserId,
    string searchType,
    string searchDescription,
    BankDbContext context,
    IHttpContextAccessor _http)
        {
            var q = context.Transactions.AsQueryable();

            if (Category == "EMI")
                q = q.Where(t => t.IsEmi);

            if (Category == "Bank_Transfer")
                q = q.Where(t => !t.IsEmi);

            if (UserId != -1)
                q = q.Where(t => t.UserId == UserId);

            if (!string.IsNullOrEmpty(fromDate))
                q = q.Where(t => t.CreatedAt >= DateTime.Parse(fromDate));

            if (!string.IsNullOrEmpty(toDate))
                q = q.Where(t => t.CreatedAt <= DateTime.Parse(toDate));

            if (!string.IsNullOrEmpty(searchTransactionId))
                q = q.Where(t => t.Trasactionsid.ToString().Contains(searchTransactionId));

            if (!string.IsNullOrEmpty(searchUserId))
                q = q.Where(t => t.UserId.ToString().Contains(searchUserId));

            if (!string.IsNullOrEmpty(searchType))
                q = q.Where(t => t.TransactionType.Contains(searchType));

            if (!string.IsNullOrEmpty(searchDescription))
                q = q.Where(t => t.Description.Contains(searchDescription));

            int totalRecords = await q.CountAsync();

            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            _http.HttpContext.Session.SetInt32("TotalPages", totalPages);

            return await q
                .OrderByDescending(t => t.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

    }
}
