using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using Application.Interfaces;

namespace infrastructure.CacheStorage
{
    public class CacheService(IDistributedCache distributedCache) : ICacheService
    {
        private readonly IDistributedCache _distributedCache = distributedCache;

        public async Task<T?> GetAsync<T>(string key)
        {
            var objectString = await _distributedCache.GetStringAsync(key);

            if (string.IsNullOrEmpty(objectString))
            {
                return default;
            }

            return JsonSerializer.Deserialize<T>(objectString)
                ?? throw new InvalidOperationException("Falha ao desserializar o cache");
        }

        public async Task RemoveAsync(string key)
        {
            await _distributedCache.RemoveAsync(key);
        }

        public async Task SetAsync<T>(string key, T value)
        {
            var memoryCacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(2400),
                SlidingExpiration = TimeSpan.FromMinutes(1200)
            };

            var objectString = JsonSerializer.Serialize(value);
            await _distributedCache.SetStringAsync(key, objectString, memoryCacheOptions);
        }
    }
}
