using Microsoft.EntityFrameworkCore;

namespace Bl.Gym.TrainingApi.Application.Repositories;

public abstract class TrainingContext
    : DbContext
{
    public DbSet<Model.Identity.RoleModel> Roles { get; set; }
    public DbSet<Model.Identity.RoleClaimModel> RoleClaims { get; set; }
    public DbSet<Model.Identity.UserRoleTrainingModel> UserTrainingRoles { get; set; }
    public DbSet<Model.Identity.UserModel> Users { get; set; }

    public DbSet<Model.Training.GymExerciseModel> Exercises { get; set; }
    public DbSet<Model.Training.GymGroupModel> GymGroups { get; set; }
    public DbSet<Model.Training.UserTrainingExercisesModel> UserTrainingExercises { get; set; }
    public DbSet<Model.Training.UserTrainingModel> UserTrainings { get; set; }   
}
