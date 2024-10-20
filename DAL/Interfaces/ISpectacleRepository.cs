using DatabaseLab.Domain.Dtos.ActorDtos;
using DatabaseLab.Domain.Dtos.SpectacleDtos;
using DatabaseLab.Domain.Entities;

namespace DatabaseLab.DAL.Interfaces;

public interface ISpectacleRepository : IRepository<Spectacle>
{
    Task<IEnumerable<SpectacleTotalDto>> GetWithTotalContractPrice();

    Task<IEnumerable<ActorWithAgencyInfo>> GetActorsWithAgencyName(long spectacleId);

    Task<IEnumerable<SpectacleWithActors>> GetSpectacleActors();

    Task<IEnumerable<TotalSpectaclesBudget>> GetSpectaclesWithTotalBudget();
}
