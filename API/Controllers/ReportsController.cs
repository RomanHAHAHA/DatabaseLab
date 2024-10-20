using Microsoft.AspNetCore.Mvc;
using DatabaseLab.Services.Interfaces;

namespace DatabaseLab.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReportsController(
    IPdfGenerator pdfGenerator,
    ICacheService cacheService) : ControllerBase
{
    private readonly IPdfGenerator _pdfGenerator = pdfGenerator;
    private readonly ICacheService _cacheService = cacheService;

    [HttpGet("generate-report")]
    public async Task<IActionResult> GenerateReport()
    {
        var reportData = await _cacheService.GetAllAsync<string>();

        if (reportData is null || reportData.Count == 0)
        {
            return BadRequest(new { message = "No reports found in cache." });
        }

        var htmlContent = _pdfGenerator.GenerateHtmlReport(reportData);
        var pdfFile = _pdfGenerator.GeneratePdfFromHtml(htmlContent);

        return File(pdfFile, "application/pdf", "Report.pdf");
    }
}
