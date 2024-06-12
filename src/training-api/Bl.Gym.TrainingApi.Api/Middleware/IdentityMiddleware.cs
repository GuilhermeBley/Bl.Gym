using Bl.Gym.TrainingApi.Api.Providers;

namespace Bl.Gym.TrainingApi.Api.Middleware
{
    internal class IdentityMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<IdentityMiddleware> _logger;

        public IdentityMiddleware(
            RequestDelegate next,
            ILogger<IdentityMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public Task InvokeAsync(HttpContext context)
        {
            _logger.LogTrace("Authenticating the user...");

            var identityProvider = context.RequestServices.GetRequiredService<Providers.ContextIdentityProvider>();
            identityProvider.ClaimPrincipal = context.User;

            return _next(context);
        }
    }
}
