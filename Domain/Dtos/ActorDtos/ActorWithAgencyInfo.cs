using System.Data.SqlClient;

namespace DatabaseLab.Domain.Dtos.ActorDtos;

public class ActorWithAgencyInfo
{
    public long ActorId { get; set; }   

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string AgencyName { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    public decimal ContractPrice { get; set; }

    public static ActorWithAgencyInfo FromReader(SqlDataReader reader)
    {
        return new ActorWithAgencyInfo
        {
            ActorId = long.Parse(reader[nameof(ActorId)].ToString() ?? string.Empty),
            FirstName = reader[nameof(FirstName)].ToString() ?? string.Empty,
            LastName = reader[nameof(LastName)].ToString() ?? string.Empty,
            AgencyName = reader[nameof(AgencyName)].ToString() ?? string.Empty,
            Role = reader[nameof(Role)].ToString() ?? string.Empty,
            ContractPrice = decimal.Parse(reader[nameof(ContractPrice)].ToString() ?? string.Empty),
        };
    }
}
