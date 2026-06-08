using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using TerraByte.Api.Extensions;
using TerraByte.Api.Middleware;
using TerraByte.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.Configure<ApiBehaviorOptions>(opcoes =>
{
    opcoes.InvalidModelStateResponseFactory = contexto =>
    {
        var erros = contexto.ModelState
            .Where(item => item.Value?.Errors.Count > 0)
            .ToDictionary(
                item => item.Key,
                item => item.Value!.Errors.Select(erro => erro.ErrorMessage).ToArray());

        return new BadRequestObjectResult(new
        {
            status = StatusCodes.Status400BadRequest,
            erro = "Requisicao invalida",
            mensagem = "Um ou mais campos estao invalidos.",
            campos = erros,
            caminho = contexto.HttpContext.Request.Path.Value,
            data = DateTime.UtcNow
        });
    };
});
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

using (var scope = app.Services.CreateScope())
{
    var contexto = scope.ServiceProvider.GetRequiredService<TerraByteContext>();
    TerraByteDataLoader.PrepararBanco(contexto);
    TerraByteDataLoader.Seed(contexto);
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

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
