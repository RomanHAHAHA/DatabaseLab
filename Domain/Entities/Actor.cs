using DatabaseLab.Domain.Enums;
using DatabaseLab.Domain.Interfaces;
using System.Data.SqlClient;

namespace DatabaseLab.Domain.Entities;

public class Actor : IEntity<Actor>
{
    public long Id { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string MiddleName { get; set; } = string.Empty;

    public ActorRank Rank { get; set; } 

    public int Experience { get; set; }

    public long? AgencyId { get; set; }

    public Actor FromReader(SqlDataReader reader)
    {
        return new Actor
        {
            Id = long.Parse(reader[nameof(Id)].ToString() ?? string.Empty),
            FirstName = reader[nameof(FirstName)].ToString() ?? string.Empty,
            LastName = reader[nameof(LastName)].ToString() ?? string.Empty,
            MiddleName = reader[nameof(MiddleName)].ToString() ?? string.Empty,
            Rank = (ActorRank)reader[nameof(Rank)],
            Experience = int.Parse(reader[nameof(Experience)].ToString() ?? string.Empty),
            AgencyId = reader[nameof(AgencyId)] == DBNull.Value ? 
                null : long.Parse(reader[nameof(AgencyId)].ToString() ?? string.Empty)
        };
    }


    public string GetPrimaryKeyName() => nameof(Id);
}
