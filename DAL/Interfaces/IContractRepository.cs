using DatabaseLab.Domain.Dtos.ContractDtos;
using DatabaseLab.Domain.Entities;

namespace DatabaseLab.DAL.Interfaces;

public interface IContractRepository : IRepository<Contract>
{
    Task<IEnumerable<ContractCountOfYear>> GetContractsOfYear(int year);
}
