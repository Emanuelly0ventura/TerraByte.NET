# TerraByte API

## Descrição do Projeto

O **TerraByte** é uma API REST em C# criada para apoiar agricultores na tomada de decisão sobre plantio, acompanhamento de terrenos e culturas.

A proposta do sistema é reunir informações importantes para o planejamento agrícola, como endereço do terreno, latitude e longitude, previsão climática, dados de solo e registros internos sobre plantações.

## Solução do Projeto

O projeto foi desenvolvido em camadas, seguindo uma organização parecida com a utilizada no PetCore:

```txt
TerraByte
├── TerraByte.Api
├── TerraByte.Application
├── TerraByte.Domain
└── TerraByte.Infrastructure
```

A API possui persistência em banco relacional com **Entity Framework Core** e **SQLite**, além de integração com APIs externas para enriquecer as informações agrícolas.

## Funcionalidades

- Cadastro, consulta, atualização e remoção de terrenos agrícolas
- Cadastro e consulta de culturas por terreno
- Consulta de endereço por CEP usando ViaCEP
- Consulta de latitude e longitude por cidade usando Open-Meteo Geocoding
- Consulta de propriedades de solo usando SoilGrids
- Consulta de previsão climática de 1 a 30 dias usando OpenWeather
- Registro de snapshots de pesquisa climática e de solo vinculados ao terreno
- Documentação interativa com Swagger
- Uso de migrations com Entity Framework Core

## Tecnologias Utilizadas

- C#
- ASP.NET Core
- Entity Framework Core
- SQLite
- Swagger / Swashbuckle
- HttpClient
- ViaCEP
- Open-Meteo Geocoding
- OpenWeather
- SoilGrids

## APIs Externas Utilizadas

### ViaCEP

Utilizada para buscar endereço a partir de um CEP.

Exemplo:

```txt
https://viacep.com.br/ws/01001000/json/
```

### Open-Meteo Geocoding

Utilizada para buscar latitude e longitude a partir do nome de uma cidade.

Exemplo:

```txt
https://geocoding-api.open-meteo.com/v1/search?name=Ribeirao%20Preto&count=1&language=pt&format=json
```

### OpenWeather

Utilizada para previsão climática de até 30 dias.

Exemplo:

```txt
https://pro.openweathermap.org/data/2.5/forecast/climate?lat={lat}&lon={lon}&cnt=30&units=metric&appid={API_KEY}
```

A chave deve ser configurada no `appsettings.json`.

### SoilGrids

Utilizada para consulta de propriedades de solo por latitude e longitude.

Exemplo de propriedade consultada:

```txt
clay
```

## Relacionamentos do Banco

O projeto possui relacionamento **1:N** entre:

```txt
FarmPlot -> Crops
FarmPlot -> ResearchSnapshots
```

Ou seja, um terreno pode ter várias culturas e várias pesquisas salvas.

## Documentação das Rotas

### Terrenos

```http
GET /api/FarmPlots
GET /api/FarmPlots/{id}
POST /api/FarmPlots
PUT /api/FarmPlots/{id}
DELETE /api/FarmPlots/{id}
```

### Culturas

```http
GET /api/farm-plots/{farmPlotId}/crops
GET /api/crops/{id}
POST /api/farm-plots/{farmPlotId}/crops
DELETE /api/crops/{id}
```

### Pesquisas Externas

```http
GET /api/Research/cep/{cep}
GET /api/Research/geocode?city={cidade}
GET /api/Research/climate?latitude={lat}&longitude={lon}&days={dias}
GET /api/Research/soil?latitude={lat}&longitude={lon}&property={propriedade}
POST /api/Research/farm-plots/{farmPlotId}/climate-snapshots
POST /api/Research/farm-plots/{farmPlotId}/soil-snapshots
```

## Configuração do Projeto

No arquivo:

```txt
TerraByte.Api/appsettings.json
```

configure a connection string do SQLite:

```json
{
  "ConnectionStrings": {
    "TerraByteSqlite": "Data Source=terrabyte.db"
  }
}
```

Para usar a API de previsão climática, configure também sua chave da OpenWeather:

```json
{
  "OpenWeather": {
    "ApiKey": "sua_api_key"
  }
}
```

## Como Executar o Projeto

### 1. Restaurar os Pacotes

Na pasta raiz da solução, execute:

```bash
dotnet restore
```

### 2. Compilar o Projeto

```bash
dotnet build
```

### 3. Aplicar as Migrations

```bash
dotnet ef database update --project TerraByte.Infrastructure --startup-project TerraByte.Api --context TerraByteContext
```

### 4. Executar a API

```bash
dotnet run --project TerraByte.Api
```

### 5. Acessar o Swagger

Com a API em execução, acesse:

```txt
http://localhost:5292/swagger
```

ou a porta exibida no terminal.

## Como Criar uma Nova Migration

Sempre que uma entidade ou configuração de banco for alterada, crie uma nova migration:

```bash
dotnet ef migrations add NomeDaMigration --project TerraByte.Infrastructure --startup-project TerraByte.Api --context TerraByteContext --output-dir Migrations
```

Depois aplique no banco:

```bash
dotnet ef database update --project TerraByte.Infrastructure --startup-project TerraByte.Api --context TerraByteContext
```

## Observações

- A API de clima da OpenWeather precisa de uma chave de API válida.
- A API pode ser testada pelo Swagger usando a opção `Try it out`.
- O arquivo `TerraByte.Api/TerraByte.Api.http` possui exemplos de requisições para teste.
- O banco SQLite local é criado no arquivo `terrabyte.db`.
