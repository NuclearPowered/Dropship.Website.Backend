using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dropship.Website.Backend.Database;
using Dropship.Website.Backend.Database.Entities;
using Dropship.Website.Backend.Models.Requests.Plugins;
using Dropship.Website.Backend.Models.Responses.Update;
using Microsoft.EntityFrameworkCore;

namespace Dropship.Website.Backend.Services
{
    public class PluginService
    {
        private readonly DatabaseContext _database;

        public PluginService(DatabaseContext database)
        {
            _database = database;
        }

        public async Task<PluginEntity> CreatePluginAsync(CreatePluginRequest request, int userId)
        {
            var pluginEntity = new PluginEntity
            {
                Guid = request.Guid,
                Name = request.Name,
                Description = request.Description,
                MarkdownDescription = request.MarkdownDescription,
                StarCount = 0,
                CreatorUserId = userId,
                ServerDistroId = request.ServerDistroId,
                ImageUrl = request.ImageUrl,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            };
            await _database.Plugins.AddAsync(pluginEntity);
            await _database.SaveChangesAsync();
            return pluginEntity;
        }

        public async Task<PluginEntity> CreatePluginAsync(CreatePluginRequest request, string userId)
        {
            return await CreatePluginAsync(request, int.Parse(userId));
        }
        
        public async Task<PluginEntity> UpdatePluginAsync(UpdatePluginRequest request)
        {
            var pluginEntity = await _database.Plugins
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            pluginEntity.Name = request.Name;
            pluginEntity.Description = request.Description;
            pluginEntity.MarkdownDescription = request.MarkdownDescription;
            pluginEntity.ImageUrl = request.ImageUrl;
            pluginEntity.UpdatedAt = DateTimeOffset.UtcNow;

            _database.Plugins.Update(pluginEntity);
            await _database.SaveChangesAsync();
            return pluginEntity;
        }

        public async Task<PluginEntity> UpdatePluginStarAsync(int pluginId)
        {
            var pluginEntity = await _database.Plugins
                .FirstOrDefaultAsync(x => x.Id == pluginId);

            pluginEntity.StarCount++;

            _database.Plugins.Update(pluginEntity);
            await _database.SaveChangesAsync();
            return pluginEntity;
        }

        public async Task<PluginEntity> GetPluginByIdAsync(int pluginId)
        {
            var plugin = await _database.Plugins.AsNoTracking()
                .Include(m => m.Creator)
                .FirstOrDefaultAsync(x => x.Id == pluginId);
            if (plugin != null)
            {
                plugin.Creator = new UserEntity
                {
                    Id = plugin.Creator.Id,
                    Username = plugin.Creator.Username
                };
            }
            return plugin;
        }

        public async Task<PluginEntity> GetPluginByGuidAsync(string guid)
        {
            var plugin = await _database.Plugins.AsNoTracking()
                .Include(m => m.Creator)
                .FirstOrDefaultAsync(x => x.Guid == guid);
            if (plugin != null)
            {
                plugin.Creator = new UserEntity
                {
                    Id = plugin.Creator.Id,
                    Username = plugin.Creator.Username
                };
            }
            return plugin;
        }

        public async Task<List<PluginEntity>> GetPluginsForUserId(int userId)
        {
            return await _database.Plugins.AsNoTracking().Where(x => x.CreatorUserId == userId).ToListAsync();
        } 

        public async Task<bool> UserIdOwnsPluginIdAsync(int userId, int pluginId)
        {
            return await _database.Plugins.AsNoTracking().AnyAsync(x =>
                x.Id == pluginId && x.CreatorUserId == userId);
        }

        public async Task<bool> UserIdOwnsPluginIdAsync(string userId, int pluginId)
        {
            return await UserIdOwnsPluginIdAsync(int.Parse(userId), pluginId);
        }

        public async Task<List<PluginEntity>> GetPluginListPaginatedAsync(int pageNumber)
        {
            var count = 20;
            if (pageNumber < 1) pageNumber = 1;
            var pluginEntities = await _database.Plugins.AsNoTracking()
                .OrderByDescending(p => p.StarCount)
                .Skip(count * (pageNumber - 1)).Take(count)
                .Include(m => m.Creator)
                .ToListAsync();
            pluginEntities.ForEach(p => p.Creator = new UserEntity
            {
                Id = p.Creator.Id,
                Username = p.Creator.Username 
            });

            return pluginEntities;
        }

        public async Task<List<CheckPluginBuildUpdateResponse>> GetLatestPluginBuildsForGuids(string[] guids)
        {
            return await _database.Plugins.AsNoTracking()
                .Where(p => guids.Contains(p.Guid))
                .Include(p => p.Builds)
                .SelectMany(p => p.Builds
                    .Where(pb => pb.Deleted == false)
                    .OrderByDescending(pb => pb.VersionCode)
                    .Take(1),
                    (p, pb) => new CheckPluginBuildUpdateResponse
                    { 
                        Guid = p.Guid,
                        PluginBuild = pb 
                    })
                .ToListAsync();
        }
        
        public async Task<List<PluginEntity>> SearchPluginList(string search)
        {
            var tokens = search.ToLower().Split(' ');
            var plugins = (await _database.Plugins.AsNoTracking()
                    .Include(p => p.Creator)
                    .ToListAsync())
                .Where(p => tokens.All(t => p.Name.ToLower().Contains(t)))
                .OrderByDescending(p => p.StarCount)
                .Take(20)
                .ToList();

            plugins.ForEach(p => p.Creator = new UserEntity
            {
                Id = p.Creator.Id,
                Username = p.Creator.Username 
            });
            return plugins;
        }
        

        public async Task DeletePluginAsync(int pluginId)
        {
            _database.Plugins.Remove(new PluginEntity {Id = pluginId});
            await _database.SaveChangesAsync();
        }
    }
}