using Microsoft.AspNetCore.Mvc;
using SecureBank_Pro.Data;

namespace SecureBank_Pro.Controllers
{
    public class ReportsController : Controller 
    {
        private readonly BankDbContext _context;

        public ReportsController(BankDbContext context)
        {
            _context = context;
        }

        public IActionResult Transaction()
        {

            return View();
        }

        public IActionResult EMI()
        {
            return View();
        }
    }
}
