using System.ComponentModel.DataAnnotations;

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
        }

        public Users() { }
    }

    public class Balance
    {
        [Key]
        public int UserId { get; set; } // PK + FK
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
        public string TransactionType { get; set; }
        public decimal Amount { get; set; }
        public decimal BalanceAfter { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }

        public Transaction(Transaction other)
        {
            this.Trasactionsid = other.Trasactionsid;
            this.UserId = other.UserId;
            this.TransactionType = other.TransactionType;
            this.Amount = other.Amount;
            this.BalanceAfter = other.BalanceAfter;
            this.Description = other.Description;
            this.CreatedAt = other.CreatedAt;
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

        public ChatHistory(ChatHistory other)
        {
            this.Id = other.Id;
            this.SenderId = other.SenderId;
            this.ReceiverId = other.ReceiverId;
            this.MessageText = other.MessageText;
            this.SentAt = other.SentAt;
            this.Section = other.Section;
            this.Room = other.Room;
        }

        public ChatHistory() { }
    }
}
