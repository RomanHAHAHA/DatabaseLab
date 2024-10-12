using DatabaseLab.Domain.Interfaces;
using System.Data.SqlClient;

namespace DatabaseLab.Domain.Entities;

public class Agency : IEntity<Agency>
{
    public long Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public Agency FromReader(SqlDataReader reader)
    {
        return new Agency
        {
            Id = long.Parse(reader[nameof(Id)].ToString() ?? string.Empty),
            Name = reader[nameof(Name)].ToString() ?? string.Empty,
            Email = reader[nameof(Email)].ToString() ?? string.Empty,
            Phone = reader[nameof(Phone)].ToString() ?? string.Empty,
            Address = reader[nameof(Address)].ToString() ?? string.Empty,
        };
    }

    public string GetPrimaryKeyName() => nameof(Id);
}
