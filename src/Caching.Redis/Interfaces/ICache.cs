using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Patterson.Caching.Redis.Interfaces
{
    public interface ICache
    {
        Task<string> GetStringAsync(string key, int? db = null);

        Task<bool> SetStringAsync(string key, string value, int? db = null, TimeSpan? expiry = null);

        Task<string> GetHashAsync(string key, string field, int? db = null);

        Task<bool> SetHashAsync(string key, string field, string value, int? db = null);

        Task<IEnumerable<KeyValuePair<string, string>>> GetAllKeyEntriesAsync(string key, int? db = null);

        Task DeleteHashKeyAsync(string key, int? db = null);
    }
}
