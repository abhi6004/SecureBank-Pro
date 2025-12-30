using Microsoft.EntityFrameworkCore;
using SecureBank_Pro.BankEntities;
using SecureBank_Pro.Data;
using System.Collections.Concurrent;

namespace SecureBank_Pro.Services
{
    public class AutoBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ConcurrentQueue<ActiveLoans> _loan = new ConcurrentQueue<ActiveLoans>();

        public AutoBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void LoanProcessing(ActiveLoans application)
        {
            _loan.Enqueue(application);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_loan.TryDequeue(out ActiveLoans application))
                {
                    ProcessApplication(application, stoppingToken);
                }

                await Task.Delay(1000, stoppingToken);
            }
        }

        private async Task ProcessApplication(ActiveLoans application, CancellationToken stoppingToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<BankDbContext>();
                int totalInstallments = application.TotalInstallments;
                decimal installmentAmount = application.InstallmentAmount;

                var balance = await context.Balances.FirstOrDefaultAsync(b => b.UserId == application.CustomerId);

                while (totalInstallments > 0 && !stoppingToken.IsCancellationRequested)
                {
                    if (balance != null)
                    {
                        balance.Amount -= installmentAmount;

                        var transaction = new Transaction
                        {
                            UserId = application.CustomerId,
                            Amount = installmentAmount,
                            TransactionType = "Debit",
                            Description = $"Loan Installment Payment for Loan ID {application.LoanId}",
                            CreatedAt = DateTime.UtcNow,
                            BalanceAfter = balance.Amount,
                            IsEmi = true
                        };

                        await context.Transactions.AddAsync(transaction);
                        await context.SaveChangesAsync(stoppingToken);
                        totalInstallments--;
                    }

                    await Task.Delay(5000);
                }

                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
