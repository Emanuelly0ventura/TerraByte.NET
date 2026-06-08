using System.Globalization;
using System.Text;
using TerraByte.Application.DTOs;
using TerraByte.Application.Interfaces;
using TerraByte.Application.Services.External;
using TerraByte.Application.Services.Interfaces;
using TerraByte.Domain.Entities;

namespace TerraByte.Application.Services.Implementations;

public class CulturaService(
    ITerrenoAgricolaRepository repositorioTerrenoAgricola,
    ICulturaRepository repositorioCultura,
    IRegistroPesquisaRepository repositorioRegistroPesquisa,
    IClienteClima clienteClima) : ICulturaService
{
    private const double PesoSolo = 50;
    private const double PesoTemperatura = 30;
    private const double PesoChuva = 20;
    private const double LimiteSemSoloCompativel = 60;
    private const double PenalidadeForaDoMesIdeal = 10;

    public IReadOnlyCollection<RespostaPlantio> ListarPlantios()
    {
        return repositorioCultura.ListarTodos()
            .Select(ParaResposta)
            .ToList();
    }

    public RespostaPlantio? BuscarPlantioPorId(Guid plantioId)
    {
        var plantio = repositorioCultura.BuscarPorId(plantioId);
        return plantio is null ? null : ParaResposta(plantio);
    }

    public async Task<RespostaAnalisePlantio?> AnalisarCompatibilidadeAsync(Guid terrenoAgricolaId, Guid plantioId)
    {
        var terreno = repositorioTerrenoAgricola.BuscarPorId(terrenoAgricolaId);
        var plantio = repositorioCultura.BuscarPorId(plantioId);

        if (terreno is null || plantio is null || terreno.Latitude is null || terreno.Longitude is null)
            return null;

        var clima = await clienteClima.BuscarClimaAsync(terreno.Latitude.Value, terreno.Longitude.Value, 30);
        var tipoSoloTerreno = ObterTipoSoloTerreno(terreno);
        var solosPlantio = plantio.TiposSolo.Select(x => x.Nome).ToList();
        var soloCompativel = solosPlantio.Any(solo => MesmoNomeSolo(solo, tipoSoloTerreno));
        var temperaturaPontuacao = CalcularPontuacaoTemperatura(clima, plantio);
        var chuvaPontuacao = CalcularPontuacaoChuva(clima.ChuvaAcumuladaMm, plantio.AguaMM);
        var mesCompativel = ObterMesesIdeais(plantio).Contains(NomeMes(DateTime.Now.Month));

        var pontuacao = (soloCompativel ? PesoSolo : 0) + temperaturaPontuacao + chuvaPontuacao;
        if (!soloCompativel)
            pontuacao = Math.Min(pontuacao, LimiteSemSoloCompativel);

        if (!mesCompativel)
            pontuacao -= PenalidadeForaDoMesIdeal;

        pontuacao = Math.Clamp(Math.Round(pontuacao, 0), 0, 100);
        var classificacao = ClassificarPontuacao(pontuacao);
        var recomendacao = MontarRecomendacao(
            soloCompativel,
            temperaturaPontuacao,
            chuvaPontuacao,
            mesCompativel,
            tipoSoloTerreno,
            plantio,
            clima,
            pontuacao,
            classificacao);

        var registro = new RegistroPesquisa
        {
            TempMin = clima.TemperaturaMinima,
            TempMax = clima.TemperaturaMaxima,
            UmidadeMed = clima.UmidadeMedia,
            ChuvaPrevistaMm = clima.ChuvaAcumuladaMm,
            AdequadoPlantio = pontuacao,
            NivelRisco = classificacao,
            Recomendacao = recomendacao,
            UsuarioId = terreno.UsuarioId,
            TerrenoAgricolaId = terreno.Id,
            CulturaId = plantio.Id
        };

        repositorioRegistroPesquisa.Criar(registro);
        repositorioRegistroPesquisa.SalvarAlteracoes();

        return new RespostaAnalisePlantio
        {
            Id = registro.Id,
            Data = registro.Data,
            NomeEndereco = MontarNomeEndereco(terreno),
            NomePlantio = plantio.Nome,
            TipoSoloEndereco = tipoSoloTerreno,
            TipoSoloPlantio = solosPlantio,
            AdequadoPlantio = $"{pontuacao:0}%",
            NivelRisco = classificacao,
            Latitude = terreno.Latitude.Value,
            Longitude = terreno.Longitude.Value,
            Argila = terreno.Argila,
            Areia = terreno.Areia,
            Silte = terreno.Silte,
            RaioKm = terreno.RaioSoloKm,
            TempMin = $"{clima.TemperaturaMinima:0.0} C",
            TempMax = $"{clima.TemperaturaMaxima:0.0} C",
            UmidadeMed = $"{clima.UmidadeMedia:0.0}%",
            Pontuacao = (int)pontuacao,
            Recomendacao = recomendacao
        };
    }

    private static RespostaPlantio ParaResposta(Cultura plantio) => new()
    {
        Id = plantio.Id,
        Nome = plantio.Nome,
        TempMin = plantio.TempMin,
        TempMax = plantio.TempMax,
        AguaMM = plantio.AguaMM,
        MesesIdeais = ObterMesesIdeais(plantio),
        UrlImg = plantio.UrlImg,
        TiposSolo = plantio.TiposSolo.Select(x => x.Nome).ToList(),
        Defensivos = plantio.Defensivos.Select(x => $"{x.Nome} ({x.Tipo})").ToList()
    };

    private static string ObterTipoSoloTerreno(TerrenoAgricola terreno)
    {
        if (!string.IsNullOrWhiteSpace(terreno.TipoSolo?.Nome))
            return terreno.TipoSolo.Nome;

        if (!string.IsNullOrWhiteSpace(terreno.NomeSolo))
            return NormalizarNomeSolo(terreno.NomeSolo);

        return ClassificarSolo(terreno.Argila, terreno.Areia, terreno.Silte).ToString();
    }

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

    private static double CalcularPontuacaoTemperatura(RespostaPrevisaoClimatica clima, Cultura plantio)
    {
        var mediaPrevista = (clima.TemperaturaMinima + clima.TemperaturaMaxima) / 2;
        var mediaIdeal = (plantio.TempMin + plantio.TempMax) / 2;
        var tolerancia = Math.Max((plantio.TempMax - plantio.TempMin) / 2, 1);
        var distancia = Math.Abs(mediaPrevista - mediaIdeal);

        if (clima.TemperaturaMinima >= plantio.TempMin && clima.TemperaturaMaxima <= plantio.TempMax)
            return PesoTemperatura;

        return Math.Clamp(PesoTemperatura * (1 - distancia / (tolerancia + 8)), 0, PesoTemperatura);
    }

    private static double CalcularPontuacaoChuva(double chuvaPrevista, double aguaNecessaria)
    {
        if (aguaNecessaria <= 0)
            return PesoChuva;

        var proporcao = chuvaPrevista / aguaNecessaria;
        if (proporcao is >= 0.75 and <= 1.35)
            return PesoChuva;

        if (proporcao < 0.75)
            return Math.Clamp(PesoChuva * (proporcao / 0.75), 0, PesoChuva);

        return Math.Clamp(PesoChuva * (1 - ((proporcao - 1.35) / 1.35)), 0, PesoChuva);
    }

    private static IReadOnlyCollection<string> ObterMesesIdeais(Cultura plantio)
    {
        return plantio.MesesIdeais
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(NormalizarNomeSolo)
            .ToList();
    }

    private static string ClassificarPontuacao(double pontuacao) => pontuacao switch
    {
        >= 85 => "MUITO_PROVAVEL",
        >= 70 => "ALTA",
        >= 45 => "MEDIA",
        _ => "BAIXA"
    };

    private static string MontarNomeEndereco(TerrenoAgricola terreno)
    {
        var partes = new[] { terreno.Nome, terreno.Logradouro, terreno.Bairro, terreno.Cidade, terreno.Estado }
            .Where(x => !string.IsNullOrWhiteSpace(x));

        return string.Join(" - ", partes);
    }

    private static string MontarRecomendacao(
        bool soloCompativel,
        double temperaturaPontuacao,
        double chuvaPontuacao,
        bool mesCompativel,
        string tipoSoloTerreno,
        Cultura plantio,
        RespostaPrevisaoClimatica clima,
        double pontuacao,
        string classificacao)
    {
        var solosAceitos = plantio.TiposSolo.Select(x => x.Nome).ToList();
        var pontos = new List<string>
        {
            soloCompativel
                ? $"O solo {tipoSoloTerreno} esta entre os solos aceitos para {plantio.Nome}."
                : $"O solo {tipoSoloTerreno} nao esta entre os solos aceitos para {plantio.Nome}; por isso a pontuacao foi limitada.",
            temperaturaPontuacao >= PesoTemperatura
                ? "A temperatura prevista esta dentro da faixa ideal da cultura."
                : $"A temperatura prevista ficou parcialmente fora da faixa ideal de {plantio.TempMin:0.0} C a {plantio.TempMax:0.0} C.",
            chuvaPontuacao >= PesoChuva
                ? "A chuva prevista atende bem a necessidade hidrica da cultura."
                : $"A chuva prevista e de {clima.ChuvaAcumuladaMm:0.0} mm para uma necessidade aproximada de {plantio.AguaMM:0.0} mm.",
            mesCompativel
                ? "O mes atual esta dentro da janela ideal de plantio."
                : $"O mes atual esta fora da janela ideal: {plantio.MesesIdeais}.",
            $"Resultado final: {pontuacao:0}/100, classificacao {classificacao}. Solos aceitos: {string.Join(", ", solosAceitos)}."
        };

        return string.Join(" ", pontos);
    }

    private static bool MesmoNomeSolo(string esquerdo, string direito)
    {
        return NormalizarNomeSolo(esquerdo) == NormalizarNomeSolo(direito);
    }

    private static string NormalizarNomeSolo(string valor)
    {
        var semAcentos = new string(valor.Normalize(NormalizationForm.FormD)
            .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
            .ToArray());

        return semAcentos
            .Normalize(NormalizationForm.FormC)
            .Trim()
            .Replace(' ', '_')
            .Replace('-', '_')
            .ToUpperInvariant();
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
}

