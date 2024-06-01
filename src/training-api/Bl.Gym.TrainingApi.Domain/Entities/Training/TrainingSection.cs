using Bl.Gym.TrainingApi.Domain.Enum;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace Bl.Gym.TrainingApi.Domain.Entities.Training;

/// <summary>
/// A relationship between users and their exercises.
/// </summary>
public class TrainingSection
    : Entity
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    /// <summary>
    /// The exercises from this training.
    /// It isn't allowed duplicate exercises in a section.
    /// </summary>
    public HashSet<ExerciseSet> ExerciseIds { get; private set; } = new(ExerciseSetComparer.Default);
    public Guid ConcurrencyStamp { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    private TrainingSection() { }

    public override bool Equals(object? obj)
    {
        return obj is TrainingSection training &&
               base.Equals(obj) &&
               EntityId.Equals(training.EntityId) &&
               Id.Equals(training.Id) &&
               EqualityComparer<HashSet<ExerciseSet>>.Default.Equals(ExerciseIds, training.ExerciseIds) &&
               Name == training.Name &&
               ConcurrencyStamp.Equals(training.ConcurrencyStamp) &&
               CreatedAt.Equals(training.CreatedAt);
    }

    public override int GetHashCode()
    {
        HashCode hash = new HashCode();
        hash.Add(base.GetHashCode());
        hash.Add(EntityId);
        hash.Add(Id);
        hash.Add(ExerciseIds);
        hash.Add(ConcurrencyStamp);
        hash.Add(CreatedAt);
        hash.Add(Name);
        return hash.ToHashCode();
    }

    public static Result<TrainingSection> CreateNew(
        Guid id,
        string name)
        => Create(
            id: id,
            name: name,
            concurrencyStamp: Guid.NewGuid(),
            createdAt: DateTimeOffset.UtcNow);

    public static Result<TrainingSection> Create(
        Guid id,
        string name,
        Guid concurrencyStamp,
        DateTimeOffset createdAt)
    {
        ResultBuilder<TrainingSection> builder = new();

        name = name?.Trim(' ', '\n') 
            ?? string.Empty;

        builder.AddIf(
            !Regex.IsMatch(name, @"^[a-z0-9 ]{1,45}$", RegexOptions.Singleline | RegexOptions.IgnoreCase),
            CoreExceptionCode.InvalidTrainingSectionName);

        return builder.CreateResult(() =>
            new()
            {
                Id = id,
                Name = name,
                CreatedAt = createdAt,
                ConcurrencyStamp = concurrencyStamp,
            });
    }

    /// <summary>
    /// Comparer ensures non duplicated exercises.
    /// </summary>
    private class ExerciseSetComparer
        : IEqualityComparer<ExerciseSet>
    {
        public readonly static ExerciseSetComparer Default = new();
        private ExerciseSetComparer() { }

        public bool Equals(ExerciseSet? x, ExerciseSet? y)
        {
            if (x is null || y is null)
                return false;

            return x.ExerciseId == y.ExerciseId;
        }

        public int GetHashCode([DisallowNull] ExerciseSet obj)
        {
            return obj.ExerciseId.GetHashCode();
        }
    }
}
