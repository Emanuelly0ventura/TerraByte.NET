using System.Globalization;
using System.Text.Json;
using TerraByte.Aplicacao.Dtos;
using TerraByte.Aplicacao.Servicos.Externo;

namespace TerraByte.Infraestrutura.Externo;

public class ClienteSoloSoilGrids(HttpClient httpClient) : IClienteSolo
{
    private const double SoilSearchRadiusKm = 5.55;

    public async Task<RespostaClassificacaoSolo> BuscarSoloAsync(double latitude, double longitude)
    {
        foreach (var point in GetSamplePoints(latitude, longitude))
        {
            var solo = await TryFetchSoilAtPointAsync(point.Latitude, point.Longitude);
            if (solo is null)
                continue;

            var (clay, sand, silt) = NormalizeTexture(solo.Value.Argila, solo.Value.Areia, solo.Value.Silte);

            return new RespostaClassificacaoSolo
            {
                Latitude = latitude,
                Longitude = longitude,
                NomeSolo = ClassifyTexture(clay, sand, silt),
                Argila = Math.Round(clay, 2),
                Areia = Math.Round(sand, 2),
                Silte = Math.Round(silt, 2),
                RaioSoloKm = SoilSearchRadiusKm
            };
        }

        throw new InvalidOperationException("SoilGrids nao retornou valores de argila, areia e silte no raio configurado.");
    }

    private async Task<(double Argila, double Areia, double Silte)?> TryFetchSoilAtPointAsync(double latitude, double longitude)
    {
        var lat = latitude.ToString(CultureInfo.InvariantCulture);
        var lon = longitude.ToString(CultureInfo.InvariantCulture);
        var url = $"soilgrids/v2.0/properties/query?lat={lat}&lon={lon}&propriedade=clay&propriedade=sand&propriedade=silt&depth=0-5cm&value=mean";

        using var response = await httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync();
        using var document = await JsonDocument.ParseAsync(stream);

        var clay = ReadSoilProperty(document.RootElement, "clay");
        var sand = ReadSoilProperty(document.RootElement, "sand");
        var silt = ReadSoilProperty(document.RootElement, "silt");

        if (clay is null || sand is null || silt is null)
            return null;

        return (clay.Value, sand.Value, silt.Value);
    }

    private static double? ReadSoilProperty(JsonElement root, string propertyName)
    {
        if (!root.TryGetProperty("properties", out var properties)
            || !properties.TryGetProperty("layers", out var layers))
            throw new InvalidOperationException("Resposta do SoilGrids nao contem camadas de solo.");

        foreach (var layer in layers.EnumerateArray())
        {
            var name = layer.GetProperty("name").GetString();
            if (!string.Equals(name, propertyName, StringComparison.OrdinalIgnoreCase))
                continue;

            foreach (var depth in layer.GetProperty("depths").EnumerateArray())
            {
                var mean = depth.GetProperty("values").GetProperty("mean");
                if (mean.ValueKind == JsonValueKind.Number)
                    return mean.GetDouble();
            }

            break;
        }

        return null;
    }

    private static IEnumerable<(double Latitude, double Longitude)> GetSamplePoints(double latitude, double longitude)
    {
        yield return (latitude, longitude);

        var latOffset = SoilSearchRadiusKm / 111.0;
        var lonOffset = SoilSearchRadiusKm / (111.0 * Math.Cos(latitude * Math.PI / 180.0));

        yield return (latitude + latOffset, longitude);
        yield return (latitude - latOffset, longitude);
        yield return (latitude, longitude + lonOffset);
        yield return (latitude, longitude - lonOffset);
        yield return (latitude + latOffset, longitude + lonOffset);
        yield return (latitude + latOffset, longitude - lonOffset);
        yield return (latitude - latOffset, longitude + lonOffset);
        yield return (latitude - latOffset, longitude - lonOffset);
    }

    private static (double Argila, double Areia, double Silte) NormalizeTexture(double clay, double sand, double silt)
    {
        if (clay + sand + silt > 150)
        {
            clay /= 10;
            sand /= 10;
            silt /= 10;
        }

        var total = clay + sand + silt;
        if (total <= 0)
            throw new InvalidOperationException("SoilGrids retornou valores invalidos para argila, areia e silte.");

        return (clay / total * 100, sand / total * 100, silt / total * 100);
    }

    private static string ClassifyTexture(double clay, double sand, double silt)
    {
        if (silt >= 80 && clay < 12)
            return "SILTE";

        if (sand >= 85 && silt + 1.5 * clay < 15)
            return "AREIA";

        if (sand >= 70 && sand < 90 && silt + 1.5 * clay >= 15 && silt + 2 * clay < 30)
            return "AREIA FRANCA";

        if ((clay >= 7 && clay < 20 && sand > 52 && silt + 2 * clay >= 30)
            || (clay < 7 && silt < 50 && sand > 43))
            return "FRANCO ARENOSO";

        if (clay >= 7 && clay < 27 && silt >= 28 && silt < 50 && sand <= 52)
            return "FRANCO";

        if ((silt >= 50 && clay >= 12 && clay < 27)
            || (silt >= 50 && silt < 80 && clay < 12))
            return "FRANCO SILTOSO";

        if (clay >= 20 && clay < 35 && silt < 28 && sand > 45)
            return "FRANCO ARGILOARENOSO";

        if (clay >= 27 && clay < 40 && sand > 20 && sand <= 45)
            return "FRANCO ARGILOSO";

        if (clay >= 27 && clay < 40 && sand <= 20)
            return "FRANCO ARGILOSSILTOSO";

        if (clay >= 35 && sand > 45)
            return "ARGILA ARENOSA";

        if (clay >= 40 && silt >= 40)
            return "ARGILA SILTOSA";

        if (clay >= 40 && sand <= 45 && silt < 40)
            return "ARGILA";

        return "SOLO TEXTURAL INDEFINIDO";
    }
}

