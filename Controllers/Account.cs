using Microsoft.AspNetCore.Mvc;

namespace SecureBank_Pro.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult TransactionForm()
        {
            return PartialView("MainForm");
        }
    }
}
