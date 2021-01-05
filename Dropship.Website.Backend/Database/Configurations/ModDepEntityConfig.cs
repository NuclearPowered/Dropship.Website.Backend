using Dropship.Website.Backend.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dropship.Website.Backend.Database.Configurations
{
    public class ModDependencyEntityConfiguration : IEntityTypeConfiguration<ModDepEntity>
    {
        public void Configure(EntityTypeBuilder<ModDepEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.ModBuildId).IsRequired();
            builder.Property(x => x.DepModBuildId).IsRequired();


            builder
                .HasOne(mde => mde.ModBuild)
                .WithMany(mbe => mbe.Dependencies)
                .HasForeignKey(mde => mde.ModBuildId)
                .HasPrincipalKey(mbe => mbe.Id);
            builder
                .HasOne(mde => mde.DepModBuild)
                .WithMany(mbe => mbe.Dependents)
                .HasForeignKey(mde => mde.DepModBuildId)
                .HasPrincipalKey(mbe => mbe.Id);
        }
    }
}