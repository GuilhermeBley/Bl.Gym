namespace Bl.Gym.TrainingApi.Domain.Entities.Training;

/// <summary>
/// It represents a training or exercise; it is composed of a title and description. 
/// The description should have the details to execute the exercise.
/// </summary>
public class GymExercise
    : Entity
{
    public Guid Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; private set; }

    public override bool Equals(object? obj)
    {
        return obj is GymExercise plan &&
               base.Equals(obj) &&
               EntityId.Equals(plan.EntityId) &&
               Title == plan.Title &&
               Description == plan.Description &&
               CreatedAt.Equals(plan.CreatedAt);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), EntityId, Title, Description, CreatedAt);
    }

    public static Result<GymExercise> Create(
        Guid id,
        string title,
        string description,
        DateTimeOffset createdAt)
    {
        ResultBuilder<GymExercise> builder = new();

        title = title?.Trim() ?? string.Empty;
        description = description?.Trim() ?? string.Empty;

        builder.AddIf(
            title.Length < 2 || title.Length > 255, 
            CoreExceptionCode.InvalidStringLength);

        return builder.CreateResult(() =>
            new()
            {
                Id = id,
                CreatedAt = createdAt,
                Title = title,
                Description = description,
            });
    }
}
