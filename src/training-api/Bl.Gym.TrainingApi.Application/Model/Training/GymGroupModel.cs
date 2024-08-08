using Bl.Gym.TrainingApi.Domain.Entities.Training;

namespace Bl.Gym.TrainingApi.Application.Model.Training;

public class GymGroupModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }

    public static GymGroupModel MapFromEntity(GymGroup entity)
    {
        return new GymGroupModel
        {
            CreatedAt = entity.CreatedAt,
            Name = entity.Name,
            Description = entity.Description,
            Id = entity.Id  
        };
    }
}
