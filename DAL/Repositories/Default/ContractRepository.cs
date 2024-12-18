﻿using DatabaseLab.DAL.Abstractions;
using DatabaseLab.DAL.Interfaces;
using DatabaseLab.Domain.Dtos.ContractDtos;
using DatabaseLab.Domain.Entities;
using DatabaseLab.Domain.Options;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;

namespace DatabaseLab.DAL.Repositories.Default;

public class ContractRepository(IOptions<DbOptions> dbOptions) :
    BaseRepository<Contract>(dbOptions.Value.ConnectionString),
    IContractRepository
{
    public async Task<IEnumerable<ContractCountOfYear>> GetContractsOfYear(int year)
    {
        const string sqlQuery = @"
        SELECT 
            ag.Id AS AgencyId,
            ag.Name AS AgencyName, 
            COUNT(c.Id) AS ContractCount, 
            SUM(c.AnnualContractPrice) AS TotalContractValue 
        FROM Agencies ag
        JOIN Actors a ON ag.Id = a.AgencyId 
        JOIN Contracts c ON a.Id = c.ActorId 
        JOIN Spectacles s ON c.SpectacleId = s.Id 
        WHERE s.ProductionDate = @ProductionDate 
        GROUP BY ag.Id, ag.Name 
        ORDER BY ContractCount";

        var contracts = new List<ContractCountOfYear>();

        using var connection = CreateConnection();

        if (connection is null)
        {
            return [];
        }

        await connection.OpenAsync();

        using var command = new SqlCommand(sqlQuery, connection);
        command.Parameters.AddWithValue("@ProductionDate", year);

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            contracts.Add(ContractCountOfYear.FromReader(reader));
        }

        return contracts;
    }

    public async Task<IEnumerable<ActorContractDto>> GetContractsOfActor(long actorId)
    {
        const string sqlQuery = @"
        SELECT 
            c.Id AS ContractId,
            c.Role,
            c.AnnualContractPrice,
            (SELECT s.Name FROM Spectacles s WHERE s.Id = c.SpectacleId) AS SpectacleName
        FROM 
            Contracts c
        WHERE 
            c.ActorId = @ActorId";

        var contracts = new List<ActorContractDto>();

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
            contracts.Add(ActorContractDto.FromReader(reader));
        }

        return contracts;
    }

    public async Task<IEnumerable<Contract>> GetContractsByAveragePrice()
    {
        const string sqlQuery = @"
        SELECT *
        FROM 
            Contracts c
        WHERE 
            c.AnnualContractPrice > (SELECT AVG(AnnualContractPrice) FROM Contracts)";

        var contracts = new List<Contract>();

        using var connection = CreateConnection();

        if (connection is null)
        {
            return [];
        }

        await connection.OpenAsync();

        using var command = new SqlCommand(sqlQuery, connection);

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            contracts.Add(new Contract().FromReader(reader));
        }

        return contracts;
    }
}
