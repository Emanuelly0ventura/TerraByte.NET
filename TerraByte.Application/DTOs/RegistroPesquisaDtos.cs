using TerraByte.Dominio.Entidades;

namespace TerraByte.Aplicacao.Dtos;

public class RespostaRegistroPesquisa
{
    public Guid Id { get; set; }
    public string Fonte { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public string Resumo { get; set; } = string.Empty;
    public DateTime SolicitadoEm { get; set; }
    public Guid TerrenoAgricolaId { get; set; }

    public static RespostaRegistroPesquisa DoDominio(RegistroPesquisa registro) => new()
    {
        Id = registro.Id,
        Fonte = registro.Fonte,
        Tipo = registro.Tipo,
        Resumo = registro.Resumo,
        SolicitadoEm = registro.SolicitadoEm,
        TerrenoAgricolaId = registro.TerrenoAgricolaId
    };
}


