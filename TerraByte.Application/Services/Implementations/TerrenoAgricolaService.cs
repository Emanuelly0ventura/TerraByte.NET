using TerraByte.Application.DTOs;
using TerraByte.Application.Interfaces;
using TerraByte.Application.Services.External;
using TerraByte.Application.Services.Interfaces;
using TerraByte.Domain.Entities;

namespace TerraByte.Application.Services.Implementations;

public class TerrenoAgricolaService(
    ITerrenoAgricolaRepository repositorioTerrenoAgricola,
    IExternalApiClient clienteConsultaEndereco,
    IClienteGeocodificacao clienteGeocodificacao,
    IClienteSolo clienteSolo) : ITerrenoAgricolaService
{
    public IReadOnlyCollection<RespostaTerrenoAgricola> ListarTodos()
    {
        return repositorioTerrenoAgricola.ListarTodos()
            .Select(RespostaTerrenoAgricola.DoDominio)
            .ToList();
    }

    public RespostaTerrenoAgricola? BuscarPorId(Guid id)
    {
        var terreno = repositorioTerrenoAgricola.BuscarPorId(id);
        return terreno is null ? null : RespostaTerrenoAgricola.DoDominio(terreno);
    }
    // // // // // //
    public async Task<RespostaTerrenoAgricola> CriarAsync(TerrenoAgricolaDtos requisicao)
    {
        if (string.IsNullOrWhiteSpace(requisicao.Nome))
            throw new ArgumentException("O nome do terreno deve ser informado.");

        if (string.IsNullOrWhiteSpace(requisicao.Cep))
            throw new ArgumentException("O CEP do terreno deve ser informado.");

        var endereco = await clienteConsultaEndereco.BuscarEnderecoAsync(requisicao.Cep)
            ?? throw new ArgumentException("CEP nÃ£o encontrado no ViaCEP.");

        var coordenadas = await BuscarCoordenadasAsync(endereco)
            ?? throw new ArgumentException("NÃ£o foi possÃ­vel encontrar latitude e longitude para o CEP informado.");

        var solo = await clienteSolo.BuscarSoloAsync(coordenadas.Latitude, coordenadas.Longitude);

        var terreno = new TerrenoAgricola
        {
            Nome = requisicao.Nome.Trim(),
            Cep = endereco.Cep,
            UsuarioId = requisicao.UsuarioId,

            Latitude = coordenadas.Latitude,
            Longitude = coordenadas.Longitude,
            NomeSolo = solo.NomeSolo,
            Argila = solo.Argila,
            Areia = solo.Areia,
            Silte = solo.Silte,
            RaioSoloKm = solo.RaioSoloKm,
            Logradouro = endereco.Logradouro,
            Bairro = endereco.Bairro,
            Cidade = endereco.Cidade,
            Estado = endereco.Estado
        };

        repositorioTerrenoAgricola.Criar(terreno);
        repositorioTerrenoAgricola.SalvarAlteracoes();

        return RespostaTerrenoAgricola.DoDominio(terreno);
    }

    public RespostaTerrenoAgricola? AtualizarParcial(Guid id, RequisicaoAtualizarTerrenoAgricola requisicao)
    {
        var terreno = repositorioTerrenoAgricola.BuscarPorId(id);
        if (terreno is null)
            return null;

        if (!string.IsNullOrWhiteSpace(requisicao.Nome))
            terreno.Nome = requisicao.Nome.Trim();

        repositorioTerrenoAgricola.AtualizarParcial(terreno);
        repositorioTerrenoAgricola.SalvarAlteracoes();

        return RespostaTerrenoAgricola.DoDominio(terreno);
    }

    public bool Excluir(Guid id)
    {
        var terreno = repositorioTerrenoAgricola.BuscarPorId(id);
        if (terreno is null)
            return false;

        repositorioTerrenoAgricola.Excluir(terreno);
        repositorioTerrenoAgricola.SalvarAlteracoes();
        return true;
    }

    private async Task<RespostaGeocodificacao?> BuscarCoordenadasAsync(RespostaConsultaEndereco endereco)
    {
        foreach (var localizacao in MontarConsultasLocalizacao(endereco))
        {
            var coordenadas = await clienteGeocodificacao.BuscarCoordenadasAsync(localizacao);
            if (coordenadas is not null)
                return coordenadas;
        }

        return null;
    }

    private static IEnumerable<string> MontarConsultasLocalizacao(RespostaConsultaEndereco endereco)
    {
        var fullAddress = JuntarPartesLocalizacao(endereco.Logradouro, endereco.Bairro, endereco.Cidade, endereco.Estado, "Brasil");
        if (!string.IsNullOrWhiteSpace(fullAddress))
            yield return fullAddress;

        var cityAddress = JuntarPartesLocalizacao(endereco.Cidade, endereco.Estado, "Brasil");
        if (!string.IsNullOrWhiteSpace(cityAddress) && cityAddress != fullAddress)
            yield return cityAddress;
    }

    private static string JuntarPartesLocalizacao(params string[] partes)
    {
        return string.Join(", ", partes.Where(parte => !string.IsNullOrWhiteSpace(parte)));
    }
}

