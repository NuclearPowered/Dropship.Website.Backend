using Dropship.Website.Backend.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dropship.Website.Backend.Database.Configurations
{
    public class ServerEntityConfiguration : IEntityTypeConfiguration<ServerEntity>
    {
        public void Configure(EntityTypeBuilder<ServerEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Name).IsRequired().HasMaxLength(32);
            builder.Property(x => x.Description).IsRequired().HasMaxLength(320);
            builder.Property(x => x.IpAddress).IsRequired().HasColumnType("int(16)");
            builder.Property(x => x.Port).IsRequired();
            builder.Property(x => x.StarCount).IsRequired();
            builder.Property(x => x.ImageUrl).IsRequired();
            builder.Property(x => x.OwnerUserId).IsRequired();

            builder.HasIndex(se => new {se.IpAddress, se.Port});

            builder
                .HasOne(se => se.Owner)
                .WithMany(ue => ue.Servers)
                .HasForeignKey(se => se.OwnerUserId)
                .HasPrincipalKey(ue => ue.Id);
        }
    }
}