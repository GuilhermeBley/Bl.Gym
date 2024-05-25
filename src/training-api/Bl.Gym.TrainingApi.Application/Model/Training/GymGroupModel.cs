namespace Bl.Gym.TrainingApi.Application.Model.Training;

public class GymGroupModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }

}
