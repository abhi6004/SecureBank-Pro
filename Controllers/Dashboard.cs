using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SecureBank_Pro.BankEntities;
using SecureBank_Pro.Data;
using SecureBank_Pro.Models;
using SecureBank_Pro.Services;
using System.Security.Claims;

namespace SecureBank_Pro.Controllers
{
    [Authorize(AuthenticationSchemes = "UserCookies")]
    public class DashboardController : Controller
    {
        private readonly BankDbContext _context;
        public DashboardController(BankDbContext context) => _context = context;

        public IActionResult Profile()
        {
            var _userObj = HttpContext.Session.GetString("UserData");

            var user = JsonConvert.DeserializeObject<Users>(_userObj);
            return View(user);
        }

        [Authorize(Roles = "Employee , Manager")]
        public async Task<IActionResult> Customers()
        {
            var Users = await GetUsers.FetchUsers("Customer", _context);
            return View(Users);
        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Employee()
        {
            var Users = await GetUsers.FetchUsers("Employee", _context);
            return View(Users);
        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Managers()
        {
            var Users = await GetUsers.FetchUsers("Manager", _context);
            return View(Users);
        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Auditor()
        {
            var Users = await GetUsers.FetchUsers("Auditor", _context);
            return View(Users);
        }

        [HttpGet]
        [Authorize(Roles = "Employee , Manager")]
        public IActionResult UserForm(string Role)
        {
            ViewBag.Role = User.FindFirst(ClaimTypes.Role).Value;
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
    }
}
