using Bl.Gym.TrainingApi.Application.Services;
using Bl.Gym.TrainingApi.Infrastructure.Options;
using Bl.Gym.TrainingApi.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace Bl.Gym.TrainingApi.Infrastructure.Extensions;

public static class DiExtension
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection serviceCollection,
        Assembly migrationAssembly)
    {
        return serviceCollection
            .AddSingleton<EventBus.IEventBus>(provider =>
            {
                var config = provider.GetRequiredService<IOptions<AzureEventbusOption>>();
                
                return new EventBus.Infrastructure.AzureEventBus(config.Value.ConnectionsString);
            })
            .AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(Application.Commands.Identity.CreateUser.CreateUserHandler).Assembly);
            })
            .AddScoped<Application.Services.GymRoleCheckerService>()
            .AddDbContext<Repositories.PostgreTrainingContext>((provider, opt) =>
            {
                var config = provider.GetRequiredService<IOptions<PostgreSqlOption>>();
                opt.UseNpgsql(config.Value.ConnectionString, opt =>
                    {
                        opt.MigrationsAssembly(migrationAssembly?.FullName ?? typeof(DiExtension).Assembly.FullName);
                    });
            })
            .AddScoped<Application.Repositories.TrainingContext>(
                provider => provider.GetRequiredService<Repositories.PostgreTrainingContext>())
            
            .AddScoped<Application.Repositories.Training.IGetAllTrainingsByCurrentUserRepository, Repositories.Training.GetAllTrainingsByCurrentUserRepository>()
            .AddSingleton<IEmailService, MsOfficeEmailService>();
    }
}
