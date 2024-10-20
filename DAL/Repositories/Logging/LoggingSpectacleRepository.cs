using DatabaseLab.DAL.Interfaces;
using DatabaseLab.Domain.Dtos.ActorDtos;
using DatabaseLab.Domain.Dtos.SpectacleDtos;
using DatabaseLab.Domain.Entities;
using DatabaseLab.Services.Interfaces;

namespace DatabaseLab.DAL.Repositories.Logging;

public class LoggingSpectacleRepository(
    ISpectacleRepository spectacleRepository,
    IReportService reportService) : ISpectacleRepository
{
    private readonly ISpectacleRepository _spectacleRepository = spectacleRepository;
    private readonly IReportService _reportService = reportService;

    public async Task<bool> CreateAsync(Spectacle entity)
    {
        var result = await _spectacleRepository.CreateAsync(entity);
        await _reportService.LogToCacheAsync(nameof(CreateAsync), result, entity);
        return result;
    }

    public async Task<IQueryable<Spectacle>> GetAllAsync()
    {
        var collection = await _spectacleRepository.GetAllAsync();
        await _reportService.LogToCacheAsync(nameof(GetAllAsync), collection);
        return collection;
    }

    public async Task<Spectacle?> GetByIdAsync(long id)
    {
        var spectacle = await _spectacleRepository.GetByIdAsync(id);
        await _reportService.LogToCacheAsync(nameof(GetByIdAsync), spectacle);
        return spectacle;
    }

    public async Task<bool> RemoveAsync(long id)
    {
        var result = await _spectacleRepository.RemoveAsync(id);
        await _reportService.LogToCacheAsync(nameof(RemoveAsync), result);
        return result;
    }

    public async Task<bool> UpdateAsync(Spectacle entity)
    {
        var result = await _spectacleRepository.UpdateAsync(entity);
        await _reportService.LogToCacheAsync(nameof(UpdateAsync), result, entity);
        return result;
    }

    public async Task<IEnumerable<SpectacleTotalDto>> GetWithTotalContractPrice()
    {
        var collection = await _spectacleRepository
            .GetWithTotalContractPrice();

        await _reportService.LogToCacheAsync(
            nameof(GetWithTotalContractPrice), 
            collection);

        return collection;
    }

    public async Task<IEnumerable<ActorWithAgencyInfo>> GetActorsWithAgencyName(long spectacleId)
    {
        var collection = await _spectacleRepository
            .GetActorsWithAgencyName(spectacleId);

        await _reportService.LogToCacheAsync(
            nameof(GetActorsWithAgencyName), 
            collection);

        return collection;
    }

    public async Task<IEnumerable<SpectacleWithActors>> GetSpectacleActors()
    {
        var collection = await _spectacleRepository
            .GetSpectacleActors();

        await _reportService.LogToCacheAsync(
            nameof(GetSpectacleActors),
            collection);

        return collection;
    }

    public async Task<IEnumerable<TotalSpectaclesBudget>> GetSpectaclesWithTotalBudget()
    {
        var collection = await _spectacleRepository
            .GetSpectaclesWithTotalBudget();

        await _reportService.LogToCacheAsync(
            nameof(GetSpectaclesWithTotalBudget),
            collection);

        return collection;
    }
}
