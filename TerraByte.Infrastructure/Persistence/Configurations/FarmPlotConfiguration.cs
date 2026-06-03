using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TerraByte.Domain.Entities;

namespace TerraByte.Infrastructure.Persistence.Configurations;

public class FarmPlotConfiguration : IEntityTypeConfiguration<FarmPlot>
{
    public void Configure(EntityTypeBuilder<FarmPlot> builder)
    {
        builder.ToTable("TB_FarmPlots");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.Cep).IsRequired().HasMaxLength(9);
        builder.Property(x => x.Street).HasMaxLength(180);
        builder.Property(x => x.District).HasMaxLength(120);
        builder.Property(x => x.City).HasMaxLength(120);
        builder.Property(x => x.State).HasMaxLength(2);
        builder.Property(x => x.SoilName).IsRequired().HasMaxLength(120);

        builder.HasMany(x => x.Crops)
            .WithOne(x => x.FarmPlot)
            .HasForeignKey(x => x.FarmPlotId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.ResearchSnapshots)
            .WithOne(x => x.FarmPlot)
            .HasForeignKey(x => x.FarmPlotId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
