using Bl.Gym.TrainingApi.Domain.Entities.Training;
using Bl.Gym.TrainingApi.Domain.Enum;

namespace Bl.Gym.TrainingApi.Application.Model.Training;

public class UserTrainingSheetModel
{
    public Guid Id { get; set; }
    public Guid StudentId { get; set; }
    public Guid GymId { get; set; }
    public UserTrainingStatus Status { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public static UserTrainingSheetModel MapFromEntity(
        UserTrainingSheet entity)
    {
        return new()
        {
            Id = entity.Id,
            CreatedAt = entity.CreatedAt,
            StudentId = entity.StudentId,
            Status = entity.Status,
            GymId = entity.GymId,
        };
    }
}
