using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SecureBank_Pro.Data;
using SecureBank_Pro.Models;
using SecureBank_Pro.Services;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

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

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email),
                new Claim("password", password)
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            if (isLogin)
            {
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                return RedirectToAction("Index", "Home");
            }
            else
            {
                throw new Exception("Invalid User Login");
            }
        }
    }
}
