using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TerraByte.Domain.Entities;

namespace TerraByte.Infrastructure.Persistence.Configurations;

public class TipoSoloConfiguration : IEntityTypeConfiguration<TipoSolo>
{
    public void Configure(EntityTypeBuilder<TipoSolo> builder)
    {
        builder.ToTable("TipoSolo_terrabyte");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Nome)
            .IsRequired()
            .HasMaxLength(150);

        builder.HasIndex(x => x.Nome)
            .IsUnique();
    }
}
