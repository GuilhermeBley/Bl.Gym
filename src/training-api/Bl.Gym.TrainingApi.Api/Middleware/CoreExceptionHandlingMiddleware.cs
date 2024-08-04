using Bl.Gym.TrainingApi.Domain.Exceptions;
using Bl.Gym.TrainingApi.Domain.Primitive;
using System.Net;
using System.Text.Json;

namespace Bl.Gym.TrainingApi.Api.Middleware;

public class CoreExceptionHandlingMiddleware
{
    private readonly ILogger<CoreExceptionHandlingMiddleware> _logger;
    private readonly RequestDelegate _next;

    public CoreExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<CoreExceptionHandlingMiddleware> logger)
    {
        _logger = logger;
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

        string result = string.Empty;
        CoreExceptionCode coreExceptionCode;
        try
        {
            if (exception is AggregateCoreException aggregate)
            {
                result = JsonSerializer.Serialize(
                    aggregate.InnerExceptions.Select(innerException =>
                        new
                        {
                            Code = (int)innerException.StatusCode,
                            Message = innerException.Message
                        }));
                coreExceptionCode = aggregate.InnerExceptions.FirstOrDefault()?.StatusCode
                    ?? aggregate.StatusCode;
            }
            else
            {
                result = JsonSerializer.Serialize(
                    ErrorResult.CreateRange(
                        code: (int)exception.StatusCode,
                        message: exception.Message
                    ));
                coreExceptionCode = exception.StatusCode;
            }

            var canParseStatus = int.TryParse(
                string.Concat(((int)coreExceptionCode).ToString().Take(3)),
                out var parsedStatusNum);

            if (canParseStatus &&
                Enum.IsDefined(typeof(HttpStatusCode), parsedStatusNum))
                response.StatusCode = parsedStatusNum;
            else
                response.StatusCode = (int)HttpStatusCode.BadRequest;

            return response.WriteAsync(result);
        }
        catch (Exception e)
        {
            _logger.LogWarning(e, "Failed to get core exception status code.");
            return Task.CompletedTask;
        }
    }

    private record ErrorResult(
        int Code,
        string Message)
    {
        public static ErrorResult[] CreateRange(
            int code,
            string message)
            => new ErrorResult[]
            {
                new(code, message)
            };
    }
}
