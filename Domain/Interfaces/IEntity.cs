using System.Data.SqlClient;

namespace DatabaseLab.Domain.Interfaces;

public interface IEntity<T>
{
    string GetPrimaryKeyName();

    T FromReader(SqlDataReader reader);
}
