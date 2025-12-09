using SecureBank_Pro.BankEntities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SecureBank_Pro.Models
{

    public class LoanData
    {
        [Key]
        public int LoanId { get; set; }
        public int CustomerId { get; set; }

        // Identify usage
        public bool IsApplicationActive { get; set; }
        public bool IsActiveLoan { get; set; }
        public bool IsOfferUsed { get; set; }

        // Lists for related data
        public List<Applications> Applications { get; set; } = new();
        public List<Offers> Offers { get; set; } = new();
        public List<ActiveLoans> ActiveLoans { get; set; } = new();
        public List<CustomerOffers> CustomerOffers { get; set; } = new();

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

    public class NewApplications
    {
        public int ApplicationId { get; set; }
        public DateTime ApplicationData { get; set; } = DateTime.Now;
        public string Nots { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public Offers Offers { get; set; } = new();
        public Users User  { get; set; } = new();
        public List<Transaction> Transactions { get; set; } = new();
        public Balance balance { get; set; } = new();
    }
}
