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
            try
            {
                var _userObj = HttpContext.Session.GetString("UserData");

                var user = JsonConvert.DeserializeObject<Users>(_userObj);
                return View(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        [Authorize(Roles = "Employee , Manager")]
        public async Task<IActionResult> Customers()
        {
            try
            {
                List<Users> Users = await GetUsers.FetchUsers("Customer", _context);
                return View(Users);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
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
            try
            {
                List<Users> Users = await GetUsers.FetchUsers("Manager", _context);
                return View(Users);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Auditor()
        {
            try
            {

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            List<Users> Users = await GetUsers.FetchUsers("Auditor", _context);
            return View(Users);
        }

        [HttpGet]
        [Authorize(Roles = "Employee , Manager")]
        public IActionResult UserForm(string Role)
        {
            try
            {
                ModelState.Clear();
                ViewBag.currentRole = User.FindFirst(ClaimTypes.Role).Value;
                ViewBag.Role = Role;
                TempData["role"] = Role;

                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> UserForm(User user)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditUsers(string email)
        {
            try
            {
                Users newUser = await GetUsers.GetUserById(email, _context);
                return View(newUser);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditUsers(Users users)
        {
            try
            {
                string role = users.role;
                bool isUpdate = await UserInserToDB.UserUpdate(_context, users);

                if (role == "Customer")
                {
                    role = "Customers";
                }

                if (role == "Manager")
                {
                    role = "Managers";
                }

                return RedirectToAction(role, "Dashboard");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<IActionResult> CustomerProfile(string email)
        {
            try
            {
                UserProfile userProfile = await GetUsers.GetUserProfile(email, _context);
                return View(userProfile);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<IActionResult> UploadDocuments(int id)
        {
            try
            {
                UserFiles _UserFile = await SecureBank_Pro.Services.Upload.GetUserFile(id , _context);
                ViewBag.userId = id;
                return View(_UserFile);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile signatureFile, IFormFile profileFile, int userId)
        {
            try
            {
                if (signatureFile != null)
                {
                    await SecureBank_Pro.Services.Upload.UploadFiles(signatureFile, userId, "Signature", _context);
                }
                if (profileFile != null)
                {
                    await SecureBank_Pro.Services.Upload.UploadFiles(profileFile, userId, "ProfilePicture", _context);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            return Redirect(Request.Path);

        }
    }
}
