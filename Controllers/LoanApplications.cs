using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecureBank_Pro.Data;
using SecureBank_Pro.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

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
            HttpContext.Session.SetString("NewApplications", JsonSerializer.Serialize(newApplications, new JsonSerializerOptions
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
                List<NewApplications> loanApplication = JsonSerializer.Deserialize<List<NewApplications>>(lonaAppilcationJson!);

                var selectedApplication = loanApplication?.FirstOrDefault(a => a.ApplicationId.ToString() == id);

                return PartialView("_Details" , selectedApplication);

            }
            catch (Exception ex)
            {
                throw new Exception("Error in LoanApplicationsController.Details: " + ex.Message);
            }
        }
    }
}
