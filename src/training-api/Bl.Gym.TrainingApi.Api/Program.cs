using Bl.Gym.TrainingApi.Api.Policies;
using Bl.Gym.TrainingApi.Api.Services;
using Bl.Gym.TrainingApi.Infrastructure.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

    // Define the BearerAuth scheme
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddScoped<Bl.Gym.TrainingApi.Api.Providers.ContextIdentityProvider>();
builder.Services.AddScoped<Bl.Gym.TrainingApi.Application.Providers.IIdentityProvider>(
    provider => provider.GetRequiredService<Bl.Gym.TrainingApi.Api.Providers.ContextIdentityProvider>());

builder.Services.AddInfrastructure(typeof(Program).Assembly);
builder.Services.AddSingleton<TokenGeneratorService>();

builder.Services.Configure<Bl.Gym.TrainingApi.Infrastructure.Options.PostgreSqlOption>(
    builder.Configuration.GetSection(Bl.Gym.TrainingApi.Infrastructure.Options.PostgreSqlOption.SECTION));

builder.Services.Configure<Bl.Gym.TrainingApi.Api.Options.JwtOptions>(
    builder.Configuration.GetSection(Bl.Gym.TrainingApi.Api.Options.JwtOptions.SECTION));

builder.Services
    .AddAuthentication(cfg =>
    {
        cfg.DefaultAuthenticateScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
        cfg.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(
        Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, 
        cfg =>
        {
            cfg.RequireHttpsMetadata = false;
            cfg.SaveToken = true;
            cfg.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                    System.Text.Encoding.ASCII.GetBytes(
                        builder.Configuration.GetSection("Jwt:Key").Value ?? string.Empty)),
                ValidateLifetime = true,
                ValidateIssuer = false,
                ValidateAudience = false,
            };
        })
    .AddJwtBearer(
        ForgotPasswordPolicy.Scheme,
        cfg =>
        {
            cfg.RequireHttpsMetadata = false;
            cfg.SaveToken = true;
            cfg.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                    System.Text.Encoding.ASCII.GetBytes(
                        builder.Configuration.GetSection("Jwt:EmailKey").Value ?? string.Empty)),
                ValidateLifetime = true,
                ValidateIssuer = false,
                ValidateAudience = false,
            };
        });
builder.Services.AddAuthorization(cfg =>
{
    cfg.AddPolicy(UserGymPolicy.POLICY_NAME, builder => 
        builder.RequireClaim(UserGymPolicy.REQUIRE_CLAIM_TYPE));
    cfg.AddPolicy(TrainingPolicy.POLICY_NAME, builder => 
        builder.RequireClaim(TrainingPolicy.REQUIRE_CLAIM_TYPE));
    cfg.AddPolicy(ForgotPasswordPolicy.POLICY_NAME, builder => 
        builder.RequireRole(ForgotPasswordPolicy.RequireRole.Value));
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<Bl.Gym.TrainingApi.Api.Middleware.CoreExceptionHandlingMiddleware>();
app.UseMiddleware<Bl.Gym.TrainingApi.Api.Middleware.IdentityMiddleware>();

#pragma warning disable ASP0014
app.UseEndpoints(Bl.Gym.TrainingApi.Api.Endpoints.TrainingEndpoint.MapEndpoints);
app.UseEndpoints(
    builder => Bl.Gym.TrainingApi.Api.Endpoints.UserEndpoint.MapEndpoints(
        builder,
        isDevelopment: app.Environment.IsDevelopment()));
app.UseEndpoints(Bl.Gym.TrainingApi.Api.Endpoints.GymEndpoint.MapEndpoints);
#pragma warning restore ASP0014

app.Run();
