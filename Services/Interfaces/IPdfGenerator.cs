namespace DatabaseLab.Services.Interfaces;

public interface IPdfGenerator
{
    string GenerateHtmlReport(List<string> reportData);

    byte[] GeneratePdfFromHtml(string htmlContent);
}