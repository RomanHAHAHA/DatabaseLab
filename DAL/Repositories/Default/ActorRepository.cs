using DatabaseLab.DAL.Abstractions;
using DatabaseLab.DAL.Interfaces;
using DatabaseLab.Domain.Dtos.ActorDtos;
using DatabaseLab.Domain.Entities;
using DatabaseLab.Domain.Enums;
using DatabaseLab.Domain.Options;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;

namespace DatabaseLab.DAL.Repositories.Default;

public class ActorRepository(IOptions<DbOptions> dbOptions) :
    BaseRepository<Actor>(dbOptions.Value.ConnectionString),
    IActorRepository
{
    public override async Task<bool> RemoveAsync(long id)
    {
        const string sqlQuery = @"
            BEGIN TRANSACTION;
            DELETE FROM Contracts WHERE ActorId = @Id;
            DELETE FROM Actors WHERE Id = @Id;
            COMMIT TRANSACTION";

        using var connection = CreateConnection();

        if (connection is null)
        {
            return false;
        }

        await connection.OpenAsync();

        using var command = new SqlCommand(sqlQuery, connection);
        command.Parameters.AddWithValue("@Id", id);

        var rowsAffected = await command.ExecuteNonQueryAsync();

        return rowsAffected > 0;
    }

    public async Task<IEnumerable<Actor>> GetActorsWithExperience(int experienceYears)
    {
        const string sqlQuery = "SELECT * FROM Actors " +
            "WHERE Experience >= @ExperienceYears " +
            "ORDER BY Experience";

        using var connection = CreateConnection() as SqlConnection;

        if (connection is null)
        {
            return [];
        }

        await connection.OpenAsync();

        using var command = new SqlCommand(sqlQuery, connection);
        command.Parameters.AddWithValue("@ExperienceYears", experienceYears);

        using var reader = await command.ExecuteReaderAsync();
        var actors = new List<Actor>();

        while (await reader.ReadAsync())
        {
            actors.Add(new Actor().FromReader(reader));
        }

        return actors;
    }

    public async Task<IEnumerable<Actor>> GetActorsWithRank(ActorRank actorRank)
    {
        const string sqlQuery = "SELECT * FROM Actors WHERE Rank = @Rank";

        using var connection = CreateConnection() as SqlConnection;

        if (connection is null)
        {
            return [];
        }

        await connection.OpenAsync();

        using var command = new SqlCommand(sqlQuery, connection);
        command.Parameters.AddWithValue("@Rank", (int)actorRank);

        using var reader = await command.ExecuteReaderAsync();
        var actors = new List<Actor>();

        while (await reader.ReadAsync())
        {
            actors.Add(new Actor().FromReader(reader));
        }

        return actors;
    }

    public async Task<IEnumerable<Actor>> GetBySurnamePrefix(string prefix)
    {
        const string sqlQuery = @"
            SELECT * FROM Actors 
            WHERE LastName LIKE @SearchString";

        using var connection = CreateConnection() as SqlConnection;

        if (connection is null)
        {
            return [];
        }

        await connection.OpenAsync();

        using var command = new SqlCommand(sqlQuery, connection);
        command.Parameters.AddWithValue("@SearchString", prefix + "%");

        using var reader = await command.ExecuteReaderAsync();
        var actors = new List<Actor>();

        while (await reader.ReadAsync())
        {
            actors.Add(new Actor().FromReader(reader));
        }

        return actors;
    }

    public async Task<IEnumerable<ActorContractInfoDto>> GetActorWithContractsInfo(
        decimal minAverageConractPrice)
    {
        const string sqlQuery = @"
        SELECT 
            a.Id,
            a.FirstName,
            a.LastName,
            AVG(c.AnnualContractPrice) AS AverageContractPrice,
            COUNT(c.Id) AS ContractCount
        FROM Actors a
        JOIN Contracts c ON a.Id = c.ActorId
        GROUP BY a.Id, a.FirstName, a.LastName
        HAVING AVG(c.AnnualContractPrice) >= @MinAveragePrice
        ORDER BY AverageContractPrice DESC";

        using var connection = CreateConnection() as SqlConnection;

        if (connection is null)
        {
            return [];
        }

        await connection.OpenAsync();

        using var command = new SqlCommand(sqlQuery, connection);
        command.Parameters.AddWithValue("@MinAveragePrice", minAverageConractPrice);

        using var reader = await command.ExecuteReaderAsync();
        var actors = new List<ActorContractInfoDto>();

        while (await reader.ReadAsync())
        {
            actors.Add(ActorContractInfoDto.FromReader(reader));
        }

        return actors;
    }

    public async Task<IEnumerable<ActorDataDto>> GetActorsData()
    {
        const string sqlQuery = @"
        SELECT 
            a.Id AS ActorId,
            a.FirstName,
            a.LastName,
            ad.Phone,
            ad.Email,
            ad.Birthday
        FROM Actors a
        LEFT JOIN ActorDetails ad ON a.Id = ad.ActorId
        ORDER BY ad.Birthday";

        using var connection = CreateConnection();

        if (connection is null)
        {
            return [];
        }

        await connection.OpenAsync();

        using var command = new SqlCommand(sqlQuery, connection);

        using var reader = await command.ExecuteReaderAsync();
        var actors = new List<ActorDataDto>();

        while (await reader.ReadAsync())
        {
            actors.Add(ActorDataDto.FromReader(reader));
        }

        return actors;
    }
}
