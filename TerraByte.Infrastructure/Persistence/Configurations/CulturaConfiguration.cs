using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TerraByte.Domain.Entities;

namespace TerraByte.Infrastructure.Persistence.Configurations;

public class CulturaConfiguration : IEntityTypeConfiguration<Cultura>
{
    public void Configure(EntityTypeBuilder<Cultura> builder)
    {
        builder.ToTable("TB_Crops");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Nome).IsRequired().HasMaxLength(120);
        builder.Property(x => x.NomeSemente).IsRequired().HasMaxLength(120);
        builder.Property(x => x.Observacoes).HasMaxLength(500);
    }
}


