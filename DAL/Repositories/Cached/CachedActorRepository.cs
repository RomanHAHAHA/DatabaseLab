using DatabaseLab.DAL.Interfaces;
using DatabaseLab.DAL.Repositories.Default;
using DatabaseLab.Domain.Entities;
using DatabaseLab.Services.Interfaces;

namespace DatabaseLab.DAL.Repositories.Cached;

public class CachedActorRepository(
    ActorRepository actorRepository,
    ICacheService cacheService) : IRepository<Actor>
{
    private readonly ActorRepository _actorRepository = actorRepository;
    private readonly ICacheService _cacheService = cacheService;

    public async Task<bool> CreateAsync(Actor entity)
    {
        var result = await _actorRepository.CreateAsync(entity);

        return result;
    }

    public async Task<IQueryable<Actor>> GetAllAsync()
    {
        var collection = await _actorRepository.GetAllAsync();

        return collection;
    }

    public async Task<Actor?> GetByIdAsync(long id)
    {
        var actor = await _actorRepository.GetByIdAsync(id);

        return actor;
    }

    public async Task<bool> RemoveAsync(long id)
    {
        var result = await _actorRepository.RemoveAsync(id);

        return result;
    }

    public async Task<bool> UpdateAsync(Actor entity)
    {
        var result = await _actorRepository.UpdateAsync(entity);

        return result;
    }
}
