using TerraByte.Application.DTOs;
using TerraByte.Application.Services.External;
using TerraByte.Application.Services.Interfaces;

namespace TerraByte.Application.Services.Implementations;

public class RegistroPesquisaService(IClienteClima clienteClima) : IRegistroPesquisaService
{
    public Task<RespostaPrevisaoClimatica> BuscarClimaAsync(double latitude, double longitude, int days)
    {
        return clienteClima.BuscarClimaAsync(latitude, longitude, days);
    }
}
