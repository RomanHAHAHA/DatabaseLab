using DatabaseLab.Domain.Interfaces;
using System.Data.SqlClient;

namespace DatabaseLab.Domain.Entities;

public class ActorDetail : IEntity<ActorDetail>
{
    public long ActorId { get; set; }

    public string Phone { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public DateTime Birthday { get; set; }

    public ActorDetail FromReader(SqlDataReader reader)
    {
        return new ActorDetail
        {
            ActorId = long.Parse(reader[nameof(ActorId)].ToString() ?? string.Empty),
            Phone = reader[nameof(Phone)].ToString() ?? string.Empty,
            Email = reader[nameof(Email)].ToString() ?? string.Empty,
            Birthday = DateTime.Parse(reader[nameof(Birthday)].ToString() ?? string.Empty)
        };
    }

    public string GetPrimaryKeyName() => nameof(ActorId);
}
