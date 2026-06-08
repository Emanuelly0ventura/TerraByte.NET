using Microsoft.EntityFrameworkCore;
using TerraByte.Application.Interfaces;
using TerraByte.Domain.Entities;
using TerraByte.Infrastructure.Persistence;

namespace TerraByte.Infrastructure.Repositories;

public class UserRepository(TerraByteContext context) : IUserRepository
{
    public IReadOnlyCollection<Usuario> ListarTodos()
    {
        return context.Usuarios
            .AsNoTracking()
            .ToList();
    }

    public Usuario? BuscarPorId(Guid id)
    {
        return context.Usuarios
            .FirstOrDefault(u => u.Id == id);
    }

    public Usuario? BuscarPorEmail(string email)
    {
        return context.Usuarios
            .FirstOrDefault(u => u.Email == email);
    }

    public void Criar(Usuario usuario)
    {
        context.Usuarios.Add(usuario);
    }

    public void AtualizarParcial(Usuario usuario)
    {
        context.Usuarios.Update(usuario);
    }

    public void Excluir(Usuario usuario)
    {
        context.Usuarios.Remove(usuario);
    }

    public void SalvarAlteracoes()
    {
        context.SaveChanges();
    }
}

