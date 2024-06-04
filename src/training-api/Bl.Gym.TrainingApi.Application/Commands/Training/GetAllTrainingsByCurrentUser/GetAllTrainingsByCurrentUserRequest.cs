namespace Bl.Gym.TrainingApi.Application.Commands.Training.GetAllTrainingsByCurrentUser;

public record GetAllTrainingsByCurrentUserRequest()
    : IRequest<IEnumerable<GetAllTrainingsByCurrentUserResponse>>;
