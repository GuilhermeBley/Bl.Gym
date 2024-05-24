namespace Bl.Gym.TrainingApi.Domain.Entities;

public abstract class Entity
{
    public Guid EntityId { get; } = Guid.NewGuid();

    public override bool Equals(object? obj)
    {
        return obj is Entity entity &&
               EntityId.Equals(entity.EntityId);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(EntityId);
    }
}
