using System.Data.SqlClient;

namespace DatabaseLab.Domain.Dtos.SpectacleDtos;

public class SpectacleWithActors
{
    public string SpectacleName { get; set; } = string.Empty;

    public int ActorsCount { get; set; }

    public double AverageExperience { get; set; }

    public static SpectacleWithActors FromReader(SqlDataReader reader)
    {
        return new SpectacleWithActors
        {
            SpectacleName = reader[nameof(SpectacleName)].ToString() ?? string.Empty,
            ActorsCount = int.Parse(reader[nameof(ActorsCount)].ToString() ?? string.Empty),
            AverageExperience = double.Parse(reader[nameof(AverageExperience)].ToString() ?? string.Empty),
        };
    }
}
