using System.Data.SqlClient;

namespace DatabaseLab.Domain.Dtos.ActorDtos;

public class ActorContractInfoDto
{
    public long Id { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public int ContractCount { get; set; }

    public decimal AverageContractPrice { get; set; }

    public static ActorContractInfoDto FromReader(SqlDataReader reader)
    {
        return new ActorContractInfoDto
        {
            Id = long.Parse(reader[nameof(Id)].ToString() ?? string.Empty),
            FirstName = reader[nameof(FirstName)].ToString() ?? string.Empty,   
            LastName = reader[nameof(LastName)].ToString() ?? string.Empty,
            ContractCount = int.Parse(reader[nameof(ContractCount)].ToString() ?? string.Empty), 
            AverageContractPrice = decimal.Parse(
                reader[nameof(AverageContractPrice)].ToString() ?? string.Empty),
        };
    }
}
