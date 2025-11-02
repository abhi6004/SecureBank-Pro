using SecureBank_Pro.BankEntities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SecureBank_Pro.Models
{

    public class Application
    {
        [Key]
        public int ApplicationId { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }       // FK to Users
        public Users Customer { get; set; } = null!;

        [ForeignKey("CustomerOffer")]
        public int OfferId { get; set; }          // FK to CustomerOffers
        public CustomerOffer CustomerOffer { get; set; } = null!;

        public DateTime ApplicationDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Notes { get; set; }

        public Application() { }

        public Application(Application other)
        {
            this.ApplicationId = other.ApplicationId;
            this.CustomerId = other.CustomerId;
            this.OfferId = other.OfferId;
            this.ApplicationDate = other.ApplicationDate;
            this.Status = other.Status;
            this.Notes = other.Notes;
        }

    }

    public class ActiveLoan
    {
        [Key]
        public int LoanId { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }       // FK to Users
        public Users Customer { get; set; } = null!;

        public DateTime LoanStartDate { get; set; }
        public decimal LoanAmount { get; set; }
        public decimal InterestRate { get; set; }

        public int TotalInstallments { get; set; }
        public decimal InstallmentAmount { get; set; }
        public decimal? PenaltyPercent { get; set; }

        public string LoanStatus { get; set; } = string.Empty;
        public DateTime? DueDate { get; set; }

        public ActiveLoan() { }

        public ActiveLoan(ActiveLoan other)
        {
            this.LoanId = other.LoanId;
            this.CustomerId = other.CustomerId;
            this.LoanStartDate = other.LoanStartDate;
            this.LoanAmount = other.LoanAmount;
            this.InterestRate = other.InterestRate;
            this.TotalInstallments = other.TotalInstallments;
            this.InstallmentAmount = other.InstallmentAmount;
            this.PenaltyPercent = other.PenaltyPercent;
            this.LoanStatus = other.LoanStatus;
            this.DueDate = other.DueDate;
        }

    }

    public class CustomerOffer
    {
        [Key]
        public int CustomerOfferId { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public Users Customer { get; set; } = null!;

        public decimal LoanAmount { get; set; }
        public decimal? CustomInterestRate { get; set; }
        public DateTime? CustomValidTill { get; set; }
        public DateTime CreatedOn { get; set; }

        public CustomerOffer() { }

        public CustomerOffer(CustomerOffer other)
        {
            this.CustomerOfferId = other.CustomerOfferId;
            this.CustomerId = other.CustomerId;
            this.LoanAmount = other.LoanAmount;
            this.CustomInterestRate = other.CustomInterestRate;
            this.CustomValidTill = other.CustomValidTill;
            this.CreatedOn = other.CreatedOn;
        }

    }

    public class Offer
    {
        [Key]
        public int OfferId { get; set; }

        public decimal LoanAmount { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public decimal? InterestRate { get; set; }

        public DateTime? ValidTill { get; set; }

        // Parameterless Constructor (Empty Constructor)
        public Offer() { }

        // Copy Constructor
        public Offer(Offer other)
        {
            this.OfferId = other.OfferId;
            this.LoanAmount = other.LoanAmount;
            this.Title = other.Title;
            this.Description = other.Description;
            this.InterestRate = other.InterestRate;
            this.ValidTill = other.ValidTill;
        }
    }
}
