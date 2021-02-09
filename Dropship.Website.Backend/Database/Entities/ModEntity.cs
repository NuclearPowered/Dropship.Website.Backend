using System;
using System.Collections.Generic;

namespace Dropship.Website.Backend.Database.Entities
{
    public class ModEntity
    {
        public int Id { get; set; }
        public string Guid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string MarkdownDescription { get; set; }
        
        /// <summary>
        ///     Tracks the amount of times the mod has been starred.
        /// </summary>
        public int StarCount { get; set; }
        public int CreatorUserId { get; set; }
        public string ImageUrl { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        // Relationships.
        public UserEntity Creator { get; set; }
        public List<ModBuildEntity> Builds { get; set; }
    }
}