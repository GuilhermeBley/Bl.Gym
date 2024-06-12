using Bl.Gym.TrainingApi.Application.Repositories;
using Bl.Gym.TrainingApi.Infrastructure.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Bl.Gym.TrainingApi.Infrastructure.Repositories;

internal class MySqlTrainingContext 
    : TrainingContext
{
    private readonly IOptions<PostgreSqlOption> _options;

    public MySqlTrainingContext(IOptions<PostgreSqlOption> options)
    {
        _options = options;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder
            .UseNpgsql(_options.Value.ConnectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new DefaultStringLengthConvention(500));

        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Application.Model.Identity.RoleClaimModel>(b =>
        {
            b.HasKey(p => p.Id);
            b.Property(p => p.Id).ValueGeneratedOnAdd();

            b.HasOne<Application.Model.Identity.RoleModel>().WithMany().HasForeignKey(b => b.RoleId);
        });

        modelBuilder.Entity<Application.Model.Identity.RoleModel>(b =>
        {
            b.HasKey(p => p.Id);
            b.Property(p => p.Id).ValueGeneratedOnAdd();

            b.HasIndex(p => p.NormalizedName).IsUnique();
        });

        modelBuilder.Entity<Application.Model.Identity.UserModel>(b =>
        {
            b.HasKey(p => p.Id);
            b.Property(p => p.Id).ValueGeneratedOnAdd();

            b.HasIndex(p => p.NormalizedUserName).IsUnique();

            b.Property(p => p.LockoutEnd).HasConversion(ConversionUtils.DateTimeToUtcDateTimeConversion);
        });

        modelBuilder.Entity<Application.Model.Identity.UserRoleTrainingModel>(b =>
        {
            b.HasKey(p => p.Id);
            b.Property(p => p.Id).ValueGeneratedOnAdd();

            b.HasOne<Application.Model.Identity.UserModel>().WithMany().HasForeignKey(p => p.UserId);
            b.HasOne<Application.Model.Identity.RoleModel>().WithMany().HasForeignKey(p => p.RoleId);
            b.HasOne<Application.Model.Training.GymGroupModel>().WithMany().HasForeignKey(p => p.GymGroupId);
        });

        modelBuilder.Entity<Application.Model.Training.ExerciseSetModel>(b =>
        {
            b.HasKey(p => p.Id);
            b.Property(p => p.Id).ValueGeneratedOnAdd();

            b.HasOne<Application.Model.Training.GymExerciseModel>().WithMany().HasForeignKey(p => p.ExerciseId);
            b.HasOne<Application.Model.Training.TrainingSectionModel>().WithMany().HasForeignKey(p => p.TrainingSectionId);

            b.Property(p => p.Set).HasMaxLength(45);
        });

        modelBuilder.Entity<Application.Model.Training.GymExerciseModel>(b =>
        {
            b.HasKey(p => p.Id);
            b.Property(p => p.Id).ValueGeneratedOnAdd();

            b.Property(p => p.Title).HasMaxLength(255);

            b.Property(p => p.CreatedAt).HasConversion(ConversionUtils.DateTimeToUtcDateTimeConversion);
        });

        modelBuilder.Entity<Application.Model.Training.GymGroupModel>(b =>
        {
            b.HasKey(p => p.Id);
            b.Property(p => p.Id).ValueGeneratedOnAdd();

            b.Property(p => p.Name).HasMaxLength(255);

            b.Property(p => p.CreatedAt).HasConversion(ConversionUtils.DateoffSetToUtcDateoffSetConversion);
        });

        modelBuilder.Entity<Application.Model.Training.TrainingSectionModel>(b =>
        {
            b.HasKey(p => p.Id);
            b.Property(p => p.Id).ValueGeneratedOnAdd();

            b.HasOne<Application.Model.Training.UserTrainingSheetModel>().WithMany().HasForeignKey(p => p.UserTrainingSheetId);

            b.Property(p => p.MuscularGroup).HasMaxLength(45);

            b.Property(p => p.CreatedAt).HasConversion(ConversionUtils.DateoffSetToUtcDateoffSetConversion);
        });

        modelBuilder.Entity<Application.Model.Training.UserTrainingSheetModel>(b =>
        {
            b.HasKey(p => p.Id);
            b.Property(p => p.Id).ValueGeneratedOnAdd();

            b.Property(p => p.CreatedAt).HasConversion(ConversionUtils.DateoffSetToUtcDateoffSetConversion);

            b.HasOne<Application.Model.Training.GymGroupModel>().WithMany().HasForeignKey(p => p.GymId);
            b.HasOne<Application.Model.Identity.UserModel>().WithMany().HasForeignKey(p => p.StudentId);
        });
    }
}
