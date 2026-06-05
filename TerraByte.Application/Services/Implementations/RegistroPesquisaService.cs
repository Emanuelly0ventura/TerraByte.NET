using TerraByte.Aplicacao.Dtos;
using TerraByte.Aplicacao.Servicos.Externo;
using TerraByte.Aplicacao.Servicos.Interfaces;

namespace TerraByte.Aplicacao.Servicos.Implementacoes;

public class ServicoPesquisa(IClienteClima clienteClima) : IServicoPesquisa
{
    public Task<RespostaPrevisaoClimatica> BuscarClimaAsync(double latitude, double longitude, int days)
    {
        return clienteClima.BuscarClimaAsync(latitude, longitude, days);
    }
}
