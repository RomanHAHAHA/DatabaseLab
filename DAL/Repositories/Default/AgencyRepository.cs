using DatabaseLab.DAL.Interfaces;
using DatabaseLab.Domain.Entities;
using DatabaseLab.Domain.Options;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;
using System.Data;

namespace DatabaseLab.DAL.Repositories.Default;

public class AgencyRepository(IOptions<DbOptions> dbOptions) : IRepository<Agency>
{
    private readonly string _connectionString = dbOptions.Value.ConnectionString;

    private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

    public async Task<bool> CreateAsync(Agency entity)
    {
        const string sqlQuery = @"
                INSERT INTO Agencies (Name, Email, Phone, Address)
                VALUES (@Name, @Email, @Phone, @Address)";

        using var connection = CreateConnection() as SqlConnection;

        if (connection is null)
        {
            return false;
        }

        await connection.OpenAsync();

        using var command = new SqlCommand(sqlQuery, connection);
        command.Parameters.AddWithValue("@Name", entity.Name);
        command.Parameters.AddWithValue("@Email", entity.Email);
        command.Parameters.AddWithValue("@Phone", entity.Phone);
        command.Parameters.AddWithValue("@Address", entity.Address);

        var rowsAffected = await command.ExecuteNonQueryAsync();

        return rowsAffected > 0;
    }

    public async Task<bool> RemoveAsync(long id)
    {
        const string sqlQuery = @"
                DELETE FROM Agencies WHERE Id = @Id";

        using var connection = CreateConnection() as SqlConnection;

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

    public async Task<IQueryable<Agency>> GetAllAsync()
    {
        const string sqlQuery = "SELECT * FROM Agencies";

        using var connection = CreateConnection() as SqlConnection;

        if (connection is null)
        {
            return Enumerable.Empty<Agency>().AsQueryable();
        }

        await connection.OpenAsync();

        using var command = new SqlCommand(sqlQuery, connection);
        using var reader = await command.ExecuteReaderAsync();

        var agencies = new List<Agency>();

        while (await reader.ReadAsync())
        {
            agencies.Add(new Agency().FromReader(reader));
        }

        return agencies.AsQueryable();
    }

    public async Task<Agency?> GetByIdAsync(long id)
    {
        const string sqlQuery = "SELECT * FROM Agencies WHERE Id = @Id";

        using var connection = CreateConnection() as SqlConnection;

        if (connection is null)
        {
            return null;
        }

        await connection.OpenAsync();

        using var command = new SqlCommand(sqlQuery, connection);
        command.Parameters.AddWithValue("@Id", id);

        using var reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new Agency().FromReader(reader);
        }

        return null;
    }

    public async Task<bool> UpdateAsync(Agency entity)
    {
        const string sqlQuery = @"
                UPDATE Agencies
                SET Name = @Name,
                    Email = @Email,
                    Phone = @Phone,
                    Address = @Address
                WHERE Id = @Id";

        using var connection = CreateConnection() as SqlConnection;

        if (connection is null)
        {
            return false;
        }

        await connection.OpenAsync();

        using var command = new SqlCommand(sqlQuery, connection);
        command.Parameters.AddWithValue("@Id", entity.Id);
        command.Parameters.AddWithValue("@Name", entity.Name);
        command.Parameters.AddWithValue("@Email", entity.Email);
        command.Parameters.AddWithValue("@Phone", entity.Phone);
        command.Parameters.AddWithValue("@Address", entity.Address);

        var rowsAffected = await command.ExecuteNonQueryAsync();

        return rowsAffected > 0;
    }
}
