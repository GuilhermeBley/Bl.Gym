namespace Bl.Gym.TrainingApi.Application.Model.Training;

public class GymExerciseModel
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }
}
