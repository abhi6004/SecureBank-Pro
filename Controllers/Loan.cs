using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecureBank_Pro.Data;

namespace SecureBank_Pro.Controllers
{
    public class LoanController : Controller
    {
        private readonly BankDbContext _context;
        public LoanController(BankDbContext context) => _context = context;

        [Authorize(Roles = "Customer")]
        public IActionResult Offers()
        {
            var offers = SecureBank_Pro.Services.Loan.GetOffers(_context);
            return View(offers);
        }
    }
}
