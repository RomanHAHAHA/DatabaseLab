using System.Data.SqlClient;
using System.Runtime.ExceptionServices;

namespace DatabaseLab.Domain.Dtos.ActorDtos;

public class ActorSpectaclesCount
{
    public long ActorId { get; set; }

    public string ActorName { get; set; } = string.Empty;

    public int SpectacleCount { get; set; }

    public static ActorSpectaclesCount FromReader(SqlDataReader reader)
    {
        return new ActorSpectaclesCount
        {
            ActorId = long.Parse(reader[nameof(ActorId)].ToString() ?? string.Empty),
            ActorName = reader[nameof(ActorName)].ToString() ?? string.Empty,
            SpectacleCount = int.Parse(reader[nameof(SpectacleCount)].ToString() ?? string.Empty),
        };
    }
}
