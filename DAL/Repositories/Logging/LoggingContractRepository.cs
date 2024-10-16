using DatabaseLab.DAL.Interfaces;
using DatabaseLab.Domain.Dtos.ContractDtos;
using DatabaseLab.Domain.Dtos.SpectacleDtos;
using DatabaseLab.Domain.Entities;
using DatabaseLab.Services.Interfaces;

namespace DatabaseLab.DAL.Repositories.Logging;

public class LoggingContractRepository(
    IContractRepository contractRepository,
    IReportService reportService) : IContractRepository
{
    private readonly IContractRepository _contractRepository = contractRepository;
    private readonly IReportService _reportService = reportService;

    public async Task<bool> CreateAsync(Contract entity)
    {
        var result = await _contractRepository.CreateAsync(entity);
        await _reportService.LogToCacheAsync(nameof(CreateAsync), result, entity);
        return result;
    }

    public async Task<IQueryable<Contract>> GetAllAsync()
    {
        var collection = await _contractRepository.GetAllAsync();
        await _reportService.LogToCacheAsync(nameof(GetAllAsync), collection);
        return collection;
    }

    public async Task<Contract?> GetByIdAsync(long id)
    {
        var contract = await _contractRepository.GetByIdAsync(id);
        await _reportService.LogToCacheAsync(nameof(GetByIdAsync), contract);
        return contract;
    }

    public async Task<bool> RemoveAsync(long id)
    {
        var result = await _contractRepository.RemoveAsync(id);
        await _reportService.LogToCacheAsync(nameof(RemoveAsync), result);
        return result;
    }

    public async Task<bool> UpdateAsync(Contract entity)
    {
        var result = await _contractRepository.UpdateAsync(entity);
        await _reportService.LogToCacheAsync(nameof(UpdateAsync), result, entity);
        return result;
    }
    public async Task<IEnumerable<ContractCountOfYear>> GetContractsOfYear(int year)
    {
        var collection = await _contractRepository.GetContractsOfYear(year);
        await _reportService.LogToCacheAsync(nameof(GetContractsOfYear), collection);
        return collection;
    }
}
