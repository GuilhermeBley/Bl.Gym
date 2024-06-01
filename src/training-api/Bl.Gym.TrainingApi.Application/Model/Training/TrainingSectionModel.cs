using Bl.Gym.TrainingApi.Domain.Entities.Training;

namespace Bl.Gym.TrainingApi.Application.Model.Training;

public class TrainingSectionModel
{
    public Guid Id { get; set; }
    public Guid UserTrainingSheetId { get; set; }
    public string MuscularGroup { get; set; } = string.Empty;
    public Guid ConcurrencyStamp { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
