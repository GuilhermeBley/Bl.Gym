namespace Bl.Gym.TrainingApi.Domain.Entities.Training;

/// <summary>
/// A relationship between users and their exercises.
/// </summary>
public class Training
    : Entity
{
    public Guid StudentId { get; private set; }
    /// <summary>
    /// The gym group that gives this training.
    /// </summary>
    public Guid GymId { get; private set; }
    /// <summary>
    /// The exercises from this training.
    /// </summary>
    public HashSet<Guid> ExerciseIds { get; private set; } = new();
    public DateTimeOffset CreatedAt { get; private set; }

    public override bool Equals(object? obj)
    {
        return obj is Training training &&
               base.Equals(obj) &&
               EntityId.Equals(training.EntityId) &&
               StudentId.Equals(training.StudentId) &&
               GymId.Equals(training.GymId) &&
               EqualityComparer<HashSet<Guid>>.Default.Equals(ExerciseIds, training.ExerciseIds);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), EntityId, StudentId, GymId, ExerciseIds);
    }

    public static Result<Training> Create(
        Guid id,
        Guid studentId,
        Guid gymId,
        DateTimeOffset createdAt)
    {
        ResultBuilder<Training> builder = new();

        return builder.CreateResult(() =>
            new()
            {
                CreatedAt = createdAt,
                GymId = gymId,
                StudentId = studentId,
            });
    }
}
