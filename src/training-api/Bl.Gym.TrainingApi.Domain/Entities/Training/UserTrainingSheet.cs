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
    private readonly Dictionary<string, TrainingSection> _sections = new();


    public Guid Id { get; private set; }
    public Guid StudentId { get; private set; }
    /// <summary>
    /// The gym group that gives this training.
    /// </summary>
    public Guid GymId { get; private set; }
    public IReadOnlyCollection<TrainingSection> Sections => _sections.Values;

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

    public static Result<UserTrainingSheet> Create()
    {

    }
}
