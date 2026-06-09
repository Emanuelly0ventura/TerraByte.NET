# 🌱 TerraByte API

## 📖 Descrição do Projeto

O TerraByte é uma API REST desenvolvida em C# com ASP.NET Core para auxiliar produtores rurais na análise de viabilidade agrícola.

A solução integra informações de localização, clima e características do solo para gerar recomendações sobre plantio, permitindo que agricultores tomem decisões mais assertivas com base em dados reais.

O sistema utiliza integrações com APIs externas para obtenção automática de endereço, coordenadas geográficas, previsão climática e propriedades do solo.

---

# 🎯 Objetivo da Solução

O principal objetivo do TerraByte é apoiar a agricultura inteligente através da análise de compatibilidade entre culturas agrícolas e terrenos cadastrados.

A aplicação permite:

* Cadastro e gerenciamento de usuários;
* Cadastro de terrenos agrícolas;
* Consulta automática de endereço por CEP;
* Identificação das características do solo;
* Consulta de condições climáticas;
* Análise de compatibilidade para plantio;
* Armazenamento do histórico de análises realizadas.

---

## Diagrama Drawio

![TerraByte.NET](/TerraByte.NET.drawio.png)

---

## Modelo Mer

![TerraByte.NET](/mer_terrabyte.jpeg)

---

# 🏗 Arquitetura do Projeto

O projeto foi desenvolvido seguindo uma arquitetura em camadas para garantir organização, manutenção e escalabilidade.

```text
Usuário
   ↓
Controllers
   ↓
Services
   ↓
Repositories
   ↓
Entity Framework Core
   ↓
SQLite Database
```

### Estrutura da Solução

```text
TerraByte
├── TerraByte.Api
├── TerraByte.Application
├── TerraByte.Domain
└── TerraByte.Infrastructure
```

### Responsabilidades das Camadas

#### API

Responsável pelos endpoints HTTP e comunicação com o cliente.

Controllers:

* UsuariosController
* TerrenosController
* CulturasController
* PesquisasController

#### Application

Contém as regras de negócio.

Services:

* UserService
* TerrenoService
* CulturaService
* PesquisaService

#### Infrastructure

Responsável pelo acesso aos dados, repositórios e integração com APIs externas.

#### Domain

Contém as entidades do sistema.

---

# 🔗 Integrações Externas

O TerraByte utiliza APIs externas para enriquecer os dados das análises.

### ViaCEP

Consulta de endereço a partir do CEP.

### Open Meteo

Consulta de latitude e longitude.

### OpenWeather

Consulta de dados climáticos.

### SoilGrids

Consulta de características do solo.

Fluxo:

```text
CEP
 ↓
ViaCEP
 ↓
Endereço
 ↓
Open Meteo
 ↓
Latitude / Longitude
 ↓
SoilGrids
 ↓
Dados do Solo
```

---

# 🗄 Banco de Dados

O sistema utiliza SQLite como banco de dados principal.

## Tabelas

* Usuario_terrabyte
* Plantio_terrabyte
* TipoSolo_terrabyte
* Defensivo_terrabyte
* plan_tip_terrabyte
* plan_def_terrabyte
* EnderecoPlantio_terrabyte
* AnalisePlantio_terrabyte

## Diagrama Conceitual

```text
Usuario
   │
   └── 1:N
         │
         ▼
EnderecoPlantio

Plantio
   │
   ├── N:N TipoSolo
   │
   └── N:N Defensivo

Plantio
   │
   └── 1:N
         │
         ▼
AnalisePlantio
```

## Relacionamentos

### Plantio ↔ TipoSolo

Relacionamento N:N.

Uma cultura pode ser compatível com vários tipos de solo.

Um tipo de solo pode atender várias culturas.

Tabela intermediária:

```text
plan_tip_terrabyte
```

### Plantio ↔ Defensivo

Relacionamento N:N.

Uma cultura pode utilizar vários defensivos.

Um defensivo pode ser utilizado em várias culturas.

Tabela intermediária:

```text
plan_def_terrabyte
```

---

# ⚙ Migrations

O projeto utiliza Entity Framework Core Migrations para controle de versão do banco de dados.

## Criar Migration

```bash
dotnet ef migrations add NomeDaMigration
```

