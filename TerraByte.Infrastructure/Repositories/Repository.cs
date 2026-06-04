using Microsoft.EntityFrameworkCore;
using TerraByte.Aplicacao.Interfaces;
using TerraByte.Infraestrutura.Persistencia;

namespace TerraByte.Infraestrutura.Repositorios;

public class Repositorio<T>(TerraByteContext context) : IRepositorio<T> where T : class
{
    protected TerraByteContext Context { get; } = context;
    protected DbSet<T> Set => Context.Set<T>();

    public virtual IReadOnlyCollection<T> ListarTodos()
    {
        return Set.ToList();
    }

    public virtual T? BuscarPorId(Guid id)
    {
        return Set.Find(id);
    }

    public void Criar(T entidade)
    {
        Set.Add(entidade);
    }

    public void AtualizarParcial(T entidade)
    {
        Set.Update(entidade);
    }

    public void Excluir(T entidade)
    {
        Set.Remove(entidade);
    }

    public void SalvarAlteracoes()
    {
        Context.SaveChanges();
    }
}


