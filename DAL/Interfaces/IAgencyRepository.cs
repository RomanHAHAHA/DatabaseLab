using DatabaseLab.Domain.Dtos.ActorDtos;
using DatabaseLab.Domain.Entities;

namespace DatabaseLab.DAL.Interfaces;

public interface IAgencyRepository : IRepository<Agency>
{
    Task<IEnumerable<CountOfActorsInRank>> GetActorGroups(long agencyId);
}
