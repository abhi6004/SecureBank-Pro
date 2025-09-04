using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SecureBank_Pro.BankEntities;
using SecureBank_Pro.Data;

namespace SecureBank_Pro.Controllers
{
    public class AccountController : Controller
    {
        private readonly BankDbContext _context;
        public AccountController(BankDbContext context) => _context = context;

        [HttpGet]
        public IActionResult TransactionForm(string email)
        {
            ViewBag.email = email;
            return PartialView("MainForm");
        }

        [HttpPost]
        public async Task<IActionResult> Transaction([FromBody] JObject data)
        {
            decimal amount = 0;
            string recipient = "";
            string Type = "";
            string Email = "";
            string Description = "";

            if (data != null)
            {
                if (data["Amount"] != null)
                {
                    amount = (decimal)data["Amount"];
                }

                if (data["RecipientId"] != null)
                {
                    recipient = (string)data["RecipientId"];
                }

                if (data["Type"] != null)
                {
                    Type = (string)data["Type"];
                }

                if (data["Email"] != null)
                {
                    Email = (string)data["Email"];
                }

                if (data["Description"] != null)
                {
                    Description = (string)data["Description"];
                }

            }


            if (Type == "withdraw")
            {
                Users user = await SecureBank_Pro.Services.GetUsers.GetUserById(Email, _context);

                if (user == null)
                {
                    return BadRequest("User not found.");
                }

                await SecureBank_Pro.Services.Account.WithdrawMoney(_context, amount, Type, user.id);
                decimal balance = await SecureBank_Pro.Services.Account.CheckBalance(_context, user.id);

                await SecureBank_Pro.Services.Account.TransectionEntry(_context, Description, amount, Type, user.id, balance);
            }
            else if (Type == "deposit")
            {
                Users user = await SecureBank_Pro.Services.GetUsers.GetUserById(Email, _context);

                if (user == null)
                {
                    return BadRequest("User not found.");
                }

                bool isSuccess = await SecureBank_Pro.Services.Account.AddBalance(_context, amount, Type, user.id);

                if (isSuccess)
                {
                    decimal balance = await SecureBank_Pro.Services.Account.CheckBalance(_context, user.id);

                    await SecureBank_Pro.Services.Account.TransectionEntry(_context, Description, amount, Type, user.id, balance);
                }
            }
            else if (Type == "transfer")
            {
                Users user = await SecureBank_Pro.Services.GetUsers.GetUserById(Email, _context);
                Users recipientUser = await SecureBank_Pro.Services.GetUsers.GetUserById(recipient, _context);

                if (user == null || recipientUser == null)
                {
                    return Json(new { success = false, message = "User not found." });
                }

                await SecureBank_Pro.Services.Account.WithdrawMoney(_context, amount, Type, user.id);
                decimal senderBalance = await SecureBank_Pro.Services.Account.CheckBalance(_context, user.id);
                await SecureBank_Pro.Services.Account.TransectionEntry(_context, $"Transfer to {recipientUser.email}", amount, Type, user.id, senderBalance);

                bool recipientSuccess = await SecureBank_Pro.Services.Account.AddBalance(_context, amount, Type, recipientUser.id);

                if (recipientSuccess)
                {
                    decimal recipientBalance = await SecureBank_Pro.Services.Account.CheckBalance(_context, recipientUser.id);
                    await SecureBank_Pro.Services.Account.TransectionEntry(_context, $"Transfer from {user.email} {Description}", amount, Type, recipientUser.id, recipientBalance);
                }
            }

            return PartialView("MainForm");
        }
    }
}
