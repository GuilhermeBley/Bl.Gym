namespace Bl.Gym.TrainingApi.Application.Model.Training;

public class ExerciseSetModel
{
    public Guid Id { get; set; }
    public string Set { get; set; } = string.Empty;
    public Guid ExerciseId { get; set; }
}
