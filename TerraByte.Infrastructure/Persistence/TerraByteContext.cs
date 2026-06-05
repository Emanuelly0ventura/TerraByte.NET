using Microsoft.EntityFrameworkCore;
using TerraByte.Domain.Entities;

namespace TerraByte.Infrastructure.Persistence;

public class TerraByteContext(DbContextOptions<TerraByteContext> opcoes) : DbContext(opcoes)
{
    public DbSet<TerrenoAgricola> TerrenosAgricolas => Set<TerrenoAgricola>();
    public DbSet<Cultura> Culturas => Set<Cultura>();
    public DbSet<RegistroPesquisa> RegistrosPesquisa => Set<RegistroPesquisa>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    protected override void OnModelCreating(ModelBuilder montadorModelo)
    {
        montadorModelo.ApplyConfigurationsFromAssembly(typeof(TerraByteContext).Assembly);
        base.OnModelCreating(montadorModelo);
    }
}

