using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SecureBank_Pro.BankEntities;
using SecureBank_Pro.Data;
using SecureBank_Pro.Services;

namespace SecureBank_Pro.Controllers
{
    public class AccountController : Controller
    {
        private readonly BankDbContext _context;
        private readonly HttpClient _httpClient;
        //private readonly ILogger<AccountController> _logger;

        public AccountController(BankDbContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
            //_logger = logger;
        }


        [HttpGet]
        public IActionResult TransactionForm(string email)
        {
            try
            {
                ViewBag.email = email;
                return PartialView("MainForm");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Transaction([FromBody] JObject data)
        {
            try
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

                    //_logger.LogInformation("Withdraw initiated for user: {Email}, Amount: {Amount}", Email, amount);
                    await SecureBank_Pro.Services.Account.WithdrawMoney(_context, amount, Type, user.id);
                    await Logs.LogTransaction($"Withdrawal of {amount} from {user.full_name} for {Description}");
                    decimal balance = await SecureBank_Pro.Services.Account.CheckBalance(_context, user.id);

                    await SecureBank_Pro.Services.Account.TransectionEntry(_context, Description, amount, Type, user.id, balance);
                    return Content(user.id.ToString());
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
                        await Logs.LogTransaction($"Deposit of {amount} from {user.full_name} for {Description}");
                        await SecureBank_Pro.Services.Account.TransectionEntry(_context, Description, amount, Type, user.id, balance);
                        return Content(user.id.ToString());
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
                        await Logs.LogTransaction($"Transfer of {amount} from {user.full_name} to {recipientUser.full_name} ({Description})");
                        await SecureBank_Pro.Services.Account.TransectionEntry(_context, $"Transfer from {user.email} {Description}", amount, Type, recipientUser.id, recipientBalance);
                        return Content(user.id.ToString());
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }

        [HttpPost]
        public async Task<IActionResult> ConvertCurrency([FromBody] JObject data)
        {
            try
            {
                if (data != null)
                {
                    if (!string.IsNullOrEmpty(data["Amount"]?.ToString()) && !string.IsNullOrEmpty(data["CurrencyCode"]?.ToString()))
                    {
                        decimal Amount = (decimal)data["Amount"];
                        string CurrencyCode = (string)data["CurrencyCode"];

                        decimal conversionAmount = await SecureBank_Pro.Services.Account.Amountconversion(_httpClient, Amount, CurrencyCode);
                        decimal ExchangeRate = conversionAmount / Amount;
                        HttpContext.Session.SetString("ConvertedAmount", ExchangeRate.ToString());
                        return Ok(new { conversionAmount = conversionAmount });
                    }
                    else
                    {
                        return new EmptyResult();
                    }
                }
                else
                {
                    return new EmptyResult();
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> ConvertCurrencyLatest([FromBody] JObject data)
        {
            try
            {
                if (data != null)
                {
                    if (!string.IsNullOrEmpty(data["Amount"]?.ToString()) && !string.IsNullOrEmpty(data["CurrencyCode"]?.ToString()))
                    {
                        decimal Amount = (decimal)data["Amount"];
                        string CurrencyCode = (string)data["CurrencyCode"];

                        if (CurrencyCode != "INR")
                        {
                            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("ConvertedAmount")))
                            {
                                decimal ExchangeRate = Convert.ToDecimal(HttpContext.Session.GetString("ConvertedAmount"));
                                decimal conversionAmount = Amount * ExchangeRate;
                                return Ok(new { conversionAmount = conversionAmount });
                            }
                            else
                            {
                                decimal conversionAmount = await SecureBank_Pro.Services.Account.Amountconversion(_httpClient, Amount, CurrencyCode);
                                decimal ExchangeRate = conversionAmount / Amount;
                                HttpContext.Session.SetString("ConvertedAmount", ExchangeRate.ToString());
                                return Ok(new { conversionAmount = conversionAmount });
                            }
                        }
                        else
                        {
                            return new EmptyResult();
                        }
                    }
                    else
                    {
                        return new EmptyResult();
                    }
                }
                else
                {
                    return new EmptyResult();
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }
        }
}
