using Microsoft.EntityFrameworkCore;
using SecureBank_Pro.BankEntities;
using SecureBank_Pro.Models;


namespace SecureBank_Pro.Data
{
    public class BankDbContext : DbContext
    {
        public DbSet<Users> Users { get; set; }
        public DbSet<Balance> Balances { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<ChatHistory> ChatHistory { get; set; }
        public DbSet<Applications> Applications { get; set; }
        public DbSet<ActiveLoans> ActiveLoans { get; set; }
        public DbSet<CustomerOffers> CustomerOffers { get; set; }
        public DbSet<Offers> Offers { get; set; }

        public BankDbContext(DbContextOptions<BankDbContext> options) : base(options) { }
    }
}
