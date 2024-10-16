using DatabaseLab.Services.Interfaces;
using DinkToPdf;
using DinkToPdf.Contracts;
using System.Text;

namespace DatabaseLab.Services.Implementations;

public class PdfGenerator(IConverter converter) : IPdfGenerator
{
    private readonly IConverter _converter = converter;

    public string GenerateHtmlReport(List<string> reportData)
    {
        var sb = new StringBuilder();
        sb.Append("<html><head><style>table { width: 100%; } th, td " +
            "{ padding: 10px; border: 1px solid black; }</style></head><body>");
        sb.Append("<h2>Report</h2>");
        sb.Append("<table>");
        sb.Append("<tr><th>#</th><th>Data</th></tr>");

        for (int i = 0; i < reportData.Count; i++)
        {
            sb.Append($"<tr><td>{i + 1}</td><td>{reportData[i]}</td></tr>");
        }

        sb.Append("</table>");
        sb.Append("</body></html>");

        return sb.ToString();
    }

    public byte[] GeneratePdfFromHtml(string htmlContent)
    {
        var document = new HtmlToPdfDocument
        {
            GlobalSettings = new GlobalSettings
            {
                PaperSize = PaperKind.A4,
                Orientation = Orientation.Portrait,
            },
            Objects = {
            new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = htmlContent,
                WebSettings = { DefaultEncoding = "utf-8" }
            }
        }
        };

        return _converter.Convert(document);
    }
}
