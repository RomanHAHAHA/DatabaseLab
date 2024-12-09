using Microsoft.AspNetCore.Mvc;
using DatabaseLab.Services.Interfaces;
using System.IO.Compression; 

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
            return BadRequest(new
            {
                message = "No reports found in cache."
            });
        }

        var zipStream = new MemoryStream();

        using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
        {
            for (int i = 0; i < reportData.Count; i++)
            {
                var htmlContent = _pdfGenerator.GenerateHtmlReport(reportData[i]);
                var pdfFile = _pdfGenerator.GeneratePdfFromHtml(htmlContent);

                var fileName = $"Report_{i + 1}.pdf";
                var zipEntry = archive.CreateEntry(fileName, CompressionLevel.Fastest);

                using var entryStream = zipEntry.Open();
                await entryStream.WriteAsync(pdfFile);
            }
        }

        zipStream.Position = 0;

        return File(zipStream, "application/zip", "Reports.zip");
    }
}
