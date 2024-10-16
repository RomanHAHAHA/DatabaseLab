using System.Data.SqlClient;

namespace DatabaseLab.Domain.Dtos.SpectacleDtos;

public class SpectacleTotalDto
{
    public long SpectacleId { get; set; }

    public string SpectacleName { get; set; } = string.Empty;

    public int ContractCount { get; set; }

    public decimal TotalContractPrice { get; set; }

    public static SpectacleTotalDto FromReader(SqlDataReader reader)
    {
        return new SpectacleTotalDto
        {
            SpectacleId = long.Parse(reader[nameof(SpectacleId)].ToString() ?? string.Empty),
            SpectacleName = reader[nameof(SpectacleName)].ToString() ?? string.Empty,
            ContractCount = int.Parse(reader[nameof(ContractCount)].ToString() ?? string.Empty),
            TotalContractPrice = decimal.Parse(reader[nameof(TotalContractPrice)].ToString() ?? string.Empty),
        };
    }
}
