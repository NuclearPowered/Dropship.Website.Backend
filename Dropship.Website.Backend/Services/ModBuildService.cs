using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dropship.Website.Backend.Database;
using Dropship.Website.Backend.Database.Entities;
using Dropship.Website.Backend.Models.Requests.ModBuilds;
using Microsoft.EntityFrameworkCore;

namespace Dropship.Website.Backend.Services
{
    public class ModBuildsService
    {
        private readonly DatabaseContext _database;

        public ModBuildsService(DatabaseContext database)
        {
            _database = database;
        }

        public async Task<ModBuildEntity> CreateModBuildAsync(CreateModBuildRequest request)
        {
            var latestModBuildVersion = await _database.ModBuilds
                .CountAsync(entity => entity.ModId == request.ModId) + 1;

            var modBuildEntity = new ModBuildEntity
            {
                ModId = request.ModId,
                VersionCode = latestModBuildVersion,
                Version = request.Version,
                FileName = request.FileName,
                DownloadUrl = request.DownloadUrl,
                Deleted = false
            };
            await _database.ModBuilds.AddAsync(modBuildEntity);

            foreach (var depId in request.DependencyIds)
            {
                await _database.ModDeps.AddAsync(new ModDepEntity
                {
                    ModBuild = modBuildEntity,
                    DepModBuildId = depId,
                });
            }
            
            await _database.SaveChangesAsync();
            return modBuildEntity;
        }

        public async Task<ModBuildEntity> GetModBuildAsync(int modBuildId)
        { 
            return await _database.ModBuilds.AsNoTracking().FirstOrDefaultAsync(mb => mb.Id == modBuildId); 
        }
        
        public async Task<ModBuildEntity> GetModBuildAsync(int modId, int versionCode)
        { 
            return await _database.ModBuilds.AsNoTracking().FirstOrDefaultAsync(mb => mb.ModId == modId && mb.VersionCode == versionCode); 
        }

        public async Task<List<ModBuildEntity>> ModBuildsPaginatedAsync(int modId, int pageNumber)
        {
            var count = 20;
            if (pageNumber < 1) pageNumber = 1;
            return await _database.ModBuilds.AsNoTracking()
                .Where(mb => mb.ModId == modId)
                .Where(mb => mb.Deleted == false)
                .OrderByDescending(mb => mb.VersionCode)
                .Skip(count * (pageNumber - 1)).Take(count).ToListAsync();
        }

        public async Task DeleteModBuildAsync(ModBuildEntity modBuild)
        {
            modBuild.Deleted = true;
            _database.ModBuilds.Update(modBuild);
            await _database.SaveChangesAsync();
        }
    }
}
