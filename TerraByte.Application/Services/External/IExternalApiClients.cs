using TerraByte.Application.DTOs;

namespace TerraByte.Application.Services.External;

public interface IExternalApiClient
{
    Task<RespostaConsultaEndereco?> BuscarEnderecoAsync(string cep);
}

public interface IClienteGeocodificacao
{
    Task<RespostaGeocodificacao?> BuscarCoordenadasAsync(string localizacao);
}

public interface IClienteClima
{
    Task<RespostaPrevisaoClimatica> BuscarClimaAsync(double latitude, double longitude, int days);
}

public interface IClienteSolo
{
    Task<RespostaClassificacaoSolo> BuscarSoloAsync(double latitude, double longitude);
}

