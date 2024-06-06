using Microsoft.Extensions.DependencyInjection;

namespace Bl.Gym.TrainingApi.Infrastructure.Extensions;

public static class DiExtension
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(Application.Commands.Identity.CreateUser.CreateUserHandler).Assembly);
            })
            .AddScoped<Application.Services.GymRoleCheckerService>()
            .AddDbContext<Application.Repositories.TrainingContext, Repositories.MySqlTrainingContext>();
    }
}
