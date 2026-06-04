using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TerraByte.Domain.Entities;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
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
        
        builder.HasMany(x => x.FarmPlots)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
    
    
}