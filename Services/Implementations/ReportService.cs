using DatabaseLab.Services.Interfaces;
using Newtonsoft.Json;

namespace DatabaseLab.Services.Implementations;

public class ReportService(ICacheService cacheService) : IReportService
{
    private readonly ICacheService _cacheService = cacheService;

    public async Task LogToCacheAsync(
        string requestType,
        bool success,
        object? entity = null)
    {
        var jsonEntity = entity is not null ? JsonConvert.SerializeObject(entity) : "NULL";
        var reportString = $"" +
            $"Request type: {requestType}, " +
            $"Date: {DateTime.Now:[dd.MM.yyyy][HH:mm]}, " +
            $"Success: {success}, " +
            $"Object: \n{jsonEntity}";

        await _cacheService.SetAsync(reportString);
    }

    public async Task LogToCacheAsync(
        string requestType,
        object? entity = null)
    {
        var jsonEntity = entity is not null ? JsonConvert.SerializeObject(entity) : "NULL";
        var reportString = $"" +
            $"Request type: {requestType}, " +
            $"Date: {DateTime.Now:[dd.MM.yyyy][HH:mm]}, " +
            $"Object: \n{jsonEntity}";

        await _cacheService.SetAsync(reportString);
    }
}
