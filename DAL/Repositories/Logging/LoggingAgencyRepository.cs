using DatabaseLab.DAL.Interfaces;
using DatabaseLab.Domain.Dtos.ActorDtos;
using DatabaseLab.Domain.Entities;
using DatabaseLab.Domain.Interfaces;
using DatabaseLab.Services.Interfaces;

namespace DatabaseLab.DAL.Repositories.Logging;

public class LoggingAgencyRepository(
    IAgencyRepository agencyRepository,
    IReportService reportService) : IAgencyRepository
{
    private readonly IAgencyRepository _agencyRepository = agencyRepository;
    private readonly IReportService _reportService = reportService;

    public async Task<bool> CreateAsync(Agency entity)
    {
        var result = await _agencyRepository.CreateAsync(entity);
        await _reportService.LogToCacheAsync(nameof(CreateAsync), result, entity);

        return result;
    }

    public async Task<IEnumerable<CountOfActorsInRank>> GetActorGroups(long agencyId)
    {
        var collection = await _agencyRepository
            .GetActorGroups(agencyId);

        await _reportService.LogToCacheAsync(
            nameof(GetActorGroups), 
            collection);

        return collection;
    }

    public async Task<IQueryable<Agency>> GetAllAsync()
    {
        var collection = await _agencyRepository.GetAllAsync();
        await _reportService.LogToCacheAsync(nameof(GetAllAsync), collection);

        return collection;
    }

    public async Task<Agency?> GetByIdAsync(long id)
    {
        var agency = await _agencyRepository.GetByIdAsync(id);
        await _reportService.LogToCacheAsync(nameof(GetByIdAsync), agency);

        return agency;
    }

    public async Task<bool> RemoveAsync(long id)
    {
        var result = await _agencyRepository.RemoveAsync(id);
        await _reportService.LogToCacheAsync(nameof(RemoveAsync), result);

        return result;
    }

    public async Task<bool> UpdateAsync(Agency entity)
    {
        var result = await _agencyRepository.UpdateAsync(entity);
        await _reportService.LogToCacheAsync(nameof(UpdateAsync), result, entity);

        return result;
    }
}
