using System.Data.SqlClient;

namespace DatabaseLab.Domain.Dtos.SpectacleDtos;

public class TotalSpectaclesBudget
{
    public int ProductionYear { get; set; }

    public int TotalSpectacles { get; set; }

    public decimal TotalBudget { get; set; }

    public static TotalSpectaclesBudget FromReader(SqlDataReader reader)
    {
        return new TotalSpectaclesBudget
        {
            ProductionYear = int.Parse(reader[nameof(ProductionYear)].ToString() ?? string.Empty),
            TotalSpectacles = int.Parse(reader[nameof(TotalSpectacles)].ToString() ?? string.Empty),
            TotalBudget = decimal.Parse(reader[nameof(TotalBudget)].ToString() ?? string.Empty),
        };
    }
}
