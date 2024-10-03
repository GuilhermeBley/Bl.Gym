namespace Bl.Gym.TrainingApi.Application.Commands.Training.GetAvailableExercises;

public record GetAvailableExercisesResponse(
    GetAvailableExercisesItemResponse[] AvailableExercises);

public record GetAvailableExercisesItemResponse(
    Guid Id,
    string Name,
    string Description);
