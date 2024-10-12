using DatabaseLab.DAL.Abstractions;
using DatabaseLab.DAL.Interfaces;
using DatabaseLab.Domain.Entities;
using DatabaseLab.Domain.Options;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;

namespace DatabaseLab.DAL.Repositories.Default;

public class ActorDetailsRepository(IOptions<DbOptions> options) :
    BaseRepository<ActorDetail>(options.Value.ConnectionString),
    IRepository<ActorDetail>
{
    public override async Task<bool> CreateAsync(ActorDetail entity)
    {
        const string sqlQuery = @"
                INSERT INTO ActorDetails (ActorId, Phone, Email, Birthday)
                VALUES (@ActorId, @Phone, @Email, @Birthday)";

        using var connection = CreateConnection();

        if (connection is null)
        {
            return false;
        }

        await connection.OpenAsync();

        using var command = new SqlCommand(sqlQuery, connection);
        command.Parameters.AddWithValue("@ActorId", entity.ActorId);
        command.Parameters.AddWithValue("@Phone", entity.Phone);
        command.Parameters.AddWithValue("@Email", entity.Email);
        command.Parameters.AddWithValue("@Birthday", entity.Birthday);

        var rowsAffected = await command.ExecuteNonQueryAsync();

        return rowsAffected > 0;
    }
}
