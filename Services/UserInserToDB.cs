using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SecureBank_Pro.BankEntities;
using SecureBank_Pro.Data;
using SecureBank_Pro.Models;
using System;

namespace SecureBank_Pro.Services
{
    public class UserInserToDB
    {
        public static async Task<bool> InsertUserToDB(BankDbContext _context, User user)
        {
            try
            {
                string email = user.Email;

                bool Ishave = await _context.Users.AnyAsync(c => c.email == email);

                if (Ishave)
                {
                    return false;
                }

                var dbUser = new Users
                {
                    full_name = user.FullName,
                    email = user.Email,
                    password_hash = user.PasswordHash,
                    phone_number = user.PhoneNumber,
                    role = user.Role,
                    address = user.Address,
                    date_of_birth = user.DateOfBirth,
                    kyc_status = user.KycStatus,
                    branch_assigned = user.BranchAssigned,
                    hire_date = user.HireDate,
                    role_title = user.RoleTitle,
                    assigned_region = user.AssignedRegion,
                    can_export_reports = user.CanExportReports,
                    last_login_time = user.LastLoginTime,
                    is_active = user.IsActive,
                    created_at = user.CreatedAt
                };

                await _context.Users.AddAsync(dbUser);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public static async Task<Users> UserLoginCheck(BankDbContext _context, string email, string password)
        {
            bool isLoginUser = await _context.Users.AnyAsync(u => u.email == email && u.password_hash == password);
            Users _user = await _context.Users.FirstOrDefaultAsync(u => u.email == email && u.password_hash == password);

            if (_user == null)
            {
                return null;
            }

            return _user;
        }

        public static async Task<bool> UserUpdate(BankDbContext _context, Users user)
        {
            try
            {
                var dbUser = await _context.Users.FirstOrDefaultAsync(u => u.email == user.email);
                if (dbUser == null) return false;

                dbUser.full_name = user.full_name;
                dbUser.email = user.email;
                dbUser.password_hash = user.password_hash;
                dbUser.phone_number = user.phone_number;
                dbUser.role = user.role;

                // Customer
                dbUser.address = user.address;
                dbUser.date_of_birth = user.date_of_birth;
                dbUser.kyc_status = user.kyc_status;

                // Manager/Employee
                dbUser.branch_assigned = user.branch_assigned;
                dbUser.hire_date = user.hire_date;
                dbUser.role_title = user.role_title;

                // Auditor
                dbUser.assigned_region = user.assigned_region;
                dbUser.can_export_reports = user.can_export_reports ?? "no";
                dbUser.last_login_time = user.last_login_time;

                dbUser.is_active = user.is_active;
                dbUser.created_at = user.created_at;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
