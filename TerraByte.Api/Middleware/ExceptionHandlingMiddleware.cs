using System.Net;
using Microsoft.EntityFrameworkCore;

namespace TerraByte.Api.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, title, message) = exception switch
        {
            ArgumentException => (HttpStatusCode.BadRequest, "Requisicao invalida", exception.Message),
            BadHttpRequestException => (HttpStatusCode.BadRequest, "Requisicao invalida", exception.Message),
            KeyNotFoundException => (HttpStatusCode.NotFound, "Recurso nao encontrado", exception.Message),
            DbUpdateException => (HttpStatusCode.Conflict, "Erro ao salvar no banco", "Nao foi possivel salvar os dados. Verifique se ja existe um registro com as mesmas informacoes."),
            HttpRequestException => (HttpStatusCode.ServiceUnavailable, "Servico externo indisponivel", "Nao foi possivel consultar um servico externo no momento."),
            TaskCanceledException => (HttpStatusCode.GatewayTimeout, "Tempo limite excedido", "A operacao demorou mais que o esperado."),
            InvalidOperationException => (HttpStatusCode.BadRequest, "Operacao invalida", exception.Message),
            _ => (HttpStatusCode.InternalServerError, "Erro interno", "Ocorreu um erro inesperado ao processar a requisicao.")
        };

        if ((int)statusCode >= 500)
            logger.LogError(exception, "Erro nao tratado na API TerraByte.");
        else
            logger.LogWarning(exception, "Erro tratado na API TerraByte.");

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            status = (int)statusCode,
            erro = title,
            mensagem = message,
            caminho = context.Request.Path.Value,
            data = DateTime.UtcNow
        };

        await context.Response.WriteAsJsonAsync(response);
    }
}
