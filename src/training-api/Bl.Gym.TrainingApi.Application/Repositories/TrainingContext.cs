using Microsoft.EntityFrameworkCore;

namespace Bl.Gym.TrainingApi.Application.Repositories;

public abstract class TrainingContext
    : DbContext
{
    public abstract DbSet<Model.Identity.RoleModel> Roles { get; set; }
    public abstract DbSet<Model.Identity.RoleClaimModel> RoleClaims { get; set; }
    public abstract DbSet<Model.Identity.UserRoleTrainingModel> UserTrainingRoles { get; set; }
    public abstract DbSet<Model.Identity.UserModel> Users { get; set; }

    public abstract DbSet<Model.Training.TrainingSectionModel> TrainingSections { get; set; }
    public abstract DbSet<Model.Training.ExerciseSetModel> ExerciseSets { get; set; }
    public abstract DbSet<Model.Training.GymExerciseModel> Exercises { get; set; }
    public abstract DbSet<Model.Training.GymGroupModel> GymGroups { get; set; }
    public abstract DbSet<Model.Training.UserTrainingSheetModel> UserTrainingSheets { get; set; }   

    public TrainingContext(DbContextOptions options)
        : base(options)
    {

    }
}
