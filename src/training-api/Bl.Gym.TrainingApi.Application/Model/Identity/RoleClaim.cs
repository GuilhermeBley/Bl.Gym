namespace Bl.Gym.TrainingApi.Application.Model.Identity;

public class RoleClaim
{
    public Guid Id { get; set; }
    public Guid RoleId { get; set; }
    public string ClaimType { get; set; } = string.Empty;
    public string ClaimValue { get; set; } = string.Empty;
}
