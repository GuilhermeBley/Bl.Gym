namespace Bl.Gym.TrainingApi.Application.Commands.Training.GetAvailableExercises;

public record GetAvailableExercisesRequest(
    Guid GymId)
    : IRequest<GetAvailableExercisesResponse>;
