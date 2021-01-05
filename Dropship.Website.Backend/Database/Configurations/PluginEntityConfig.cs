using Dropship.Website.Backend.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dropship.Website.Backend.Database.Configurations
{
    public class PluginEntityConfiguration : IEntityTypeConfiguration<PluginEntity>
    {
        public void Configure(EntityTypeBuilder<PluginEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Guid).IsRequired().HasMaxLength(128);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(32);
            builder.Property(x => x.Description).IsRequired().HasColumnType("mediumtext");
            builder.Property(x => x.StarCount).IsRequired();
            builder.Property(x => x.CreatorUserId).IsRequired();
            builder.Property(x => x.ServerDistroId).IsRequired();
            builder.Property(x => x.ImageUrl).IsRequired();
            builder.Property(x => x.CreatedAt).IsRequired();
            builder.Property(x => x.UpdatedAt).IsRequired();

            builder.HasIndex(x => x.Guid).IsUnique();

            builder
                .HasOne(x => x.Creator)
                .WithMany(x => x.Plugins)
                .HasForeignKey(x => x.CreatorUserId)
                .HasPrincipalKey(x => x.Id);

            builder
                .HasMany(x => x.Builds)
                .WithOne(x => x.Plugin)
                .HasForeignKey(x => x.PluginId)
                .HasPrincipalKey(x => x.Id);
        }
    }
}