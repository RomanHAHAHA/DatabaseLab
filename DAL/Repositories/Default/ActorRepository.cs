using DatabaseLab.DAL.Abstractions;
using DatabaseLab.DAL.Interfaces;
using DatabaseLab.Domain.Dtos.ActorDtos;
using DatabaseLab.Domain.Entities;
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

    public async Task<IEnumerable<ActorContractInfoDto>> GetActorWithContractsInfo()
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
        ORDER BY AverageContractPrice DESC";

        using var connection = CreateConnection();

        if (connection is null)
        {
            return [];
        }

        await connection.OpenAsync();

        using var command = new SqlCommand(sqlQuery, connection);

        using var reader = await command.ExecuteReaderAsync();
        var actors = new List<ActorContractInfoDto>();

        while (await reader.ReadAsync())
        {
            actors.Add(ActorContractInfoDto.FromReader(reader));
        }

        return actors;
    }

    public async Task<IEnumerable<ActorDataDto>> GetActorsData(DateTime birthday)
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
        WHERE ad.Birthday > @Birthday
        ORDER BY ad.Birthday";

        using var connection = CreateConnection();

        if (connection is null)
        {
            return [];
        }

        await connection.OpenAsync();

        using var command = new SqlCommand(sqlQuery, connection);
        command.Parameters.AddWithValue("@Birthday", birthday);
        using var reader = await command.ExecuteReaderAsync();
        var actors = new List<ActorDataDto>();

        while (await reader.ReadAsync())
        {
            actors.Add(ActorDataDto.FromReader(reader));
        }

        return actors;
    }

    public async Task<IEnumerable<ActorSpectaclesCount>> GetActorsWithSpectaclesCount(int spectaclesCount)
    {
        const string sqlQuery = @"
        SELECT 
            a.Id AS ActorId,
            a.FirstName AS ActorName,
            COUNT(c.SpectacleId) AS SpectacleCount
        FROM Actors a
        JOIN Contracts c ON a.Id = c.ActorId
        WHERE 
            c.SpectacleId IN (
                SELECT SpectacleId 
                FROM Contracts 
                WHERE ActorId = a.Id  
            )
        GROUP BY a.Id, a.FirstName
        HAVING COUNT(c.SpectacleId) > 1  
        ORDER BY SpectacleCount DESC";

        using var connection = CreateConnection();

        if (connection is null)
        {
            return [];
        }

        await connection.OpenAsync();

        using var command = new SqlCommand(sqlQuery, connection);
        command.Parameters.AddWithValue("@SpectaclesCount", spectaclesCount);
        using var reader = await command.ExecuteReaderAsync();
        var actors = new List<ActorSpectaclesCount>();

        while (await reader.ReadAsync())
        {
            actors.Add(ActorSpectaclesCount.FromReader(reader));
        }

        return actors;
    }

    public async Task<IEnumerable<ActorWithBirthday>> GetActorsBornInMonth(int month)
    {
        const string sqlQuery = @"
        SELECT 
            a.Id AS ActorId,
            a.FirstName AS ActorName,
            ad.Birthday
        FROM 
            Actors a
        JOIN 
            ActorDetails ad ON a.Id = ad.ActorId
        WHERE 
            MONTH(ad.Birthday) = @Month";

        using var connection = CreateConnection();

        if (connection is null)
        {
            return [];
        }

        await connection.OpenAsync();

        using var command = new SqlCommand(sqlQuery, connection);
        command.Parameters.AddWithValue("@Month", month);

        using var reader = await command.ExecuteReaderAsync();
        var actors = new List<ActorWithBirthday>();

        while (await reader.ReadAsync())
        {
            actors.Add(ActorWithBirthday.FromReader(reader));
        }

        return actors;
    }
}
