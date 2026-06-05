using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TerraByte.Domain.Entities;

namespace TerraByte.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("TB_Users");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Nome)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Senha)
            .IsRequired();
        
        builder.Property(x => x.Telefone)
            .HasMaxLength(20);

        builder.Property(x => x.Genero)
            .HasMaxLength(20);

        builder.Property(x => x.FotoPerfil)
            .HasMaxLength(500);

        builder.Property(x => x.DataNascimento)
            .IsRequired();
        
        builder.HasMany(x => x.TerrenosAgricolas)
            .WithOne(x => x.Usuario)
            .HasForeignKey(x => x.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);
    }
    
    
}