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
        { "carbono orgÃ¢nico", "soc" },
        { "soc", "soc" },

        { "nitrogenio", "nitrogen" },
        { "nitrogÃªnio", "nitrogen" },
        { "nitrogen", "nitrogen" },

        { "densidade", "bdod" },
        { "bdod", "bdod" },

        { "cec", "cec" },
        { "capacidade de troca cationica", "cec" },
        { "capacidade de troca catiÃ´nica", "cec" }
    };

    public static string Traduzir(string propriedade)
    {
        if (string.IsNullOrWhiteSpace(propriedade))
            return "clay";

        var normalizado = propriedade.Trim().ToLowerInvariant();

        return Translations.TryGetValue(normalizado, out var traduzido)
            ? traduzido
            : normalizado;
    }
}

