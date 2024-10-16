using System.Data.SqlClient;

namespace DatabaseLab.Domain.Dtos.ContractDtos;

public class ContractCountOfYear
{
    public long AgencyId { get; set; }

    public string AgencyName { get; set; } = string.Empty;

    public int ContractCount { get; set; }

    public decimal TotalContractValue { get; set; }

    public static ContractCountOfYear FromReader(SqlDataReader reader)
    {
        return new ContractCountOfYear
        {
            AgencyId = long.Parse(reader[nameof(AgencyId)].ToString() ?? string.Empty),
            AgencyName = reader[nameof(AgencyName)].ToString() ?? string.Empty,
            ContractCount = int.Parse(reader[nameof(ContractCount)].ToString() ?? string.Empty),
            TotalContractValue = decimal.Parse(reader[nameof(TotalContractValue)].ToString() ?? string.Empty),
        };
    }
}
