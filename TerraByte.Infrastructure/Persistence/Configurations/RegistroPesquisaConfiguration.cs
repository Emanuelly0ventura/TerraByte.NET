using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TerraByte.Domain.Entities;

namespace TerraByte.Infrastructure.Persistence.Configurations;

public class RegistroPesquisaConfiguration : IEntityTypeConfiguration<RegistroPesquisa>
{
    public void Configure(EntityTypeBuilder<RegistroPesquisa> builder)
    {
        builder.ToTable("TB_ResearchSnapshots");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Fonte).IsRequired().HasMaxLength(80);
        builder.Property(x => x.Tipo).IsRequired().HasMaxLength(60);
        builder.Property(x => x.Resumo).IsRequired().HasMaxLength(2000);
    }
}


