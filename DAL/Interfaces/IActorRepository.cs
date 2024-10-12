using DatabaseLab.Domain.Dtos.ActorDtos;
using DatabaseLab.Domain.Entities;
using DatabaseLab.Domain.Enums;

namespace DatabaseLab.DAL.Interfaces;

public interface IActorRepository : IRepository<Actor>
{
    Task<IEnumerable<Actor>> GetActorsWithExperience(int experianceYears);

    Task<IEnumerable<Actor>> GetActorsWithRank(ActorRank actorRank);

    Task<IEnumerable<Actor>> GetBySurnamePrefix(string searchString);

    Task<IEnumerable<ActorDataDto>> GetActorsData();

    Task<IEnumerable<ActorContractInfoDto>> GetActorWithContractsInfo(decimal minAveragePrice);
}
