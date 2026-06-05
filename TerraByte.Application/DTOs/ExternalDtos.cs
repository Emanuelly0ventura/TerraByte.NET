namespace TerraByte.Application.DTOs;

public class RespostaConsultaEndereco
{
    public string Cep { get; set; } = string.Empty;
    public string Logradouro { get; set; } = string.Empty;
    public string Bairro { get; set; } = string.Empty;
    public string Cidade { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
}

public class RespostaGeocodificacao
{
    public string Nome { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}

public class RespostaPrevisaoClimatica
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int Dias { get; set; }
    public string Resumo { get; set; } = string.Empty;
    public double TemperaturaMinima { get; set; }
    public double TemperaturaMaxima { get; set; }
    public double UmidadeMedia { get; set; }
    public double ChuvaAcumuladaMm { get; set; }
}

public class RespostaClassificacaoSolo
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string NomeSolo { get; set; } = string.Empty;
    public double Argila { get; set; }
    public double Areia { get; set; }
    public double Silte { get; set; }
    public double RaioSoloKm { get; set; } = 5.55;
}


