using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SecureBank_Pro.BankEntities
{
    public class Users
    {
        public int id { get; set; }
        public string full_name { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string password_hash { get; set; } = string.Empty;
        public string? phone_number { get; set; }
        public string role { get; set; } = string.Empty;

        // Customer
        public string? address { get; set; }
        public DateTime? date_of_birth { get; set; }
        public string? kyc_status { get; set; }

        // Manager/Employee
        public string? branch_assigned { get; set; }
        public DateTime? hire_date { get; set; }
        public string? role_title { get; set; }

        // Auditor
        public string? assigned_region { get; set; }
        public string can_export_reports { get; set; }
        public DateTime? last_login_time { get; set; }

        public bool is_active { get; set; } = false;
        public DateTime? created_at { get; set; }
        public bool otp_active { get; set; }
        public Balance? Balance { get; set; }
        public ICollection<Transaction> Transactions { get; set; } = new HashSet<Transaction>();
        public ICollection<Applications> Applications { get; set; } = new HashSet<Applications>();


        public Users(Users other)
        {
            this.id = other.id;
            this.full_name = other.full_name;
            this.email = other.email;
            this.password_hash = other.password_hash;
            this.phone_number = other.phone_number;
            this.role = other.role;
            this.address = other.address;
            this.date_of_birth = other.date_of_birth;
            this.kyc_status = other.kyc_status;
            this.branch_assigned = other.branch_assigned;
            this.hire_date = other.hire_date;
            this.role_title = other.role_title;
            this.assigned_region = other.assigned_region;
            this.can_export_reports = other.can_export_reports;
            this.last_login_time = other.last_login_time;
            this.is_active = other.is_active;
            this.created_at = other.created_at;
            this.otp_active = other.otp_active;
        }

        public Users() { }
    }

    public class Balance
    {
        [Key]
        public int UserId { get; set; } // PK + FK
            
        [ForeignKey("UserId")]
        public Users User { get; set; } = null!;
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public DateTime LastUpdated { get; set; }
        public bool IsFrozen { get; set; }

        public Balance(Balance other)
        {
            this.UserId = other.UserId;
            this.Amount = other.Amount;
            this.CurrencyCode = other.CurrencyCode;
            this.LastUpdated = other.LastUpdated;
            this.IsFrozen = other.IsFrozen;
        }

        public Balance() { }
    }

    public class Transaction
    {
        [Key]
        public long Trasactionsid { get; set; } // PK
        public int UserId { get; set; } // FK

        [ForeignKey("UserId")]
        public Users User { get; set; } = null!;

        public string TransactionType { get; set; }
        public decimal Amount { get; set; }
        public decimal BalanceAfter { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsEmi { get; set; } = false;

        public Transaction(Transaction other)
        {
            this.Trasactionsid = other.Trasactionsid;
            this.UserId = other.UserId;
            this.TransactionType = other.TransactionType;
            this.Amount = other.Amount;
            this.BalanceAfter = other.BalanceAfter;
            this.Description = other.Description;
            this.CreatedAt = other.CreatedAt;
            this.IsEmi = other.IsEmi;
        }

        public Transaction() { }
    }

    public class ChatHistory
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int? ReceiverId { get; set; }
        public string MessageText { get; set; }
        public DateTime SentAt { get; set; }
        public string Section { get; set; }
        public string? Room { get; set; }
        public string SenderName { get; set; }
        public string Department { get; set; }

        public ChatHistory(ChatHistory other)
        {
            this.Id = other.Id;
            this.SenderId = other.SenderId;
            this.ReceiverId = other.ReceiverId;
            this.MessageText = other.MessageText;
            this.SentAt = other.SentAt;
            this.Section = other.Section;
            this.Room = other.Room;
            this.SenderName = other.SenderName;
            this.Department = other.Department;
        }

        public ChatHistory() { }
    }

    public class Applications
    {
        [Key]
        public int ApplicationId { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }       // FK to Users
        public Users Customer { get; set; } = null!;

        [ForeignKey("CustomerOffer")]
        public int OfferId { get; set; }          // FK to CustomerOffers
        public CustomerOffers CustomerOffer { get; set; } = null!;

        public DateTime ApplicationDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Notes { get; set; }

        public Applications() { }

        public Applications(Applications other)
        {
            this.ApplicationId = other.ApplicationId;
            this.CustomerId = other.CustomerId;
            this.OfferId = other.OfferId;
            this.ApplicationDate = other.ApplicationDate;
            this.Status = other.Status;
            this.Notes = other.Notes;
        }

    }

    public class ActiveLoans
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
        public int LoanApproverId { get; set; }

        public ActiveLoans() { }

        public ActiveLoans(ActiveLoans other)
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
            this.LoanApproverId = other.LoanApproverId;
        }

    }

    public class CustomerOffers
    {
        [Key]
        public int CustomerOfferId { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public Users Customer { get; set; } = null!;

        public decimal LoanAmount { get; set; }
        public decimal? CustomInterestRate { get; set; }
        public DateTime CreatedOn { get; set; }

        public CustomerOffers() { }

        public CustomerOffers(CustomerOffers other)
        {
            this.CustomerOfferId = other.CustomerOfferId;
            this.CustomerId = other.CustomerId;
            this.LoanAmount = other.LoanAmount;
            this.CustomInterestRate = other.CustomInterestRate;
            this.CreatedOn = other.CreatedOn;
        }

    }

    public class Offers
    {
        [Key]
        public int OfferId { get; set; }

        public decimal LoanAmount { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public decimal InterestRate { get; set; }

        public DateTime? ValidTill { get; set; }

        // Parameterless Constructor (Empty Constructor)
        public Offers() { }

        // Copy Constructor
        public Offers(Offers other)
        {
            this.OfferId = other.OfferId;
            this.LoanAmount = other.LoanAmount;
            this.Title = other.Title;
            this.Description = other.Description;
            this.InterestRate = other.InterestRate;
            this.ValidTill = other.ValidTill;
        }
    }

    public class UserFiles
    {
        [Key]
        public int user_file_id { get; set; }

        public int user_id { get; set; }

        public string? signature_path { get; set; }

        public string? profile_photo_path { get; set; }

        // Parameterless Constructor (Empty Constructor)
        public UserFiles() { }

        // Copy Constructor
        public UserFiles(UserFiles other)
        {
            if (other != null)
            {
                this.user_file_id = other.user_file_id;
                this.user_id = other.user_id;
                this.signature_path = other.signature_path;
                this.profile_photo_path = other.profile_photo_path;
            }
        }
    }


}
