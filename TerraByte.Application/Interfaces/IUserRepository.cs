using TerraByte.Domain.Entities;

namespace TerraByte.Application.Interfaces;

public interface IUserRepository : IRepository<Usuario>
{
    Usuario? BuscarPorEmail(string email);
}
