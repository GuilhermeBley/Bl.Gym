using Microsoft.Extensions.DependencyInjection;

namespace Bl.Gym.TrainingApi.Infrastructure.Extensions;

public static class DiExtension
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddDbContext<Application.Repositories.TrainingContext, Repositories.MySqlTrainingContext>();
    }
}
