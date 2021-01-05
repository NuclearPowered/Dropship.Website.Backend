using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dropship.Website.Backend.Database;
using Dropship.Website.Backend.Database.Entities;
using Dropship.Website.Backend.Models.Requests.PluginBuilds;
using Microsoft.EntityFrameworkCore;

namespace Dropship.Website.Backend.Services
{
    public class PluginBuildService
    {
        private readonly DatabaseContext _database;

        public PluginBuildService(DatabaseContext database)
        {
            _database = database;
        }

        public async Task<PluginBuildEntity> CreatePluginBuildAsync(CreatePluginBuildRequest request)
        {
            var latestPluginBuildVersion = await _database.PluginBuilds.AsNoTracking()
                .CountAsync(entity => entity.PluginId == request.PluginId) + 1;

            var pluginBuildEntity = new PluginBuildEntity
            {
                PluginId = request.PluginId,
                VersionCode = latestPluginBuildVersion,
                Version = request.Version,
                FileName = request.FileName,
                DownloadUrl = request.DownloadUrl,
                Deleted = false
            };
            await _database.PluginBuilds.AddAsync(pluginBuildEntity);
            await _database.SaveChangesAsync();
            return pluginBuildEntity;
        }

        public async Task<PluginBuildEntity> GetPluginBuildAsync(int pluginBuildId)
        { 
            return await _database.PluginBuilds.AsNoTracking()
                .FirstOrDefaultAsync(pb => pb.Id == pluginBuildId); 
        }
        
        public async Task<PluginBuildEntity> GetPluginBuildAsync(int pluginId, int versionCode)
        { 
            return await _database.PluginBuilds.AsNoTracking()
                .FirstOrDefaultAsync(pb => pb.PluginId == pluginId && pb.VersionCode == versionCode); 
        }

        public async Task<List<PluginBuildEntity>> PluginBuildsPaginatedAsync(int pluginId, int pageNumber)
        {
            var count = 20;
            if (pageNumber < 1) pageNumber = 1;
            return await _database.PluginBuilds.AsNoTracking()
                .Where(pb => pb.PluginId == pluginId)
                .Where(pb => pb.Deleted == false)
                .OrderByDescending(pb => pb.VersionCode)
                .Skip(count * (pageNumber - 1)).Take(count).ToListAsync();
        }

        public async Task DeletePluginBuildAsync(PluginBuildEntity pluginBuild)
        {
            pluginBuild.Deleted = true;
            _database.PluginBuilds.Update(pluginBuild);
            await _database.SaveChangesAsync();
        }
    }
}
