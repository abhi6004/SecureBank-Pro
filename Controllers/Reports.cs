using Microsoft.AspNetCore.Mvc;

namespace SecureBank_Pro.Controllers
{
    public class ReportsController : Controller
    {
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
