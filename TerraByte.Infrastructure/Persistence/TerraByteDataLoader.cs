using Microsoft.EntityFrameworkCore;
using TerraByte.Domain.Entities;

namespace TerraByte.Infrastructure.Persistence;

public static class TerraByteDataLoader
{
    public static void PrepararBanco(TerraByteContext context)
    {
        if (BancoExisteComEsquemaAntigo(context))
            context.Database.EnsureDeleted();

        context.Database.EnsureCreated();
    }

    private static bool BancoExisteComEsquemaAntigo(TerraByteContext context)
    {
        if (!context.Database.CanConnect())
            return false;

        var connection = context.Database.GetDbConnection();
        var fecharDepois = connection.State != System.Data.ConnectionState.Open;

        if (fecharDepois)
            connection.Open();

        try
        {
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT COUNT(*) FROM sqlite_master WHERE type = 'table' AND name = 'Usuario_terrabyte'";
            var existeTabelaAtual = Convert.ToInt32(command.ExecuteScalar()) > 0;
            return !existeTabelaAtual;
        }
        finally
        {
            if (fecharDepois)
                connection.Close();
        }
    }
    private static readonly Guid UsuarioId = Guid.Parse("10000000-0000-0000-0000-000000000001");
    private static readonly Guid TerrenoId = Guid.Parse("20000000-0000-0000-0000-000000000001");

    public static void Seed(TerraByteContext context)
    {
        if (context.Usuarios.Any())
            return;

        var solos = CriarTiposSolo();
        var defensivos = CriarDefensivos();
        var culturas = CriarCulturas(solos, defensivos);

        var usuario = new Usuario
        {
            Id = UsuarioId,
            Nome = "Usuario TerraByte",
            Email = "demo@terrabyte.com",
            Senha = "senha-criptografada-demo",
            Telefone = "11999999999",
            Genero = "NAO_INFORMADO",
            DataNascimento = new DateTime(2000, 1, 1),
            FotoPerfil = "https://exemplo.com/usuario.png"
        };

        var terreno = new TerrenoAgricola
        {
            Id = TerrenoId,
            Nome = "Fazenda Demonstração",
            Cep = "89801-000",
            Logradouro = "Area rural",
            Bairro = "Interior",
            Cidade = "Chapeco",
            Estado = "SC",
            Latitude = -27.0968,
            Longitude = -52.6185,
            NomeSolo = "FRANCO_ARGILOSA",
            Argila = 320,
            Areia = 360,
            Silte = 320,
            RaioSoloKm = 5.55,
            TipoSolo = solos["FRANCO_ARGILOSA"],
            Usuario = usuario
        };

        var analise = new RegistroPesquisa
        {
            Data = DateTime.UtcNow,
            TempMin = 18,
            TempMax = 29,
            UmidadeMed = 72,
            ChuvaPrevistaMm = 48,
            AdequadoPlantio = 88,
            NivelRisco = "MUITO_PROVAVEL",
            Recomendacao = "Carga inicial: solo, temperatura, chuva e mes estao adequados para soja nesta regiao.",
            Usuario = usuario,
            TerrenoAgricola = terreno,
            Cultura = culturas.First(x => x.Nome == "Soja")
        };

        context.TiposSolo.AddRange(solos.Values);
        context.Defensivos.AddRange(defensivos.Values);
        context.Culturas.AddRange(culturas);
        context.Usuarios.Add(usuario);
        context.TerrenosAgricolas.Add(terreno);
        context.RegistrosPesquisa.Add(analise);
        context.SaveChanges();
    }

    private static Dictionary<string, TipoSolo> CriarTiposSolo()
    {
        var nomes = new[]
        {
            "AREIA", "AREIA_FRANCA", "FRANCO_ARENOSO", "FRANCA", "FRANCO_SILTOSA", "SILTE",
            "FRANCO_ARGILO_ARENOSA", "FRANCO_ARGILOSA", "FRANCO_ARGILO_SILTOSA", "ARGILO_ARENOSA",
            "ARGILA", "ARGILO_SILTOSA", "MUITO_ARGILOSA"
        };

        return nomes.ToDictionary(nome => nome, nome => new TipoSolo { Nome = nome });
    }

    private static Dictionary<string, Defensivo> CriarDefensivos()
    {
        return new[]
        {
            new Defensivo { Nome = "glifosato", Tipo = "Herbicida" },
            new Defensivo { Nome = "mancozebe", Tipo = "Fungicida" },
            new Defensivo { Nome = "atrazina", Tipo = "Herbicida" },
            new Defensivo { Nome = "tebuconazol", Tipo = "Fungicida" },
            new Defensivo { Nome = "azoxistrobina", Tipo = "Fungicida" },
            new Defensivo { Nome = "imidacloprido", Tipo = "Inseticida" },
            new Defensivo { Nome = "clorpirifos", Tipo = "Inseticida" },
            new Defensivo { Nome = "fipronil", Tipo = "Inseticida" },
            new Defensivo { Nome = "paraquate", Tipo = "Herbicida" },
            new Defensivo { Nome = "2,4-D", Tipo = "Herbicida" }
        }.ToDictionary(defensivo => defensivo.Nome, StringComparer.OrdinalIgnoreCase);
    }

