using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecureBank_Pro.Data;

namespace SecureBank_Pro.Controllers
{
    [Authorize(Roles = "Manager")]
    public class LoanApplicationsController : Controller
    {
        private readonly BankDbContext _context;
        public LoanApplicationsController(BankDbContext context) => _context = context;

        public async Task<IActionResult> NewApplications()
        {
            var newApplications = await Services.LoanApplications.GetNewApplications(_context);
            HttpContext.Session.SetString("NewApplications", System.Text.Json.JsonSerializer.Serialize(newApplications));
            return View();
        }

        public async Task<IActionResult> HistoryOfApplications()
        {
            return View();
        }

        public async Task<IActionResult> LoanInfo()
        {
            return View();
        }
    }
}
