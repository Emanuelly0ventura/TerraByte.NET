using System.Text.Json.Serialization;

namespace TerraByte.Application.DTOs;

public class DefensivoDtos
{
    public class RespostaDefensivo
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("nome")]
        public string Nome { get; set; } = string.Empty;

        [JsonPropertyName("tipo")]
        public string Tipo { get; set; }

        
    }
}