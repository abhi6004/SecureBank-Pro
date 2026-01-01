using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SecureBank_Pro.BankEntities;
using SecureBank_Pro.Data;
using SecureBank_Pro.Models;
using SecureBank_Pro.Services;
using StackExchange.Redis;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SecureBank_Pro.Controllers
{
    public class UsersController : Controller
    {

        private readonly IDatabase _redis;
        private readonly BankDbContext _context;

        public UsersController(IConnectionMultiplexer connection, BankDbContext context)
        {
            _redis = connection.GetDatabase();
            _context = context;
        }


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
                    if (!isLogin.otp_active)
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
                        return RedirectToAction("VerifyOTP", "Users", new { email = email ,  role = isLogin.role , full_name = isLogin.full_name });
                    }
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

        [HttpGet]
        public async Task<IActionResult> VerifyOTP(string email , string role , string full_name)
        {
            try
            {
                ViewBag.email = email;
                ViewBag.role = role;
                ViewBag.full_name = full_name;

                bool isOTPsend = await OTP.SendOTP(_redis , email);
                return View();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CheckOTP(string otp, string role, string email , string full_name)
        {
            try
            {
                Users isLogin = await UserInserToDB.GetuserFromEmail(_context, email);

                if(otp != null)
                {
                    bool isCorrect = await OTP.VerifyOTP(_redis, email , otp);
                    var claims = new List<Claim>
                        {
                        new Claim(ClaimTypes.Email, email) ,
                        new Claim(ClaimTypes.Role , role)
                        };
                    var claimsIdentity = new ClaimsIdentity(claims, "UserCookies");

                    ViewBag.UserName = full_name;
                    HttpContext.Session.SetString("UserData", JsonConvert.SerializeObject(isLogin));
                    await HttpContext.SignInAsync("UserCookies", new ClaimsPrincipal(claimsIdentity));
                    return RedirectToAction("Profile", "Dashboard");
                }
                else
                {
                    throw new Exception("Invalid OTP");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
