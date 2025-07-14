using Microsoft.AspNetCore.Mvc;
using SecureBank_Pro.Data;
using SecureBank_Pro.Models;
using SecureBank_Pro.Services;

namespace SecureBank_Pro.Controllers
{
    public class UsersController : Controller
    {

        private readonly BankDbContext _context;
        public UsersController(BankDbContext context) => _context = context;

        [HttpGet]
        public IActionResult UserForm(string Role)
        {
            User user = new User { Role = Role }; 
            TempData["role"] = Role;
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> UserForm(User user)
        {
            user.Role = TempData["role"].ToString();
            bool isUserCreate = await UserInserToDB.InsertUserToDB(_context, user);
            if (isUserCreate)
            {
                await UserInserToDB.InsertUserToDB(_context, user);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                throw new Exception("User Is Alredy Create");
            }

        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Login(string email, string password)
        {
            bool isLogin = await UserInserToDB.UserLoginCheck(_context, email, password);

            if (isLogin)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                throw new Exception("Invalid User Login");
            }
        }
    }
}
