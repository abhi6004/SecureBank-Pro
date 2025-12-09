using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SecureBank_Pro.BankEntities;
using SecureBank_Pro.Data;
using SecureBank_Pro.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using SystemTextJson = System.Text.Json.JsonSerializer;

namespace SecureBank_Pro.Controllers
{
    [Authorize(Roles = "Manager")]
    public class LoanApplicationsController : Controller
    {
        private readonly BankDbContext _context;
        public LoanApplicationsController(BankDbContext context) => _context = context;

        public async Task<IActionResult> NewApplications()
        {
            var newApplications = await Services.LoanApplications.GetNewApplications(_context);
            HttpContext.Session.SetString("NewApplications", SystemTextJson.Serialize(newApplications, new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            })
            );

            return View();
        }

        public async Task<IActionResult> HistoryOfApplications()
        {
            return View();
        }

        public async Task<IActionResult> LoanInfo()
        {
            return View();
        }

        public IActionResult Details(string id)
        {
            try
            {
                var lonaAppilcationJson = HttpContext.Session.GetString("NewApplications");
                List<NewApplications> loanApplication = SystemTextJson.Deserialize<List<NewApplications>>(lonaAppilcationJson!);

                var selectedApplication = loanApplication?.FirstOrDefault(a => a.ApplicationId.ToString() == id);

                return PartialView("_Details" , selectedApplication);

            }
            catch (Exception ex)
            {
                throw new Exception("Error in LoanApplicationsController.Details: " + ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ApplicationProcess([FromBody] JObject data)
        {
            if(data != null)
            {
                if (data["id"] != null && data["status"] != null)
                {
                    int id = data["id"].Value<int>();
                    string status = data["status"].Value<string>();
                    var _userObj = HttpContext.Session.GetString("UserData");
                    var user = JsonConvert.DeserializeObject<Users>(_userObj);

                    bool isSuccess = await SecureBank_Pro.Services.LoanApplications.ApplicationUpdate(id, status , _context, user.id);

                    var newApplications = await Services.LoanApplications.GetNewApplications(_context);
                    HttpContext.Session.SetString("NewApplications", SystemTextJson.Serialize(newApplications, new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.IgnoreCycles
                    })
                    );

                    return Json(new { success = isSuccess });
                }
                else
                {
                    return Json(new { success = "false" });

                }
            }
            else
            {
                return Json(new { success = "false" });
            }
        }
    }
}
