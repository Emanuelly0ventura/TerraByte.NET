using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TerraByte.Domain.Entities;

namespace TerraByte.Infrastructure.Persistence.Configurations;

public class ResearchSnapshotConfiguration : IEntityTypeConfiguration<ResearchSnapshot>
{
    public void Configure(EntityTypeBuilder<ResearchSnapshot> builder)
    {
        builder.ToTable("TB_ResearchSnapshots");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Source).IsRequired().HasMaxLength(80);
        builder.Property(x => x.Kind).IsRequired().HasMaxLength(60);
        builder.Property(x => x.Summary).IsRequired().HasMaxLength(2000);
    }
}
