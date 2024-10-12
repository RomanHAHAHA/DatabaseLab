using System.Data.SqlClient;

namespace DatabaseLab.Domain.Dtos.SpectacleDtos;

public class SpectacleTotalDto
{
    public long Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int ContractCount { get; set; }

    public decimal TotalContractPrice { get; set; }

    public static SpectacleTotalDto FromReader(SqlDataReader reader)
    {
        return new SpectacleTotalDto
        {
            Id = long.Parse(reader[nameof(Id)].ToString() ?? string.Empty),
            Name = reader[nameof(Name)].ToString() ?? string.Empty,
            ContractCount = int.Parse(reader[nameof(ContractCount)].ToString() ?? string.Empty),
            TotalContractPrice = decimal.Parse(reader[nameof(TotalContractPrice)].ToString() ?? string.Empty),
        };
    }
}
