using System.Reflection;
using Microsoft.OpenApi.Models;
using TerraByte.Api.Extensions;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opcoes =>
{
    opcoes.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "TerraByte API",
        Version = "v1",
        Description = "API para apoiar agricultores com consultas de clima, solo, localizacao de terrenos e culturas."
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        opcoes.IncludeXmlComments(xmlPath);
});

builder.Services
    .AdicionarPersistenciaTerraByte(builder.Configuration)
    .AdicionarServicosTerraByte()
    .AdicionarClientesApisExternas();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(opcoes =>
    {
        opcoes.SwaggerEndpoint("/swagger/v1/swagger.json", "TerraByte API v1");
        opcoes.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();


