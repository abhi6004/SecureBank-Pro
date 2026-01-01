namespace SecureBank_Pro.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string Role { get; set; } = string.Empty; // Manager, Employee, Customer, Auditor

        // Customer fields
        public string? Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? KycStatus { get; set; } // Pending, Verified, Rejected

        // Manager/Employee fields
        public string? BranchAssigned { get; set; }
        public DateTime? HireDate { get; set; }
        public string? RoleTitle { get; set; } // For Employee

        // Auditor fields
        public string? AssignedRegion { get; set; }
        public string CanExportReports { get; set; } = "false";
        public DateTime? LastLoginTime { get; set; }

        // Common fields
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool otp_active { get; set; }

        // Empty constructor (optional but explicit)
        public User() { }

        // Copy constructor
        public User(User other)
        {
            if (other == null) return;

            Id = other.Id;
            FullName = other.FullName;
            Email = other.Email;
            PasswordHash = other.PasswordHash;
            PhoneNumber = other.PhoneNumber;
            Role = other.Role;
            Address = other.Address;
            DateOfBirth = other.DateOfBirth;
            KycStatus = other.KycStatus;
            BranchAssigned = other.BranchAssigned;
            HireDate = other.HireDate;
            RoleTitle = other.RoleTitle;
            AssignedRegion = other.AssignedRegion;
            CanExportReports = other.CanExportReports;
            LastLoginTime = other.LastLoginTime;
            IsActive = other.IsActive;
            CreatedAt = other.CreatedAt;
            otp_active = other.otp_active;
        }
    }
}
