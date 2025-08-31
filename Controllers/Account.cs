using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
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
        public IActionResult Transaction([FromBody] JObject data)
        {
            //var amount = data["Amount"]?.ToObject<decimal>();
            //var recipient = data["RecipientId"]?.ToObject<string?>();

            decimal amount = 0;
            string recipient = "";
            string Type = "";
            string Email = "";

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

            }


            if (Type == "withdraw")
            {
                SecureBank_Pro.Services.Account.WithdrawMoney(_context, amount, Type , Email);
            }
            else if (Type == "deposit")
            {
                SecureBank_Pro.Services.Account.AddBalance(_context ,amount, Type , Email);
            }
            else if (Type == "transfer")
            {
                SecureBank_Pro.Services.Account.UserTransfer(_context , amount, Type, Email, recipient);
            }

            return PartialView("MainForm");
        }
    }
}
