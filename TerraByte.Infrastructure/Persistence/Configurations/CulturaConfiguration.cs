using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TerraByte.Domain.Entities;

namespace TerraByte.Infrastructure.Persistence.Configurations;

public class CulturaConfiguration : IEntityTypeConfiguration<Cultura>
{
    public void Configure(EntityTypeBuilder<Cultura> builder)
    {
        builder.ToTable("Plantio_terrabyte");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Nome).IsRequired().HasMaxLength(120);
        builder.Property(x => x.TempMin).HasPrecision(8, 2);
        builder.Property(x => x.TempMax).HasPrecision(8, 2);
        builder.Property(x => x.AguaMM).HasPrecision(8, 2);
        builder.Property(x => x.MesesIdeais).IsRequired().HasMaxLength(200);
        builder.Property(x => x.UrlImg).HasMaxLength(500);

        builder.HasMany(x => x.TiposSolo)
            .WithMany(x => x.Culturas)
            .UsingEntity<Dictionary<string, object>>(
                "plan_tp_terrabyte",
                right => right.HasOne<TipoSolo>().WithMany().HasForeignKey("TipoSoloId").OnDelete(DeleteBehavior.Cascade),
                left => left.HasOne<Cultura>().WithMany().HasForeignKey("CulturaId").OnDelete(DeleteBehavior.Cascade),
                join =>
                {
                    join.HasKey("CulturaId", "TipoSoloId");
                    join.ToTable("plan_tp_terrabyte");
                });

        builder.HasMany(x => x.Defensivos)
            .WithMany(x => x.Culturas)
            .UsingEntity<Dictionary<string, object>>(
                "plan_def_terrabyte",
                right => right.HasOne<Defensivo>().WithMany().HasForeignKey("DefensivoId").OnDelete(DeleteBehavior.Cascade),
                left => left.HasOne<Cultura>().WithMany().HasForeignKey("CulturaId").OnDelete(DeleteBehavior.Cascade),
                join =>
                {
                    join.HasKey("CulturaId", "DefensivoId");
                    join.ToTable("plan_def_terrabyte");
                });

        builder.HasMany(x => x.RegistrosPesquisa)
            .WithOne(x => x.Cultura)
            .HasForeignKey(x => x.CulturaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