## Atualizar Banco

```bash
dotnet ef database update
```

## Benefícios

* Controle de alterações do banco;
* Versionamento da estrutura;
* Facilidade de atualização em diferentes ambientes.

---

# 🚀 Como Executar o Projeto

## 1. Clonar Repositório

```bash
git clone URL_DO_REPOSITORIO
```

## 2. Restaurar Dependências

```bash
dotnet restore
```

## 3. Compilar

```bash
dotnet build
```

## 4. Aplicar Migrations

```bash
dotnet ef database update
```

## 5. Executar(o link do swagger vai aparecer aqui)

```bash
dotnet run --project TerraByte.Api
```

---

# 📄 Swagger

Após iniciar a aplicação(exemplo):

```text
http://localhost:xxxx/swagger
```

O Swagger permite testar todos os endpoints diretamente pelo navegador.

---

# 🧪 Testes da API

Os testes foram realizados através do Swagger e Postman.

## Teste 1 – Cadastro de Usuário

```http
POST /api/usuarios/cadastro
```

Exemplo:

```json
{
  "nome": "nometeste",
  "email": "teste@gmail.com",
  "senha": "12345678",
  "telefone": "11986547664",
  "genero": "Feminino",
  "dataNascimento": "2000-06-08",
  "fotoPerfil": "string"
}

```

Resultado esperado(infromações de exemplo):

```http
201 Created
```

---

## Teste 2 – Login

```http
POST /api/usuarios/login
```
Exemplo:

```json
{
  "email": "teste@gmail.com",
  "senha": "12345678"
}


```

Resultado esperado(infromações de exemplo):

```http
200 OK
```

---

## Teste 3 – Cadastro de Terreno

```http
POST /api/terrenos
```

Exemplo(caso o id do usuario n funcione e so copiar o id do usuario q vc cadastrou):

```json
{
  "nome": "Sitio Amarelo",
  "cep": "13291-256",
  "usuarioId": "3582fb0d-2664-488b-8303-7af2804e9e91"
}

```

Resultado esperado(infromações de exemplo):

* Consulta ViaCEP;
* Consulta Open Meteo;
* Consulta SoilGrids;
* Salvamento do terreno.

---

## Teste 4 – Análise Agrícola

```http
POST /api/analise
```

Exemplo:

```json
terreno - id do terro q acabou de criar

plnatio - id do plantio(pode achar depois de fazer um getAll)

```

Resultado esperado(infromações de exemplo):

```json
{
  "pontuacao": 85,
  "nivelRisco": "BAIXO",
  "recomendacao": "Plantio recomendado"
}
```

---

## Teste 5 - Pesquisa Clima

```http
GET /api/pesquisas/clima
```

Exemplo:

```json
lat:-23.69389
long:-46.565

```

Resultado esperado(infromações de exemplo):

```json
{

  "latitude": -23.69389,
  "longitude": -46.565,
  "dias": 30,
  "resumo": "Previsao solicitada para 30 dia(s). A API retornou 40 leituras de 3 em 3 horas: minima 11,6 C, maxima 23,5 C, umidade media 69,5% e chuva acumulada aproximada de 14,7 mm.",
  "temperaturaMinima": 11.62,
  "temperaturaMaxima": 23.54,
  "umidadeMedia": 69.475,
  "chuvaAcumuladaMm": 14.709999999999997

}
```

# ❌ Tratamento de Erros

A aplicação possui validações e middleware global para captura de exceções.

Exemplos:

### CEP inválido

```http
400 Bad Request
```

### Usuário não encontrado

```http
404 Not Found
```

### Dados obrigatórios ausentes

```http
400 Bad Request
```

---

# 🎬 Vídeo Demonstração

**Link:** link do video

---

# 🎤 Vídeo Pitch

**Link:** link do video

---

## 👩‍💻 Time

### Carolina Nascimento Gonçalves
RM564786 - 2TDSPJ

### Julia Sayuri Kina
RM564555 - 2TDSPJ

### Emanuelly Ventura do Nascimento
RM562339 - 2TDSPJ
---

# 🔗 Repositório GitHub

Link do projeto:

```text
https://github.com/Emanuelly0ventura/TerraByte.NET.git
```
