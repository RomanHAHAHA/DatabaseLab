using DatabaseLab.DAL.Interfaces;
using DatabaseLab.Domain.Dtos.ActorDtos;
using DatabaseLab.Domain.Entities;
using DatabaseLab.Services.Interfaces;

namespace DatabaseLab.DAL.Repositories.Logging;

public class LoggingActorRepository(
    IActorRepository actorRepository,
    IReportService reportService) : IActorRepository
{
    private readonly IActorRepository _actorRepository = actorRepository;
    private readonly IReportService _reportService = reportService;

    #region CRUD operations
    public async Task<bool> CreateAsync(Actor entity)
    {
        var result = await _actorRepository.CreateAsync(entity);
        await _reportService.LogToCacheAsync(nameof(CreateAsync), result, entity);

        return result;
    }

    public async Task<IQueryable<Actor>> GetAllAsync()
    {
        var collection = await _actorRepository.GetAllAsync();
        await _reportService.LogToCacheAsync(nameof(GetAllAsync), collection);

        return collection;
    }

    public async Task<Actor?> GetByIdAsync(long id)
    {
        var actor = await _actorRepository.GetByIdAsync(id);
        await _reportService.LogToCacheAsync(nameof(GetByIdAsync), actor);

        return actor;
    }

    public async Task<bool> RemoveAsync(long id)
    {
        var result = await _actorRepository.RemoveAsync(id);
        await _reportService.LogToCacheAsync(nameof(RemoveAsync), result);

        return result;
    }

    public async Task<bool> UpdateAsync(Actor entity)
    {
        var result = await _actorRepository.UpdateAsync(entity);
        await _reportService.LogToCacheAsync(nameof(UpdateAsync), result, entity);

        return result;
    }
    #endregion

    public async Task<IEnumerable<ActorDataDto>> GetActorsData(DateTime birthday)
    {
        var collection = await _actorRepository
            .GetActorsData(birthday);

        await _reportService.LogToCacheAsync(
            nameof(GetActorsData), 
            collection);

        return collection;
    }

    public async Task<IEnumerable<ActorContractInfoDto>> GetActorWithContractsInfo()
    {
        var collection = await _actorRepository
            .GetActorWithContractsInfo();

        await _reportService.LogToCacheAsync(
            nameof(GetActorWithContractsInfo), 
            collection);

        return collection;
    }

    public async Task<IEnumerable<ActorSpectaclesCount>> GetActorsWithSpectaclesCount(int spectaclesCount)
    {
        var collection  = await _actorRepository
            .GetActorsWithSpectaclesCount(spectaclesCount);

        await _reportService.LogToCacheAsync(
            nameof(GetActorsWithSpectaclesCount),
            collection);

        return collection;
    }

    public async Task<IEnumerable<ActorWithBirthday>> GetActorsBornInMonth(int month)
    {
        var collection = await _actorRepository
            .GetActorsBornInMonth(month);

        await _reportService.LogToCacheAsync(
            nameof(GetActorsBornInMonth),
            collection);

        return collection;
    }
}
