using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecureBank_Pro.BankEntities;
using SecureBank_Pro.Data;
using SecureBank_Pro.Services;

namespace SecureBank_Pro.Controllers
{
    [Authorize(AuthenticationSchemes = "UserCookies")]
    public class DashboardController : Controller
    {
        private readonly BankDbContext _context;
        public DashboardController(BankDbContext context) => _context = context;

        public IActionResult Profile()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Customers()
        {
            var Users = await GetUsers.FetchUsers("Customer", _context);
            return View(Users);
        }
        public async Task<IActionResult> Employee()
        {
            var Users = await GetUsers.FetchUsers("Employee", _context);
            return View(Users);
        }

        public async Task<IActionResult> Managers()
        {
            var Users = await GetUsers.FetchUsers("Manager", _context);
            return View(Users);
        }

        public async Task<IActionResult> Auditor()
        {
            var Users = await GetUsers.FetchUsers("Auditor", _context);
            return View(Users);
        }
    }
}
