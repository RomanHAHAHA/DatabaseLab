using DatabaseLab.Domain.Entities;

namespace DatabaseLab.DAL.Interfaces;

public interface IContractRepository : IRepository<Contract>
{
    Task<IEnumerable<Contract>> GetConractsWithPrice(decimal yearPrice);

    Task<IEnumerable<Contract>> GetContractsWithRolePrefix(string rolePrefix);

    Task<IEnumerable<Contract>> GetContractsOfActor(long actorId);

    Task<IEnumerable<Contract>> GetContractsOfSpectacle(long spectacleId);
}
