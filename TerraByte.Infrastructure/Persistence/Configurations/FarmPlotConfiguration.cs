using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TerraByte.Dominio.Entidades;

namespace TerraByte.Infraestrutura.Persistencia.Configuracoes;

public class ConfiguracaoTerrenoAgricola : IEntityTypeConfiguration<TerrenoAgricola>
{
    public void Configure(EntityTypeBuilder<TerrenoAgricola> builder)
    {
        builder.ToTable("TB_FarmPlots");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Nome).IsRequired().HasMaxLength(150);
        builder.Property(x => x.Cep).IsRequired().HasMaxLength(9);
        builder.Property(x => x.Logradouro).HasMaxLength(180);
        builder.Property(x => x.Bairro).HasMaxLength(120);
        builder.Property(x => x.Cidade).HasMaxLength(120);
        builder.Property(x => x.Estado).HasMaxLength(2);
        builder.Property(x => x.NomeSolo).IsRequired().HasMaxLength(120);

        builder.HasMany(x => x.Culturas)
            .WithOne(x => x.TerrenoAgricola)
            .HasForeignKey(x => x.TerrenoAgricolaId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.RegistrosPesquisa)
            .WithOne(x => x.TerrenoAgricola)
            .HasForeignKey(x => x.TerrenoAgricolaId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder
            .HasOne(fp => fp.Usuario)
            .WithMany(u => u.TerrenosAgricolas)
            .HasForeignKey(fp => fp.UsuarioId);
    }
}


