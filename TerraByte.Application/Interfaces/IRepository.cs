namespace TerraByte.Application.Interfaces;

public interface IRepository<T> where T : class
{
    IReadOnlyCollection<T> FetchAll();
    T? FetchById(Guid id);
    void Create(T entity);
    void Patch(T entity);
    void Delete(T entity);
    void SaveChanges();
}
