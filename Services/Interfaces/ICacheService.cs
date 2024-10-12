namespace DatabaseLab.Services.Interfaces;

public interface ICacheService
{
    Task<T?> GetAsync<T>(string key) where T : class;

    Task<T?> GetAsync<T>(
        string key, 
        Func<Task<T?>> factory, 
        TimeSpan expiration) where T : class;

    void LogAllKeys();

    Task RemoveAsync(string key);

    Task RemoveByPrefixAsync(string prefix);

    Task SetAsync<T>(
        string key, 
        T data, 
        TimeSpan expiration) where T : class;
}