using DatabaseLab.Domain.Interfaces;
using System.Data.SqlClient;

namespace DatabaseLab.Domain.Entities;

public class Contract : IEntity<Contract>
{
    public long Id { get; set; }

    public long ActorId { get; set; }

    public long SpectacleId { get; set; }

    public string Role { get; set; } = string.Empty;

    public decimal AnnualContractPrice { get; set; }

    public Contract FromReader(SqlDataReader reader)
    {
        return new Contract
        {
            Id = long.Parse(reader[nameof(Id)].ToString() ?? string.Empty),
            ActorId = long.Parse(reader[nameof(ActorId)].ToString() ?? string.Empty),
            SpectacleId = long.Parse(reader[nameof(SpectacleId)].ToString() ?? string.Empty),
            Role = reader[nameof(Role)].ToString() ?? string.Empty,
            AnnualContractPrice = decimal.Parse(reader[nameof(AnnualContractPrice)].ToString() ?? string.Empty)
        };
    }

    public string GetPrimaryKeyName() => nameof(Id);
}
