using Microsoft.EntityFrameworkCore;

namespace Bl.Gym.TrainingApi.Application.Repositories;

public abstract class TrainingContext
    : DbContext
{
    public DbSet<Model.Identity.RoleModel> Roles { get; set; }
    public DbSet<Model.Identity.RoleClaimModel> RoleClaims { get; set; }
    public DbSet<Model.Identity.UserRoleTrainingModel> UserTrainingRoles { get; set; }
    public DbSet<Model.Identity.UserModel> Users { get; set; }

    public DbSet<Model.Training.TrainingSectionModel> TrainingSections { get; set; }
    public DbSet<Model.Training.ExerciseSetModel> ExerciseSets { get; set; }
    public DbSet<Model.Training.GymExerciseModel> Exercises { get; set; }
    public DbSet<Model.Training.GymGroupModel> GymGroups { get; set; }
    public DbSet<Model.Training.UserTrainingWithSetsModel> UserTrainingWithSets { get; set; }
    public DbSet<Model.Training.UserTrainingSheetModel> UserTrainings { get; set; }   
}
