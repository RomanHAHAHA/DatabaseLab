using System.Data.SqlClient;

namespace DatabaseLab.Domain.Dtos.ActorDtos;

public class ActorWithBirthday
{
    public long ActorId { get; set; }

    public string ActorName { get; set; } = string.Empty;

    public string Birthday { get; set; } = string.Empty;

    public static ActorWithBirthday FromReader(SqlDataReader reader)
    {
        return new ActorWithBirthday
        {
            ActorId = long.Parse(reader[nameof(ActorId)].ToString() ?? string.Empty),
            ActorName = reader[nameof(ActorName)].ToString() ?? string.Empty,
            Birthday = reader.IsDBNull(reader.GetOrdinal("Birthday"))
                ? "NULL"
                : DateTime.Parse(reader["Birthday"].ToString() ?? string.Empty)
                    .ToString("dd.MM.yyyy")
        };
    }
}
