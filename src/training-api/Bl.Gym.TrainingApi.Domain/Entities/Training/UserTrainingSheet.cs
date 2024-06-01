using Bl.Gym.TrainingApi.Domain.Enum;
using static System.Collections.Specialized.BitVector32;

namespace Bl.Gym.TrainingApi.Domain.Entities.Training;

/// <summary>
/// A user training sheet can store a group of exercises. 
/// An example of a use case is a student who has a 
/// training sheet with several trainings, including A, 
/// B, C, etc. And this is represented by a range of 
/// <see cref="TrainingSection"/>.
/// </summary>
public class UserTrainingSheet
    : Entity
{
    /// <summary>
    /// A range composed of the training section and its group name.
    /// </summary>
    private readonly Dictionary<string, TrainingSection> _sections = new(StringComparer.OrdinalIgnoreCase);

    public Guid Id { get; private set; }
    public Guid StudentId { get; private set; }
    /// <summary>
    /// The gym group that gives this training.
    /// </summary>
    public Guid GymId { get; private set; }
    public IReadOnlyCollection<TrainingSection> Sections => _sections.Values;
    public UserTrainingStatus Status { get; private set; }
    public Guid ConcurrencyStamp { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private UserTrainingSheet() { }

    public override bool Equals(object? obj)
    {
        return obj is UserTrainingSheet sheet &&
               base.Equals(obj) &&
               EntityId.Equals(sheet.EntityId) &&
               EqualityComparer<Dictionary<string, TrainingSection>>.Default.Equals(_sections, sheet._sections) &&
               Id.Equals(sheet.Id) &&
               StudentId.Equals(sheet.StudentId) &&
               GymId.Equals(sheet.GymId) &&
               EqualityComparer<IReadOnlyCollection<TrainingSection>>.Default.Equals(Sections, sheet.Sections);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), EntityId, _sections, Id, StudentId, GymId, Sections);
    }

    public static Result<UserTrainingSheet> CreateNow(
        Guid studentId,
        Guid gymId)
        => Create(
            id: Guid.NewGuid(),
            studentId: studentId,
            gymId: gymId,
            status: UserTrainingStatus.InProgress,
            concurrencyStamp: Guid.NewGuid(),
            createdAt: DateTime.UtcNow,
            updatedAt: DateTime.UtcNow,
            sections: Enumerable.Empty<TrainingSection>());

    public static Result<UserTrainingSheet> Create(
        Guid id,
        Guid studentId,
        Guid gymId,
        UserTrainingStatus status,
        Guid concurrencyStamp,
        DateTime updatedAt,
        DateTime createdAt,
        IEnumerable<TrainingSection> sections)
    {
        ResultBuilder<UserTrainingSheet> builder = new();

        builder.AddIf(
            !IsTrainingValid(sections),
            CoreExceptionCode.InvalidSetOfTrainingSections);

        return builder.CreateResult(() =>
        {
            var result = new UserTrainingSheet()
            {
                CreatedAt = createdAt,
                GymId = gymId,
                Id = id,
                StudentId = studentId,
                UpdatedAt = updatedAt,
                Status = status,
                ConcurrencyStamp = concurrencyStamp,
            };

            foreach (var s in sections)
                result._sections.Add(s.Name, s);

            return result;
        });
    }

    private static bool IsTrainingValid(
        IEnumerable<TrainingSection> sections)
    {
        var sectionCount = sections.Count();

        var set = new HashSet<string>(
            sections
            .Where(s => s is not null)
            .Select(s => s.Name),
            StringComparer.OrdinalIgnoreCase);

        return set.Count == sectionCount;
    }
}
