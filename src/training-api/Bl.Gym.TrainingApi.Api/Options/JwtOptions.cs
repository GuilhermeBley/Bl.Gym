using System.ComponentModel.DataAnnotations;

namespace Bl.Gym.TrainingApi.Api.Options;

public class JwtOptions
{
    public const string SECTION = "Jwt";

    [Required]
    [MinLength(32)]
    public string Key { get; set; } = string.Empty;

    [Required]
    [MinLength(32)]
    public string KeyEmailInvitation { get; set; } = string.Empty;
}
