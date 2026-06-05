using TerraByte.Application.DTOs;
using TerraByte.Application.Interfaces;
using TerraByte.Application.Services.External;
using TerraByte.Application.Services.Interfaces;
using TerraByte.Domain.Entities;

namespace TerraByte.Application.Services.Implementations;

public class CulturaService(
    ITerrenoAgricolaRepository repositorioTerrenoAgricola,
    IClienteClima clienteClima) : ICulturaService
{
    private static readonly IReadOnlyCollection<PlantioInfo> Plantios = [
        new("4d9f5671-8371-4a29-8342-b292ffe2b939", "Mandioca", 22, 34, 40, ["SETEMBRO", "OUTUBRO", "NOVEMBRO"], "https://exemplo.com/mandioca.png", [TipoSoloEnum.AREIA], ["glifosato", "mancozebe"]),
        new("b03a3726-0b59-46dd-903c-05a0c1e713f7", "Amendoim", 20, 32, 35, ["SETEMBRO", "OUTUBRO"], "https://exemplo.com/amendoim.png", [TipoSoloEnum.AREIA], ["atrazina", "tebuconazol"]),
        new("cc6d8c2c-4560-4b71-a75f-9ce35a959dd6", "Melancia", 20, 35, 50, ["SETEMBRO", "OUTUBRO", "NOVEMBRO"], "https://exemplo.com/melancia.png", [TipoSoloEnum.AREIA_FRANCA], ["mancozebe", "azoxistrobina"]),
        new("7831f3c7-51dc-4d7f-b459-1e3ed87814a9", "Milho Verde", 18, 33, 45, ["AGOSTO", "SETEMBRO", "OUTUBRO"], "https://exemplo.com/milho_verde.png", [TipoSoloEnum.AREIA_FRANCA], ["atrazina", "clorpirifos"]),
        new("9c16ce2a-7316-41c1-9ca4-905d289b5372", "Feijao", 18, 30, 55, ["OUTUBRO", "NOVEMBRO"], "https://exemplo.com/feijao.png", [TipoSoloEnum.FRANCO_ARENOSO], ["mancozebe", "imidacloprido"]),
        new("b3129aa5-028f-416c-8761-4d87143c846b", "Sorgo", 20, 36, 40, ["SETEMBRO", "OUTUBRO"], "https://exemplo.com/sorgo.png", [TipoSoloEnum.FRANCO_ARENOSO], ["glifosato", "atrazina"]),
        new("97abc9f5-ec88-44c3-85a8-a678c9d23973", "Cafe", 18, 26, 60, ["MARCO", "ABRIL", "MAIO"], "https://exemplo.com/cafe.png", [TipoSoloEnum.FRANCA], ["tebuconazol", "azoxistrobina"]),
        new("84485c05-3790-4c10-a47e-7032fc4a88ad", "Cana-de-acucar", 20, 35, 70, ["SETEMBRO", "OUTUBRO"], "https://exemplo.com/cana.png", [TipoSoloEnum.FRANCA], ["glifosato", "paraquate"]),
        new("1ae97b15-6a2e-4675-b199-43759e8f4551", "Alface", 15, 25, 50, ["MARCO", "ABRIL", "MAIO"], "https://exemplo.com/alface.png", [TipoSoloEnum.FRANCO_SILTOSA], ["imidacloprido", "fipronil"]),
        new("30833675-f4a3-41bf-89ed-bc2d673ccb9a", "Tomate", 18, 28, 55, ["AGOSTO", "SETEMBRO"], "https://exemplo.com/tomate.png", [TipoSoloEnum.FRANCO_SILTOSA], ["mancozebe", "azoxistrobina"]),
        new("0d2e8c47-af6b-43fa-9358-17df42793b36", "Trigo", 12, 24, 45, ["MAIO", "JUNHO"], "https://exemplo.com/trigo.png", [TipoSoloEnum.SILTE], ["tebuconazol", "atrazina"]),
        new("06b2afcd-3fd0-42f7-9276-d77a46c87938", "Cevada", 10, 22, 40, ["MAIO", "JUNHO"], "https://exemplo.com/cevada.png", [TipoSoloEnum.SILTE], ["tebuconazol", "azoxistrobina"]),
        new("06905ab5-fe14-4c50-883a-25e5a534bfe2", "Soja", 18, 30, 50, ["OUTUBRO", "NOVEMBRO"], "https://exemplo.com/soja.png", [TipoSoloEnum.FRANCO_ARGILOSA], ["glifosato", "2,4-D"]),
        new("e6de3d74-cccd-407f-a44f-5f276df623cd", "Milho", 20, 35, 45, ["SETEMBRO", "OUTUBRO"], "https://exemplo.com/milho.png", [TipoSoloEnum.FRANCO_ARGILOSA], ["atrazina", "clorpirifos"]),
        new("e3ab1402-049e-4973-978a-bd7e75791759", "Arroz", 20, 35, 80, ["SETEMBRO", "OUTUBRO"], "https://exemplo.com/arroz.png", [TipoSoloEnum.ARGILA], ["paraquate", "fipronil"]),
        new("82ff98a3-82dd-4a74-a858-83cd8e1d9c2f", "Feijao Preto", 18, 30, 55, ["OUTUBRO", "NOVEMBRO"], "https://exemplo.com/feijao_preto.png", [TipoSoloEnum.ARGILA], ["mancozebe", "imidacloprido"])
    ];

    public IReadOnlyCollection<RespostaPlantio> ListarPlantios()
    {
        return Plantios.Select(ParaResposta).ToList();
    }

    public RespostaPlantio? BuscarPlantioPorId(Guid plantioId)
    {
        var plantio = Plantios.FirstOrDefault(x => x.Id == plantioId);
        return plantio is null ? null : ParaResposta(plantio);
    }

    public async Task<RespostaAnalisePlantio?> AnalisarCompatibilidadeAsync(Guid terrenoAgricolaId, Guid plantioId)
    {
        var terreno = repositorioTerrenoAgricola.BuscarPorId(terrenoAgricolaId);
        var plantio = Plantios.FirstOrDefault(x => x.Id == plantioId);

        if (terreno is null || plantio is null || terreno.Latitude is null || terreno.Longitude is null)
            return null;

        var clima = await clienteClima.BuscarClimaAsync(terreno.Latitude.Value, terreno.Longitude.Value, 30);
        var tipoSoloTerreno = ClassificarSolo(terreno.Argila, terreno.Areia, terreno.Silte);
        var soloCompativel = plantio.TiposSolo.Contains(tipoSoloTerreno);
        var climaCompativel = TemperaturaCompativel(clima, plantio);
        var mesCompativel = plantio.MesesIdeais.Contains(NomeMes(DateTime.UtcNow.Month));

        var pontuacao = 0;
        if (soloCompativel) pontuacao += 45;
        if (climaCompativel) pontuacao += 35;
        if (mesCompativel) pontuacao += 20;

        var adequado = pontuacao >= 70 ? "Adequado" : pontuacao >= 45 ? "Parcialmente adequado" : "Nao adequado";
        var risco = pontuacao >= 70 ? "Baixo" : pontuacao >= 45 ? "Medio" : "Alto";

        return new RespostaAnalisePlantio
        {
            NomeEndereco = MontarNomeEndereco(terreno),
            NomePlantio = plantio.Nome,
            TipoSoloEndereco = tipoSoloTerreno.ToString(),
            TipoSoloPlantio = plantio.TiposSolo.Select(x => x.ToString()).ToList(),
            AdequadoPlantio = adequado,
            NivelRisco = risco,
            Latitude = terreno.Latitude.Value,
            Longitude = terreno.Longitude.Value,
            Argila = terreno.Argila,
            Areia = terreno.Areia,
            Silte = terreno.Silte,
            RaioKm = terreno.RaioSoloKm,
            TempMin = $"{clima.TemperaturaMinima:0.0} C",
            TempMax = $"{clima.TemperaturaMaxima:0.0} C",
            UmidadeMed = $"{clima.UmidadeMedia:0.0}%",
            Pontuacao = pontuacao,
            Recomendacao = MontarRecomendacao(soloCompativel, climaCompativel, mesCompativel, tipoSoloTerreno, plantio, clima)
        };
    }

    private static RespostaPlantio ParaResposta(PlantioInfo plantio) => new()
    {
        Id = plantio.Id,
        Nome = plantio.Nome,
        TempMin = plantio.TempMin,
        TempMax = plantio.TempMax,
        AguaMM = plantio.AguaMm,
        MesesIdeais = plantio.MesesIdeais,
        UrlImg = plantio.UrlImg,
        TiposSolo = plantio.TiposSolo.Select(x => x.ToString()).ToList(),
        Defensivos = plantio.Defensivos
    };

    private static TipoSoloEnum ClassificarSolo(double argila, double areia, double silte)
    {
        argila /= 10.0;
        areia /= 10.0;
        silte /= 10.0;

        if (argila >= 60) return TipoSoloEnum.MUITO_ARGILOSA;
        if (argila >= 40 && silte >= 40) return TipoSoloEnum.ARGILO_SILTOSA;
        if (argila >= 35 && areia >= 45) return TipoSoloEnum.ARGILO_ARENOSA;
        if (argila >= 40) return TipoSoloEnum.ARGILA;
        if (argila >= 27 && silte >= 40) return TipoSoloEnum.FRANCO_ARGILO_SILTOSA;
        if (argila >= 27 && areia >= 45) return TipoSoloEnum.FRANCO_ARGILO_ARENOSA;
        if (argila >= 27) return TipoSoloEnum.FRANCO_ARGILOSA;
        if (silte >= 80) return TipoSoloEnum.SILTE;
        if (silte >= 50) return TipoSoloEnum.FRANCO_SILTOSA;
        if (areia >= 85) return TipoSoloEnum.AREIA;
        if (areia >= 70) return TipoSoloEnum.AREIA_FRANCA;
        if (areia >= 52) return TipoSoloEnum.FRANCO_ARENOSO;

        return TipoSoloEnum.FRANCA;
    }

    private static bool TemperaturaCompativel(RespostaPrevisaoClimatica clima, PlantioInfo plantio)
    {
        return clima.TemperaturaMinima >= plantio.TempMin - 3
            && clima.TemperaturaMaxima <= plantio.TempMax + 3;
    }

    private static string MontarNomeEndereco(TerrenoAgricola terreno)
    {
        var partes = new[] { terreno.Nome, terreno.Logradouro, terreno.Bairro, terreno.Cidade, terreno.Estado }
            .Where(x => !string.IsNullOrWhiteSpace(x));

        return string.Join(" - ", partes);
    }

    private static string MontarRecomendacao(
        bool soloCompativel,
        bool climaCompativel,
        bool mesCompativel,
        TipoSoloEnum tipoSoloTerreno,
        PlantioInfo plantio,
        RespostaPrevisaoClimatica clima)
    {
        var pontos = new List<string>();

        pontos.Add(soloCompativel
            ? $"O solo {tipoSoloTerreno} esta entre os tipos indicados para {plantio.Nome}."
            : $"O solo {tipoSoloTerreno} nao esta entre os tipos indicados para {plantio.Nome}.");

        pontos.Add(climaCompativel
            ? "A previsao de temperatura esta dentro da faixa aceitavel para o plantio."
            : $"A previsao indica minima de {clima.TemperaturaMinima:0.0} C e maxima de {clima.TemperaturaMaxima:0.0} C, fora da faixa ideal de {plantio.TempMin:0.0} C a {plantio.TempMax:0.0} C.");

        pontos.Add(mesCompativel
            ? "O mes atual esta dentro da janela ideal de plantio."
            : $"A janela ideal informada para esse plantio e: {string.Join(", ", plantio.MesesIdeais)}.");

        pontos.Add($"Necessidade de agua aproximada: {plantio.AguaMm:0.0} mm. Chuva prevista disponivel: {clima.ChuvaAcumuladaMm:0.0} mm.");

        return string.Join(" ", pontos);
    }

    private static string NomeMes(int mes) => mes switch
    {
        1 => "JANEIRO",
        2 => "FEVEREIRO",
        3 => "MARCO",
        4 => "ABRIL",
        5 => "MAIO",
        6 => "JUNHO",
        7 => "JULHO",
        8 => "AGOSTO",
        9 => "SETEMBRO",
        10 => "OUTUBRO",
        11 => "NOVEMBRO",
        12 => "DEZEMBRO",
        _ => string.Empty
    };

    private sealed record PlantioInfo(
        Guid Id,
        string Nome,
        double TempMin,
        double TempMax,
        double AguaMm,
        IReadOnlyCollection<string> MesesIdeais,
        string UrlImg,
        IReadOnlyCollection<TipoSoloEnum> TiposSolo,
        IReadOnlyCollection<string> Defensivos)
    {
        public PlantioInfo(
            string id,
            string nome,
            double tempMin,
            double tempMax,
            double aguaMm,
            IReadOnlyCollection<string> mesesIdeais,
            string urlImg,
            IReadOnlyCollection<TipoSoloEnum> tiposSolo,
            IReadOnlyCollection<string> defensivos)
            : this(Guid.Parse(id), nome, tempMin, tempMax, aguaMm, mesesIdeais, urlImg, tiposSolo, defensivos)
        {
        }
    }
}
