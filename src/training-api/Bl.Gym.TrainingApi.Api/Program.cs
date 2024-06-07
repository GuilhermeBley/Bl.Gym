using Bl.Gym.TrainingApi.Infrastructure.Extensions;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<Bl.Gym.TrainingApi.Api.Providers.ContextIdentityProvider>();
builder.Services.AddScoped<Bl.Gym.TrainingApi.Application.Providers.IIdentityProvider>(
    provider => provider.GetRequiredService<Bl.Gym.TrainingApi.Api.Providers.ContextIdentityProvider>());

builder.Services.AddInfrastructure();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<Bl.Gym.TrainingApi.Api.Middleware.IdentityMiddleware>();

#pragma warning disable ASP0014
app.UseEndpoints(Bl.Gym.TrainingApi.Api.Endpoints.TrainingEndpoint.MapEndpoints);
#pragma warning restore ASP0014

app.Run();
