using Microsoft.EntityFrameworkCore;
using SecureBank_Pro.BankEntities;


namespace SecureBank_Pro.Data
{
    public class BankDbContext : DbContext
    {
        public DbSet<Users> Users { get; set; }

        public BankDbContext(DbContextOptions<BankDbContext> options) : base(options) { }
    }
}
