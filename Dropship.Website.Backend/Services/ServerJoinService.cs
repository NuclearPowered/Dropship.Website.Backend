using System;
using System.Text;
using System.Threading.Tasks;
using Dropship.Website.Backend.Database;
using Dropship.Website.Backend.Database.Entities;
using Microsoft.Extensions.Caching.Distributed;

namespace Dropship.Website.Backend.Services
{
    public class ServerJoinService
    {
        private readonly IDistributedCache _cache;
        private readonly DatabaseContext _database;

        public ServerJoinService(DatabaseContext database, IDistributedCache cache)
        {
            _database = database;
            _cache = cache;
        }

        public async ValueTask ClientWillJoinAsync(string serverNonce, string userId)
        {
            await _cache.SetStringAsync(userId, serverNonce,
                new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(1)));
        }

        public async Task<UserEntity> CheckClientJoinAsync(string userId, string serverNonce)
        {
            if (await _cache.GetStringAsync(userId) == serverNonce)
            {
                await _cache.RemoveAsync(userId);
                return await _database.Users.FindAsync(int.Parse(userId));
            }
            return null;
        }
    }
}