using DatabaseLab.Domain.Dtos.ActorDetailsDtos;
using DatabaseLab.Domain.Entities;

namespace DatabaseLab.DAL.Interfaces;

public interface IActorDetailRepository : IRepository<ActorDetail>
{
    Task<IEnumerable<ActorWithPhone>> GetActorDetailsByAgencyId(long agencyId);
}
