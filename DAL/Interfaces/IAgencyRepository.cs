using DatabaseLab.Domain.Dtos.ActorDtos;
using DatabaseLab.Domain.Dtos.AgencyDtos;
using DatabaseLab.Domain.Entities;

namespace DatabaseLab.DAL.Interfaces;

public interface IAgencyRepository : IRepository<Agency>
{
    Task<IEnumerable<CountOfActorsInRank>> GetActorGroups(long agencyId);

    Task<IEnumerable<AgencyWithSpectacleBudget>> GetMaxMinSpectacleBudget();
}
