
using Microsoft.AspNetCore.Mvc;

namespace Bl.Gym.TrainingApi.Api.Extensions;

public static class HttpContextExtension
{
    /// <summary>
    /// Get the current base URL from the context. Example: https://mysite
    /// </summary>
    public static string GetBaseUrl(this HttpContext context)
    {
        var request = context.Request;
        var baseUrl = $"{request.Scheme}://{request.Host}";
        return baseUrl.Trim('/');
    }
}
