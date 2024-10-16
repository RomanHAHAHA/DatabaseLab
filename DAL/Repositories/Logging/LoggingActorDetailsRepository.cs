using DatabaseLab.DAL.Interfaces;
using DatabaseLab.Domain.Dtos.ActorDetailsDtos;
using DatabaseLab.Domain.Entities;
using DatabaseLab.Services.Interfaces;

namespace DatabaseLab.DAL.Repositories.Logging;

public class LoggingActorDetailsRepository(
    IActorDetailRepository actorDetailsRepository,
    IReportService reportService) : IActorDetailRepository
{
    private readonly IActorDetailRepository _actorDetailsRepository = actorDetailsRepository;
    private readonly IReportService _reportService = reportService;

    public async Task<bool> CreateAsync(ActorDetail entity)
    {
        var result = await _actorDetailsRepository.CreateAsync(entity);
        await _reportService.LogToCacheAsync(nameof(CreateAsync), result, entity);

        return result;
    }

    public async Task<IEnumerable<ActorWithPhone>> GetActorDetailsByAgencyId(long agencyId)
    {
        var collection = await _actorDetailsRepository
            .GetActorDetailsByAgencyId(agencyId);

        await _reportService.LogToCacheAsync(
            nameof(GetActorDetailsByAgencyId), 
            collection);

        return collection;
    }

    public async Task<IQueryable<ActorDetail>> GetAllAsync()
    {
        var collection = await _actorDetailsRepository.GetAllAsync();
        await _reportService.LogToCacheAsync(nameof(GetAllAsync), collection);

        return collection;
    }

    public async Task<ActorDetail?> GetByIdAsync(long id)
    {
        var actorDetail = await _actorDetailsRepository.GetByIdAsync(id);
        await _reportService.LogToCacheAsync(nameof(GetByIdAsync), actorDetail);

        return actorDetail;
    }

    public async Task<bool> RemoveAsync(long id)
    {
        var result = await _actorDetailsRepository.RemoveAsync(id);
        await _reportService.LogToCacheAsync(nameof(RemoveAsync), result);

        return result;
    }

    public async Task<bool> UpdateAsync(ActorDetail entity)
    {
        var result = await _actorDetailsRepository.UpdateAsync(entity);
        await _reportService.LogToCacheAsync(nameof(UpdateAsync), result, entity);

        return result;
    }
}
