using Microsoft.AspNetCore.Mvc;
using SecureBank_Pro.BankEntities;
using SecureBank_Pro.Data;
using static System.Net.WebRequestMethods;

namespace SecureBank_Pro.Controllers
{
    public class ReportsController : Controller
    {
        private readonly BankDbContext _context;
        private readonly IHttpContextAccessor _http;

        public ReportsController(BankDbContext context, IHttpContextAccessor http)
        {
            _context = context;
            _http = http;
        }

        public IActionResult Transaction()
        {
            return View();
        }

        public IActionResult EMI()
        {
            return View();
        }

        [HttpGet]
        public IActionResult TransactionReportTable(int pageSize, int pageNumber)
        {
            try
            {
                var transactions =
                    SecureBank_Pro.Services.Reports.GenerateTransactionReport(
                        pageSize,
                        pageNumber,
                        _context,
                        _http
                    );

                return PartialView("_transactionData", transactions);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult GetTotalPage()
        {
            try
            {
                int? totalPage = _http.HttpContext.Session.GetInt32("TotalPages");
                return Ok(new { totalPage = totalPage }); // wrap in Ok() and use proper object syntax
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest();
            }
        }

    }
}
