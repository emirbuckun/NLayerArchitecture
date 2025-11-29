using App.Application.Contracts.Caching;
using Microsoft.Extensions.Caching.Memory;

namespace App.Caching {
    public class CacheService(IMemoryCache memoryCache) : ICacheService {
        public Task<T?> GetAsync<T>(string key) {
            memoryCache.TryGetValue(key, out T? value);
            return Task.FromResult(value);
        }

        public Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpirationRelativeToNow = null) {
            memoryCache.Set(key, value, absoluteExpirationRelativeToNow ?? TimeSpan.FromMinutes(60));
            return Task.CompletedTask;
        }

        public Task RemoveAsync(string key) {
            memoryCache.Remove(key);
            return Task.CompletedTask;
        }
    }
}