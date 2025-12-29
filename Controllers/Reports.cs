using Microsoft.AspNetCore.Mvc;
using SecureBank_Pro.BankEntities;
using SecureBank_Pro.Data;
using static System.Net.WebRequestMethods;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using QuestPDF.Helpers;


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
        public async Task<IActionResult> TransactionReportTable(int pageSize, int pageNumber)
        {
            try
            {
                var transactions =
                    await SecureBank_Pro.Services.Reports.GenerateTransactionReport(
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
                return Ok(new { totalPage = totalPage });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest();
            }
        }

        public async Task<IActionResult> DownloadPdf(int pageSize, int pageNumber)
        {
            try
            {
                var transactions =
                    await SecureBank_Pro.Services.Reports.GenerateTransactionReport(
                        pageSize,
                        pageNumber,
                        _context,
                        _http
                    );

                var doucment = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Margin(50);
                        page.Size(PageSizes.A4);
                        page.PageColor(Colors.White);
                        page.DefaultTextStyle(x => x.FontSize(12).FontColor(Colors.Black));
                        page.Header()
                            .Text("Transaction Report")
                            .SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);
                        page.Content()
                            .Table(table =>
                            {
                                // Define columns
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.ConstantColumn(50); // ID
                                    columns.RelativeColumn();   // From Account
                                    columns.RelativeColumn();   // To Account
                                    columns.RelativeColumn();   // Amount
                                    columns.RelativeColumn();   // Date
                                    columns.RelativeColumn();   // Type
                                });
                                // Header row
                                table.Header(header =>
                                {
                                    header.Cell().Element(CellStyle).Text("Trasactionsid");
                                    header.Cell().Element(CellStyle).Text("UserId");
                                    header.Cell().Element(CellStyle).Text("TransactionType");
                                    header.Cell().Element(CellStyle).Text("Amount");
                                    header.Cell().Element(CellStyle).Text("Amount");
                                    header.Cell().Element(CellStyle).Text("BalanceAfter");
                                    header.Cell().Element(CellStyle).Text("Description");
                                    header.Cell().Element(CellStyle).Text("CreatedAt");
                                });
                                // Data rows
                                foreach (var tx in transactions)
                                {
                                    table.Cell().Element(CellStyle).Text(tx.Trasactionsid.ToString());
                                    table.Cell().Element(CellStyle).Text(tx.UserId.ToString());
                                    table.Cell().Element(CellStyle).Text(tx.TransactionType);
                                    table.Cell().Element(CellStyle).Text(tx.Amount.ToString("C"));
                                    table.Cell().Element(CellStyle).Text(tx.BalanceAfter.ToString("C"));
                                    table.Cell().Element(CellStyle).Text(tx?.Description ?? "");
                                    table.Cell().Element(CellStyle).Text(tx.CreatedAt.ToString("yyyy-MM-dd"));
                                }
                                // Cell styling
                                IContainer CellStyle(IContainer cell)
                                {
                                    return cell.Padding(5).BorderBottom(1).BorderColor(Colors.Grey.Lighten2);
                                }
                            });
                        page.Footer()
                            .AlignCenter()
                            .Text(x =>
                            {
                                x.Span("Page ");
                                x.CurrentPageNumber();
                            });
                    });
                });

                return File(doucment.GeneratePdf(), "application/pdf", "TransactionReport.pdf");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest();
            }
        }

    }
}
