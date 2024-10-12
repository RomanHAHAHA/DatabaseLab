using DatabaseLab.Domain.Dtos.SpectacleDtos;
using DatabaseLab.Domain.Entities;

namespace DatabaseLab.DAL.Interfaces;

public interface ISpectacleRepository : IRepository<Spectacle>
{
    Task<IEnumerable<Spectacle>> GetSpectaclesWithBudgetGreaterThan(decimal minBudget);

    Task<IEnumerable<Spectacle>> GetSpectaclesWithProductionDate(int productionDate);
    
    Task<IEnumerable<Spectacle>> GetSpectaclesByNameStartsWith(string prefix);

    Task<IEnumerable<SpectacleTotalDto>> GetTotalSpectaclesInfo(decimal minTotalPrice);
}
