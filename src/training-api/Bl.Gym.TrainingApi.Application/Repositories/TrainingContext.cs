using Microsoft.EntityFrameworkCore;

namespace Bl.Gym.TrainingApi.Application.Repositories;

public abstract class TrainingContext
    : DbContext
{
    public DbSet<Model.Identity.UserModel> Users { get; protected set; }
}
