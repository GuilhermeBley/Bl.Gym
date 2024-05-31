namespace Bl.Gym.TrainingApi.Application.Model.Training;

/// <summary>
/// Relationship between <see cref="UserTrainingSheetModel"/> and <see cref="GymExerciseModel"/>
/// </summary>
public class UserTrainingWithSetsModel
{
    public Guid UserTrainingId { get; set; }
    public Guid SetId { get; set; }
}
