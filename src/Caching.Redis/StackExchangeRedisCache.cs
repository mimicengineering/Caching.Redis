using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Options;
using Patterson.Caching.Redis.Interfaces;
using StackExchange.Redis;

namespace Patterson.Caching.Redis
{
    public class StackExchangeRedisCache : ICache, IDisposable
    {
        private readonly RedisCacheOptions _options;

        private IConnectionMultiplexer _connection;

        public StackExchangeRedisCache(IOptions<RedisCacheOptions> options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _options = options.Value;
        }

        public void Dispose()
        {
            _connection?.Close();
        }

        public async Task<string> GetStringAsync(string key, int? db = null)
        {
            await ConnectAsync();

            return await GetDatabase(db).StringGetAsync(key);
        }

        public async Task<bool> SetStringAsync(string key, string value, int? db = null, TimeSpan? expiry = null)
        {
            await ConnectAsync();

            return await GetDatabase(db).StringSetAsync(key, value, expiry);
        }

        public async Task<string> GetHashAsync(string key, string field, int? db = null)
        {
            await ConnectAsync();

            return await GetDatabase(db).HashGetAsync(key, field);
        }

        public async Task<bool> SetHashAsync(string key, string field, string value, int? db = null)
        {
            await ConnectAsync();

            return await GetDatabase(db).HashSetAsync(key, field, value);
        }

        public async Task<IEnumerable<KeyValuePair<string, string>>> GetAllKeyEntriesAsync(string key, int? db = null)
        {
            await ConnectAsync();

            return
                (await GetDatabase(db).HashGetAllAsync(key))
                .Select(a => new KeyValuePair<string, string>(a.Name, a.Value))
                .ToList();
        }

        public async Task DeleteHashKeyAsync(string key, int? db = null)
        {
            await ConnectAsync();

            var redisDatabase = GetDatabase(db);
            var entries = await redisDatabase.HashGetAllAsync(key);
            var fields = entries.Select(a => a.Name).ToArray();

            await redisDatabase.HashDeleteAsync(key, fields);
        }

        private async Task ConnectAsync()
        {
            if (_connection == null)
            {
                _connection = await ConnectionMultiplexer.ConnectAsync(_options.Configuration);
            }
        }

        private IDatabase GetDatabase(int? db = null)
        {
            //return _connection.GetDatabase(db ?? _defaultRedisDatabase ?? -1);
            return _connection.GetDatabase(db ?? -1);
        }
    }
}
