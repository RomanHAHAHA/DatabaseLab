using DatabaseLab.DAL.Abstractions;
using DatabaseLab.DAL.Interfaces;
using DatabaseLab.Domain.Entities;
using DatabaseLab.Domain.Options;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;

namespace DatabaseLab.DAL.Repositories.Default;

public class ContractRepository(IOptions<DbOptions> dbOptions) :
    BaseRepository<Contract>(dbOptions.Value.ConnectionString),
    IContractRepository
{
    public async Task<IEnumerable<Contract>> GetContractsOfActor(long actorId)
    {
        const string sqlQuery = "SELECT * FROM Contracts WHERE ActorId = @ActorId";
        var contracts = new List<Contract>();

        using var connection = CreateConnection();

        if (connection is null)
        {
            return [];
        }

        await connection.OpenAsync();

        using var command = new SqlCommand(sqlQuery, connection);
        command.Parameters.AddWithValue("@ActorId", actorId);

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var contract = new Contract().FromReader(reader);

            contracts.Add(contract);
        }

        return contracts;
    }

    public async Task<IEnumerable<Contract>> GetContractsOfSpectacle(long spectacleId)
    {
        const string sqlQuery = "SELECT * FROM Contracts " +
            "WHERE SpectacleId = @SpectacleId " +
            "ORDER BY AnnualContractPrice DESC";
        var contracts = new List<Contract>();

        using var connection = CreateConnection() as SqlConnection;

        if (connection is null)
        {
            return [];
        }

        await connection.OpenAsync();

        using var command = new SqlCommand(sqlQuery, connection);
        command.Parameters.AddWithValue("@SpectacleId", spectacleId);

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var contract = new Contract().FromReader(reader);

            contracts.Add(contract);
        }

        return contracts;
    }

    public async Task<IEnumerable<Contract>> GetConractsWithPrice(decimal yearPrice)
    {
        const string sqlQuery = "SELECT * FROM Contracts " +
            "WHERE AnnualContractPrice <= @AnnualContractPrice " +
            "ORDER BY AnnualContractPrice DESC";
        var contracts = new List<Contract>();

        using var connection = CreateConnection();

        if (connection is null)
        {
            return [];
        }

        await connection.OpenAsync();

        using var command = new SqlCommand(sqlQuery, connection);
        command.Parameters.AddWithValue("@AnnualContractPrice", yearPrice);

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var contract = new Contract().FromReader(reader);

            contracts.Add(contract);
        }

        return contracts;
    }

    public async Task<IEnumerable<Contract>> GetContractsWithRolePrefix(string rolePrefix)
    {
        const string sqlQuery = "SELECT * FROM Contracts WHERE Role LIKE @RolePrefix";
        var contracts = new List<Contract>();

        using var connection = CreateConnection();

        if (connection is null)
        {
            return [];
        }

        await connection.OpenAsync();

        using var command = new SqlCommand(sqlQuery, connection);
        command.Parameters.AddWithValue("@RolePrefix", rolePrefix + "%");

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var contract = new Contract().FromReader(reader);
            contracts.Add(contract);
        }

        return contracts;
    }
}
