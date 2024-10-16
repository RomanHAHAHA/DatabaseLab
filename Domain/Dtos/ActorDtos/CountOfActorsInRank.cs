using DatabaseLab.Domain.Enums;
using System.Data.SqlClient;

namespace DatabaseLab.Domain.Dtos.ActorDtos;

public class CountOfActorsInRank
{
    public string Rank { get; set; } = string.Empty;

    public int ActorCount { get; set; }

    public static CountOfActorsInRank FromReader(SqlDataReader reader)
    {
        var rank = (ActorRank)Enum.Parse(
            typeof(ActorRank),
            reader[nameof(Rank)].ToString() ?? string.Empty);

        return new CountOfActorsInRank
        {
            Rank = rank.ToString(),
            ActorCount = reader.GetInt32(reader.GetOrdinal(nameof(ActorCount)))
        };
    }
}
