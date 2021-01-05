using Dropship.Website.Backend.Database.Entities;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Dropship.Website.Backend.Database
{
    public class DatabaseContext : DbContext, IDataProtectionKeyContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<ModEntity> Mods { get; set; }
        public DbSet<ModBuildEntity> ModBuilds { get; set; }
        public DbSet<PluginEntity> Plugins { get; set; }
        public DbSet<PluginBuildEntity> PluginBuilds { get; set; }
        public DbSet<ServerEntity> ServerList { get; set; }
        public DbSet<ModDepEntity> ModDeps { get; set; }
        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
        }
    }
}