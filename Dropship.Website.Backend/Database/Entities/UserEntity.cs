using System;
using System.Collections.Generic;

namespace Dropship.Website.Backend.Database.Entities
{
    public class UserEntity
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        // Relationships.
        public List<ModEntity> Mods { get; set; }
        public List<PluginEntity> Plugins { get; set; }

        public List<ServerEntity> Servers { get; set; }
    }
}