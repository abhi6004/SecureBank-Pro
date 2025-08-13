using SecureBank_Pro.BankEntities;
using System.ComponentModel.DataAnnotations;

namespace SecureBank_Pro.Models
{
    public class UserProfile
    {
        public Users Users { get; set; }
        public Balance Balance { get; set; }
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
    }

}
