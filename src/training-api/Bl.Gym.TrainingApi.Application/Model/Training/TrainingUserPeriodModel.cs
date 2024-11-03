using Bl.Gym.TrainingApi.Domain.Entities.Training;

namespace Bl.Gym.TrainingApi.Application.Model.Training;

/// <summary>
/// This model represents an relation between User and Training Section.
/// </summary>
public class TrainingUserPeriodModel
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid SectionId { get; set; }

    public DateTime? StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public string? Observation { get; set; }

    public DateTime UpdatedAt { get; set; }
    public DateTime CreatedAt { get; set; }

    public static TrainingUserPeriodModel MapFromEntity(TrainingUserPeriod entity)
    {
        return new()
        {
            CreatedAt = entity.CreatedAt,
            Id = entity.Id,
            EndedAt = entity.EndedAt,
            Observation = entity.Observation,
            SectionId = entity.SectionId,
            StartedAt = entity.StartedAt,
            UpdatedAt = entity.UpdatedAt,
            UserId = entity.UserId,
        };
    }
}
