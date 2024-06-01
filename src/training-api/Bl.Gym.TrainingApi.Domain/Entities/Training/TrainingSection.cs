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
    /// <summary>
    /// This field works like a name for the section. 
    /// Usually it takes the following names: A, B, C,
    /// D, etc. It represents the muscular group that 
    /// will be worked out.
    /// </summary>
    public string MuscularGroup { get; private set; } = string.Empty;
    /// <summary>
    /// The exercises from this training.
    /// It isn't allowed duplicate exercises in a section.
    /// </summary>
    public HashSet<ExerciseSet> Sets { get; private set; } = new(ExerciseSetComparer.Default);
    public Guid ConcurrencyStamp { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    private TrainingSection() { }

    public override bool Equals(object? obj)
    {
        return obj is TrainingSection training &&
               base.Equals(obj) &&
               EntityId.Equals(training.EntityId) &&
               Id.Equals(training.Id) &&
               EqualityComparer<HashSet<ExerciseSet>>.Default.Equals(Sets, training.Sets) &&
               MuscularGroup == training.MuscularGroup &&
               ConcurrencyStamp.Equals(training.ConcurrencyStamp) &&
               CreatedAt.Equals(training.CreatedAt);
    }

    public override int GetHashCode()
    {
        HashCode hash = new HashCode();
        hash.Add(base.GetHashCode());
        hash.Add(EntityId);
        hash.Add(Id);
        hash.Add(Sets);
        hash.Add(ConcurrencyStamp);
        hash.Add(CreatedAt);
        hash.Add(MuscularGroup);
        return hash.ToHashCode();
    }

    public static Result<TrainingSection> CreateNew(
        Guid id,
        string muscularGroup,
        IEnumerable<ExerciseSet> sets)
        => Create(
            id: id,
            muscularGroup: muscularGroup,
            sets: sets,
            concurrencyStamp: Guid.NewGuid(),
            createdAt: DateTimeOffset.UtcNow);

    public static Result<TrainingSection> Create(
        Guid id,
        string muscularGroup,
        IEnumerable<ExerciseSet> sets,
        Guid concurrencyStamp,
        DateTimeOffset createdAt)
    {
        ResultBuilder<TrainingSection> builder = new();

        muscularGroup = muscularGroup?.Trim(' ', '\n') 
            ?? string.Empty;

        builder.AddIf(
            !Regex.IsMatch(muscularGroup, @"^[a-z0-9 ]{1,45}$", RegexOptions.Singleline | RegexOptions.IgnoreCase),
            CoreExceptionCode.InvalidTrainingSectionName);

        builder.AddIf(
            !sets.Where(set => set is not null).Any(),
            CoreExceptionCode.ItsRequiredAtLeastOneExerciseForSection);

        return builder.CreateResult(() =>
        {
            TrainingSection section = new()
            {
                Id = id,
                MuscularGroup = muscularGroup,
                CreatedAt = createdAt,
                ConcurrencyStamp = concurrencyStamp,
            };

            foreach (var set in sets)
                section.Sets.Add(set);

            return section;
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
