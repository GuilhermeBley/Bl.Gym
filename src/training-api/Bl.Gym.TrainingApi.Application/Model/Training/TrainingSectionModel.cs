using Bl.Gym.TrainingApi.Domain.Entities.Training;

namespace Bl.Gym.TrainingApi.Application.Model.Training;

/// <summary>
/// This model represents an traning set, that contains 1 or n <see cref="ExerciseSet"/>.
/// </summary>
public class TrainingSectionModel
{
    public Guid Id { get; set; }
    public Guid UserTrainingSheetId { get; set; }
    public string MuscularGroup { get; set; } = string.Empty;
    public int TargetDaysCount { get; set; }
    public int CurrentDaysCount { get; set; }
    public Guid ConcurrencyStamp { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    
    public TrainingSection MapToEntity()
    {
        return TrainingSection.Create(
            id: this.Id,
            muscularGroup: this.MuscularGroup,
            targetDaysCount: this.TargetDaysCount,
            currentDaysCount: this.CurrentDaysCount,
            sets: ExerciseSet.CreateEmptyRange(),
            concurrencyStamp: this.ConcurrencyStamp,
            createdAt: this.CreatedAt)
            .RequiredResult;
    }
    public static TrainingSectionModel MapFromEntity(
        TrainingSection entity,
        Guid userTrainingSheetId)
    {
        return new()
        {
            ConcurrencyStamp = entity.ConcurrencyStamp,
            CreatedAt = entity.CreatedAt,
            Id = entity.Id,
            MuscularGroup = entity.MuscularGroup,
            UserTrainingSheetId = userTrainingSheetId,
            CurrentDaysCount = entity.CurrentDaysCount,
            TargetDaysCount = entity.TargetDaysCount
        };
    }
}
