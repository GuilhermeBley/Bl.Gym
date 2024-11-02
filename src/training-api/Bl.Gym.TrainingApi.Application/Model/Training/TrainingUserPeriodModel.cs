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
}
