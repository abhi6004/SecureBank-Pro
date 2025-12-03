using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SecureBank_Pro.BankEntities;
using SecureBank_Pro.Data;
using SecureBank_Pro.Models;

namespace SecureBank_Pro.Controllers
{
    public class LoanController : Controller
    {
        private readonly BankDbContext _context;
        public LoanController(BankDbContext context) => _context = context;

        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Offers()
        {
            var _userObj = HttpContext.Session.GetString("UserData");

            var user = JsonConvert.DeserializeObject<Users>(_userObj);

            var loanString = SecureBank_Pro.Services.Loan.CustomerappliedLoan(_context, user.id);

            if(loanString != null)
            {
                HttpContext.Session.SetString("CustomerLoans", loanString);
            }
            else
            {
                HttpContext.Session.SetString("CustomerLoans", "");
            }


            var LoanData = await SecureBank_Pro.Services.Loan.GetOffers(_context , user.id);

            var json = JsonConvert.SerializeObject(LoanData, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            HttpContext.Session.SetString("LoanData", json);
            return View();
        }

        public async Task<IActionResult> ApplyLoan([FromBody] JObject data)
        {
            try
            {
                var _userObj = HttpContext.Session.GetString("UserData");
                var user = JsonConvert.DeserializeObject<Users>(_userObj);

                if(user != null && data != null)
                {
                    if (data["offerId"] != null)
                    {
                        int OfferId = (int)data["offerId"];
                        var response = await SecureBank_Pro.Services.Loan.ApplyLoan(_context, user.id, OfferId);
                        return Content(response.ToString());
                    }
                    else
                    {
                        return Content("Error");
                    }
                }
                else
                {
                    return Content("Error");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateHistory()   
        {
            try
            {
                var _userObj = HttpContext.Session.GetString("UserData");
                var user = JsonConvert.DeserializeObject<Users>(_userObj);

                if (user != null)
                {
                    var loanString = SecureBank_Pro.Services.Loan.CustomerappliedLoan(_context, user.id);

                    if (loanString != null)
                    {
                        HttpContext.Session.SetString("CustomerLoans", loanString);
                    }
                    else
                    {
                        HttpContext.Session.SetString("CustomerLoans", "");
                    }
                    LoanData loanData = HttpContext.Session.GetString("LoanData") != null ? Newtonsoft.Json.JsonConvert.DeserializeObject<LoanData>(HttpContext.Session.GetString("LoanData")) : null;
                    LoanData newloanData = await SecureBank_Pro.Services.Loan.HistoryUpdate(_context , user.id , loanData);

                    if (newloanData != null)
                    {
                        HttpContext.Session.SetString("LoanData", Newtonsoft.Json.JsonConvert.SerializeObject(newloanData, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
                    }
                    return Ok();
                }

                return BadRequest("");
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
