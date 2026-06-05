using TerraByte.Application.DTOs;

namespace TerraByte.Application.Services.Interfaces;

public interface IRegistroPesquisaService
{
    Task<RespostaPrevisaoClimatica> BuscarClimaAsync(double latitude, double longitude, int days);
}
