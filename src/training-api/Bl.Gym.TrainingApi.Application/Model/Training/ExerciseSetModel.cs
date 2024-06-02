using Bl.Gym.TrainingApi.Domain.Entities.Training;

namespace Bl.Gym.TrainingApi.Application.Model.Training;

public class ExerciseSetModel
{
    public Guid Id { get; set; }
    public Guid TrainingSectionId { get; set; }
    public string Set { get; set; } = string.Empty;
    public Guid ExerciseId { get; set; }

    public static ExerciseSetModel MapFromEntity(
        ExerciseSet entity,
        Guid trainingSectionId)
    {
        return new()
        {
            ExerciseId = entity.ExerciseId,
            Id = entity.Id,
            Set = entity.Set,
            TrainingSectionId = trainingSectionId
        };
    }
}
