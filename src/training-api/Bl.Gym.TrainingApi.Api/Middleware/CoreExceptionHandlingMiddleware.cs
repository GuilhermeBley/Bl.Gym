using Bl.Gym.TrainingApi.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace Bl.Gym.TrainingApi.Api.Middleware;

public class CoreExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public CoreExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (CoreException coreException)
        {
            await HandleExceptionAsync(context, coreException);
        }
        catch
        {
            throw;
        }
    }

    private Task HandleExceptionAsync(HttpContext context, CoreException exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        var result = JsonSerializer.Serialize(
            new {
                Code = (int)exception.StatusCode,
                Message = "An error occurred. Please try again later." 
            });

        try
        {
            var statusCode =
                int.Parse(
                    string.Concat(((int)exception.StatusCode).ToString().Take(3))
                );

            response.StatusCode = (int)(HttpStatusCode)statusCode;
        }
        catch { }

        return response.WriteAsync(result);
    }
}
