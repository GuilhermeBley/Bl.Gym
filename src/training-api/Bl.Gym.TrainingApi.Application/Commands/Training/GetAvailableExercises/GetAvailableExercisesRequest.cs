namespace Bl.Gym.TrainingApi.Application.Commands.Training.GetAvailableExercises;

public record GetAvailableExercisesRequest(
    Guid GymId,
    int Skip,
    int Take)
    : IRequest<GetAvailableExercisesResponse>;
