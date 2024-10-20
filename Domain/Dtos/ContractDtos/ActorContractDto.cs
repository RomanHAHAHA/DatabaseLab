using System.Data.SqlClient;

namespace DatabaseLab.Domain.Dtos.ContractDtos;

public class ActorContractDto
{
    public long ContractId { get; set; }

    public string Role { get; set; } = string.Empty;

    public decimal AnnualContractPrice { get; set; }

    public string SpectacleName { get; set; } = string.Empty;

    public static ActorContractDto FromReader(SqlDataReader reader)
    {
        return new ActorContractDto
        {
            ContractId = long.Parse(reader[nameof(ContractId)].ToString() ?? string.Empty),
            Role = reader[nameof(Role)].ToString() ?? string.Empty,
            AnnualContractPrice = decimal.Parse(reader[nameof(AnnualContractPrice)].ToString() ?? string.Empty),
            SpectacleName = reader[nameof(SpectacleName)].ToString() ?? string.Empty,
        };
    }
}
