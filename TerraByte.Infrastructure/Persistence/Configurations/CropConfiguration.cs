using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TerraByte.Dominio.Entidades;

namespace TerraByte.Infraestrutura.Persistencia.Configuracoes;

public class ConfiguracaoCultura : IEntityTypeConfiguration<Cultura>
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


