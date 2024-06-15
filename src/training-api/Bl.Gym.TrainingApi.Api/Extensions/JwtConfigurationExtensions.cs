using Microsoft.AspNetCore.Authentication;

namespace Bl.Gym.TrainingApi.Api.Extensions;

public static class JwtConfigurationExtensions
{
    public const string SCHEME_API_TOKEN = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
    public const string SCHEME_EMAIL_TOKEN = "Email";

    public static AuthenticationBuilder AddApiToken(this AuthenticationBuilder builder,
        string key)
    {
        return builder.AddJwtBearer(SCHEME_API_TOKEN, options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = DefaultTokenValidationParameters(key);
        });
    }


    public static AuthenticationBuilder AddEmailToken(this AuthenticationBuilder builder,
        string key)
    {
        return builder.AddJwtBearer(SCHEME_EMAIL_TOKEN, options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = DefaultTokenValidationParameters(key);
        });
    }

    public static TokenValidationParameters DefaultTokenValidationParameters(string key) =>
        new TokenValidationParameters()
        {
            IssuerSigningKey = new SymmetricSecurityKey(
                    System.Text.Encoding.ASCII.GetBytes(key)),
            ValidateLifetime = true,
            ValidateIssuer = false,
            ValidateAudience = false,
            RoleClaimType = RoleClaim.DEFAULT_ROLE_TYPE
        };
}
