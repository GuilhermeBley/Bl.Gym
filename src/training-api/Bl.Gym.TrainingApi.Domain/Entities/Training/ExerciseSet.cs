using Bl.Gym.TrainingApi.Domain.Validations;

namespace Bl.Gym.TrainingApi.Domain.Entities.Training;

/// <summary>
/// Entity stores an exercise and its respective set.
/// </summary>
public class ExerciseSet
    : Entity
{
    public Guid Id { get; private set; }
    /// <summary>
    /// This field complements the exercise, it says 
    /// about the quantity and way to do the exercise.
    /// </summary>
    public string Set { get; private set; } = string.Empty;
    public Guid ExerciseId { get; private set; }

    private ExerciseSet() { }

    public override bool Equals(object? obj)
    {
        return obj is ExerciseSet set &&
               base.Equals(obj) &&
               EntityId.Equals(set.EntityId) &&
               Id.Equals(set.Id) &&
               Set == set.Set &&
               ExerciseId.Equals(set.ExerciseId);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), EntityId, Id, Set, ExerciseId);
    }

    public static Result<ExerciseSet> Create(
        Guid id,
        string set,
        Guid exerciseId)
    {
        ResultBuilder<ExerciseSet> builder = new();

        set = set?.Trim(' ', '\n') ?? string.Empty;

        set = StringValidation.RemoveBreakRow(set);

        builder.AddIf(
            !StringValidation.IsInLength(set, 45),
            CoreExceptionCode.InvalidSetValue);

        return builder.CreateResult(() =>
            new()
            {
                ExerciseId = exerciseId,
                Id = id,
                Set = set,
            });
    }
}
