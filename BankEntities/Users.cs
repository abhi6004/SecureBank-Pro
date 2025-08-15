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
    }
    public class Balance
    {
        [Key]
        public int UserId { get; set; } // PK + FK
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public DateTime LastUpdated { get; set; }
        public bool IsFrozen { get; set; }

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
    }

}
