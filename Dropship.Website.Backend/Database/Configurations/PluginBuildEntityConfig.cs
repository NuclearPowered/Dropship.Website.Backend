using Dropship.Website.Backend.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dropship.Website.Backend.Database.Configurations
{
    public class PluginBuildEntityConfiguration : IEntityTypeConfiguration<PluginBuildEntity>
    {
        public void Configure(EntityTypeBuilder<PluginBuildEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.PluginId).IsRequired();
            builder.Property(x => x.Version).IsRequired().HasMaxLength(16);
            builder.Property(x => x.VersionCode).IsRequired();
            builder.Property(x => x.FileName).IsRequired().HasMaxLength(128);
            builder.Property(x => x.Deleted).IsRequired();

            builder.HasIndex(x => new {x.PluginId, x.Version});
            builder.HasIndex(x => new {x.PluginId, x.VersionCode});

            builder
                .HasOne(x => x.Plugin)
                .WithMany(x => x.Builds)
                .HasForeignKey(x => x.PluginId)
                .HasPrincipalKey(x => x.Id);
        }
    }
}