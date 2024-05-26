namespace Bl.Gym.TrainingApi.Application.Model.Identity;

public class RoleModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NormalizedName { get; set; } = string.Empty;
    public Guid ConcurrencyStamp { get; set; }
}
