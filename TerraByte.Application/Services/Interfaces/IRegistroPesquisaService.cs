using TerraByte.Aplicacao.Dtos;

namespace TerraByte.Aplicacao.Servicos.Interfaces;

public interface IServicoPesquisa
{
    Task<RespostaPrevisaoClimatica> BuscarClimaAsync(double latitude, double longitude, int days);
}
