using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TerraByte.Domain.Entities;

namespace TerraByte.Infrastructure.Persistence.Configurations;

public class DefensivoConfiguration : IEntityTypeConfiguration<Defensivo>
{
    public void Configure(EntityTypeBuilder<Defensivo> builder)
    {
        builder.ToTable("Defensivo_terrabyte");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Nome)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Tipo)
            .IsRequired()
            .HasMaxLength(100);
    }
}
