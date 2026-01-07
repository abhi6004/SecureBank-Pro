using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SecureBank_Pro.Data;
using SecureBank_Pro.Services;

namespace SecureBank_Pro.Controllers
{
    public class AccountController : Controller
    {
        private readonly BankDbContext _context;
        private readonly HttpClient _httpClient;

        public AccountController(BankDbContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }

        [HttpGet]
        public IActionResult TransactionForm(string email)
        {
            ViewBag.email = email;
            return PartialView("MainForm");
        }

        [HttpPost]
        public async Task<IActionResult> Transaction([FromBody] JObject data)
        {
            string email = (string)data["Email"];
            string type = (string)data["Type"];
            string recipient = (string)data["RecipientId"];
            string description = (string)data["Description"];
            decimal amount = (decimal)data["Amount"];

            // Withdraw
            if (type == "withdraw")
                await Account.MoneyTransaction(_context, email, "withdraw", description, amount);

            // Deposit
            if (type == "deposit")
                await Account.MoneyTransaction(_context, email, "deposit", description, amount);

            // Transfer
            if (type == "transfer")
            {
                await Account.MoneyTransaction(_context, email, "withdraw", description, amount);
                await Account.MoneyTransaction(_context, recipient, "deposit", description, amount);
            }

            return Ok(email);
        }

        [HttpPost]
        public async Task<IActionResult> ConvertCurrencyLatest([FromBody] JObject data)
        {
            if (data == null || data["Amount"] == null || string.IsNullOrWhiteSpace(data["Amount"].ToString()))
                return Ok(new { conversionAmount = (decimal?)null });

            if (!decimal.TryParse(data["Amount"].ToString(), out decimal amount))
                return Ok(new { conversionAmount = (decimal?)null });

            string code = (string)data["CurrencyCode"];

            if (code == "INR")
                return Ok(new { conversionAmount = amount });

            var rate = HttpContext.Session.GetString("Rate");

            if (!string.IsNullOrEmpty(rate) && decimal.TryParse(rate, out decimal ex))
                return Ok(new { conversionAmount = amount * ex });

            decimal converted = await Account.Amountconversion(_httpClient, amount, code);

            return Ok(new { conversionAmount = converted });
        }


    }
}
