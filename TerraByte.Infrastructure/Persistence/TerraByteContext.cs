using Microsoft.EntityFrameworkCore;
using TerraByte.Domain.Entities;

namespace TerraByte.Infrastructure.Persistence;

public class TerraByteContext(DbContextOptions<TerraByteContext> options) : DbContext(options)
{
    public DbSet<FarmPlot> FarmPlots => Set<FarmPlot>();
    public DbSet<Crop> Crops => Set<Crop>();
    public DbSet<ResearchSnapshot> ResearchSnapshots => Set<ResearchSnapshot>();
    public DbSet<User> Users => Set<User>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TerraByteContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
