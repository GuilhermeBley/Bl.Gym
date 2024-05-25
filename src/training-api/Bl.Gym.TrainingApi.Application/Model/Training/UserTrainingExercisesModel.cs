namespace Bl.Gym.TrainingApi.Application.Model.Training;

/// <summary>
/// Relationship between <see cref="UserTrainingModel"/> and <see cref="GymExerciseModel"/>
/// </summary>
public class UserTrainingExercisesModel
{
    public Guid UserTrainingId { get; set; }
    public Guid ExerciseId { get; set; }
}
