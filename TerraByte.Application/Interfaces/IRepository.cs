namespace TerraByte.Application.Interfaces;

public interface IRepository<T> where T : class
{
    IReadOnlyCollection<T> ListarTodos();
    T? BuscarPorId(Guid id);
    void Criar(T entidade);
    void AtualizarParcial(T entidade);
    void Excluir(T entidade);
    void SalvarAlteracoes();
}

