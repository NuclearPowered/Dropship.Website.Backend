using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dropship.Website.Backend.Database;
using Dropship.Website.Backend.Database.Entities;
using Dropship.Website.Backend.Models.Requests.Mods;
using Dropship.Website.Backend.Models.Requests.Update;
using Dropship.Website.Backend.Models.Responses.Update;
using Microsoft.EntityFrameworkCore;

namespace Dropship.Website.Backend.Services
{
    public class ModsService
    {
        private readonly DatabaseContext _database;

        public ModsService(DatabaseContext database)
        {
            _database = database;
        }

        public async Task<ModEntity> CreateModAsync(CreateModRequest request, int userId)
        {
            var modEntity = new ModEntity
            {
                Guid = request.Guid,
                Name = request.Name,
                Description = request.Description,
                StarCount = 0,
                CreatorUserId = userId,
                ImageUrl = request.ImageUrl,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            };
            await _database.Mods.AddAsync(modEntity);
            await _database.SaveChangesAsync();
            return modEntity;
        }

        public async Task<ModEntity> CreateModAsync(CreateModRequest request, string userId)
        {
            return await CreateModAsync(request, int.Parse(userId));
        }
        
        public async Task<ModEntity> UpdateModAsync(UpdateModRequest request)
        {
            var modEntity = await _database.Mods
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            modEntity.Name = request.Name;
            modEntity.Description = request.Description;
            modEntity.ImageUrl = request.ImageUrl;
            modEntity.UpdatedAt = DateTimeOffset.UtcNow;

            _database.Mods.Update(modEntity);
            await _database.SaveChangesAsync();
            return modEntity;
        }

        public async Task<ModEntity> UpdateModStarAsync(int modId)
        {
            var modEntity = await _database.Mods
                .FirstOrDefaultAsync(x => x.Id == modId);

            modEntity.StarCount++;

            _database.Mods.Update(modEntity);
            await _database.SaveChangesAsync();
            return modEntity;
        }

        public async Task<ModEntity> GetModByIdAsync(int modId)
        {
            var mod = await _database.Mods.AsNoTracking()
                .Include(m => m.Creator)
                .FirstOrDefaultAsync(x => x.Id == modId);
            if (mod != null)
            {
                mod.Creator = new UserEntity
                {
                    Id = mod.Creator.Id,
                    Username = mod.Creator.Username
                };
            }
            return mod;
        }
        public async Task<ModEntity> GetModByGuidAsync(string guid)
        {
            var mod = await _database.Mods.AsNoTracking()
                .Include(m => m.Creator)
                .FirstOrDefaultAsync(x => x.Guid == guid);
            if (mod != null)
            {
                mod.Creator = new UserEntity
                {
                    Id = mod.Creator.Id,
                    Username = mod.Creator.Username
                };
            }
            return mod;
        }

        public async Task<List<ModEntity>> GetModsForUserId(int userId)
        {
            return await _database.Mods.AsNoTracking().Where(x => x.CreatorUserId == userId).ToListAsync();
        } 

        public async Task<bool> UserIdOwnsModIdAsync(int userId, int modId)
        {
            return await _database.Mods.AsNoTracking().AnyAsync(x =>
                x.Id == modId && x.CreatorUserId == userId);
        }

        public async Task<bool> UserIdOwnsModIdAsync(string userId, int modId)
        {
            return await UserIdOwnsModIdAsync(int.Parse(userId), modId);
        }

        public async Task<List<ModEntity>> GetModListPaginatedAsync(int pageNumber)
        {
            var count = 20;
            if (pageNumber < 1) pageNumber = 1;
            var modEntities = await _database.Mods.AsNoTracking()
                .OrderByDescending(m => m.StarCount)
                .Skip(count * (pageNumber - 1)).Take(count)
                .Include(m => m.Creator)
                .ToListAsync();
            modEntities.ForEach(m => m.Creator = new UserEntity
            {
                Id = m.Creator.Id,
                Username = m.Creator.Username 
            });
            return modEntities;
        }

        public async Task<List<CheckModBuildUpdateResponse>> GetLatestModBuilds(CheckModBuildUpdateRequest request)
        {
            return await _database.Mods.AsNoTracking()
                .Where(m => request.Guids.Contains(m.Guid))
                .Include(m => m.Builds)
                .SelectMany(g => g.Builds
                        .Where(mb => mb.Deleted == false)
                        .Where(mb => mb.GamePlatform == request.GamePlatform)
                        .Where(mb => mb.GameVersion == request.GameVersion)
                        .OrderByDescending(mb => mb.VersionCode)
                        .Take(1),
                    (m, mb) => new CheckModBuildUpdateResponse
                    { 
                        Guid = m.Guid,
                        ModBuild = mb 
                    })
                .ToListAsync();
        }
        
        public async Task<List<ModEntity>> SearchModList(string search)
        {
            var tokens = search.ToLower().Split(' ');
            var mods = (await _database.Mods.AsNoTracking()
                    .Include(m => m.Creator)
                    .ToListAsync())
                .Where(m => tokens.All(t => m.Name.ToLower().Contains(t)))
                .OrderByDescending(m => m.StarCount)
                .Take(20)
                .ToList();
            
            mods.ForEach(m => m.Creator = new UserEntity
            {
                Id = m.Creator.Id,
                Username = m.Creator.Username 
            });
            return mods;
        }

        public async Task DeleteModAsync(int modId)
        {
            _database.Mods.Remove(new ModEntity {Id = modId});
            await _database.SaveChangesAsync();
        }
    }
}