using Microsoft.AspNetCore.Mvc;
using SecureBank_Pro.Models;

namespace SecureBank_Pro.Controllers
{
    public class UsersController : Controller
    {
        [HttpGet]
        public IActionResult UserForm(string Role)
        {
            // Fixed CS1002 and CS1717 by correcting object initialization syntax
            User user = new User { Role = Role };
            return View(user); // Pass the user object to the view
        }

        [HttpPost]
        public IActionResult UserForm(User user)
        {
            return View(user); // Pass the user object to the view
        }
    }
}
