using TerraByte.Application.DTOs;
using TerraByte.Application.Interfaces;
using TerraByte.Application.Services.Interfaces;
using TerraByte.Domain.Entities;

namespace TerraByte.Application.Services.Implementations;

public class DefensivoService(
    IDefensivoRepository repositorioDefensivo) : IDefensivoService
{
    public IReadOnlyCollection<DefensivoDtos.RespostaDefensivo> ListarDefensivos()
    {
        return repositorioDefensivo.ListarTodos()
            .Select(ParaResposta)
            .ToList();
    }

    public DefensivoDtos.RespostaDefensivo? BuscarDefensivoPorId(Guid DefensivoId)
    {
        var defensivo = repositorioDefensivo.BuscarPorId(DefensivoId);
        return defensivo is null ? null : ParaResposta(defensivo);
    }
    
    private static DefensivoDtos.RespostaDefensivo ParaResposta(Defensivo defensivo) => new()
    {
        
        Id = defensivo.Id,
        Nome = defensivo.Nome,
        Tipo = defensivo.Tipo
    
        
    };
}
