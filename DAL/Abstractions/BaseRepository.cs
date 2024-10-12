using System.Data;
using System.Data.SqlClient;
using DatabaseLab.DAL.Interfaces;
using DatabaseLab.Domain.Interfaces;

namespace DatabaseLab.DAL.Abstractions;

public abstract class BaseRepository<T>(string connectionString) :
    IRepository<T> where T : class, IEntity<T>, new()
{
    protected readonly string _connectionString = connectionString;

    protected SqlConnection CreateConnection() => new(_connectionString);

    public virtual async Task<bool> CreateAsync(T entity)
    {
        var properties = typeof(T).GetProperties().Where(p => p.Name != entity.GetPrimaryKeyName()); 
        var columnNames = string.Join(", ", properties.Select(p => p.Name));
        var parameters = string.Join(", ", properties.Select(p => $"@{p.Name}"));

        var query = $"INSERT INTO {typeof(T).Name}s ({columnNames}) VALUES ({parameters})";

        using var connection = CreateConnection();
        await connection.OpenAsync();
        using var command = new SqlCommand(query, connection);

        foreach (var property in properties)
        {
            command.Parameters.AddWithValue(
                $"@{property.Name}",
                property.GetValue(entity) ?? DBNull.Value);
        }

        var result = await command.ExecuteNonQueryAsync();
        return result > 0;
    }

    public virtual async Task<T?> GetByIdAsync(long id)
    {
        var primaryKeyName = new T().GetPrimaryKeyName();
        var query = $"SELECT * FROM {typeof(T).Name}s WHERE {primaryKeyName} = @{primaryKeyName}";

        using var connection = CreateConnection();
        await connection.OpenAsync();
        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue($"@{primaryKeyName}", id);

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            var entity = new T();
            return entity.FromReader(reader);
        }

        return null;
    }

    public virtual async Task<IQueryable<T>> GetAllAsync()
    {
        var query = $"SELECT * FROM {typeof(T).Name}s";

        using var connection = CreateConnection();
        await connection.OpenAsync();
        using var command = new SqlCommand(query, connection);

        using var reader = await command.ExecuteReaderAsync();
        var entities = new List<T>();

        while (await reader.ReadAsync())
        {
            var entity = new T();
            entities.Add(entity.FromReader(reader));
        }

        return entities.AsQueryable();
    }

    public virtual async Task<bool> UpdateAsync(T entity)
    {
        var primaryKeyName = entity.GetPrimaryKeyName();

        var properties = typeof(T).GetProperties().Where(p => p.Name != primaryKeyName); 
        var updateFields = string.Join(", ", properties.Select(p => $"{p.Name} = @{p.Name}"));

        var query = $"UPDATE {typeof(T).Name}s SET {updateFields} " +
            $"WHERE {primaryKeyName} = @{primaryKeyName}";

        using var connection = CreateConnection();
        await connection.OpenAsync();
        using var command = new SqlCommand(query, connection);

        foreach (var property in properties)
        {
            command.Parameters.AddWithValue(
                $"@{property.Name}",
                property.GetValue(entity) ?? DBNull.Value);
        }

        command.Parameters.AddWithValue(
            $"@{primaryKeyName}", 
            entity.GetType().GetProperty(primaryKeyName).GetValue(entity));

        var result = await command.ExecuteNonQueryAsync();
        return result > 0;
    }


    public virtual async Task<bool> RemoveAsync(long id)
    {
        var primaryKeyName = new T().GetPrimaryKeyName();
        var query = $"DELETE FROM {typeof(T).Name}s WHERE {primaryKeyName} = @{primaryKeyName}";

        using var connection = CreateConnection();
        await connection.OpenAsync();
        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue($"@{primaryKeyName}", id);

        var result = await command.ExecuteNonQueryAsync();

        return result > 0;
    }
}