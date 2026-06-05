using Microsoft.EntityFrameworkCore;
using TerraByte.Application.Interfaces;
using TerraByte.Application.Services.External;
using TerraByte.Application.Services.Implementations;
using TerraByte.Application.Services.Interfaces;
using TerraByte.Infrastructure.External;
using TerraByte.Infrastructure.Persistence;
using TerraByte.Infrastructure.Repositories;

namespace TerraByte.Api.Extensions;

public static class ExtensoesColecaoServicos
{
    public static IServiceCollection AdicionarPersistenciaTerraByte(this IServiceCollection servicos, IConfiguration configuracao)
    {
        var stringConexao = configuracao.GetConnectionString("TerraByteSqlite")
            ?? "Data Source=terrabyte.db";

        servicos.AddDbContext<TerraByteContext>(opcoes =>
            opcoes.UseSqlite(stringConexao));

        servicos.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        servicos.AddScoped<ITerrenoAgricolaRepository, TerrenoAgricolaRepository>();
        servicos.AddScoped<ICulturaRepository, CulturaRepository>();
        servicos.AddScoped<IRegistroPesquisaRepository, RegistroPesquisaRepository>();
        servicos.AddScoped<IUserRepository, UserRepository>();

        return servicos;
    }

    public static IServiceCollection AdicionarServicosTerraByte(this IServiceCollection servicos)
    {
        servicos.AddScoped<ITerrenoAgricolaService, TerrenoAgricolaService>();
        servicos.AddScoped<ICulturaService, CulturaService>();
        servicos.AddScoped<IRegistroPesquisaService, RegistroPesquisaService>();
        servicos.AddScoped<IUserService, UserService>();

        return servicos;
    }

    public static IServiceCollection AdicionarClientesApisExternas(this IServiceCollection servicos)
    {
        servicos.AddHttpClient<IExternalApiClient, ClientEnderecoViaCep>(client =>
        {
            client.BaseAddress = new Uri("https://viacep.com.br/");
        });

        servicos.AddHttpClient<IClienteGeocodificacao, ClientGeocodificacaoOpenMeteo>(client =>
        {
            client.BaseAddress = new Uri("https://geocoding-api.open-meteo.com/");
        });

        servicos.AddHttpClient<IClienteClima, ClientClimaOpenWeather>(client =>
        {
            client.BaseAddress = new Uri("https://api.openweathermap.org/");
        });

        servicos.AddHttpClient<IClienteSolo, ClientSoloSoilGrids>(client =>
        {
            client.BaseAddress = new Uri("https://rest.isric.org/");
        });

        return servicos;
    }
}


