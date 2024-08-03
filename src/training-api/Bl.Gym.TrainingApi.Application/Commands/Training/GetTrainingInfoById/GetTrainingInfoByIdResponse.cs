using Bl.Gym.TrainingApi.Domain.Enum;

namespace Bl.Gym.TrainingApi.Application.Commands.Training.GetTrainingInfoById;

/// <summary>
/// This represents the sheet of training and contains the training for an entire week.
/// </summary>
/// <param name="Sections"></param>
public record GetTrainingInfoByIdResponse(
    IEnumerable<GetTrainingInfoByIdResponseSection> Sections,
    string Status,
    DateTimeOffset CreatedAt);

/// <summary>
/// Specific from an single training of day.
/// </summary>
public record GetTrainingInfoByIdResponseSection(
    Guid Id,
    string MuscularGroup,
    int TargetDaysCount,
    int CurrentDaysCount,
    Guid ConcurrencyStamp,
    IEnumerable<GetTrainingInfoByIdResponseExerciseSet> Sets,
    DateTimeOffset CreatedAt);

public record GetTrainingInfoByIdResponseExerciseSet(
    string Set,
    string Title,
    string Description,
    DateTimeOffset CreatedAt);
