using System.Globalization;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using TerraByte.Application.DTOs;
using TerraByte.Application.Services.External;

namespace TerraByte.Infrastructure.External;

public class ClientClimaOpenWeather(
    HttpClient httpClient,
    IConfiguration configuracao) : IClienteClima
{
    public async Task<RespostaPrevisaoClimatica> BuscarClimaAsync(double latitude, double longitude, int days)
    {
        var apiKey = configuracao["OpenWeather:ApiKey"];
        if (string.IsNullOrWhiteSpace(apiKey))
            throw new InvalidOperationException("Configure a chave em OpenWeather:ApiKey para consultar a previsao climatica.");

        var safeDays = Math.Clamp(days, 1, 30);
        var timestamps = Math.Min(safeDays, 5) * 8;
        var lat = latitude.ToString(CultureInfo.InvariantCulture);
        var lon = longitude.ToString(CultureInfo.InvariantCulture);
        var url = $"data/2.5/forecast?lat={lat}&lon={lon}&cnt={timestamps}&units=metric&lang=pt_br&appid={apiKey}";

        using var response = await httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync();
        using var document = await JsonDocument.ParseAsync(stream);

        var list = document.RootElement.GetProperty("list");
        var total = list.GetArrayLength();
        if (total == 0)
        {
            return new RespostaPrevisaoClimatica
            {
                Latitude = latitude,
                Longitude = longitude,
                Dias = safeDays,
                Resumo = "A API retornou a previsao, mas sem horarios disponiveis."
            };
        }

        var min = double.MaxValue;
        var max = double.MinValue;
        var rainTotal = 0d;
        var humidityTotal = 0d;

        foreach (var item in list.EnumerateArray())
        {
            var main = item.GetProperty("main");
            min = Math.Min(min, main.GetProperty("temp_min").GetDouble());
            max = Math.Max(max, main.GetProperty("temp_max").GetDouble());
            humidityTotal += main.GetProperty("humidity").GetDouble();

            if (!item.TryGetProperty("rain", out var rain))
                continue;

            if (rain.TryGetProperty("3h", out var rain3h))
                rainTotal += rain3h.GetDouble();
            else if (rain.ValueKind == JsonValueKind.Number)
                rainTotal += rain.GetDouble();
        }

        return new RespostaPrevisaoClimatica
        {
            Latitude = latitude,
            Longitude = longitude,
            Dias = safeDays,
            Resumo = $"Previsao solicitada para {safeDays} dia(s). A API retornou {total} leituras de 3 em 3 horas: minima {min:0.0} C, maxima {max:0.0} C, umidade media {humidityTotal / total:0.0}% e chuva acumulada aproximada de {rainTotal:0.0} mm.",
            TemperaturaMinima = min,
            TemperaturaMaxima = max,
            UmidadeMedia = humidityTotal / total,
            ChuvaAcumuladaMm = rainTotal
        };
    }
}


