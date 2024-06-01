using Bl.Gym.TrainingApi.Domain.Enum;

namespace Bl.Gym.TrainingApi.Application.Model.Training;

public class UserTrainingSheetModel
{
    public Guid Id { get; set; }
    public Guid StudentId { get; set; }
    public Guid GymId { get; set; }
    public UserTrainingStatus Status { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
