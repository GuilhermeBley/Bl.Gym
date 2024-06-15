using Bl.Gym.TrainingApi.Api.Options;
using Bl.Gym.TrainingApi.Domain.Entities.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Bl.Gym.TrainingApi.Api.Services;

public class TokenGeneratorService
{
    private readonly IOptions<Options.JwtOptions> _options;

    public TokenGeneratorService(IOptions<JwtOptions> options)
    {
        _options = options;
    }

    public string Generate(Claim[] claims, DateTime expiresIn)
    {
        var handler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_options.Value.Key);
        var credentials = new SigningCredentials(
        new SymmetricSecurityKey(key),
        SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = GenerateClaims(claims),
            Expires = expiresIn,
            SigningCredentials = credentials,
        };
        var token = handler.CreateToken(tokenDescriptor);
        return handler.WriteToken(token);
    }

    private static ClaimsIdentity GenerateClaims(Claim[] claims)
    {
        var ci = new ClaimsIdentity();

        Dictionary<string, Claim> claimsNonDuplicated
            = new(claims.Length, StringComparer.OrdinalIgnoreCase);

        foreach (var claim in claims)
            claimsNonDuplicated.TryAdd(
                string.Concat(claim.ValueType, claim.Value),
                claim);

        ci.AddClaims(claimsNonDuplicated.Values);

        return ci;
    }
}
