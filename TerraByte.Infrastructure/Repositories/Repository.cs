using Microsoft.EntityFrameworkCore;
using TerraByte.Application.Interfaces;
using TerraByte.Infrastructure.Persistence;

namespace TerraByte.Infrastructure.Repositories;

public class Repository<T>(TerraByteContext context) : IRepository<T> where T : class
{
    protected TerraByteContext Context { get; } = context;
    protected DbSet<T> Set => Context.Set<T>();

    public virtual IReadOnlyCollection<T> FetchAll()
    {
        return Set.ToList();
    }

    public virtual T? FetchById(Guid id)
    {
        return Set.Find(id);
    }

    public void Create(T entity)
    {
        Set.Add(entity);
    }

    public void Patch(T entity)
    {
        Set.Update(entity);
    }

    public void Delete(T entity)
    {
        Set.Remove(entity);
    }

    public void SaveChanges()
    {
        Context.SaveChanges();
    }
}
