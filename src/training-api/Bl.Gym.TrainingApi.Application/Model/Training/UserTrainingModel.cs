namespace Bl.Gym.TrainingApi.Application.Model.Training;

public class UserTrainingModel
{
    public Guid Id { get; set; }
    public Guid StudentId { get; set; }
    public Guid GymId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

}
