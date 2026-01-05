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

        [HttpGet]
        public async Task<IActionResult> TransactionReportTable(int pageSize, int pageNumber , string Category , int UserId)
        {
            try
            {
                var transactions =
                    await SecureBank_Pro.Services.Reports.GenerateTransactionReport(
                        pageSize,
                        pageNumber,
                        Category,
                        UserId,
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

        [HttpGet]
        public async Task<IActionResult> DownloadPdf(int pageSize, int pageNumber , string Category, int UserId)
        {
            try
            {
                var transactions =
                    await SecureBank_Pro.Services.Reports.GenerateTransactionReport(
                        pageSize,
                        pageNumber,
                        Category,
                        UserId,
                        _context,
                        _http
                    );

                var document = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Margin(40);
                        page.Size(PageSizes.A4);
                        page.PageColor(Colors.White);
                        page.DefaultTextStyle(x => x.FontSize(11));

                        page.Header()
                            .Row(row =>
                            {
                                row.RelativeItem().Column(col =>
                                {
                                    col.Item().Text("Transaction Report")
                                        .SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);

                                    col.Item().Text($"Generated: {DateTime.Now:dd MMM yyyy, hh:mm tt}")
                                        .FontSize(9).FontColor(Colors.Grey.Medium);
                                });
                            });

                        page.Content().PaddingVertical(10).Table(table =>
                        {
                            // ---------- Column Layout ----------
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(40);  // #
                                columns.RelativeColumn();    // User Id
                                columns.RelativeColumn(2);   // Type
                                columns.RelativeColumn(2);   // Description
                                columns.RelativeColumn();    // Amount
                                columns.RelativeColumn();    // Balance
                                columns.RelativeColumn(2);   // Date
                            });

                            // ---------- Header ----------
                            table.Header(header =>
                            {
                                header.Cell().Element(HeaderCell).Text("#");
                                header.Cell().Element(HeaderCell).Text("User Id");
                                header.Cell().Element(HeaderCell).Text("Transaction Type");
                                header.Cell().Element(HeaderCell).Text("Description");
                                header.Cell().Element(HeaderCell).Text("Amount");
                                header.Cell().Element(HeaderCell).Text("Balance After");
                                header.Cell().Element(HeaderCell).Text("Date");
                            });

                            // ---------- Rows ----------
                            int index = 1;
                            foreach (var tx in transactions)
                            {
                                bool isEven = index % 2 == 0;

                                table.Cell().Element(c => DataCell(c, isEven)).Text(index.ToString());
                                table.Cell().Element(c => DataCell(c, isEven)).Text(tx.UserId.ToString());
                                table.Cell().Element(c => DataCell(c, isEven)).Text(tx.TransactionType);
                                table.Cell().Element(c => DataCell(c, isEven)).Text(tx?.Description ?? "-");

                                table.Cell().Element(c => DataCellRight(c, isEven))
                                    .Text(tx.Amount.ToString("C"));

                                table.Cell().Element(c => DataCellRight(c, isEven))
                                    .Text(tx.BalanceAfter.ToString("C"));

                                table.Cell().Element(c => DataCell(c, isEven))
                                    .Text(tx.CreatedAt.ToString("dd MMM yyyy"));

                                index++;
                            }

                            // ---------- Styles ----------
                            IContainer HeaderCell(IContainer container) =>
                                container
                                    .Padding(6)
                                    .BorderBottom(1)
                                    .BorderColor(Colors.Grey.Medium)
                                    .Background(Colors.Grey.Lighten3)
                                    .DefaultTextStyle(x => x.SemiBold());

                            IContainer DataCell(IContainer container, bool even) =>
                                container
                                    .Padding(6)
                                    .Background(even ? Colors.Grey.Lighten4 : Colors.White);

                            IContainer DataCellRight(IContainer container, bool even) =>
                                DataCell(container, even).AlignRight();
                        });

                        page.Footer()
                            .AlignCenter()
                            .Text(x =>
                            {
                                x.Span("Page ");
                                x.CurrentPageNumber();
                                x.Span(" of ");
                                x.TotalPages();
                            });
                    });
                });

                return File(document.GeneratePdf(), "application/pdf", "TransactionReport.pdf");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest();
            }
        }

    }
}
