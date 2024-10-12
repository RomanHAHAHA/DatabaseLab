using DatabaseLab.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace DatabaseLab.Domain.Dtos.ContractDtos;

public class ContractCreateDto
{
    [Required(ErrorMessage = $"{nameof(ActorId)} is required.")]
    [Range(0, long.MaxValue, ErrorMessage = $"{nameof(ActorId)} must be greater then 0.")]
    public long? ActorId { get; set; }

    [Required(ErrorMessage = $"{nameof(SpectacleId)} is required.")]
    [Range(0, long.MaxValue, ErrorMessage = $"{nameof(SpectacleId)} must be greater then 0.")]
    public long? SpectacleId { get; set; }

    [Required(ErrorMessage = $"{nameof(Role)} is required.")]
    public string Role { get; set; } = string.Empty;

    [Required(ErrorMessage = "Contract price is required.")]
    [Range(0, long.MaxValue, ErrorMessage = "Contract price must be greater then 0.")]
    public decimal? AnnualContractPrice { get; set; }

    public Contract ToEntity()
    {
        if (ActorId is null ||
            SpectacleId is null ||
            AnnualContractPrice is null)
            throw new ArgumentNullException();

        return new Contract
        {
            ActorId = (long)ActorId,
            SpectacleId = (long)SpectacleId,
            Role = Role,
            AnnualContractPrice = (decimal)AnnualContractPrice
        };
    }

    public Contract ToEntity(long id)
    {
        var contract = ToEntity();
        contract.Id = id;

        return contract;
    }
}
