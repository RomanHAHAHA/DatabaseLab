using DatabaseLab.DAL.Interfaces;
using DatabaseLab.Domain.Entities;
using DatabaseLab.Domain.Options;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;
using DatabaseLab.DAL.Abstractions;
using DatabaseLab.Domain.Dtos.ActorDtos;
using DatabaseLab.Domain.Dtos.AgencyDtos;

namespace DatabaseLab.DAL.Repositories.Default;

public class AgencyRepository(IOptions<DbOptions> dbOptions) : 
    BaseRepository<Agency>(dbOptions.Value.ConnectionString),
    IAgencyRepository
{
    public override async Task<bool> RemoveAsync(long id)
    {
        const string sqlQuery = @"
                DELETE FROM Agencies WHERE Id = @Id";

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

    public async Task<IEnumerable<CountOfActorsInRank>> GetActorGroups(long agencyId)
    {
        const string sqlQuery = @"
        SELECT 
            a.Rank AS Rank,
            COUNT(a.Id) AS ActorCount
        FROM Actors a
        WHERE a.AgencyId = @AgencyId
        GROUP BY a.Rank;";

        using var connection = CreateConnection();

        if (connection is null)
        {
            return [];
        }

        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = sqlQuery;

        var agencyIdParam = new SqlParameter("@AgencyId", agencyId);
        command.Parameters.Add(agencyIdParam);

        using var reader = await command.ExecuteReaderAsync();
        var actorGroups = new List<CountOfActorsInRank>();

        while (await reader.ReadAsync())
        {
            actorGroups.Add(CountOfActorsInRank.FromReader(reader));
        }

        return actorGroups;
    }

    public async Task<IEnumerable<AgencyWithSpectacleBudget>> GetMaxMinSpectacleBudget()
    {
        const string sqlQuery = @"
        SELECT 
            a.Name AS AgencyName,
            MAX(s.Budget) AS MaxSpectacleBudget,  
            MIN(s.Budget) AS MinSpectacleBudget   
        FROM Agencies a
        JOIN Actors ac ON a.Id = ac.AgencyId
        JOIN Contracts c ON ac.Id = c.ActorId
        JOIN Spectacles s ON c.SpectacleId = s.Id
        GROUP BY a.Name
        ORDER BY MaxSpectacleBudget DESC";

        using var connection = CreateConnection();

        if (connection is null)
        {
            return [];
        }

        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = sqlQuery;

        using var reader = await command.ExecuteReaderAsync();
        var actorGroups = new List<AgencyWithSpectacleBudget>();

        while (await reader.ReadAsync())
        {
            actorGroups.Add(AgencyWithSpectacleBudget.FromReader(reader));
        }

        return actorGroups;
    }
}
