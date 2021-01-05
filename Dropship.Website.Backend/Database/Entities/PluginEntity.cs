using System;
using System.Collections.Generic;
using Dropship.Website.Backend.Database.Enums;

namespace Dropship.Website.Backend.Database.Entities
{
    public class PluginEntity
    {
        public int Id { get; set; }
        public string Guid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
        /// <summary>
        ///     Tracks the amount of times the mod has been starred.
        /// </summary>
        public int StarCount { get; set; }
        public int CreatorUserId { get; set; }
        public ServerDistro ServerDistroId { get; set; }
        public string ImageUrl { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        // Relationships.
        public UserEntity Creator { get; set; }
        public List<PluginBuildEntity> Builds { get; set; }
    }
}