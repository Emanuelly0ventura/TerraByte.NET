namespace TerraByte.Application.Services.Translators;

public static class SoilPropertyTranslator
{
    private static readonly Dictionary<string, string> Translations = new(StringComparer.OrdinalIgnoreCase)
    {
        { "clay", "clay" },
        { "argila", "clay" },

        { "sand", "sand" },
        { "areia", "sand" },

        { "silt", "silt" },
        { "silte", "silt" },

        { "ph", "phh2o" },
        { "phh2o", "phh2o" },

        { "carbono", "soc" },
        { "carbono organico", "soc" },
        { "carbono orgânico", "soc" },
        { "soc", "soc" },

        { "nitrogenio", "nitrogen" },
        { "nitrogênio", "nitrogen" },
        { "nitrogen", "nitrogen" },

        { "densidade", "bdod" },
        { "bdod", "bdod" },

        { "cec", "cec" },
        { "capacidade de troca cationica", "cec" },
        { "capacidade de troca catiônica", "cec" }
    };

    public static string Translate(string property)
    {
        if (string.IsNullOrWhiteSpace(property))
            return "clay";

        var normalized = property.Trim().ToLowerInvariant();

        return Translations.TryGetValue(normalized, out var translated)
            ? translated
            : normalized;
    }
}
