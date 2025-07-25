﻿using SecureBank_Pro.Data;
using SecureBank_Pro.Models;
using SecureBank_Pro.BankEntities;
using System;
using Microsoft.EntityFrameworkCore;

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

        public static async Task<bool> UserLoginCheck(BankDbContext _context, string email, string password)
        {
            bool isLoginUser = await _context.Users.AnyAsync(u => u.email == email && u.password_hash == password);
            return isLoginUser;
        }
    }
}
