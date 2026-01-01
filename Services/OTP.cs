using StackExchange.Redis;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using static System.Net.WebRequestMethods;

namespace SecureBank_Pro.Services
{
    public class OTP
    {
        private readonly IDatabase _redis;

        public OTP(IConnectionMultiplexer connection)
        {
            _redis = connection.GetDatabase();
        }

        public async Task<bool> SendOTP(string userId, string email)
        {
            try
            {
                email = "abhiradadiya987@gmail.com";
                var random = new Random();
                var otp = random.Next(100000, 999999).ToString();

                using var sha = SHA256.Create();
                var hash = Convert.ToHexString(sha.ComputeHash(Encoding.UTF8.GetBytes(otp)));

                await _redis.StringSetAsync($"OTP:{userId}", hash, TimeSpan.FromMinutes(2));

                var message = new MailMessage();
                message.To.Add(email);
                message.Subject = "Your OTP Code";
                message.Body = $"Your OTP is: {otp}\nIt will expire in 2 minutes.";

                using var smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.EnableSsl = true;

                smtp.Credentials = new NetworkCredential(
                    "abhiradadiya30012004@gmail.com",
                    "Abhi30@1040"
                );

                await smtp.SendMailAsync(message);

                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> VerifyOTP(string userId, string OTP)
        {
            try
            {
                using var sha = SHA256.Create();
                var hashInput = Convert.ToHexString(sha.ComputeHash(Encoding.UTF8.GetBytes(OTP)));

                var storedHash = await _redis.StringGetAsync($"OTP:{userId}");

                if (storedHash.IsNullOrEmpty) return false;

                return (hashInput == storedHash) ? true : false;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
