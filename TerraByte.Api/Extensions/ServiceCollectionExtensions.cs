using Microsoft.EntityFrameworkCore;
using TerraByte.Application.Interfaces;
using TerraByte.Application.Services.External;
using TerraByte.Application.Services.Implementations;
using TerraByte.Application.Services.Interfaces;
using TerraByte.Infrastructure.External;
using TerraByte.Infrastructure.Persistence;
using TerraByte.Infrastructure.Repositories;

namespace TerraByte.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTerraBytePersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("TerraByteSqlite")
            ?? "Data Source=terrabyte.db";

        services.AddDbContext<TerraByteContext>(options =>
            options.UseSqlite(connectionString));

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IFarmPlotRepository, FarmPlotRepository>();
        services.AddScoped<ICropRepository, CropRepository>();
        services.AddScoped<IResearchSnapshotRepository, ResearchSnapshotRepository>();

        return services;
    }

    public static IServiceCollection AddTerraByteServices(this IServiceCollection services)
    {
        services.AddScoped<IFarmPlotService, FarmPlotService>();
        services.AddScoped<ICropService, CropService>();
        services.AddScoped<IResearchService, ResearchService>();

        return services;
    }

    public static IServiceCollection AddExternalApiClients(this IServiceCollection services)
    {
        services.AddHttpClient<IAddressLookupClient, ViaCepAddressClient>(client =>
        {
            client.BaseAddress = new Uri("https://viacep.com.br/");
        });

        services.AddHttpClient<IGeocodingClient, OpenMeteoGeocodingClient>(client =>
        {
            client.BaseAddress = new Uri("https://geocoding-api.open-meteo.com/");
        });

        services.AddHttpClient<IClimateClient, OpenWeatherClimateClient>(client =>
        {
            client.BaseAddress = new Uri("https://pro.openweathermap.org/");
        });

        services.AddHttpClient<ISoilClient, SoilGridsClient>(client =>
        {
            client.BaseAddress = new Uri("https://rest.isric.org/");
        });

        return services;
    }
}
