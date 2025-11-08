using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecureBank_Pro.Data;
using SecureBank_Pro.BankEntities;
using Newtonsoft.Json;

namespace SecureBank_Pro.Controllers
{
    public class LoanController : Controller
    {
        private readonly BankDbContext _context;
        public LoanController(BankDbContext context) => _context = context;

        [Authorize(Roles = "Customer")]
        public IActionResult Offers()
        {
            var _userObj = HttpContext.Session.GetString("UserData");

            var user = JsonConvert.DeserializeObject<Users>(_userObj);

            var loanString = SecureBank_Pro.Services.Loan.CustomerappliedLoan(_context , user.id);
            HttpContext.Session.SetString("CustomerLoans", loanString);

            var offers = SecureBank_Pro.Services.Loan.GetOffers(_context);
            return View(offers);
        }
    }
}
