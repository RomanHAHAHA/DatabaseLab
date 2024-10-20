using DatabaseLab.Domain.Dtos.ActorDtos;
using DatabaseLab.Domain.Entities;

namespace DatabaseLab.DAL.Interfaces;

public interface IActorRepository : IRepository<Actor>
{ 
    Task<IEnumerable<ActorDataDto>> GetActorsData(DateTime birthday);

    Task<IEnumerable<ActorContractInfoDto>> GetActorWithContractsInfo();

    Task<IEnumerable<ActorSpectaclesCount>> GetActorsWithSpectaclesCount(int spectaclesCount);

    Task<IEnumerable<ActorWithBirthday>> GetActorsBornInMonth(int month);
}
