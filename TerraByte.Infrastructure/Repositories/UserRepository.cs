using Microsoft.EntityFrameworkCore;
using TerraByte.Aplicacao.Interfaces;
using TerraByte.Dominio.Entidades;
using TerraByte.Infraestrutura.Persistencia;

namespace TerraByte.Infraestrutura.Repositorios;

public class RepositorioUsuario(TerraByteContext context) : IRepositorioUsuario
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

    public void AtualizarParcial(Usuario entidade)
    {
        throw new NotImplementedException();
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

