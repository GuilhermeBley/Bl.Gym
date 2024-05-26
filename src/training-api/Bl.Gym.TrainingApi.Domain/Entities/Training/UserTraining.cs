using Bl.Gym.TrainingApi.Domain.Enum;

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
    public UserTrainingStatus Status { get; private set; }
    public Guid ConcurrencyStamp { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    public override bool Equals(object? obj)
    {
        return obj is UserTraining training &&
               base.Equals(obj) &&
               EntityId.Equals(training.EntityId) &&
               Id.Equals(training.Id) &&
               StudentId.Equals(training.StudentId) &&
               GymId.Equals(training.GymId) &&
               EqualityComparer<HashSet<Guid>>.Default.Equals(ExerciseIds, training.ExerciseIds) &&
               Status == training.Status &&
               ConcurrencyStamp.Equals(training.ConcurrencyStamp) &&
               CreatedAt.Equals(training.CreatedAt);
    }

    public override int GetHashCode()
    {
        HashCode hash = new HashCode();
        hash.Add(base.GetHashCode());
        hash.Add(EntityId);
        hash.Add(Id);
        hash.Add(StudentId);
        hash.Add(GymId);
        hash.Add(ExerciseIds);
        hash.Add(Status);
        hash.Add(ConcurrencyStamp);
        hash.Add(CreatedAt);
        return hash.ToHashCode();
    }

    public static Result<UserTraining> CreateNew(
        Guid id,
        Guid studentId,
        Guid gymId)
        => Create(
            id: id,
            studentId: studentId,
            gymId: gymId,
            status: UserTrainingStatus.ToStart,
            concurrencyStamp: Guid.NewGuid(),
            createdAt: DateTimeOffset.UtcNow);

    public static Result<UserTraining> Create(
        Guid id,
        Guid studentId,
        Guid gymId,
        UserTrainingStatus status,
        Guid concurrencyStamp,
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
                ConcurrencyStamp = concurrencyStamp,
                Status = status
            });
    }
}
