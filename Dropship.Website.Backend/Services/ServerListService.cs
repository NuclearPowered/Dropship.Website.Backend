using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dropship.Website.Backend.Database;
using Dropship.Website.Backend.Database.Entities;
using Dropship.Website.Backend.Models.Requests.ServerList;
using Microsoft.EntityFrameworkCore;

namespace Dropship.Website.Backend.Services
{
    public class ServerListService
    {
        private readonly DatabaseContext _database;

        public ServerListService(DatabaseContext database)
        {
            _database = database;
        }

        public async Task<ServerEntity> CreateUpdateServerAsync(CreateUpdateServerRequest request, int userId)
        {
            var server = await _database.ServerList.FindAsync(request.Id);
            if (server == null)
            {
                server = new ServerEntity
                {
                    Name = request.Name,
                    Description = request.Description,
                    IpAddress = request.IpAddress,
                    Port = request.Port,
                    OwnerUserId = userId,
                    StarCount = 0,
                    ImageUrl = request.ImageUrl
                };
                await _database.ServerList.AddAsync(server);
            }
            else
            {
                if (userId != server.OwnerUserId)
                    return null;
                server.Name = request.Name;
                server.Description = request.Description;
                server.IpAddress = request.IpAddress;
                server.Port = request.Port;
                server.ImageUrl = request.ImageUrl;
                _database.ServerList.Update(server);
            }
            await _database.SaveChangesAsync();
            return server;
        }

        public Task<ServerEntity> CreateUpdateServerAsync(CreateUpdateServerRequest request, string userId)
        {
            return CreateUpdateServerAsync(request, int.Parse(userId));
        }
        
        public async Task<ServerEntity> UpdateServerStarAsync(int serverId)
        {
            var serverEntity = await _database.ServerList
                .FirstOrDefaultAsync(x => x.Id == serverId);

            serverEntity.StarCount++;

            _database.ServerList.Update(serverEntity);
            await _database.SaveChangesAsync();
            return serverEntity;
        }

        public async Task<ServerEntity> GetServerByIdAsync(int serverId)
        {
            var server = await _database.ServerList.AsNoTracking()
                .Include(s => s.Owner)
                .FirstOrDefaultAsync(x => x.Id == serverId);
            if (server != null)
            {
                server.Owner = new UserEntity
                {
                    Id = server.Owner.Id,
                    Username = server.Owner.Username
                };
            }
 
            return server;
        }
        
        public async Task<List<ServerEntity>> GetServersForUserId(int userId)
        {
            return await _database.ServerList.AsNoTracking().Where(x => x.OwnerUserId == userId).ToListAsync();
        }

        public async Task<List<ServerEntity>> GetServerListPaginatedAsync(int pageNumber)
        {
            var count = 20;
            if (pageNumber < 1) pageNumber = 1;
            var serverEntities = await _database.ServerList.AsNoTracking()
                .OrderByDescending(s => s.StarCount)
                .Skip(count * (pageNumber - 1)).Take(count)
                .Include(s => s.Owner)
                .ToListAsync();
            
            serverEntities.ForEach(s => s.Owner = new UserEntity
            {
                Id = s.Owner.Id,
                Username = s.Owner.Username
            });

            return serverEntities;
        }


        public bool UserIdOwnsServer(int userId, ServerEntity serverEntity)
        {
            return serverEntity.OwnerUserId == userId;
        }

        public bool UserIdOwnsServer(string userId, ServerEntity serverEntity)
        {
            return UserIdOwnsServer(int.Parse(userId), serverEntity);
        }
        
        public async Task<List<ServerEntity>> SearchServerList(string search)
        {
            var tokens = search.ToLower().Split(' ');
            var servers = (await _database.ServerList.AsNoTracking()
                    .Include(p => p.Owner)
                    .ToListAsync())
                .Where(p => tokens.All(t => p.Name.ToLower().Contains(t)))
                .OrderByDescending(p => p.StarCount)
                .Take(20)
                .ToList();

            servers.ForEach(s => s.Owner = new UserEntity
            {
                Id = s.Owner.Id,
                Username = s.Owner.Username 
            });
            return servers;
        }

        public async Task DeleteServerAsync(ServerEntity serverEntity)
        {
            _database.ServerList.Remove(serverEntity);
            await _database.SaveChangesAsync();
        }
    }
}