using Dropship.Website.Backend.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dropship.Website.Backend.Database.Configurations
{
    public class ModBuildEntityConfiguration : IEntityTypeConfiguration<ModBuildEntity>
    {
        public void Configure(EntityTypeBuilder<ModBuildEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.ModId).IsRequired();
            builder.Property(x => x.Version).IsRequired().HasMaxLength(16);
            builder.Property(x => x.VersionCode).IsRequired();
            builder.Property(x => x.FileName).IsRequired().HasMaxLength(128);
            builder.Property(x => x.Deleted).IsRequired();

            builder.HasIndex(x => new {x.ModId, x.Version});
            builder.HasIndex(x => new {x.ModId, x.VersionCode});

            builder
                .HasOne(x => x.Mod)
                .WithMany(x => x.Builds)
                .HasForeignKey(x => x.ModId)
                .HasPrincipalKey(x => x.Id);
            builder
                .HasMany(mbe => mbe.Dependencies)
                .WithOne(mde => mde.ModBuild)
                .HasForeignKey(mde => mde.ModBuildId)
                .HasPrincipalKey(mbe => mbe.Id);
            builder
                .HasMany(mbe => mbe.Dependents)
                .WithOne(mde => mde.DepModBuild)
                .HasForeignKey(mde => mde.DepModBuildId)
                .HasPrincipalKey(mbe => mbe.Id);
        }
    }
}