using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SecureBank_Pro.BankEntities;
using SecureBank_Pro.Data;
using SecureBank_Pro.Models;
using SecureBank_Pro.Services;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SecureBank_Pro.Controllers
{
    public class UsersController : Controller
    {

        private readonly BankDbContext _context;
        public UsersController(BankDbContext context) => _context = context;

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            await HttpContext.SignOutAsync("UserCookies");
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Login(string email, string password)
        {
            try
            {
                Users isLogin = await UserInserToDB.UserLoginCheck(_context, email, password);

                if (isLogin != null)
                {
                    var claims = new List<Claim>
                {
                new Claim(ClaimTypes.Email, email) ,
                new Claim(ClaimTypes.Role , isLogin.role)
                };
                    var claimsIdentity = new ClaimsIdentity(claims, "UserCookies");

                    ViewBag.UserName = isLogin.full_name;
                    HttpContext.Session.SetString("UserData", JsonConvert.SerializeObject(isLogin));
                    await HttpContext.SignInAsync("UserCookies", new ClaimsPrincipal(claimsIdentity));
                    return RedirectToAction("Profile", "Dashboard");
                }
                else
                {
                    throw new Exception("Invalid User Login");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("UserCookies");
            return RedirectToAction("Login", "Users");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
