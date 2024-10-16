
namespace DatabaseLab.Services.Interfaces
{
    public interface IReportService
    {
        Task LogToCacheAsync(string requestType, object? entity = null);

        Task LogToCacheAsync(string requestType, bool success, object? entity = null);
    }
}