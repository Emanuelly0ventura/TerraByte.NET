using TerraByte.Domain.Entities;

namespace TerraByte.Application.DTOs;

public class RegistroPesquisaDtos
{
    public Guid Id { get; set; }
    public string Fonte { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public string Resumo { get; set; } = string.Empty;
    public DateTime SolicitadoEm { get; set; }
    public Guid TerrenoAgricolaId { get; set; }

    public static RegistroPesquisaDtos DoDominio(RegistroPesquisaDtos registro) => new()
    {
        Id = registro.Id,
        Fonte = registro.Fonte,
        Tipo = registro.Tipo,
        Resumo = registro.Resumo,
        SolicitadoEm = registro.SolicitadoEm,
        TerrenoAgricolaId = registro.TerrenoAgricolaId
    };
}


