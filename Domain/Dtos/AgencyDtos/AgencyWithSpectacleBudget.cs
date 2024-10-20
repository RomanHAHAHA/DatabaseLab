using System.Data.SqlClient;

namespace DatabaseLab.Domain.Dtos.AgencyDtos;

public class AgencyWithSpectacleBudget
{
    public string AgencyName { get; set; } = string.Empty;

    public decimal MinSpectacleBudget { get; set; }

    public decimal MaxSpectacleBudget { get; set; }

    public static AgencyWithSpectacleBudget FromReader(SqlDataReader reader)
    {
        return new AgencyWithSpectacleBudget
        {
            AgencyName = reader[nameof(AgencyName)].ToString() ?? string.Empty,
            MinSpectacleBudget = decimal.Parse(reader[nameof(MinSpectacleBudget)].ToString() ?? string.Empty),
            MaxSpectacleBudget = decimal.Parse(reader[nameof(MaxSpectacleBudget)].ToString() ?? string.Empty),
        };
    }
}
