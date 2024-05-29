namespace Bl.Gym.TrainingApi.Application.Commands.Training.CreateUserTraining;

public record CreateTrainingToUserRequest(
    Guid GymId,)
    : IRequest<CreateTrainingToUserResponse>;
