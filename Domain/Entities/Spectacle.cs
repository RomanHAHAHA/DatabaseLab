using DatabaseLab.Domain.Interfaces;
using System.Data.SqlClient;

namespace DatabaseLab.Domain.Entities;

public class Spectacle : IEntity<Spectacle>
{
    public long Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int ProductionDate { get; set; }

    public decimal Budget { get; set; }

    public Spectacle FromReader(SqlDataReader reader)
    {
        return new Spectacle
        {
            Id = long.Parse(reader[nameof(Id)].ToString() ?? string.Empty),
            Name = reader[nameof(Name)].ToString() ?? string.Empty,
            ProductionDate = int.Parse(reader[nameof(ProductionDate)].ToString() ?? string.Empty),
            Budget = decimal.Parse(reader[nameof(Budget)].ToString() ?? string.Empty),
        };
    }

    public string GetPrimaryKeyName() => nameof(Id);
}
