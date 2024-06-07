using Bl.Gym.TrainingApi.Api.Providers;

namespace Bl.Gym.TrainingApi.Api.Middleware
{
    internal class IdentityMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<IdentityMiddleware> _logger;
        private readonly ContextIdentityProvider _identityProvider;

        public IdentityMiddleware(
            RequestDelegate next,
            ILogger<IdentityMiddleware> logger,
            Providers.ContextIdentityProvider identityProvider)
        {
            _next = next;
            _logger = logger;
            _identityProvider = identityProvider;
        }

        public Task InvokeAsync(HttpContext context)
        {
            _logger.LogTrace("Authenticating the user...");

            _identityProvider.ClaimPrincipal = context.User;

            return _next(context);
        }
    }
}
