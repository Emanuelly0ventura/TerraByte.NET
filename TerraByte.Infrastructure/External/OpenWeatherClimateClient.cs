using System.Globalization;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using TerraByte.Application.DTOs;
using TerraByte.Application.Services.External;

namespace TerraByte.Infrastructure.External;

public class OpenWeatherClimateClient(
    HttpClient httpClient,
    IConfiguration configuration) : IClimateClient
{
    public async Task<ClimateForecastResponse> FetchClimateAsync(double latitude, double longitude, int days)
    {
        var apiKey = configuration["OpenWeather:ApiKey"];
        if (string.IsNullOrWhiteSpace(apiKey))
            throw new InvalidOperationException("Configure a chave em OpenWeather:ApiKey para consultar a previsão climática.");

        var safeDays = Math.Clamp(days, 1, 30);
        var lat = latitude.ToString(CultureInfo.InvariantCulture);
        var lon = longitude.ToString(CultureInfo.InvariantCulture);
        var url = $"data/2.5/forecast/climate?lat={lat}&lon={lon}&cnt={safeDays}&units=metric&lang=pt_br&appid={apiKey}";

        using var response = await httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync();
        using var document = await JsonDocument.ParseAsync(stream);

        var list = document.RootElement.GetProperty("list");
        var total = list.GetArrayLength();
        if (total == 0)
        {
            return new ClimateForecastResponse
            {
                Latitude = latitude,
                Longitude = longitude,
                Days = safeDays,
                Summary = "A API retornou a previsão, mas sem dias disponíveis."
            };
        }

        var min = double.MaxValue;
        var max = double.MinValue;
        var rainTotal = 0d;

        foreach (var item in list.EnumerateArray())
        {
            var temp = item.GetProperty("temp");
            min = Math.Min(min, temp.GetProperty("min").GetDouble());
            max = Math.Max(max, temp.GetProperty("max").GetDouble());

            if (item.TryGetProperty("rain", out var rain))
                rainTotal += rain.GetDouble();
        }

        return new ClimateForecastResponse
        {
            Latitude = latitude,
            Longitude = longitude,
            Days = safeDays,
            Summary = $"Previsão de {total} dias: mínima média observada {min:0.0} C, máxima {max:0.0} C e chuva acumulada aproximada de {rainTotal:0.0} mm."
        };
    }
}
