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
            List<Users> Users = await GetUsers.FetchUsers("Customer", _context);
            return View(Users);
        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Employee()
        {
            List<Users> Users = await GetUsers.FetchUsers("Employee", _context);
            return View(Users);
        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Managers()
        {
            List<Users> Users = await GetUsers.FetchUsers("Manager", _context);
            return View(Users);
        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Auditor()
        {
            List<Users> Users = await GetUsers.FetchUsers("Auditor", _context);
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

        [HttpGet]
        public async Task<IActionResult> EditUsers(string email)
        {
            Users newUser = await GetUsers.GetUserById(email, _context);
            return View(newUser);
        }

        [HttpPost]
        public async Task<IActionResult> EditUsers(Users users)
        {
            string role = users.role;
            bool isUpdate = await UserInserToDB.UserUpdate(_context, users);
            return RedirectToAction(role , "Dashboard");
        }

        public async Task<IActionResult> CustomerProfile(string email)
        {
            UserProfile userProfile = await GetUsers.GetUserProfile(email, _context);
            return View(userProfile);
        }
    }
}