    private static List<Cultura> CriarCulturas(Dictionary<string, TipoSolo> solos, Dictionary<string, Defensivo> defensivos)
    {
        return
        [
            Cultura("4d9f5671-8371-4a29-8342-b292ffe2b939", "Mandioca", 22, 34, 40, "SETEMBRO,OUTUBRO,NOVEMBRO", "https://exemplo.com/mandioca.png", solos, defensivos, ["AREIA", "AREIA_FRANCA"], ["glifosato", "mancozebe"]),
            Cultura("b03a3726-0b59-46dd-903c-05a0c1e713f7", "Amendoim", 20, 32, 35, "SETEMBRO,OUTUBRO", "https://exemplo.com/amendoim.png", solos, defensivos, ["AREIA", "FRANCO_ARENOSO"], ["atrazina", "tebuconazol"]),
            Cultura("cc6d8c2c-4560-4b71-a75f-9ce35a959dd6", "Melancia", 20, 35, 50, "SETEMBRO,OUTUBRO,NOVEMBRO", "https://exemplo.com/melancia.png", solos, defensivos, ["AREIA_FRANCA", "FRANCA"], ["mancozebe", "azoxistrobina"]),
            Cultura("7831f3c7-51dc-4d7f-b459-1e3ed87814a9", "Milho Verde", 18, 33, 45, "AGOSTO,SETEMBRO,OUTUBRO", "https://exemplo.com/milho_verde.png", solos, defensivos, ["AREIA_FRANCA", "FRANCO_ARGILOSA"], ["atrazina", "clorpirifos"]),
            Cultura("97abc9f5-ec88-44c3-85a8-a678c9d23973", "Cafe", 18, 26, 60, "MARCO,ABRIL,MAIO", "https://exemplo.com/cafe.png", solos, defensivos, ["FRANCA", "FRANCO_ARGILOSA"], ["tebuconazol", "azoxistrobina"]),
            Cultura("30833675-f4a3-41bf-89ed-bc2d673ccb9a", "Tomate", 18, 28, 55, "AGOSTO,SETEMBRO", "https://exemplo.com/tomate.png", solos, defensivos, ["FRANCO_SILTOSA", "FRANCA"], ["mancozebe", "azoxistrobina"]),
            Cultura("06905ab5-fe14-4c50-883a-25e5a534bfe2", "Soja", 18, 30, 50, "OUTUBRO,NOVEMBRO", "https://exemplo.com/soja.png", solos, defensivos, ["FRANCO_ARGILOSA", "ARGILA"], ["glifosato", "2,4-D"]),
            Cultura("e6de3d74-cccd-407f-a44f-5f276df623cd", "Milho", 20, 35, 45, "SETEMBRO,OUTUBRO", "https://exemplo.com/milho.png", solos, defensivos, ["FRANCO_ARGILOSA", "ARGILA"], ["atrazina", "clorpirifos"]),
            Cultura("e3ab1402-049e-4973-978a-bd7e75791759", "Arroz", 20, 35, 80, "SETEMBRO,OUTUBRO", "https://exemplo.com/arroz.png", solos, defensivos, ["ARGILA", "ARGILO_SILTOSA"], ["paraquate", "fipronil"]),
            Cultura("82ff98a3-82dd-4a74-a858-83cd8e1d9c2f", "Feijao Preto", 18, 30, 55, "OUTUBRO,NOVEMBRO", "https://exemplo.com/feijao_preto.png", solos, defensivos, ["ARGILA", "FRANCO_ARGILOSA"], ["mancozebe", "imidacloprido"])
        ];
    }

    private static Cultura Cultura(
        string id,
        string nome,
        double tempMin,
        double tempMax,
        double aguaMm,
        string mesesIdeais,
        string urlImg,
        Dictionary<string, TipoSolo> solos,
        Dictionary<string, Defensivo> defensivos,
        IReadOnlyCollection<string> solosAceitos,
        IReadOnlyCollection<string> defensivosIndicados)
    {
        var cultura = new Cultura
        {
            Id = Guid.Parse(id),
            Nome = nome,
            TempMin = tempMin,
            TempMax = tempMax,
            AguaMM = aguaMm,
            MesesIdeais = mesesIdeais,
            UrlImg = urlImg
        };

        foreach (var solo in solosAceitos)
            cultura.TiposSolo.Add(solos[solo]);

        foreach (var defensivo in defensivosIndicados)
            cultura.Defensivos.Add(defensivos[defensivo]);

        return cultura;
    }
}

