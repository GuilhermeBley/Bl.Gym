namespace Bl.Gym.TrainingApi.Domain.Entities.Training;

/// <summary>
/// A relationship between users and their exercises.
/// </summary>
public class UserTraining
    : Entity
{
    public Guid Id { get; private set; }
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
        return obj is UserTraining training &&
               base.Equals(obj) &&
               Id.Equals(training.Id) &&
               EntityId.Equals(training.EntityId) &&
               StudentId.Equals(training.StudentId) &&
               GymId.Equals(training.GymId) &&
               EqualityComparer<HashSet<Guid>>.Default.Equals(ExerciseIds, training.ExerciseIds);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), Id, EntityId, StudentId, GymId, ExerciseIds);
    }

    public static Result<UserTraining> Create(
        Guid id,
        Guid studentId,
        Guid gymId,
        DateTimeOffset createdAt)
    {
        ResultBuilder<UserTraining> builder = new();

        return builder.CreateResult(() =>
            new()
            {
                Id = id,
                CreatedAt = createdAt,
                GymId = gymId,
                StudentId = studentId,
            });
    }
}
