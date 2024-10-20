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

    public async Task<IEnumerable<SpectacleTotalDto>> GetWithTotalContractPrice()
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
            ORDER BY TotalContractPrice DESC";

        using var connection = CreateConnection();

        if (connection is null)
        {
            return [];
        }

        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = sqlQuery;

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
            (
                SELECT ag.Name
                FROM Agencies ag
                WHERE ag.Id = a.AgencyId
            ) AS AgencyName,
            c.Role AS Role,  
            c.AnnualContractPrice AS ContractPrice
        FROM Actors a
        JOIN Contracts c ON a.Id = c.ActorId
        WHERE c.SpectacleId = @SpectacleId";

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

    public async Task<IEnumerable<SpectacleWithActors>> GetSpectacleActors()
    {
        const string sqlQuery = @"
        SELECT 
            s.Name AS SpectacleName,
            COUNT(c.Id) AS ActorsCount,
            AVG(ac.Experience) AS AverageExperience
        FROM 
            Spectacles s
        JOIN 
            Contracts c ON s.Id = c.SpectacleId
        JOIN 
            Actors ac ON c.ActorId = ac.Id
        GROUP BY 
            s.Name
        ORDER BY 
            AverageExperience DESC";

        using var connection = CreateConnection();

        if (connection is null)
        {
            return [];
        }

        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = sqlQuery;

        using var reader = await command.ExecuteReaderAsync();
        var actors = new List<SpectacleWithActors>();

        while (await reader.ReadAsync())
        {
            actors.Add(SpectacleWithActors.FromReader(reader));
        }

        return actors;
    }

    public async Task<IEnumerable<TotalSpectaclesBudget>> GetSpectaclesWithTotalBudget()
    {
        const string sqlQuery = @"
        SELECT 
            s.ProductionDate AS ProductionYear,
            COUNT(DISTINCT s.Id) AS TotalSpectacles,  
            SUM(s.Budget) AS TotalBudget
        FROM 
            Spectacles s
        JOIN 
            Contracts c ON s.Id = c.SpectacleId
        GROUP BY 
            s.ProductionDate
        ORDER BY 
            s.ProductionDate";

        using var connection = CreateConnection();

        if (connection is null)
        {
            return [];
        }

        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = sqlQuery;

        using var reader = await command.ExecuteReaderAsync();
        var actors = new List<TotalSpectaclesBudget>();

        while (await reader.ReadAsync())
        {
            actors.Add(TotalSpectaclesBudget.FromReader(reader));
        }

        return actors;
    }
}
