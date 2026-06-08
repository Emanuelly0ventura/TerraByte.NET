using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TerraByte.Domain.Entities;

namespace TerraByte.Infrastructure.Persistence.Configurations;

public class RegistroPesquisaConfiguration : IEntityTypeConfiguration<RegistroPesquisa>
{
    public void Configure(EntityTypeBuilder<RegistroPesquisa> builder)
    {
        builder.ToTable("AnalisePlantio_terrabyte");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.TempMin).HasPrecision(8, 2);
        builder.Property(x => x.TempMax).HasPrecision(8, 2);
        builder.Property(x => x.UmidadeMed).HasPrecision(8, 2);
        builder.Property(x => x.ChuvaPrevistaMm).HasPrecision(8, 2);
        builder.Property(x => x.AdequadoPlantio).HasPrecision(8, 2);
        builder.Property(x => x.NivelRisco).IsRequired().HasMaxLength(60);
        builder.Property(x => x.Recomendacao).IsRequired().HasMaxLength(2000);

        builder.HasOne(x => x.Usuario)
            .WithMany(x => x.RegistrosPesquisa)
            .HasForeignKey(x => x.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.TerrenoAgricola)
            .WithMany(x => x.RegistrosPesquisa)
            .HasForeignKey(x => x.TerrenoAgricolaId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Cultura)
            .WithMany(x => x.RegistrosPesquisa)
            .HasForeignKey(x => x.CulturaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
