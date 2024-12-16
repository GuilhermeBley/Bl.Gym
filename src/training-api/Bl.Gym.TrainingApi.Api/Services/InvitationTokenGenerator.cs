using Bl.Gym.TrainingApi.Api.Options;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Bl.Gym.TrainingApi.Api.Services;

public class InvitationTokenGenerator
{
    private readonly IOptions<Options.JwtOptions> _options;

    public InvitationTokenGenerator(IOptions<JwtOptions> options)
    {
        _options = options;
    }

    public string Generate(
        Claim[] claims, 
        DateTime expiresIn)
    {
        throw new NotImplementedException();
    }
}
