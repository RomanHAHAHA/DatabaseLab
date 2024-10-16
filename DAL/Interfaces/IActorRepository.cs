using DatabaseLab.Domain.Dtos.ActorDtos;
using DatabaseLab.Domain.Entities;

namespace DatabaseLab.DAL.Interfaces;

public interface IActorRepository : IRepository<Actor>
{ 
    Task<IEnumerable<ActorDataDto>> GetActorsData();

    Task<IEnumerable<ActorContractInfoDto>> GetActorWithContractsInfo(decimal minAveragePrice);
}
