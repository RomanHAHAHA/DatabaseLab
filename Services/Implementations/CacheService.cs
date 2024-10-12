using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;
using DatabaseLab.Services.Interfaces;

namespace DatabaseLab.Services.Implementations;

public class CacheService(
    IMemoryCache memoryCache, 
    ILogger<CacheService> logger) : ICacheService
{
    private static readonly ConcurrentDictionary<string, bool> CacheKeys = new();

    private readonly IMemoryCache _memoryCache = memoryCache; 
    private readonly ILogger<CacheService> _logger = logger;

    public Task SetAsync<T>(
        string key,
        T data,
        TimeSpan expiration) where T : class
    {
        _memoryCache.Set(key, data, expiration); 

        CacheKeys.TryAdd(key, false); // Добавляем ключ в список кэшированных
        _logger.LogInformation($"Cache [{key}] set.");

        LogAllKeys(); // Логируем все ключи

        return Task.CompletedTask;
    }

    // Метод для получения данных из кэша в памяти
    public Task<T?> GetAsync<T>(string key) where T : class
    {
        if (_memoryCache.TryGetValue(key, out T? cachedValue))
        {
            _logger.LogInformation($"Cache hit for key [{key}].");
            return Task.FromResult(cachedValue);
        }

        _logger.LogInformation($"Cache miss for key [{key}].");
        return Task.FromResult<T?>(null);
    }

    // Метод для получения данных с фабрикой (если данных в кэше нет)
    public async Task<T?> GetAsync<T>(
        string key,
        Func<Task<T?>> factory,
        TimeSpan expiration) where T : class
    {
        var cachedValue = await GetAsync<T>(key);

        if (cachedValue is not null)
        {
            return cachedValue;
        }

        var newValue = await factory();

        if (newValue is not null)
        {
            await SetAsync(key, newValue, expiration); // Установка нового значения в кэш
            return newValue;
        }

        return null;
    }

    // Метод для удаления данных из кэша
    public Task RemoveAsync(string key)
    {
        _memoryCache.Remove(key); // Удаление из памяти
        CacheKeys.TryRemove(key, out bool _); // Удаление из списка ключей

        _logger.LogInformation($"Cache [{key}] removed.");

        LogAllKeys(); // Логируем оставшиеся ключи

        return Task.CompletedTask;
    }

    // Удаление по префиксу
    public Task RemoveByPrefixAsync(string prefix)
    {
        var keysToRemove = CacheKeys.Keys
            .Where(k => k.StartsWith(prefix))
            .ToList();

        foreach (var key in keysToRemove)
        {
            _memoryCache.Remove(key); // Удаление ключа из кэша
            CacheKeys.TryRemove(key, out bool _); // Удаление ключа из списка
        }

        _logger.LogInformation($"All cache keys with prefix [{prefix}] removed.");

        LogAllKeys(); // Логируем оставшиеся ключи

        return Task.CompletedTask;
    }

    // Метод для логирования всех текущих ключей в кэше
    public void LogAllKeys()
    {
        if (!CacheKeys.Any())
        {
            _logger.LogInformation("No cache keys present in memory.");
        }
        else
        {
            _logger.LogInformation("Current memory cache keys:");
            foreach (var key in CacheKeys.Keys)
            {
                Console.WriteLine($"\t- {key}");
            }
        }
    }
}
