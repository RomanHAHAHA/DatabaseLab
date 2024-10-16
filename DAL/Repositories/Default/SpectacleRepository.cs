using DatabaseLab.DAL.Abstractions;
using DatabaseLab.DAL.Interfaces;
using DatabaseLab.Domain.Dtos.ActorDtos;
using DatabaseLab.Domain.Dtos.SpectacleDtos;
using DatabaseLab.Domain.Entities;
using DatabaseLab.Domain.Options;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;

namespace DatabaseLab.DAL.Repositories.Default;

public class SpectacleRepository(IOptions<DbOptions> dbOptions) :
    BaseRepository<Spectacle>(dbOptions.Value.ConnectionString),
    ISpectacleRepository
{
    public override async Task<bool> RemoveAsync(long id)
    {
        const string sqlQuery = @"
            BEGIN TRANSACTION;
            DELETE FROM Contracts WHERE SpectacleId = @Id;
            DELETE FROM Spectacles WHERE Id = @Id;
            COMMIT TRANSACTION;";

        using var connection = CreateConnection();

        if (connection is null)
        {
            return false;
        }

        await connection.OpenAsync();
        using var command = connection.CreateCommand();
        command.CommandText = sqlQuery;
        command.Parameters.Add(new SqlParameter("@Id", id));

        var rowsAfferted = await command.ExecuteNonQueryAsync();

        return rowsAfferted > 0;
    }

    public async Task<IEnumerable<SpectacleTotalDto>> GetTotalSpectaclesInfo(
        decimal minTotalPrice)
    {
        const string sqlQuery = @"
            SELECT 
                s.Id AS SpectacleId,
                s.Name AS SpectacleName,
                COUNT(c.Id) AS ContractCount,
                SUM(c.AnnualContractPrice) AS TotalContractPrice
            FROM Spectacles s
            JOIN Contracts c ON s.Id = c.SpectacleId
            GROUP BY s.Id, s.Name
            HAVING SUM(c.AnnualContractPrice) >= @MinTotalPrice
            ORDER BY TotalContractPrice DESC";

        using var connection = CreateConnection();

        if (connection is null)
        {
            return [];
        }

        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = sqlQuery;
        command.Parameters.AddWithValue("@MinTotalPrice", minTotalPrice);

        using var reader = await command.ExecuteReaderAsync();
        var spectacles = new List<SpectacleTotalDto>();

        while (await reader.ReadAsync())
        {
            spectacles.Add(SpectacleTotalDto.FromReader(reader));
        }

        return spectacles;
    }

    public async Task<IEnumerable<ActorWithAgencyInfo>> GetActorsWithAgencyName(long spectacleId)
    {
        const string sqlQuery = @"
            SELECT 
                a.Id AS ActorId,
                a.FirstName,
                a.LastName,
                ag.Name AS AgencyName,
                c.Role AS Role,  
                c.AnnualContractPrice AS ContractPrice
            FROM Actors a
            LEFT JOIN Agencies ag ON a.AgencyId = ag.Id
            JOIN Contracts c ON a.Id = c.ActorId
            WHERE c.SpectacleId = @SpectacleId;";

        using var connection = CreateConnection();

        if (connection is null)
        {
            return [];
        }

        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = sqlQuery;
        command.Parameters.AddWithValue("@SpectacleId", spectacleId); 

        using var reader = await command.ExecuteReaderAsync();
        var actors = new List<ActorWithAgencyInfo>();

        while (await reader.ReadAsync())
        {
            actors.Add(ActorWithAgencyInfo.FromReader(reader));
        }

        return actors;
    }

}
