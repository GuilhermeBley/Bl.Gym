namespace Bl.Gym.TrainingApi.Domain.Entities.Training;

/// <summary>
/// A 'Gym Group' can be understood as a real gym.
/// </summary>
public class GymGroup
    : Entity
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;

    /// <summary>
    /// A range of teacher ID's that give class to the students.
    /// </summary>
    public HashSet<Guid> TeacherIds { get; } = new();

    /// <summary>
    /// A range of user ID's that are students in this group
    /// </summary>
    public HashSet<Guid> StudentIds { get; } = new();
    public DateTimeOffset CreatedAt { get; private set; }

    private GymGroup() { }

    public static Result<GymGroup> Create(
        Guid id,
        string name,
        string description,
        DateTimeOffset createdAt)
    {
        ResultBuilder<GymGroup> builder = new();

        name = name?.Trim() ?? string.Empty;
        description = description?.Trim() ?? string.Empty;

        builder.AddIf(
            name.Length < 2 || name.Length > 255,
            CoreExceptionCode.InvalidStringLength);

        return builder.CreateResult(() =>
            new()
            {
                Id = id,
                CreatedAt = createdAt,
                Name = name,
                Description = description,
            });
    }
}
