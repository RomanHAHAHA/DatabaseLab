using System.Data.SqlClient;

namespace DatabaseLab.Domain.Dtos.ActorDetailsDtos;

public class ActorWithPhone
{
    public long Id { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Phone {  get; set; } = string.Empty;

    public static ActorWithPhone FromReader(SqlDataReader reader)
    {
        return new ActorWithPhone
        {
            Id = long.Parse(reader[nameof(Id)].ToString() ?? string.Empty),
            FirstName = reader[nameof(FirstName)].ToString() ?? string.Empty,
            LastName = reader[nameof(LastName)].ToString() ?? string.Empty,
            Phone = reader[nameof(Phone)].ToString() ?? string.Empty,
        };
    }
}
