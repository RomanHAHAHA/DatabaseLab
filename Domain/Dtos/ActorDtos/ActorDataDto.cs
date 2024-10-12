using System.Data.SqlClient;

namespace DatabaseLab.Domain.Dtos.ActorDtos;

public class ActorDataDto
{
    public long ActorId { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Birthday { get; set; } = string.Empty;

    public static ActorDataDto FromReader(SqlDataReader reader)
    {
        return new ActorDataDto
        {
            ActorId = long.Parse(reader[nameof(ActorId)].ToString() ?? string.Empty),
            FirstName = reader[nameof(FirstName)]?.ToString() ?? string.Empty,
            LastName = reader[nameof(LastName)]?.ToString() ?? string.Empty,
            Phone = string.IsNullOrWhiteSpace(reader[nameof(Phone)].ToString())
                ? "NULL"
                : reader[nameof(Phone)].ToString() ?? string.Empty,
            Email = string.IsNullOrWhiteSpace(reader[nameof(Email)].ToString())
                ? "NULL"
                : reader[nameof(Email)].ToString() ?? string.Empty,
            Birthday = reader.IsDBNull(reader.GetOrdinal("Birthday"))
                ? "NULL"
                : DateTime.Parse(reader["Birthday"].ToString() ?? string.Empty)
                    .ToString("dd.MM.yyyy")
        };
    }
}
