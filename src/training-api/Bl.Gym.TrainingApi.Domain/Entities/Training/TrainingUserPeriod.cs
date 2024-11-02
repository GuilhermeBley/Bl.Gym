namespace Bl.Gym.TrainingApi.Domain.Entities.Training;

/// <summary>
/// Training period
/// </summary>
public class TrainingUserPeriod
    : Entity
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid SectionId { get; private set; }
    public DateTime? StartedAt { get; private set; }
    public DateTime? EndedAt { get; private set; }
    public string? Observation { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private TrainingUserPeriod() { }

    public Result Update(
        DateTime? startedAt,
        DateTime? endedAt)
    {
        ResultBuilder result = new();

        result.AddIf(
            (startedAt is null && endedAt is not null) ||
            (endedAt <= startedAt),
            CoreExceptionCode.InvalidStartAndEndDate);

        return result.CreateResult(() =>
        {
            StartedAt = startedAt;
            EndedAt = endedAt;
            UpdatedAt = DateTime.UtcNow;
        });
    }

    public static Result<TrainingUserPeriod> Create(
        Guid id,
        Guid userId,
        Guid sectionId,
        DateTime? startedAt,
        DateTime? endedAt,
        string? observation,
        DateTime updatedAt,
        DateTime createdAt)
    {
        ResultBuilder<TrainingUserPeriod> result = new();

        if (observation is not null)
        {
            observation = observation.Trim();

            result.AddIf(observation.Length > 1000, CoreExceptionCode.InvalidObservationLength);
        }

        result.AddIf(
            (startedAt is null && endedAt is not null) ||
            (endedAt <= startedAt),
            CoreExceptionCode.InvalidStartAndEndDate);

        return result.CreateResult(() =>
        {
            return new()
            {
                CreatedAt = createdAt,
                EndedAt = endedAt,
                Id = id,
                SectionId = sectionId,
                Observation = observation,
                StartedAt = startedAt,
                UpdatedAt = updatedAt,
                UserId = userId,
            };
        });
    }
}
