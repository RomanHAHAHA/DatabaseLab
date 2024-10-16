using System.Data.SqlClient;

namespace DatabaseLab.Domain.Dtos.ActorDtos;

public class ActorWithMaxContractPrice
{
    public long Id { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public decimal MaxContractPrice { get; set; }

    public static ActorWithMaxContractPrice FromReader(SqlDataReader reader)
    {
        return new ActorWithMaxContractPrice
        {
            Id = long.Parse(reader[nameof(Id)].ToString() ?? string.Empty),
            FirstName = reader[nameof(FirstName)].ToString() ?? string.Empty,
            LastName = reader[nameof(LastName)].ToString() ?? string.Empty,
            MaxContractPrice = decimal.Parse(reader[nameof(MaxContractPrice)].ToString() ?? string.Empty),
        };
    }
}
